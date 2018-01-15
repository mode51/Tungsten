using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using W.AsExtensions;
using W.FromExtensions;
using W.Threading.ThreadExtensions;

namespace W.Net
{
    /// <summary>
    /// A Tcp client
    /// </summary>
    public class Client<TMessageType> : Client
    {
        /// <summary>
        /// Like DataReceived, except the data is deserialized to the appropriate TMessageType
        /// </summary>
        public Action<object, TMessageType> MessageReceived { get; set; }

        /// <summary>
        /// Calls RaiseMessageReceived
        /// </summary>
        /// <param name="message">The received message</param>
        protected void OnMessageReceived(ref TMessageType message)
        {
            RaiseMessageReceived(this, ref message);
        }
        /// <summary>
        /// Overridden to deserialize the data to an object of type TMessageType.  Forwards the call to base.OnDataReceived, then calls OnMessageReceived.
        /// </summary>
        /// <param name="bytes">The data received</param>
        protected override void OnDataReceived(ref byte[] bytes)
        {
            TMessageType message = SerializationMethods.Deserialize<TMessageType>(ref bytes);
            if (message != null)
            {
                base.OnDataReceived(ref bytes);
                OnMessageReceived(ref message);
            }
        }
        /// <summary>
        /// Calls MessageReceived
        /// </summary>
        /// <param name="sender">The Client instance which received the message</param>
        /// <param name="message">The message received</param>
        protected void RaiseMessageReceived(object sender, ref TMessageType message)
        {
            Debug.i("Raising Client.MessageReceived");
            MessageReceived?.Invoke(sender, message);
            Debug.i("Raised Client.MessageReceived");
        }

        /// <summary>
        /// Overload to send messages of type TMessageType via serialization
        /// </summary>
        /// <param name="message">The message to send</param>
        public void Send(TMessageType message)
        {
            byte[] bytes = SerializationMethods.Serialize(message).AsBytes();
            base.Send(bytes);
        }
        /// <summary>
        /// Overload to send messages of type TMessageType via serialization
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="response">The response from the remote machine</param>
        /// <param name="msTimeout">A timeout, in milliseconds, if desired</param>
        public bool SendAndWaitForResponse(TMessageType message, out TMessageType response, int msTimeout = -1)
        {
            byte[] bytes = SerializationMethods.Serialize(message).AsBytes();
            if (SendAndWaitForResponse(bytes, out byte[] responseBytes, msTimeout))
            {
                response = SerializationMethods.Deserialize<TMessageType>(ref responseBytes);
                return true;
            }
            response = default(TMessageType);
            return false;
        }
    }

    /// <summary>
    /// A Tcp client
    /// </summary>
    public partial class Client : Disposable, ITcpClient
    {
        private SocketTransceiverSlim _transceiver;
        private LockableSlim<bool> _disconnecting = new LockableSlim<bool>(false);
        private System.Threading.ManualResetEventSlim _mreConnected = new System.Threading.ManualResetEventSlim(false);
        private W.Threading.CPUProfileEnum _cpuProfile = W.Threading.CPUProfileEnum.SpinWait1;
        private ITcpServer _server;

        /// <summary>
        /// Get or set the number of messages which have been sent
        /// </summary>
        protected ulong SentMessageCount { get; set; }

        /// <summary>
        /// The Socket associated with this Client
        /// </summary>
        public System.Net.Sockets.Socket Socket { get; private set; }
        ///// <summary>
        ///// The TcpClient used with this Client
        ///// </summary>
        //public System.Net.Sockets.TcpClient TcpClient { get; set; }
        /// <summary>
        /// This multi-cast delegate is called after a connection has been established
        /// </summary>
        public Action<object, IPEndPoint> Connected { get; set; }
        /// <summary>
        /// This multi-cast delgate is called after a connection has been closed
        /// </summary>
        /// <remarks>The exception argument will be null unless an exception caused the connecion to close</remarks>
        public Action<object, IPEndPoint, Exception> Disconnected { get; set; }
        ///// <summary>
        ///// This multi-cast delgate is called when one or more messages have been received
        ///// </summary>
        //public Action<object> MessageReady { get; set; }

        ///// <summary>
        ///// Returns the number of received messages which haven't been dequeued
        ///// </summary>
        //public int MessageCount
        //{
        //    get
        //    {
        //        return ReceivedMessages.Count;
        //    }
        //}
        /// <summary>
        /// This multi-cast delgate is called as complete byte messages are received
        /// </summary>
        /// <remarks>Return True to ignore this message, otherwise the data will be queued as usual.</remarks>
        public Action<object, byte[]> DataReceived { get; set; }
        /// <summary>
        /// True if the socket is connected, otherwise False
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return _mreConnected.IsSet;
            }
            protected set
            {
                if (value)
                    _mreConnected.Set();
                else
                    _mreConnected.Reset();
            }
        }
        /// <summary>
        /// Gets or sets a Name for this Client
        /// </summary>
        /// <remarks>The default value is the remote endpoint</remarks>
        public string Name { get; set; }
        /// <summary>
        /// The remote endpoint to which the socket is connected.  This value is null if not connected.
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; protected set; }
        /// <summary>
        /// Select the CPU profile to choose your needs
        /// </summary>
        public W.Threading.CPUProfileEnum CPUProfile { get { return _cpuProfile; } set { _cpuProfile = value; if (_transceiver != null) _transceiver.CPUProfile = value; } }
        /// <summary>
        /// If True, data will be compressed before sending and decompressed when received
        /// </summary>
        public bool UseCompression { get; set; }

        #region Explicit ISocketClient
        /// <summary>
        /// This interface exists for server-side connections
        /// </summary>
        /// <param name="server"></param>
        /// <param name="socket"></param>
        /// <param name="cpuProfile"></param>
        void ITcpClient.ConfigureForServerSide(ITcpServer server, System.Net.Sockets.Socket socket, W.Threading.CPUProfileEnum cpuProfile)
        {
            _server = server;
            Socket = socket;
            RemoteEndPoint = Socket.RemoteEndPoint as IPEndPoint;
            Name = RemoteEndPoint.ToString();
            IsConnected = true; //do we need to set this here?
            CPUProfile = cpuProfile;
            ConfigureTransceiver(); //this must execute after _server has been assigned
            OnConnected(RemoteEndPoint);
        }
        #endregion

        private void ConfigureTransceiver()
        {
            if (_transceiver == null)
            {
                _transceiver = new SocketTransceiverSlim();
                _transceiver.CPUProfile = CPUProfile;
                _transceiver.Disconnected += (s, e) =>
                {
                    Debug.i("Disconnecting server-side client because the remote machine disconnected");
                    OnDisconnect(e);
                };
                _transceiver.MessageReceived += (s, bytes) =>
                {
                    if (bytes == null) //why is bytes null?  It should never be null.
                    System.Diagnostics.Debugger.Break();
                    FormatDataReceived(ref bytes);
                    OnDataReceived(ref bytes);
                };
            }
            //if (startPaused)
            _transceiver.Pause();
            _transceiver.Start(Socket);
        }
        private void Compress(ref byte[] bytes)
        {
            var size = bytes.Length;
            bytes = bytes.AsCompressed(); //using an intermediate variable simply so the origianl isn't modified
            var newSize = bytes.Length;
            Debug.i(string.Format("Compressed {0} to {1}", size, newSize));
        }
        private void Decompress(ref byte[] bytes)
        {
            if (bytes == null)
            {
                System.Diagnostics.Debugger.Break();//we should never get here
                return;
            }
            var size = bytes.Length;
            bytes = bytes.FromCompressed();
            var newSize = bytes.Length;
            Debug.i(string.Format("Decompressed {0} to {1}", size, newSize));
        }

        /// <summary>
        /// Calls the Connected multi-cast delegate when a connection has been established
        /// </summary>
        /// <param name="sender">The local client on which the connected was established</param>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote machine</param>
        protected void RaiseConnected(object sender, IPEndPoint remoteEndPoint)
        {
            //try
            //{
            //IsConnected = true; //_mreConnected.Set(); //TODO: this is the wrong place to set this. Move it and re-test.
            Connected?.Invoke(sender, remoteEndPoint);
            //            }
            //#if NET45
            //            catch (System.Threading.ThreadAbortException e)
            //            {
            //                System.Threading.Thread.ResetAbort();
            //            }
            //#else
            //            catch (ObjectDisposedException)
            //            {
            //                //ignore it, the task is shutting down forcefully
            //            }
            //            catch (AggregateException)
            //            {
            //                //ignore it, the task might be shutting down forcefully
            //            }
            //#endif
            //            catch (Exception e)
            //            {
            //                Debug.e(e);
            //                System.Diagnostics.Debugger.Break();
            //            }
        }
        /// <summary>
        /// Calls the Disconnected multi-cast delegate when a connection has been closed
        /// </summary>
        /// <param name="sender">The local client on which the connected was terminated</param>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote machine</param>
        /// <param name="e">The exception, if one occurred</param>
        protected void RaiseDisconnected(object sender, IPEndPoint remoteEndPoint, Exception e = null)
        {
            //try
            //{
            //Task.Run(() =>
            //{
            Disconnected?.Invoke(sender, remoteEndPoint, e);
            //});
            //            }
            //#if NET45
            //            catch (System.Threading.ThreadAbortException e)
            //            {
            //                System.Threading.Thread.ResetAbort();
            //            }
            //#else
            //            catch (ObjectDisposedException)
            //            {
            //                //ignore it, the task is shutting down forcefully
            //            }
            //            catch (AggregateException)
            //            {
            //                //ignore it, the task might be shutting down forcefully
            //            }
            //#endif
            //            catch (Exception ex)
            //            {
            //                Debug.e(new Exception(ex.ToString(), e));
            //                System.Diagnostics.Debugger.Break();
            //            }
        }
        ///// <summary>
        ///// Calls the MessageReady multi-cast delegate when one or more messages have been received
        ///// </summary>
        ///// <param name="sender">The client on which a message has been received</param>
        //protected void RaiseMessageReady(object sender)
        //{
        //    try
        //    {
        //        var evt = MessageReady;
        //        if (evt == null)
        //            return;
        //        if (MessageCount == 0)
        //            return;
        //        Debug.i("Raising Client.MessageReady");
        //        DelegateExtensions.Raise(evt, sender);
        //        Debug.i("Raised Client.MessageReady");
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.e(e);
        //        System.Diagnostics.Debugger.Break();
        //    }
        //}
        /// <summary>
        /// Calls the DataReceived multi-cast delegate as when one or more messages have been received
        /// </summary>
        /// <param name="sender">The client on which a message has been received</param>
        /// <param name="data">The data received from the remote machine</param>
        /// <remarks>Using this delegate negates MessageReady and the use of GetNextMessage</remarks>
        protected void RaiseDataReceived(object sender, byte[] data)
        {
            //try
            //{
            //Task.Run(() =>
            //{
            Debug.i("Raising ClientBase.DataReceived");
            DataReceived?.Invoke(sender, data);
            Debug.i("Raised ClientBase.DataReceived");
            //});
            //            }
            //#if NET45
            //            catch (System.Threading.ThreadAbortException e)
            //            {
            //                System.Diagnostics.Debug.WriteLine("RaiseDataReceived: " + e.ToString());
            //                System.Threading.Thread.ResetAbort();
            //            }
            //#else
            //            catch (ObjectDisposedException)
            //            {
            //                //ignore it, the task is shutting down forcefully
            //            }
            //            catch (AggregateException)
            //            {
            //                //ignore it, the task might be shutting down forcefully
            //            }
            //#endif
            //            catch (Exception e)
            //            {
            //                System.Diagnostics.Debugger.Break();
            //                Debug.e(e);
            //                System.Diagnostics.Debugger.Break();
            //            }
        }

        ///// <summary>
        ///// Override to customize how incoming messages are handled
        ///// </summary>
        ///// <param name="bytes">The byte array containing the message received</param>
        ///// <returns>Return True if the message should be enqueued. Return False, if this message should not be enqueued.  Enqueued messages should be handled with the MessageReady and GetNextMessage members.</returns>
        //protected virtual bool EnqueueMessageOk(ref byte[] bytes)
        //{
        //    try
        //    {
        //        return !RaiseDataReceived(this, bytes);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.e(e);
        //        System.Diagnostics.Debugger.Break();
        //    }
        //    return true;
        //}

        /// <summary>
        /// Decompresses the data if UseCompression is true.  Can be overridden to provide additional formatting.
        /// </summary>
        /// <param name="bytes">The array of bytes to format</param>
        protected virtual void FormatDataReceived(ref byte[] bytes)
        {
            if (UseCompression)
            {
                Decompress(ref bytes);
            }
        }
        /// <summary>
        /// Compresses the data if UseCompression is true.  Can be overridden to provide additional formatting.
        /// </summary>
        /// <param name="bytes">The array of bytes to format</param>
        protected virtual void FormatDataToSend(ref byte[] bytes)
        {
            if (UseCompression)
            {
                Compress(ref bytes);
            }
        }
        /// <summary>
        /// Adds the data to the ReceivedMessages queue.  Can be overridden to provide specific handling of received data
        /// </summary>
        /// <param name="bytes">The byte array as received from the remote machine</param>
        protected virtual void OnDataReceived(ref byte[] bytes)
        {
            //if (bytes != null)
            //ReceivedMessages.Enqueue(bytes);
            RaiseDataReceived(this, bytes);
        }
        /// <summary>
        /// Calls the Connected multi-cast delegate
        /// </summary>
        /// <param name="remoteEndPoint">The ip endpoint of the remote machine</param>
        protected virtual void OnConnected(IPEndPoint remoteEndPoint)
        {
            RaiseConnected(this, remoteEndPoint);
            _transceiver.Resume();
        }
        /// <summary>
        /// Calls the Disconnected multi-cast delegate
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote machine</param>
        /// <param name="e">An exception if one occurred</param>
        protected virtual void OnDisconnected(IPEndPoint remoteEndPoint, Exception e)
        {
            RaiseDisconnected(this, remoteEndPoint, e);
        }
        /// <summary>
        /// Disconnects from the remote server and cleans up resources
        /// </summary>
        /// <param name="e">An exception if one occurred</param>
        protected virtual void OnDisconnect(Exception e)
        {
            try
            {
                if (IsConnected && !_disconnecting.Value)
                {
                    _disconnecting.Value = true;
                    IsConnected = false;
                    _transceiver?.Stop(); //can this be above the call to _server.DisconnectClient?
                    _transceiver.Join();
                    _server?.DisconnectClient(this, RemoteEndPoint, e);

                    //_server?.DisconnectClient(this, RemoteEndPoint, e);
                    OnDisconnected(RemoteEndPoint, e);
                    //RemoteEndPoint = null;
                    Socket?.Shutdown(SocketShutdown.Both);
#if NETSTANDARD1_3
                    Socket?.Dispose();
#else
                    Socket?.Close();
                    Socket?.Dispose();
#endif
                }
            }
#if NET45
            catch (System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
            }
#endif  
            catch (Exception ex)
            {
                Debug.e(new Exception(ex.ToString(), e));
                System.Diagnostics.Debugger.Break();
            }
            finally
            {
                _disconnecting.Value = false;
            }
        }

        /// <summary>
        /// Attempts to connect to a local or remote Tungsten server
        /// </summary>
        /// <param name="ipAddress">The IP address of the Tungsten server</param>
        /// <param name="port">The port over which to communicate</param>
        /// <returns>True if a connection was made, otherwise False</returns>
        /// <remarks>If an exception occurs, the Disconnected delegate will be called with the specific exception</remarks>
        /// <code>
        /// Connect("127.0.0.1", 32000);
        /// </code>
        public bool Connect(string ipAddress, int port)
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            return Connect(ipEndPoint);
        }
        /// <summary>
        /// Attempts to connect to a local or remote Tungsten server
        /// </summary>
        /// <param name="remoteEndPoint">The IP address and port of the remote server</param>
        /// <returns>True if a connection was made, otherwise False</returns>
        /// <remarks>If an exception occurs, the Disconnected delegate will be called with the specific exception</remarks>
        /// <code>
        /// Connect(remoteIPEndPoint);
        /// </code>
        public bool Connect(IPEndPoint remoteEndPoint)
        {
            Exception ex = null;
            var result = false;
            try
            {
                RemoteEndPoint = remoteEndPoint;
                Socket = new System.Net.Sockets.Socket(SocketType.Stream, ProtocolType.Tcp);
                Socket.Connect(remoteEndPoint);
                ConfigureTransceiver();
                IsConnected = true;
                OnConnected(RemoteEndPoint);
                result = true;
            }
            catch (ArgumentNullException e) //the address parameter is null
            {
                ex = e;
                Debug.i(string.Format("Argument Null Exception: {0}", e.Message));
            }
            catch (ArgumentOutOfRangeException e) //the port is not between MinPort and MaxPort
            {
                ex = e;
                Debug.i(string.Format("Argument Out of Range Exception: {0}", e.Message));
            }
            catch (SocketException e) //an error occured while accessing the socket.
            {
                ex = e;
                var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
                Debug.i(string.Format("Socket Exception({0}): {1}", errorCode, e.Message));
            }
            catch (ObjectDisposedException e) //TcpClient is closed
            {
                ex = e;
                Debug.i(string.Format("Object Disposed Exception: {0}", e.Message));
            }
            catch (System.Security.SecurityException e) //a caller higher in the callstack does not have permission for the operation
            {
                ex = e;
                Debug.e(e);
            }
            catch (InvalidOperationException e) //the Socket has been placed in a listening state by calling Listen
            {
                ex = e;
                Debug.e(e);
            }
            if (ex != null)
            {
                result = false;
                OnDisconnect(ex);
            }
            return result;
        }
        ///// <summary>
        ///// Attempts to connect to a local or remote Tungsten server
        ///// </summary>
        ///// <param name="remoteEndPoint">The IP address and port of the remote server</param>
        ///// <returns>A bool specifying success/failure</returns>
        ///// <remarks>If an exception occurs, the Disconnected delegate will be called with the specific exception</remarks>
        ///// <code>
        ///// Connect(remoteIPEndPoint);
        ///// </code>
        //public virtual bool Connect(IPEndPoint remoteEndPoint)
        //{
        //    Exception ex = null;
        //    var result = false;
        //    try
        //    {
        //        RemoteEndPoint = remoteEndPoint;
        //        TcpClient = new System.Net.Sockets.TcpClient();
        //        var r = TcpClient
        //            .ConnectAsync(RemoteEndPoint.Address, RemoteEndPoint.Port)
        //            .ContinueWith(task =>
        //            {
        //                if (task.IsCanceled || task.IsFaulted)
        //                {
        //                    OnDisconnected(remoteEndPoint, task.Exception);
        //                    //_isConnected.Value = false;
        //                    return;
        //                }
        //                if (TcpClient.Connected)
        //                {
        //                    //_isConnected.Value = true;
        //                    ConfigureTransceiver();
        //                    IsConnected = true;
        //                    OnConnected(RemoteEndPoint);
        //                    result = true;
        //                }
        //            }).Wait(15000);
        //        result = r && result;
        //    }
        //    catch (ArgumentNullException e) //the address parameter is null
        //    {
        //        ex = e;
        //        Debug.i(string.Format("Argument Null Exception: {0}", e.Message));
        //    }
        //    catch (ArgumentOutOfRangeException e) //the port is not between MinPort and MaxPort
        //    {
        //        ex = e;
        //        Debug.i(string.Format("Argument Out of Range Exception: {0}", e.Message));
        //    }
        //    catch (SocketException e) //an error occured while accessing the socket.
        //    {
        //        ex = e;
        //        var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
        //        Debug.i(string.Format("Socket Exception({0}): {1}", errorCode, e.Message));
        //    }
        //    catch (ObjectDisposedException e) //TcpClient is closed
        //    {
        //        ex = e;
        //        Debug.i(string.Format("Object Disposed Exception: {0}", e.Message));
        //    }
        //    if (ex != null)
        //    {
        //        result = false;
        //        Disconnect(ex);
        //    }
        //    return result;
        //}
        /// <summary>
        /// Disconnects from the remote server and cleans up resources
        /// </summary>
        public void Disconnect()
        {
            OnDisconnect(null);
        }

        /// <summary>
        /// Disposes the Client and releases resources
        /// </summary>
        protected override void OnDispose()
        {
            _mreConnected.Dispose();
            base.OnDispose();
        }
        /// <summary>
        /// Sends data to the remote
        /// </summary>
        /// <param name="bytes">The data to send</param>
        public void Send(byte[] bytes)
        {
            FormatDataToSend(ref bytes);
            _transceiver.Send(ref bytes);
            SentMessageCount += 1;
        }
        /// <summary>
        /// Sends data to the remote and waits for a response.  Data is formatted as usual.
        /// </summary>
        /// <param name="bytes">The data to send</param>
        /// <param name="response">The response from the remote machine</param>
        /// <param name="msTimeout">A timeout, in milliseconds, if desired.  If -1, the call will wait indefinitely.</param>
        public bool SendAndWaitForResponse(byte[] bytes, out byte[] response, int msTimeout = -1)
        {
            //provide the usual formatting
            FormatDataToSend(ref bytes);
            if (SendRawAndWaitForResponse(bytes, out response, msTimeout))
            {
                //provide the usual formatting
                FormatDataReceived(ref response);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Sends raw data to the remote and waits the specified amount of time (in milliseconds) for a response.  No formatting is performed.
        /// </summary>
        /// <param name="bytes">The data to send</param>
        /// <param name="response">The response from the remote machine</param>
        /// <param name="msTimeout">A timeout, in milliseconds, if desired.  If -1, the call will wait indefinitely.</param>
        public bool SendRawAndWaitForResponse(byte[] bytes, out byte[] response, int msTimeout = -1)
        {
            var result = false;
            response = null;
            _transceiver.Pause();
            if (System.Threading.SpinWait.SpinUntil(() => _transceiver.IsIdle == true, 15000))
            //if (_transceiver.WaitForValue(t => t.IsIdle == true, 15000))
                result = Socket.SendAndWaitForResponse(ref bytes, out response, msTimeout);
            _transceiver.Resume();
            return result;
        }
        /// <summary>
        /// Sends raw data to the remote and waits the specified amount of time (in milliseconds) for a response.  No formatting is performed.
        /// </summary>
        /// <param name="bytes">The data to send</param>
        /// <param name="msTimeout">A timeout, in milliseconds, if desired.  If -1, the call will wait indefinitely.</param>
        public void SendRaw(byte[] bytes, int msTimeout = -1)
        {
            _transceiver.Pause();
            if (System.Threading.SpinWait.SpinUntil(() => _transceiver.IsIdle == true, msTimeout))
            //if (_transceiver.WaitForValue(t => t.IsIdle == true, msTimeout))
                Socket.SendAndForget(ref bytes);
            _transceiver.Resume();
        }

        /// <summary>
        /// Constructs a new Client
        /// </summary>
        public Client()
        {
        }
        /// <summary>
        /// Deconstructs the Client and calls Dispose
        /// </summary>
        ~Client()
        {
            Dispose();
        }
    }
}