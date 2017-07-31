using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
    /// <summary>
    /// Specified the direction of data for the Pipe
    /// </summary>
    public enum EPipeDirection
    {
        /// <summary>
        /// Receive data only
        /// </summary>
        In = PipeDirection.In,
        /// <summary>
        /// Send data only
        /// </summary>
        Out = PipeDirection.Out,
        /// <summary>
        /// Send and Receive data
        /// </summary>
        InOut = PipeDirection.InOut
    }
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
        private System.Threading.Mutex _mutex = null;

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
        /// Can be useful for large data sets.  Set to True to use compression, otherwise False
        /// </summary>
        /// <remarks>Make sure both server and client have the same value</remarks>
        public bool UseCompression { get; set; }

        /// <summary>
        /// Constructs a new PipeClient
        /// </summary>
        /// <param name="name">The name of the pipe to use</param>
        public PipeServer(string name)
        {
            _name = name;
            _mutex = new Mutex(false, name);
            if (!_mutex.WaitOne(5000)) //this should wait for another server to finish cleanup (if one is cleaning up)
                throw new Exception("The named pipe is already in use");
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
        public void Start()
        {
            Start(-1, EPipeDirection.InOut);
        }
        /// <summary>
        /// Creates the underlying NamedPipeClientStream and connects to the server
        /// </summary>
        public void Start(int maxConnections = -1, EPipeDirection direction = EPipeDirection.InOut)
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
                        stream = new NamedPipeServerStream(_name, (PipeDirection)direction, maxConnections, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                        //Task.Run(() => { Started?.Invoke(this); });
                    }
                    catch (System.IO.IOException)
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
#if NETSTANDARD1_4
                        stream?.WaitForConnectionAsync(_cts.Token).ContinueWith(task => InitializeClient(stream, cts)).Wait(cts.Token);
#else
                        stream.WaitForConnection();
                        InitializeClient(stream, cts);
                        //var test = stream.BeginWaitForConnection(ar => 
                        //{
                        //    var s = (NamedPipeServerStream)ar.AsyncState;
                        //    s.EndWaitForConnection(ar);

                        //}, stream);
#endif
//                        {
//                            if (_cts?.IsCancellationRequested ?? true)
//                            {
//                                stream?.Dispose();
//                                return;
//                            }
//                            if (stream?.IsConnected ?? false)
//                            {
//                                //var p = (TClientType)Activator.CreateInstance(typeof(TClientType), stream);
//                                var client = (TClientType)Activator.CreateInstance(typeof(TClientType));
//                                client.Initialize(stream, true);
//                                client.Disconnected += (o, e) =>
//                                {
//                                    lock (_lockCleanup)
//                                    {
//                                        try
//                                        {
//                                            if (o != null && o is IPipeClient c)
//                                            {
//                                                c.Dispose();
//                                                _clients.Remove(c);
//                                            }
//                                        }
//                                        finally
//                                        {
//                                        }
//                                    }
//                                };
//                                _clients.Add(client);
//                                ClientConnected?.Invoke(client);
//                            }
//                        }
//#if NETSTANDARD1_4
//                        ).Wait(cts.Token);
//#endif
                    }
#if !NETSTANDARD1_4
                    catch (System.Threading.ThreadAbortException)
                    {
                        System.Threading.Thread.ResetAbort();
                    }
#endif
                    catch (System.IO.IOException e)
                    {
                        System.Diagnostics.Debugger.Break(); //why are we here?
                        Exception?.Invoke(this, e);
                    }
                    catch (System.Threading.Tasks.TaskCanceledException)
                    {
                        //ignore
                        System.Diagnostics.Debugger.Break(); //why are we here?
                        //Exception?.Invoke(this, e);
                    }
                    catch (System.OperationCanceledException) //happens when the server is closed
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

        private void InitializeClient(NamedPipeServerStream stream, CancellationTokenSource cts)
        {
            try
            {
                if (_cts?.IsCancellationRequested ?? true)
                {
                    stream?.Dispose();
                    return;
                }
                if (stream?.IsConnected ?? false)
                {
                    //var p = (TClientType)Activator.CreateInstance(typeof(TClientType), stream);
                    var client = (IPipeClient)Activator.CreateInstance(typeof(TClientType));
                    client.Initialize(stream, true);
                    client.UseCompression = this.UseCompression;
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
                    ClientConnected?.Invoke((TClientType)client);
                }
            }
            catch (System.IO.IOException e)
            {
                System.Diagnostics.Debugger.Break(); //why are we here?
                Exception?.Invoke(this, e);
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                //ignore
                System.Diagnostics.Debugger.Break(); //why are we here?
                                                     //Exception?.Invoke(this, e);
            }
            catch (System.OperationCanceledException) //happens when the server is closed
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
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
            _mutex = null;
        }
    }
}