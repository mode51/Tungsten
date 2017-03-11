using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;

namespace W.Pipes
{
    /// <summary>
    /// Wraps a NamedPipeServerStream for easier use
    /// </summary>
    public class PipeServer<TClientType, TType> : IDisposable where TType : class
    {
        private CancellationTokenSource _cts;
        private readonly string _name;
        private W.Threading.Thread _thread;

        /// <summary>
        /// Called if an exception occurs
        /// </summary>
        public Action<PipeServer<TClientType, TType>, Exception> Exception { get; set; }

        /// <summary>
        /// Called when a client connects
        /// </summary>
        public Action<TClientType, string> ClientConnected { get; set; }

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
        public void Start(PipeTransmissionMode mode = PipeTransmissionMode.Byte)
        {
            Stop();
            _cts = new CancellationTokenSource();
            _thread = W.Threading.Thread.Create(cts =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    NamedPipeServerStream stream = null;
                    try
                    {
                        stream = new NamedPipeServerStream(_name, PipeDirection.InOut, 2, mode);
                    }
                    catch (System.IO.IOException e)
                    {
                        Exception?.Invoke(this, e);
                    }
                    catch (Exception e)
                    {
                        Exception?.Invoke(this, e);
                    }
                    if (stream != null)
                    {
                        try
                        {
                            //blocks until a client has connected
                            stream.WaitForConnectionAsync(_cts.Token).Wait(_cts.Token);
                        }
                        catch (System.IO.IOException e)
                        {
                            Exception?.Invoke(this, e);
                        }
                        catch (System.OperationCanceledException e) //happens when the server is closed
                        {
                            Exception?.Invoke(this, e);
                        }
                        catch (Exception e)
                        {
                            Exception?.Invoke(this, e);
                        }
                        if (stream.IsConnected)
                        {
                            var p = (TClientType)Activator.CreateInstance(typeof(TClientType), stream);
                            //var p = new GenericPipe<TType>(stream);
                            //stream.GetImpersonationUserName();
                            ClientConnected?.Invoke(p, "Unknown");
                        }
                    }
                    System.Threading.Thread.Sleep(1); //play nice with other threads
                } //while
            }, null, _cts);
        }
        /// <summary>
        /// Stops the server and frees resources
        /// </summary>
        public void Stop()
        {
            _cts?.Cancel();
            _cts = null;
            _thread?.Cancel();
            _thread = null;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Stop();
        }
    }
}