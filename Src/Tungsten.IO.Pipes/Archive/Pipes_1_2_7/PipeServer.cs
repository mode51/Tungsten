using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
    /// <summary>
    /// Wraps a NamedPipeServerStream for easier use
    /// </summary>
    public class PipeServer<TClientType> : W.Disposable where TClientType : IPipeClient
    {
        private readonly string _name;
        private W.Threading.Thread _thread;
        private List<IPipeClient> _clients = new List<IPipeClient>();
        private object _lockStop = new object();
        private System.Threading.Mutex _uniquePipeNameMutex = null;
        private EPipeDirection _direction = EPipeDirection.InOut;
        private int _maxConnections = -1;
        private ManualResetEventSlim _mreStarted = new ManualResetEventSlim(false);
        private ManualResetEventSlim _mreComplete = new ManualResetEventSlim(false);

        private void ThreadProc(CancellationToken token)
        {
            //var mreStarted = new ManualResetEventSlim(false);
            _mreComplete.Reset();
            try
            {
                //9.10.2017 - Started must be called after we're blocking for an incoming connection
                ////Task.Run(() => { Task.Delay(100); Started?.Invoke(this); });
                //Task.Run(() => { mreStarted.Wait(token); if (!token.IsCancellationRequested) Started?.Invoke(this); });
                Started?.BeginInvoke(this, ar => Started.EndInvoke(ar), null);
                while (!token.IsCancellationRequested)
                {
                    NamedPipeServerStream stream = null;

                    W.Threading.Thread.Sleep(1); //play nice with other threads
                    try
                    {
                        stream = new NamedPipeServerStream(_name, (PipeDirection)_direction, _maxConnections, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                        //Task.Run(() => { Started?.Invoke(this); });
                    }
                    catch (System.IO.IOException)
                    {
                        W.Threading.Thread.Sleep(100);
                    }
                    catch (Exception e)
                    {
                        Exception?.Invoke(this, e);
                    }
                    _mreStarted.Set();
                    try
                    {
                        //blocks until a client has connected
#if NETSTANDARD1_4
                        stream?.WaitForConnectionAsync(token).ContinueWith(task => { if (!task.IsFaulted && !task.IsCanceled) InitializeClient(stream, token); }).Wait(token);
#elif NET45
                        stream.WaitForConnection();
                        if (!token.IsCancellationRequested)
                            InitializeClient(stream, token);
#endif
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
                } //while
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debugger.Break();
            }
            finally
            {
                //mreStarted.Dispose();
                //Stopped?.Invoke(this);
                _mreStarted.Reset();
                _mreComplete.Set();
                Stopped?.BeginInvoke(this, ar => Stopped.EndInvoke(ar), null);
            }
        }
        private void InitializeClient(NamedPipeServerStream stream, CancellationToken token)
        {
            try
            {
                if (token.IsCancellationRequested)
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
                        lock (_lockStop)
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
        private void ConnectToShutdown(Action action)
        {
            using (var npcs = new NamedPipeClientStream(_name))
            {
                npcs.Connect(100);
                action.Invoke();
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        protected override void OnDispose()
        {
            //lock (_disposeLock)
            //{
            //    if (!_disposed)
            //    {
            Stop();
            _thread.Dispose();
            _uniquePipeNameMutex?.ReleaseMutex();
            _uniquePipeNameMutex?.Dispose();
            _uniquePipeNameMutex = null;
            _mreStarted.Dispose();
            _mreComplete.Dispose();
            //_disposed = true;
            //    }
            //}
        }

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
        /// Creates the underlying NamedPipeClientStream and connects to the server
        /// </summary>
        public bool Start()
        {
            return Start(-1, EPipeDirection.InOut);
        }
        /// <summary>
        /// Creates the underlying NamedPipeClientStream and connects to the server
        /// </summary>
        public bool Start(int maxConnections = -1, EPipeDirection direction = EPipeDirection.InOut)
        {
            Stop();
            _maxConnections = maxConnections;
            _direction = direction;
            _thread.Start();//.Run();
            W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1); //allow the thread to start
            return true;
        }
        /// <summary>
        /// Stops the server and frees resources
        /// </summary>
        public void Stop()
        {
            if (_mreStarted.IsSet)
            {
                ConnectToShutdown(() => _thread.Stop());
                _mreComplete.Wait();
            }
            lock (_lockStop)
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
            //_thread.Cancel();
        }

        /// <summary>
        /// Constructs a new PipeClient
        /// </summary>
        /// <param name="name">The name of the pipe to use</param>
        public PipeServer(string name)
        {
            _name = name;
            _uniquePipeNameMutex = new Mutex(false, name);
            if (!_uniquePipeNameMutex.WaitOne(5000)) //this should wait for another server to finish cleanup (if one is cleaning up)
                throw new Exception("The named pipe is already in use");
            //_thread = new W.Threading.Gate(ThreadProc);
            _thread = new W.Threading.Thread(ThreadProc, false);
        }
    }
}