using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
    /// <summary>
    /// Wraps a NamedPipeServerStream for easier use
    /// </summary>
    public class PipeServer<TClientType> : IDisposable where TClientType : IPipeClient
    {
        private CancellationTokenSource _cts;
        private readonly string _name;
        private W.Threading.Thread _thread;
        private List<IPipeClient> _clients = new List<IPipeClient>();
        private object _lockCleanup = new object();

        /// <summary>
        /// Called when the server starts
        /// </summary>
        public Action<PipeServer<TClientType>> Started { get; set; }
        /// <summary>
        /// Called when the server stops
        /// </summary>
        public Action<PipeServer<TClientType>> Stopped { get; set; }

        /// <summary>
        /// Called if an exception occurs
        /// </summary>
        public Action<PipeServer<TClientType>, Exception> Exception { get; set; }

        /// <summary>
        /// Called when a client connects
        /// </summary>
        public Action<TClientType> ClientConnected { get; set; }

        /// <summary>
        /// Constructs a new PipeClient
        /// </summary>
        /// <param name="name">The name of the pipe to use</param>
        public PipeServer(string name)
        {
            _name = name;
        }
        /// <summary>
        /// Disposes the PipeServer
        /// </summary>
        ~PipeServer()
        {
            Dispose();
        }

        /// <summary>
        /// Creates the underlying NamedPipeClientStream and connects to the server
        /// </summary>
        public void Start(int maxConnections = -1)
        {
            Stop();
            _cts = new CancellationTokenSource();
            _thread = W.Threading.Thread.Create(cts =>
            {
                Task.Run(() => { Task.Delay(100); Started?.Invoke(this); });
                while (!_cts?.IsCancellationRequested ?? false)
                {
                    NamedPipeServerStream stream = null;
                    try
                    {
                        stream = new NamedPipeServerStream(_name, PipeDirection.InOut, maxConnections, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                    }
                    catch (System.IO.IOException e)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    catch (Exception e)
                    {
                        Exception?.Invoke(this, e);
                    }
                    try
                    {
                        //blocks until a client has connected
                        stream?.WaitForConnectionAsync(_cts.Token).ContinueWith(task =>
                        {
                            if (_cts?.IsCancellationRequested ?? true)
                            {
                                stream?.Dispose();
                                return;
                            }
                            if (stream?.IsConnected ?? false)
                            {
                                //var p = (TClientType)Activator.CreateInstance(typeof(TClientType), stream);
                                var client = (TClientType)Activator.CreateInstance(typeof(TClientType));
                                client.Initialize(stream, true);
                                client.Disconnected += (o, e) =>
                                {
                                    lock (_lockCleanup)
                                    {
                                        try
                                        {
                                            if (o != null && o is IPipeClient c)
                                            {
                                                c.Dispose();
                                                _clients.Remove(c);
                                            }
                                        }
                                        finally
                                        {
                                        }
                                    }
                                };
                                _clients.Add(client);
                                ClientConnected?.Invoke(client);
                            }
                        }).Wait(cts.Token);
                    }
                    catch (System.IO.IOException e)
                    {
                        System.Diagnostics.Debugger.Break(); //why are we here?
                        Exception?.Invoke(this, e);
                    }
                    catch (System.Threading.Tasks.TaskCanceledException e)
                    {
                        //ignore
                        System.Diagnostics.Debugger.Break(); //why are we here?
                        //Exception?.Invoke(this, e);
                    }
                    catch (System.OperationCanceledException e) //happens when the server is closed
                    {
                        stream?.Dispose(); //dispose the last allocated stream
                        //ignore
                        //Exception?.Invoke(this, e);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debugger.Break(); //why are we here?
                        Exception?.Invoke(this, e);
                    }
                    System.Threading.Thread.Sleep(1); //play nice with other threads
                } //while
            });
        }
        /// <summary>
        /// Stops the server and frees resources
        /// </summary>
        public void Stop()
        {
            lock (_lockCleanup)
            {
                //ensure all clients are disconnected and disposed
                while (_clients.Count > 0)
                {
                    try
                    {
                        var client = _clients[0];
                        client.Dispose();
                        _clients.Remove(client);
                    }
                    finally
                    {
                    }
                }
            }
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _thread?.Cancel();
            _thread = null;
            Stopped?.Invoke(this);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Stop();
        }
    }
}