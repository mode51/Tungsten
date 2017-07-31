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

        /// <summary>
        /// Called when the client connects to the server
        /// </summary>
        public Action<Socket, IPAddress> Connected { get; set; }
        /// <summary>
        /// Called when the client disconnects from the server
        /// </summary>
        public Action<Socket, Exception> Disconnected { get; set; }

        /// <summary>
        /// Called when a message has been sent to the server
        /// </summary>
        public Action<Socket> MessageSent { get; set; }
        /// <summary>
        /// Called when a message is received from the server
        /// </summary>
        public Action<Socket, byte[]> MessageReceived { get; set; }

        /// <summary>
        /// Can be useful for large data sets.  Set to True to use compression, otherwise False.
        /// </summary>
        /// <remarks>Make sure both server and client have the same value</remarks>
        public bool UseCompression { get; set; }

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
            if (_client != null)
                FinalizeConnection(_client.Client.RemoteEndPoint.As<IPEndPoint>()?.Address);
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// Creates the TcpClientReader and TcpClientWriter
        /// </summary>
        /// <param name="remoteAddress">The remote server address</param>
        private void FinalizeConnection(IPAddress remoteAddress)
        {
            _networkStream = _client.GetStream();
            _reader = new TcpClientReader(_client);
            _reader.OnException += Disconnect;
            _reader.OnMessageReceived += OnMessageReceived;
            _reader.Start();

            _writer = new TcpClientWriter(_client);
            _writer.OnException += Disconnect;
            _writer.OnMessageSent += () => { MessageSent?.Invoke(this); };
            _writer.Start();

            if (string.IsNullOrEmpty(Name))
                Name = _client.Client.RemoteEndPoint.As<IPEndPoint>()?.Address.ToString() ?? "";
        }

        /// <summary>
        /// Calls the Notifications.Connected callback
        /// </summary>
        /// <param name="remoteAddress"></param>
        protected virtual void OnConnected(IPAddress remoteAddress)
        {
            Connected?.Invoke(this, remoteAddress);
        }
        /// <summary>
        /// Calls the Notifications.OnDisconnected callback
        /// </summary>
        /// <param name="e">The exception if one occurred</param>
        protected virtual void OnDisconnected(Exception e = null)
        {
            Disconnected?.Invoke(this, e);
        }
        /// <summary>
        /// Calls the Notification.MessageReceived callback
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnMessageReceived(byte[] message)
        {
            if (message != null && message.Length > 0 && UseCompression)
            {
                try
                {
                    var msg = message.AsDecompressed();
                    message = msg;
                }
                catch (System.IO.InvalidDataException)
                {
                    //ignore - the key will be sent uncompressed
                    //so just pass it on as is
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
            }
            MessageReceived?.Invoke(this, message);
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
        public async Task Connect(string remoteAddress, int remotePort)
        {
            await Connect(IPAddress.Parse(remoteAddress), remotePort);
        }
        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <returns>A bool specifying success/failure</returns>
        /// <remarks>If an exception occurs, the Disconnected delegate will be called with the specific exception</remarks>
        public async Task Connect(IPAddress remoteAddress, int remotePort)
        {
            Exception ex = null;
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(remoteAddress, remotePort);
            }
            catch (ArgumentNullException e) //the address parameter is null
            {
                ex = e;
                Log.e(e);
            }
            catch (ArgumentOutOfRangeException e) //the port is not between MinPort and MaxPort
            {
                ex = e;
                Log.e(e);
            }
            catch (SocketException e) //an error occured while accessing the socket.
            {
                ex = e;
                var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
                Log.e("Socket Exception({0}): {1}", errorCode, e.Message);
            }
            catch (ObjectDisposedException e) //TcpClient is closed
            {
                ex = e;
                Log.e(e);
            }
            if (ex != null)
            {
                Disconnect(ex);
                return;
            }
            FinalizeConnection(remoteAddress);
            OnConnected(remoteAddress);
        }
        /// <summary>
        /// Disconnects from the remote server and cleans up resources
        /// </summary>
        /// <param name="e">An exception if one occurred</param>
        public void Disconnect(Exception e = null)
        {
            _reader?.Stop();
            _reader = null;
            _writer?.Stop();
            _writer = null;
            _networkStream?.Dispose();
            _networkStream = null;
            _client?.Close();
            _client = null;
            OnDisconnected(e);
        }

        /// <summary>
        /// Enqueues message to send
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="immediate">If true, the message is sent unformatted and immediately</param>
        public virtual void Send(byte[] message, bool immediate = false)
        {
            if (!IsConnected || message == null)
                return;
            if (UseCompression)
            {
                var size = message.Length;
                message = message.AsCompressed();
                Log.v("Original Size = {0}, Compressed Size = {1}", size, message.Length);
            }
            if (immediate)
                W.Net.TcpHelpers.SendMessageAsync(_networkStream, _client.SendBufferSize, message);
            else
                _writer.Send(message);
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