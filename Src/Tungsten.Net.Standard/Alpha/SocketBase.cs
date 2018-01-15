using System;
using System.Threading;
using System.Threading.Tasks;
using W.AsExtensions;
using W.FromExtensions;
using W.DelegateExtensions;
#if NET45
using System.Security.Principal;
#endif

namespace W.Net.Alpha
{
    /// <summary>
    /// Provides shared functionality for client-side and server-side TcpClients
    /// </summary>
    /// <typeparam name="TSocket">The Type of the class inheriting this base class</typeparam>
    public abstract class SocketBase<TSocket> : IDisposable where TSocket : SocketBase<TSocket>
    {
        private W.Threading.ThreadSlim _thread;
        private LockableSlim<bool> _canChangeSocketValue = new LockableSlim<bool>(true);
        private ManualResetEventSlim _mreWriteComplete = new ManualResetEventSlim(false);
        private LockableSlim<bool> _readOk = new LockableSlim<bool>();
        private System.Collections.Concurrent.ConcurrentQueue<byte[]> _receivedMessages = new System.Collections.Concurrent.ConcurrentQueue<byte[]>();
        private System.Collections.Concurrent.ConcurrentQueue<byte[]> _outgoingMessages = new System.Collections.Concurrent.ConcurrentQueue<byte[]>();

        /// <summary>
        /// Injects code snippets to be executed by ThreadProc
        /// </summary>
        /// 
        protected System.Collections.Concurrent.ConcurrentQueue<Action> WorkQueue = new System.Collections.Concurrent.ConcurrentQueue<Action>();

        /// <summary>
        /// True if this named TcpClient is a server-side client handler, otherwise False
        /// </summary>
        protected bool IsServerSide { get; set; }

        /// <summary>
        /// Called when the client connects to the server
        /// </summary>
        public event Action<TSocket> Connected;// { get; set; }
        /// <summary>
        /// Called when the client disconnects from the server
        /// </summary>
        public event Action<TSocket> Disconnected;// { get; set; }
        /// <summary>
        /// Raised when a message is received from a client
        /// </summary>
        public event Action<TSocket, byte[]> BytesReceived;// { get; set; }
        /// <summary>
        /// Reflects the status of reading data
        /// </summary>
        public event Action<TSocket, int, int> ReadProgress; //TODO: implement this for Client and Server/Host (like it is in W.IO.Pipes)

        /// <summary>
        /// A unique identifier for this socket
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        ///// <summary>
        ///// A handle to the TcpClient
        ///// </summary>
        //public System.Net.Sockets.TcpClient TcpClient { get; private set; }
        /// <summary>
        /// A handle to the underlying Socket
        /// </summary>
        public System.Net.Sockets.Socket Socket { get; private set; }

        /// <summary>
        /// True if the TcpClient is connected, otherwise False
        /// </summary>
        public bool IsConnected => Socket?.Connected ?? false;

        /// <summary>
        /// Gets or sets a value indicating that the data should be compressed before transmission and decompressed after reception
        /// </summary>
        public bool UseCompression { get; set; }

        private void Compress(ref byte[] bytes)
        {
            if (UseCompression)
            {
                var originalLength = bytes.Length;
                bytes = bytes.AsCompressed();
                System.Diagnostics.Debug.WriteLine("Original={0}, Compressed={1}", originalLength, bytes.Length);
#if DEBUG
                Console.WriteLine("Original={0}, Compressed={1}", originalLength, bytes.Length);
#endif
            }
        }
        private void Decompress(ref byte[] bytes)
        {
            if (UseCompression)
            {
                var originalLength = bytes.Length;
                bytes = bytes.FromCompressed();
                System.Diagnostics.Debug.WriteLine("Original={0}, Decompressed={1}", originalLength, bytes.Length);
#if DEBUG
                Console.WriteLine("Original={0}, Decompressed={1}", originalLength, bytes.Length);
#endif
            }
        }
        private void WriteMessage(byte[] bytes, CancellationToken token)
        {
            try
            {
                //                if (UseCompression)
                //                {
                //                    var originalLength = bytes.Length;
                //                    bytes = bytes.AsCompressed();
                //                    System.Diagnostics.Debug.WriteLine("Original={0}, Compressed={1}", originalLength, bytes.Length);
                //#if DEBUG
                //                    Console.WriteLine("Original={0}, Compressed={1}", originalLength, bytes.Length);
                //#endif
                //                }
                Socket.SendMessage(ref bytes);

                System.Diagnostics.Debug.WriteLine("SendAsync sent {0} bytes", bytes.Length);
            }
            catch (OperationCanceledException)
            {
                //token was cancelled
            }
            catch (InvalidOperationException e)
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

        }
        private CallResult<int> GetMessageSize(bool peek)
        {
            var result = new CallResult<int>(false, 0);
            try
            {
                var buffer = new byte[4];
                int byteCount = 0;
                System.Net.Sockets.SocketError errorCode;
                if (peek)
                    byteCount = Socket.Receive(buffer, 0, 4, System.Net.Sockets.SocketFlags.Peek, out errorCode);
                else
                    byteCount = Socket.Receive(buffer, 0, 4, System.Net.Sockets.SocketFlags.None, out errorCode);
                if (errorCode != System.Net.Sockets.SocketError.Success)
                    result.Exception = new Exception("SocketTransceiver.GetMessageSize.Receive: " + errorCode.ToString());
                if (byteCount != 4)
                    result.Exception = new Exception("Unable to read enough bytes from the network", result.Exception);
                result.Result = BitConverter.ToInt32(buffer, 0);
                result.Success = byteCount == 4;
            }
            catch (Exception e)
            {
                Debug.e(e);
            }
            return result;
        }
        private bool GetMessage(int length, out byte[] bytes)
        {
            try
            {
                var bufferSize = Socket.ReceiveBufferSize;
                bytes = new byte[length];
                var bytesRead = 0;

                var numberOfChunks = length % bufferSize;
                while (bytesRead < length)
                {
                    System.Net.Sockets.SocketError errorCode;
                    var bytesToRead = 0;

                    if (bytesRead < length)
                        bytesToRead = Math.Min(bufferSize, length - bytesRead);

                    var read = Socket.Receive(bytes, bytesRead, bytesToRead, System.Net.Sockets.SocketFlags.None, out errorCode);
                    if (errorCode != System.Net.Sockets.SocketError.Success)
                        throw new Exception("GetMessage: " + errorCode.ToString());
                    if (read == 0) //socket closed?
                        throw new Exception("GetMessage: Socket Closed");
                    bytesRead += read;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.e(e);
            }
            bytes = null;
            return false;
        }
        private bool GetResponse(out byte[] response)
        {
            response = null;
            if (Socket?.Available >= 4)
            {
                //receive any incoming data
                Debug.i("Receiving bytes");
                var length = GetMessageSize(false);
                if (length.Success)
                {
                    Debug.i(string.Format("Data Size = {0}", length.Result));
                    if (GetMessage(length.Result, out response))
                    //if (response != null)
                    {
                        Debug.i(string.Format("Received {0} bytes", response.Length));
                        return true;
                    }
                }
            }
            return false;
        }
        private byte[] GetNextMessage()
        {
            try
            {
                if (GetResponse(out byte[] response))
                {
                    //                    if (UseCompression)
                    //                    {
                    //                        var originalLength = response.Length;
                    //                        response = response.FromCompressed();
                    //                        System.Diagnostics.Debug.WriteLine("Original={0}, Decompressed={1}", originalLength, response.Length);
                    //#if DEBUG
                    //                        Console.WriteLine("Original={0}, Decompressed={1}", originalLength, response.Length);
                    //#endif
                    //                    }
                    return response;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            return null;
        }
        private void RaiseReceivedMessages()
        {
            while (_receivedMessages.Count > 0)
            {
                if (_receivedMessages.TryDequeue(out byte[] message))
                {
                    Decompress(ref message);
                    OnBytesReceived(message);
                }
            }
        }
        private void ThreadProc(CancellationToken token)
        {
            while (_canChangeSocketValue.Value != true)
                W.Threading.Thread.Sleep(1);
            //_canChangeSocketValue.WaitForValue(true);
            _canChangeSocketValue.Value = false;
            do
            {
                if (Socket != null)
                {
                    try
                    {
                        //send messages (or do other actions)
                        while (_outgoingMessages.Count > 0)
                        {
                            if (_outgoingMessages.TryDequeue(out byte[] bytes))
                                WriteMessage(bytes, token);
#if NETSTANDARD1_3
                            W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
#else
                            W.Threading.Thread.Sleep(1);
#endif
                        }
                        _mreWriteComplete.Set();
                        if (token.IsCancellationRequested)
                            break;

                        if (_readOk.Value)
                        {
                            //check for new message
                            while (Socket.Available > 4)
                            {

                                var bytes = GetNextMessage();
                                _receivedMessages.Enqueue(bytes);
#if NETSTANDARD1_3
                                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
#else
                            W.Threading.Thread.Sleep(1);
#endif
                            }
                        }
                        //raise any completed messages
                        RaiseReceivedMessages();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debugger.Break();
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }
                    finally
                    {
                    }
                } //if (Socket != null)
#if NETSTANDARD1_3
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
#else
                W.Threading.Thread.Sleep(1);
#endif
                //may not be fastest, but best cpu use is with this as sleep 1
                //W.Threading.Thread.Sleep(1);// W.Threading.CPUProfileEnum.Sleep);
            } while (!token.IsCancellationRequested);
            _canChangeSocketValue.Value = true;
            System.Diagnostics.Debug.WriteLine("Exiting SocketBase.ThreadProc");
        }

        /// <summary>
        /// Disposes the Socket and releases resources
        /// </summary>
        public void Dispose()
        {
            //can't dispose the socket until the thread exits
            if (_thread?.IsRunning ?? false)
                _thread?.Stop();
            _thread?.Dispose();
            if (Socket?.IsConnected() ?? false)
                Disconnect();
            //base.OnDispose();
        }
        /// <summary>
        /// Disconnects from the server
        /// </summary>
        protected virtual void Disconnect()
        {
            try
            {
                if (_thread?.IsRunning ?? false)
                    _thread?.Stop();
                _thread?.Dispose();
                //if (_thread != null) //no need to do this if we've never connected
                //{
                //    try
                //    {
                //        _thread?.Stop();
                //        _thread?.Dispose();
                //    }
                //    catch (Exception e)
                //    {
                //        System.Diagnostics.Debugger.Break();
                //        System.Diagnostics.Debug.WriteLine(e.ToString());
                //    }
                //    _thread = null;
                //    //_mreWaitingForMessagesComplete.Wait();
                //}
                if (Socket == null) //because it will be null once it's disconnected
                    return;
                while (_canChangeSocketValue.Value != true)
                    W.Threading.Thread.Sleep(1);
                //_canChangeSocketValue.WaitForValue(true);
                _canChangeSocketValue.Value = false;
                try
                {
                    if (Socket.Connected)
                        Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
#if NET45
                    Socket.Close();
#endif
                Socket.Dispose();
                Socket = null;
                Disconnected?.Raise(this);

            }
            finally
            {
                _canChangeSocketValue.Value = true;
            }
        }
        /// <summary>
        /// Calls the BytesReceived delegate
        /// </summary>
        /// <param name="bytes">The bytes received</param>
        protected virtual void OnBytesReceived(byte[] bytes)
        {
            BytesReceived?.Raise(this, bytes);
        }

        /// <summary>
        /// Send an array of bytes
        /// </summary>
        /// <param name="bytes">The message as an array of bytes</param>
        public void Write(byte[] bytes)
        {
            _mreWriteComplete.Reset();
            Compress(ref bytes);
            _outgoingMessages.Enqueue(bytes);
        }
        /// <summary>
        /// Blocks the current thread until all messages have been written to the network
        /// </summary>
        public void WaitForWriteToComplete()
        {
            _mreWriteComplete.Wait();
        }
        /// <summary>
        /// Blocks the current thread until all messages have been written to the socket
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait for the operation to complete</param>
        /// <returns>True if all messages were sent withint the timeout period, otherwise False</returns>
        public bool WaitForWriteToComplete(int msTimeout)
        {
            return _mreWriteComplete.Wait(msTimeout);
        }
        /// <summary>
        /// Blocks the current thread until all messages have been written to the socket
        /// </summary>
        /// <param name="token">A CancellationToken which can be used to cancel waiting for the operation to complete</param>
        public void WaitForWriteToComplete(CancellationToken token)
        {
            _mreWriteComplete.Wait(token);
        }

        /// <summary>
        /// Initializes the SocketBase with a previously connected socket
        /// </summary>
        /// <param name="socket">The previously created and connected TcpClient</param>
        internal void InitializeConnection(System.Net.Sockets.Socket socket)
        {
            if (Socket != null)
                System.Diagnostics.Debugger.Break();
            //if (_thread != null)
            //{
            //    _thread.Stop();
            //    _thread.Dispose();
            //}
            _canChangeSocketValue.Value = true;
            Socket = socket;
            _thread = new W.Threading.ThreadSlim(ThreadProc);
            _thread.Name = "SocketBase";
            _thread.Start();
            Connected?.Raise(this);
            _readOk.Value = true;
        }

        /// <summary>
        /// Constructs a new Socket
        /// </summary>
        public SocketBase()
        {
        }
    }
}
