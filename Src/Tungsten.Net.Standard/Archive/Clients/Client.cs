using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using W.Logging;
using W.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// A generic TCP client to be used with W.Net.Server
    /// </summary>
    /// <typeparam name="TDataType">The Type of data to transmit and receive</typeparam>
    public class Client<TDataType> : IDisposable, ISocket where TDataType : class
    {
        //private Lockable<bool> _useCompression = new Lockable<bool>(false);
        private object _lock = new object();
        private W.Net.Sockets.Socket _client = null;
        /// <summary>
        /// A ManualResetEventSlim which is used to signal when a connection has been established
        /// </summary>
        protected System.Threading.ManualResetEventSlim IsConnectedResetEvent { get; private set; } = new System.Threading.ManualResetEventSlim(false);
        /// <summary>
        /// True if the client was created with an existing connection
        /// </summary>
        protected bool IsServerSide { get; private set; }

        /// <summary>
        /// Called when a connection has been established
        /// </summary>
        public Action<object, IPEndPoint> Connected { get; set; }
        /// <summary>
        /// Called when the connection has been terminated
        /// </summary>
        public Action<object, IPEndPoint, Exception> Disconnected { get; set; }
        /// <summary>
        /// Called when data has been received and formatted
        /// </summary>
        public Action<object, TDataType> MessageReceived { get; set; }
        /// <summary>
        /// Called after data has been formatted and sent
        /// </summary>
        public Action<object> MessageSent { get; set; }

        /// <summary>
        /// Exposes the Socket used to send and receive data
        /// </summary>
        public Sockets.Socket Socket => _client;

        /// <summary>
        /// If True, this client will compress data before sending and decompress data received
        /// </summary>
        public bool UseCompression
        {
            get
            {
                return _client?.UseCompression ?? false;
                //return _useCompression.Value;
            }
            set
            {
                if (_client != null)
                    _client.UseCompression = value;
                //_useCompression.Value = value;
            }
        }

        /// <summary>
        /// Queues a message to send to the remote
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="InvalidOperationException">An InvalidOperationException exception will be thrown if a connection has not been established</exception>
        public void Send(TDataType message)
        {
            //if (!IsConnected)
            //    throw new InvalidOperationException("A connection has not been established");
            var formattedMessage = FormatToSend(message);
            _client.Send(formattedMessage);
        }

        /// <summary>
        /// Constructs a new SecureStringClient
        /// </summary>
        public Client()
        {
            _client = new Sockets.Socket();
            InitializeConnection();
        }
        /// <summary>
        /// Constructs a new SecureStringClient
        /// </summary>
        /// <param name="client">An existing connected TcpClient</param>
        public Client(TcpClient client)
        {
            IsServerSide = true;
            _client = new Sockets.Socket(client);
            IsConnectedResetEvent.Set();
            InitializeConnection();
        }
        /// <summary>
        /// Disposes the Client and releases resources
        /// </summary>
        public void Dispose()
        {
            lock (_lock)
            {
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Converts the object to json and then to a byte array
        /// </summary>
        /// <param name="message">The object to convert</param>
        /// <returns>A byte array containing the serialized object</returns>
        protected virtual byte[] FormatToSend(TDataType message)
        {
            var bytes = Newtonsoft.Json.JsonConvert.SerializeObject(message).AsBytes();
            //if (UseCompression)
            //    bytes = bytes.AsCompressed();
            return bytes;
        }
        /// <summary>
        /// Converts a byte array into a deserialized object
        /// </summary>
        /// <param name="msgBytes">The byte array to convert</param>
        /// <returns>The deserialized object</returns>
        protected virtual TDataType FormatReceived(byte[] msgBytes)
        {
            //if (UseCompression)
            //    msgBytes = msgBytes.AsDecompressed();
            var message = msgBytes.AsString();
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TDataType>(message);
            return obj;
        }

        /// <summary>
        /// Called when a connection has been established with the server
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the server</param>
        protected virtual void OnConnected(IPEndPoint remoteEndPoint)
        {
            IsConnectedResetEvent.Set();
            Connected?.Invoke(this, remoteEndPoint);
        }
        /// <summary>
        /// Called when a connection has been terminated
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the server</param>
        /// <param name="e">An exception if one ocurred</param>
        protected virtual void OnDisconnected(IPEndPoint remoteEndPoint, Exception e)
        {
            IsConnectedResetEvent.Reset();
            Disconnected?.Invoke(this, remoteEndPoint, e);
        }
        /// <summary>
        /// Called when a message has been received from the server
        /// </summary>
        /// <param name="bytes">The message as a byte array</param>
        protected virtual void OnMessageReceived(byte[] bytes)
        {
            var obj = FormatReceived(bytes);
            MessageReceived?.Invoke(this, obj);
        }

        private void InitializeConnection()
        {
            _client.Connected += (client, remoteEndPoint) =>
            {
                OnConnected(remoteEndPoint);
            };
            _client.Disconnected += (socket, remoteEndPoint, e) =>
            {
                OnDisconnected(remoteEndPoint, e);
            };
            _client.MessageReceived += (c, bytes) =>
            {
                OnMessageReceived(bytes);
            };
            _client.MessageSent += (socket, bytes) => { MessageSent?.Invoke(socket); };
        }
    }
}