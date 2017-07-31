using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using W.Encryption;
using W.Logging;

namespace W.Net.Sockets
{
    /// <summary>
    /// A binary socket client.  Data is sent and received as byte arrays.
    /// </summary>
    public class Socket : IDisposable
    {
        private System.Net.Sockets.TcpClient _client;
        private TcpClientReader _reader;
        private TcpClientWriter _writer;
        private NetworkStream _networkStream;
        private ulong _messageCount = 0;

        /// <summary>
        /// Called when the client connects to the server
        /// </summary>
        public Action<Socket, IPEndPoint> Connected { get; set; }
        /// <summary>
        /// Called when the client disconnects from the server
        /// </summary>
        public Action<Socket, IPEndPoint, Exception> Disconnected { get; set; }

        /// <summary>
        /// Called when a message has been sent to the server
        /// </summary>
        public Action<Socket, SocketData> DataSent { get; set; }
        /// <summary>
        /// Called when a message is received from the server
        /// </summary>
        public Action<Socket, byte[]> RawDataReceived { get; set; }
        /// <summary>
        /// Called when a message is received from the server and decompressed (if UseCompression is True)
        /// </summary>
        public Action<Socket, byte[]> DataReceived { get; set; }

        /// <summary>
        /// Can be useful for large data sets.  Set to True to use compression, otherwise False.
        /// </summary>
        /// <remarks>Make sure both server and client have the same value</remarks>
        public bool UseCompression { get; set; }

        /// <summary>
        /// The remote IPEndPoint for this socket
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; private set; }
        //public IPEndPoint RemoteEndPoint
        //{
        //    get
        //    {
        //        return _client?.Client?.RemoteEndPoint.As<IPEndPoint>() ?? null;
        //    }
        //}
        /// <summary>
        /// Constructs a ByteClient
        /// </summary>
        public Socket()
        {
        }
        /// <summary>
        /// Constructs a ByteClient
        /// </summary>
        /// <param name="tcpClient">A handle to an existing TcpClient</param>
        public Socket(TcpClient tcpClient)
        {
            if (tcpClient == null)
                throw new ArgumentNullException(nameof(tcpClient), "Client must not be null and must already be connected");
            if (!tcpClient.Connected)
                throw new ArgumentOutOfRangeException(nameof(tcpClient), "Client must already be connected");
            _client = tcpClient;
            var ep = _client.Client.RemoteEndPoint.As<IPEndPoint>();
            if (ep != null)
                RemoteEndPoint = new IPEndPoint(ep.Address, ep.Port);
            if (_client != null)
                FinalizeConnection(RemoteEndPoint);
        }
        /// <summary>
        /// Disposes and deconstructs the Socket instance
        /// </summary>
        ~Socket()
        {
            Dispose();
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates the TcpClientReader and TcpClientWriter
        /// </summary>
        /// <param name="remoteEndPoint">The remote server address</param>
        private void FinalizeConnection(IPEndPoint remoteEndPoint)
        {
            _networkStream = _client.GetStream();
            _reader = new TcpClientReader(_client);
            _reader.OnException += Disconnect;
            _reader.OnMessageReceived += OnMessageReceived;
            _reader.Start();

            _writer = new TcpClientWriter(_client);
            _writer.OnException += Disconnect;
            _writer.OnMessageSent += (message) => { DataSent?.Invoke(this, message); };
            _writer.Start();


            //Name = Guid.NewGuid().ToString() + "." + _client.Client.RemoteEndPoint.As<IPEndPoint>()?.Address.ToString() ?? "";
            Name = RemoteEndPoint.ToString();
            //var ipEndPoint = RemoteEndPoint;
            //if (ipEndPoint != null)
            //    Name = ipEndPoint.Address.ToString() + ":" + ipEndPoint.Port.ToString();
        }

        /// <summary>
        /// Calls the Notifications.Connected callback
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        protected virtual void OnConnected(IPEndPoint remoteEndPoint)
        {
            Connected?.Invoke(this, remoteEndPoint);
        }
        /// <summary>
        /// Calls the Notifications.OnDisconnected callback
        /// </summary>
        /// <param name="remoteEndPoint">The remote IPEndPoint which has disconnected</param>
        /// <param name="e">The exception if one occurred</param>
        protected virtual void OnDisconnected(IPEndPoint remoteEndPoint, Exception e = null)
        {
            Disconnected?.Invoke(this, remoteEndPoint, e);
        }
        /// <summary>
        /// Calls the Notification.MessageReceived callback
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnMessageReceived(byte[] message)
        {
            RawDataReceived?.Invoke(this, message);
            if (message != null && message.Length > 0 && UseCompression)
            {
                try
                {
                    var msg = message.AsDecompressed();
                    message = msg;
                }
                catch (System.IO.InvalidDataException)
                {
                    //ignore - the public key could be sent uncompressed
                    //so just pass it on as is
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    System.Diagnostics.Debugger.Break();
                }
            }
            DataReceived?.Invoke(this, message);
        }

        /// <summary>
        /// Gets or sets a Name property
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// True if the Client is currently connected to a Tungsten RPC Server, otherwise False
        /// </summary>
        public bool IsConnected => _client?.Connected ?? false;
        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <returns>A bool specifying success/failure</returns>
        /// <remarks>If an exception occurs, the Disconnected delegate will be called with the specific exception</remarks>
        public async Task<bool> ConnectAsync(string remoteAddress, int remotePort)
        {
            return await ConnectAsync(IPAddress.Parse(remoteAddress), remotePort);
        }
        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <returns>A bool specifying success/failure</returns>
        /// <remarks>If an exception occurs, the Disconnected delegate will be called with the specific exception</remarks>
        public async Task<bool> ConnectAsync(IPAddress remoteAddress, int remotePort)
        {
            Exception ex = null;
            try
            {
                RemoteEndPoint = new IPEndPoint(remoteAddress, remotePort);
                _client = new TcpClient();

                await _client.ConnectAsync(remoteAddress, remotePort);
            }
            catch (ArgumentNullException e) //the address parameter is null
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine(string.Format("Argument Null Exception: {0}", e.Message));
            }
            catch (ArgumentOutOfRangeException e) //the port is not between MinPort and MaxPort
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine(string.Format("Argument Out of Range Exception: {0}", e.Message));
            }
            catch (SocketException e) //an error occured while accessing the socket.
            {
                ex = e;
                var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
                System.Diagnostics.Debug.WriteLine(string.Format("Socket Exception({0}): {1}", errorCode, e.Message));
            }
            catch (ObjectDisposedException e) //TcpClient is closed
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine(string.Format("Object Disposed Exception: {0}", e.Message));
            }
            if (ex != null)
            {
                Disconnect(ex);
                return false;
            }
            FinalizeConnection(RemoteEndPoint);
            OnConnected(RemoteEndPoint);
            return true;
        }
        /// <summary>
        /// Disconnects from the remote server and cleans up resources
        /// </summary>
        /// <param name="e">An exception if one occurred</param>
        public void Disconnect(Exception e = null)
        {
            if (_client == null)
                return;
            //var remoteEndPoint = _client.Client.RemoteEndPoint.As<IPEndPoint>(); //retain a reference
            _reader?.Stop();
            _reader = null;
            _writer?.Stop();
            _writer = null;
            _networkStream?.Dispose();
            _networkStream = null;
#if NETSTANDARD1_3 || NETSTANDARD1_4
            _client?.Dispose();
#else
            _client?.Close();
#endif
            _client = null;
            OnDisconnected(RemoteEndPoint, e);
        }
        /// <summary>
        /// Enqueues message to send
        /// </summary>
        /// <param name="message">The message to send</param>
        public virtual ulong Send(byte[] message)
        {
            return Send(message, false, false);
        }

        /// <summary>
        /// Enqueues message to send
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="exact">Can be used to prevent compression</param>
        /// <param name="immediate">If true, the message is sent unformatted and immediately</param>
        public virtual ulong Send(byte[] message, bool exact, bool immediate)
        {
            if (!IsConnected || message == null)
                return 0;
            var size = message.Length;
            if (!exact && UseCompression)
            {
                message = message.AsCompressed();
            }
            //if (System.Diagnostics.Debugger.IsAttached)
                //Console.WriteLine("UseCompression = {0}: Original Size = {1}, Actual Size = {2}", UseCompression, size, message.Length);
            System.Diagnostics.Debug.WriteLine("UseCompression = {0}: Original Size = {1}, Actual Size = {2}", UseCompression, size, message.Length);
            _messageCount += 1;
            if (immediate)
                W.Net.TcpHelpers.SendMessageAsync(_networkStream, _client.SendBufferSize, message);
            else
                _writer.Send(new SocketData() { Id = _messageCount, Data = message });
            return _messageCount;
        }
        ///// <summary>
        ///// Enqueues message to send
        ///// </summary>
        ///// <param name="message">The message to send</param>
        ///// <param name="immediate">If true, the message is sent unformatted and immediately</param>
        //public virtual void Send(string message, bool immediate = false)
        //{
        //    Send(message.AsBytes(), immediate); //TODO: 2.26.17 - should this conver to base64 before sending?
        //}
    }
}