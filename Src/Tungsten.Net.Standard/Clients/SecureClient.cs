using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using W.Logging;

namespace W.Net
{
    //this one works, but we'll try to use some inheritance
    //public class SecureClient<TDataType> : IDisposable, ISecureSocket where TDataType : class
    //{
    //    private W.Net.Sockets.Socket _client = null;
    //    private W.Encryption.RSA _rsa;
    //    private RSAParameters? _remotePublicKey;
    //    private System.Threading.ManualResetEvent _mreConnected = new System.Threading.ManualResetEvent(false);
    //    private Lockable<bool> _useCompression = new Lockable<bool>(false);
    //    private object _lock = new object();

    //    /// <summary>
    //    /// Called when a connection has been established
    //    /// </summary>
    //    public Action<object, IPEndPoint> Connected { get; set; }
    //    /// <summary>
    //    /// Called when the connection has been terminated
    //    /// </summary>
    //    public Action<object, IPEndPoint, Exception> Disconnected { get; set; }
    //    /// <summary>
    //    /// Called when data has been received and formatted
    //    /// </summary>
    //    public Action<object, TDataType> MessageReceived { get; set; }
    //    /// <summary>
    //    /// Called after data has been formatted and sent
    //    /// </summary>
    //    public Action<object> MessageSent { get; set; }

    //    public Sockets.Socket Socket => _client;

    //    public bool WaitForConnected(int msTimeout = -1)
    //    {
    //        return _mreConnected.WaitOne(msTimeout);
    //    }
    //    public bool IsConnected => _mreConnected.WaitOne(1);
    //    public bool UseCompression
    //    {
    //        get
    //        {
    //            return _useCompression.Value;
    //        }
    //        set
    //        {
    //            _useCompression.Value = value;
    //        }
    //    }

    //    public void Send(TDataType message)
    //    {
    //        if (!IsConnected)
    //            throw new InvalidOperationException("A connection has not been established");
    //        var formattedMessage = FormatToSend(message);
    //        _client.Send(formattedMessage);
    //    }

    //    /// <summary>
    //    /// Constructs a new SecureStringClient
    //    /// </summary>
    //    public SecureClient()
    //    {
    //        _rsa = new W.Encryption.RSA();
    //        _client = new Sockets.Socket();
    //        InitializeConnection();
    //    }
    //    /// <summary>
    //    /// Constructs a new SecureStringClient
    //    /// </summary>
    //    /// <param name="client">An existing connected TcpClient</param>
    //    /// <param name="rsa">An existing instance of RSA to be used for encryption</param>
    //    public SecureClient(TcpClient client, W.Encryption.RSA rsa)
    //    {
    //        _rsa = rsa;
    //        _client = new Sockets.Socket(client);
    //        InitializeConnection();
    //        SendPublicKey(); //immediately send the public key
    //        Log.v("Server Sent Public Key");
    //    }

    //    protected virtual byte[] FormatToSend(TDataType message)
    //    {
    //        var bytes = Newtonsoft.Json.JsonConvert.SerializeObject(message).AsBytes(); //Base64 encodes the message while encrypting
    //        var msgBytes = _rsa.Encrypt(bytes, (RSAParameters)_remotePublicKey).AsBytes(); //msg should be base64 encoded going into _rsa.Encrypt
    //        if (UseCompression)
    //            msgBytes = msgBytes.AsCompressed();
    //        return msgBytes;
    //    }
    //    protected virtual TDataType FormatReceived(byte[] msgBytes)
    //    {
    //        if (UseCompression)
    //            msgBytes = msgBytes.AsDecompressed();
    //        var message = msgBytes.AsString();
    //        message = _rsa.Decrypt(message); //msg should be base64 encoded going into _rsa.Decrypt
    //        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TDataType>(message);
    //        return obj;
    //    }

    //    private void InitializeConnection()
    //    {
    //        _client.Connected += (client, address) =>
    //        {
    //            _mreConnected.Reset();
    //            SendPublicKey(); //immediately send the public key
    //            Log.v("Client Sent Public Key");
    //        };
    //        _client.Disconnected += (socket, remoteEndPoint, e) =>
    //        {
    //            _mreConnected.Reset();
    //            _remotePublicKey = null;
    //            Disconnected?.Invoke(this, remoteEndPoint, e);
    //        };
    //        _client.MessageReceived += (c, msgBytes) =>
    //        {
    //            if (_remotePublicKey != null) //then we're secure
    //            {
    //                var obj = FormatReceived(msgBytes);
    //                MessageReceived?.Invoke(this, obj);
    //            }
    //            else
    //            {
    //                try
    //                {
    //                    var json = msgBytes.AsString();
    //                    _remotePublicKey = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(json);
    //                }
    //                catch (Exception e)
    //                {
    //                    Log.e(e);
    //                    System.Diagnostics.Debug.WriteLine(e.ToString());
    //                }
    //                Log.v("Received Public Key");
    //                msgBytes = null; //not a real message, so set it to null
    //                Connected?.Invoke(this, _client.RemoteEndPoint);
    //                _mreConnected.Set();
    //            }
    //        };
    //        _client.MessageSent += MessageSent;
    //    }
    //    private void SendPublicKey()
    //    {
    //        //var publicKey = _rsa.PublicKey.AsXml<RSAParameters>();
    //        string publicKey = string.Empty;
    //        try
    //        {
    //            publicKey = Newtonsoft.Json.JsonConvert.SerializeObject(_rsa.PublicKey);
    //            _client.Send(publicKey.AsBytes(), true);
    //        }
    //        catch (Exception e)
    //        {
    //            Log.e(e);
    //            System.Diagnostics.Debug.WriteLine(e.ToString());
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        lock (_lock)
    //        {
    //            if (_client != null)
    //            {
    //                _client.Dispose();
    //                _client = null;
    //                GC.SuppressFinalize(this);
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// A secure TCP client which connects to a SecureServer and transceives serializable objects.  Assymetric encryption is used to secure the transmitted data.
    /// </summary>
    /// <typeparam name="TDataType"></typeparam>
    public class SecureClient<TDataType> : Client<TDataType> where TDataType : class
    {
        private W.Encryption.RSA _rsa;
        private RSAParameters? _remotePublicKey = null;

        /// <summary>
        /// Constructs a new SecureStringClient
        /// </summary>
        public SecureClient() : base()
        {
            _rsa = new W.Encryption.RSA();
        }
        /// <summary>
        /// Constructs a new SecureStringClient (used by SecureServer)
        /// </summary>
        /// <param name="client">An existing connected TcpClient</param>
        /// <param name="rsa">An existing instance of RSA to be used for encryption</param>
        public SecureClient(TcpClient client, W.Encryption.RSA rsa) : base(client)
        {
            _rsa = rsa;
            SendPublicKey(); //immediately send the public key
            //can't log messages yet - Log.v("Server Sent Public Key");
        }

        /// <summary>
        /// Determines if a string contains only Base64 characters
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value contains only Base64 characters, otherwise False</returns>
        protected bool IsBase64Encoded(string value)
        {
            var result = false;
            var regex = new System.Text.RegularExpressions.Regex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$");
            result = regex.IsMatch(value);
            return result;
        }
        /// <summary>
        /// Converts the object to json and then to a byte array
        /// </summary>
        /// <param name="message">The object to convert</param>
        /// <returns>A byte array containing the serialized object</returns>
        protected override byte[] FormatToSend(TDataType message)
        {
            var bytes = Newtonsoft.Json.JsonConvert.SerializeObject(message).AsBytes();
            var msgBytes = _rsa.Encrypt(bytes, (RSAParameters)_remotePublicKey).AsBytes(); //Base64 encodes the message while encrypting //msg should be base64 encoded going into _rsa.Encrypt
            //if (UseCompression)
            //    msgBytes = msgBytes.AsCompressed();
            return msgBytes;
        }
        /// <summary>
        /// Converts a byte array into a deserialized object
        /// </summary>
        /// <param name="msgBytes">The byte array to convert</param>
        /// <returns>The deserialized object</returns>
        protected override TDataType FormatReceived(byte[] msgBytes)
        {
            //if (UseCompression)
            //    msgBytes = msgBytes.AsDecompressed();
            var message = msgBytes.AsString();
            message = _rsa.Decrypt(message); //msg should be base64 encoded (by a previous Encrypt call) going into _rsa.Decrypt
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TDataType>(message);
            return obj;
        }
        /// <summary>
        /// Called when a connection has been established with the server
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the server</param>
        protected override void OnConnected(IPEndPoint remoteEndPoint)
        {
            //don't call the base because we don't want to raise the Connected event yet
            //base.OnConnected(remoteEndPoint);
            IsConnectedResetEvent.Reset();
            SendPublicKey(); //immediately send the public key
            //can't log messages yet - Log.v("Client Sent Public Key");
        }
        /// <summary>
        /// Called when a connection has been terminated
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the server</param>
        /// <param name="e">An exception if one ocurred</param>
        protected override void OnDisconnected(IPEndPoint remoteEndPoint, Exception e)
        {
            _remotePublicKey = null;
            //call the base to finish the disconnect
            base.OnDisconnected(remoteEndPoint, e);
        }
        /// <summary>
        /// Called when a message has been received from the server
        /// </summary>
        /// <param name="bytes">The message as a byte array</param>
        protected override void OnMessageReceived(byte[] bytes)
        {
            //base.OnMessageReceived(bytes);
            if (_remotePublicKey != null) //then we're secure
            {
                base.OnMessageReceived(bytes);
                //var obj = FormatReceived(bytes);
                //MessageReceived?.Invoke(this, obj);
            }
            else
            {
                try
                {
                    var json = bytes.AsString();//.FromBase64();
                    if (IsBase64Encoded(json))
                    {
                        System.Diagnostics.Debug.WriteLine("Remote public key is curiously Base64 encoded");
                        json = json.FromBase64(); //still not sure how this gets base64 encoded in the first place
                    }
                    _remotePublicKey = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(json);

                    System.Diagnostics.Debug.WriteLine("{0} received {1}'s public Key", IsServerSide ? "Server" : "Client", IsServerSide ? "client" : "server");
                    //bytes = null; //not a real message, so set it to null
                    IsConnectedResetEvent.Set();
                    Connected?.Invoke(this, Socket.RemoteEndPoint);
                }
                catch (Exception e)
                {
                    //Log.e(e);
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        private void SendPublicKey()
        {
            //var publicKey = _rsa.PublicKey.AsXml<RSAParameters>();
            string publicKey = string.Empty;
            try
            {
                publicKey = Newtonsoft.Json.JsonConvert.SerializeObject(_rsa.PublicKey);
                var bytes = publicKey.AsBytes();
                if (IsBase64Encoded(bytes.AsString()))
                    System.Diagnostics.Debugger.Break();
                Socket.Send(bytes, true, true);
            }
            catch (Exception e)
            {
                //Log.e(e);
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debugger.Break();
            }
        }
    }
}