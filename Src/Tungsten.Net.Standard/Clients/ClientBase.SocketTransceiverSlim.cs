using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using W;

namespace W.Net
{
    public partial class Client
    {
        //internal class ThreadProc_RunToCompletion
        //{
        //    private Action<System.Threading.CancellationToken> _action;
        //    private W.Threading.ThreadHandle _thread = null;
        //    private System.Threading.ManualResetEventSlim _mreComplete = new System.Threading.ManualResetEventSlim(false);
        //    private System.Threading.ManualResetEventSlim _mreStarted = new System.Threading.ManualResetEventSlim(false);
        //    private System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();

        //    public bool IsStarted
        //    {
        //        get
        //        {
        //            return _mreStarted.IsSet;
        //        }
        //    }
        //    public bool IsComplete
        //    {
        //        get
        //        {
        //            return _mreComplete.IsSet;
        //        }
        //    }
        //    public void WaitForCompletion()
        //    {
        //        _mreComplete.Wait();
        //    }
        //    public void Start()
        //    {
        //        if (IsStarted)
        //            throw new InvalidOperationException("The thread has already been started.  It cannot be restarted");
        //        var task = Task.Factory.StartNew(() =>
        //        {
        //            try
        //            {
        //                _mreComplete.Reset();
        //                _mreStarted.Set();
        //                _action.Invoke(cts.Token);
        //            }
        //            finally
        //            {
        //                _mreComplete.Set();
        //            }
        //        }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        //        _mreStarted.Wait();
        //    }
        //    public void Stop()
        //    {
        //        cts.Cancel();
        //        _mreComplete.Wait();
        //    }

        //    public ThreadProc_RunToCompletion(Action<System.Threading.CancellationToken> action)
        //    {
        //        _action = action;
        //    }
        //    ~ThreadProc_RunToCompletion()
        //    {
        //        cts.Dispose();
        //        _mreStarted.Dispose();
        //        _mreComplete.Dispose();
        //    }
        //}

        /// <summary>
        /// Sends and receives messages.  Queues messages to send to speed up the process.
        /// </summary>
        internal class SocketTransceiverSlim
        {
            private W.Threading.Thread _thread;
            //private System.Collections.Concurrent.ConcurrentQueue<byte[]> _sendQueue = new System.Collections.Concurrent.ConcurrentQueue<byte[]>();
            private LockableSlim<bool> _paused = new LockableSlim<bool>(false);
            private LockableSlim<bool> _isIdle = new LockableSlim<bool>(false);
            private System.Threading.CancellationTokenSource _ctsPauseTimeout = null; //so that we can auto-resume
            private LockableSlim<System.Net.Sockets.Socket> _socket = new LockableSlim<Socket>(null);

            /// <summary>
            /// Select the CPU profile to choose your needs
            /// </summary>
            public W.Threading.CPUProfileEnum CPUProfile { get; set; } = W.Threading.CPUProfileEnum.Sleep;

            ///// <summary>
            ///// The Socket on which to send/receive messages
            ///// </summary>
            //public Socket Socket => _socket.Value; // { get; set; }
            /// <summary>
            /// Called when one or more messages have been received
            /// </summary>
            public Action<SocketTransceiverSlim, byte[]> MessageReceived { get; set; }
            /// <summary>
            /// Called when the Socket disconnects.  If an exception caused the disconnection, it is also provided.
            /// </summary>
            public Action<SocketTransceiverSlim, Exception> Disconnected { get; set; }

            protected virtual void OnMessageReceived(byte[] bytes)
            {
                Debug.i(string.Format("Received {0} bytes", bytes.Length));
                RaiseMessageReceived(bytes);
            }

            public bool IsIdle
            {
                get
                {
                    return _isIdle.Value;
                }
            }
            /// <summary>
            /// If the transceiver is running, the background thread continues to run, but it does not send or transmit messages.
            /// </summary>
            /// <remarks>Use caution because the Transceiver can be started in a paused state</remarks>
            public void Pause(int msTimeout = -1)
            {
                _ctsPauseTimeout?.Dispose();
                if (msTimeout < 0)
                    _ctsPauseTimeout = new System.Threading.CancellationTokenSource();
                else
                    _ctsPauseTimeout = new System.Threading.CancellationTokenSource(msTimeout);
                _paused.Value = true;
            }
            /// <summary>
            /// If the transceiver is running, the background thread will send and receive as usual.
            /// </summary>
            public void Resume()
            {
                _paused.Value = false;
                _ctsPauseTimeout?.Dispose();
                _ctsPauseTimeout = null;
            }
            /// <summary>
            /// Starts a thread which will listen for incoming messages and send outgoing messages
            /// </summary>
            /// <remarks>Use caution because the Transceiver can be started in a paused state</remarks>
            public void Start(Socket socket)
            {
                _socket.Value = socket;
                Stop();
                _thread = new Threading.Thread(ThreadProc);
                _thread.Start();
            }
            public void Stop()
            {
                //_thread will be null on the first call to Start (Start initially calls Stop)
                _thread?.Stop();
            }
            public void Join()
            {
                _thread.Join();
            }
            /// <summary>
            /// Enqueues a message for transmission
            /// </summary>
            /// <param name="bytes">The byte array containing the message</param>
            /// <remarks>All messages waiting in the socket buffer will be received before message are sent.</remarks>
            public void Send(ref byte[] bytes)
            {
                //_sendQueue.Enqueue(bytes);
                _socket.Value.SendMessage(ref bytes);
                Debug.i(string.Format("Send {0} bytes enqueued", bytes.Length));
            }

            private void RaiseMessageReceived(byte[] bytes)
            {
                //try
                //{
                //Task.Run(() =>
                //{
                //make a copy of the byte array because we're raising this event in a task
                //var receivedBytes = (byte[])bytes.Clone();
                Debug.i("Raising MessageReceived");
                MessageReceived?.Invoke(this, bytes);// receivedBytes);
                Debug.i("Raised MessageReceived");
                //});
                //}
                //catch (Exception e)
                //{
                //    Debug.e(e);
                //    System.Diagnostics.Debugger.Break();
                //}
            }
            private void RaiseDisconnected(Exception e)
            {
                //Task.Run(() =>
                //{
                Disconnected?.Invoke(this, e);
                //});
            }
            private void ThreadProc(System.Threading.CancellationToken token)
            {
                SocketException ex = null;
                var isValidConnection = true;
                try
                {
                    //run until canceled
                    while (!token.IsCancellationRequested)
                    {
                        //if paused, don't process socket transmissions (likely another thread is sending/receiving)
                        if (_paused.Value)
                        {
                            if (_ctsPauseTimeout?.IsCancellationRequested ?? true)
                                Resume();
                            else
                            {
                                _isIdle.Value = true;
                                W.Threading.Thread.Sleep(1);
                            }
                            continue;
                        }
                        _isIdle.Value = false;
                        isValidConnection = _socket.Value?.IsConnected() ?? false;
                        if (!isValidConnection)
                        {
                            Debug.i("Disconnecting via invalid connection");
                            RaiseDisconnected(ex); //causes this.Dispose to be called
                            break;
                        }

                        // use while just in case there are multiple incoming messages?
                        while (_socket.Value.Available > 4)
                        {
                            byte[] bytes = null;
                            bool result = false;
                            result = _socket.Value.GetResponse(out bytes);
                            if (result)
                            {
                                Debug.i(string.Format("Received {0} bytes", bytes?.Length));
                                OnMessageReceived(bytes);
                                bytes = null;
                            }
                        }

                        ////send any queued outgoing data
                        //while ((!token.IsCancellationRequested) && _sendQueue.Count > 0)
                        //{
                        //    byte[] sendData;
                        //    if (_sendQueue.TryDequeue(out sendData))
                        //    {
                        //        if (sendData == null)
                        //            System.Diagnostics.Debugger.Break();
                        //        _socket.Value.SendMessage(ref sendData);
                        //    }
                        //    else
                        //        System.Diagnostics.Debugger.Break();
                        //    //W.Threading.Thread.Sleep(CPUProfile);
                        //}
                        W.Threading.Thread.Sleep(CPUProfile);
                    }
                }
#if !NETSTANDARD1_3
                    catch(System.Threading.ThreadAbortException)
                    {
                        System.Threading.Thread.ResetAbort();
                        System.Diagnostics.Debugger.Break();
                    }
#endif
                catch (AggregateException e)
                {
                    Debug.e(e);
                    System.Diagnostics.Debugger.Break();
                }
                catch (ObjectDisposedException)
                {
                    ex = new SocketException((int)SocketError.OperationAborted);
                    Debug.i("Disconnecting via object disposed");
                    RaiseDisconnected(ex);
                }
                catch (TimeoutException)
                {
                    ex = new SocketException((int)SocketError.TimedOut);
                    Debug.i("Disconnecting via timeout");
                    RaiseDisconnected(ex);
                }
                catch (SocketException e)
                {
                    ex = e;
                    Debug.i("Disconnecting via {0}", e.SocketErrorCode.ToString());
                    RaiseDisconnected(ex);
                }
                catch (Exception)
                {
                    ex = new SocketException((int)SocketError.Disconnecting);
                    Debug.i("Disconnecting via general exception");
                    RaiseDisconnected(ex);
                }
            }

            /// <summary>
            /// Constructs a new SocketTransceiver
            /// </summary>
            /// <param name="socket">The socket on which to send and receive messages</param>
            public SocketTransceiverSlim()//Socket socket)
            {
                //Client = client;
                //_socket.Value = socket;
            }
            ~SocketTransceiverSlim()
            {
                _ctsPauseTimeout?.Dispose();
            }
        }

        ///// <summary>
        ///// Sends and receives messages.  Queues both to speed up the process.
        ///// </summary>
        //internal class SocketTransceiver : SocketTransceiverSlim
        //{
        //    private System.Collections.Concurrent.ConcurrentQueue<byte[]> _receivedQueue = new System.Collections.Concurrent.ConcurrentQueue<byte[]>();

        //    /// <summary>
        //    /// Called when one or more messages have been received
        //    /// </summary>
        //    public Action<SocketTransceiver> MessageReady { get; set; }

        //    /// <summary>
        //    /// The number of messages in the receive queue
        //    /// </summary>
        //    public int MessageCount
        //    {
        //        get
        //        {
        //            return _receivedQueue.Count;
        //        }
        //    }
        //    /// <summary>
        //    /// Dequeues the next messages in the receive queue
        //    /// </summary>
        //    /// <returns>A byte array containing the message</returns>
        //    public bool GetNextMessage(out byte[] bytes)
        //    {
        //        if (_receivedQueue.TryDequeue(out bytes))
        //            return true;
        //        return false;
        //    }

        //    protected override void OnMessageReceived(ref byte[] bytes)
        //    {
        //        _receivedQueue.Enqueue(bytes);
        //        Debug.i("Bytes Enqueued");
        //        RaiseMessageReady();
        //        base.OnMessageReceived(ref bytes); //raises MessageReceived
        //    }

        //    private void RaiseMessageReady()
        //    {
        //        try
        //        {
        //            Debug.i("Raising MessageReady");
        //            MessageReceived?.Raise(this);
        //            Debug.i("Raised MessageReady");
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.e(e);
        //            System.Diagnostics.Debugger.Break();
        //        }
        //    }

        //    /// <summary>
        //    /// Constructs a new SocketTransceiver
        //    /// </summary>
        //    /// <param name="socket">The socket on which to send and receive messages</param>
        //    public SocketTransceiver(Socket socket) : base(socket)
        //    {
        //    }
        //}
    }
}