using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using W.Net.Sockets;
using W.Logging;

namespace W.Net
{
    /// <summary>
    /// Secures XClient with RSA encryption
    /// </summary>
    public class SecureClient : Client
    {
        private W.Encryption.PublicPrivateKeyPair _keys = W.Encryption.RSAMethods.CreateKeyPair(2048);
        //private W.Encryption.RSA _rsa;
        private RSAParameters? _remotePublicKey = null;
        private long _messageSentCount = 0;
        private System.Threading.ManualResetEventSlim _mreSecured = new System.Threading.ManualResetEventSlim(false);

        /// <summary>
        /// True if the public key for the remote has been set, otherwise False
        /// </summary>
        protected bool IsSecured => (_remotePublicKey != null) && (_messageSentCount > 0);

        /// <summary>
        /// Multi-cast delegate called after data is received and decrypted
        /// </summary>
        public Action<SecureClient, byte[]> SecureDataReceived { get; set; }
        ///// <summary>
        ///// Multi-cast delegate called after data is sent
        ///// </summary>
        //public Action<SecureClient, byte[]> SecureDataSent { get; set; }

        /// <summary>
        /// Calls the Connected multi-cast delegate after the connection has been secured
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote machine</param>
        protected override void OnConnected(IPEndPoint remoteEndPoint)
        {
            //base.OnConnected(remoteEndPoint); //don't call the base because we don't want to call Connected yet
            //IsConnectedResetEvent.Reset();
            if (_keys == null) //8.15.2017 - this has been called by the constructor, so don't do anything yet
                return;
            SendPublicKey(); //immediately send the public key
            //_mreSecured.Wait();
            if (!_mreSecured.Wait(10000))
                throw new TimeoutException("Unable to secure the connection");
        }
        /// <summary>
        /// Calls the Disconnected multi-cast delegate when the connection is terminated
        /// </summary>
        /// <param name="remoteEndPoint">The IPEndPoint of the remote machine</param>
        /// <param name="e">The Exception object if an exception occurred</param>
        protected override void OnDisconnected(IPEndPoint remoteEndPoint, Exception e)
        {
            _mreSecured.Reset();
            _remotePublicKey = null;
            base.OnDisconnected(remoteEndPoint, e);
        }
        /// <summary>
        /// Establishes a secure connection and calls the DataRecieved and MessageReceived multi-cast delegates
        /// </summary>
        /// <param name="bytes">The data received from the remote machine</param>
        /// <returns>The data received from the remote machine</returns>
        protected override byte[] OnDataReceived(byte[] bytes)
        {
            byte[] result = base.OnDataReceived(bytes);
            if (_remotePublicKey != null) //then we're secure
            {
                OnSecureDataReceived(result); //not sure we want to set the result to the unencrypted bytes
            }
            else
            {
                string json = string.Empty;
                
                try
                {
                    var json_str = bytes.FromBase64();
                    //var json_str = result.AsString();
                    json = json_str;//.FromBase64();
                    Console.WriteLine("Remote Public Key (json): " + json);
                    //if (TcpHelpers.IsBase64Encoded(json))
                    //{
                    //    System.Diagnostics.Debug.WriteLine("Remote public key is curiously Base64 encoded");
                    //    json = json.FromBase64(); //still not sure how this gets base64 encoded in the first place
                    //}
                    System.Diagnostics.Debug.WriteLine((IsServerSide ? "Server" : "Client") + " obtaining remote public key");
                    _remotePublicKey = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(json);

                    System.Diagnostics.Debug.WriteLine("{0} received {1}'s public Key", IsServerSide ? "Server" : "Client", IsServerSide ? "client" : "server");

                    //Complete the connection
                    base.OnConnected(Socket.RemoteEndPoint);
                    //IsConnectedResetEvent.Set();
                    //Connected?.Invoke(this, Socket.RemoteEndPoint);
                    _mreSecured.Set();
                }
                catch (Exception e)
                {
                    //ignore this message and move on
                    System.Diagnostics.Debug.WriteLine("SecureClient.OnDataReceived.Exception: " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("json = {0}", json);
                    //System.Diagnostics.Debug.WriteLine(e.ToString());
                    //System.Diagnostics.Debugger.Break();
                }
            }
            return result;
        }
        /// <summary>
        /// Calls SecureDataReceived with the decrypted data
        /// </summary>
        /// <param name="bytes">The secure data received from the remote</param>
        /// <returns>The decrypted data</returns>
        protected virtual byte[] OnSecureDataReceived(byte[] bytes)
        {
            System.Diagnostics.Debug.WriteLine((IsServerSide ? "Server" : "Client") + " decrypting " + bytes.Length + " bytes");
            var message = W.Encryption.RSAMethods.Decrypt(bytes.AsString(), _keys.PrivateKey); //msg should be base64 encoded (by a previous Encrypt call) going into _rsa.Decrypt
            var msgBytes = message.AsBytes();
            SecureDataReceived?.Invoke(this, msgBytes);// FormatReceivedMessage<byte[]>(bytes));
            return msgBytes;
        }
        //protected virtual byte[] OnSecureDataSent(byte[] bytes)
        //{
        //    //we can't decrypt the data because we don't have the private key

        //    try
        //    {
        //        var message = _rsa.Decrypt(bytes.AsString()); //msg should be base64 encoded (by a previous Encrypt call) going into _rsa.Decrypt
        //        if (message == null)
        //            return (byte[])Array.CreateInstance(typeof(byte), 0);

        //        var msgBytes = message.AsBytes();
        //        SecureDataSent?.Invoke(this, msgBytes);
        //        return msgBytes;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.WriteLine(e.ToString());
        //        return bytes;
        //    }
        //}
        //protected override void OnDataSent(byte[] bytes)
        //{
        //    //_messageSentCount += 1;
        //    base.OnDataSent(bytes);
        //    //if ((_messageSentCount > 1)) //skip the public key or if there are no listeners
        //    OnSecureDataSent(bytes);
        //}
        /// <summary>
        /// Called by Dispose.  Override to provide specific dispose functionality.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
        }

        /// <summary>
        /// Encrypts the data and sends it to the remote
        /// </summary>
        /// <param name="bytes">The data to encrypt and send</param>
        public override ulong Send(byte[] bytes)
        {
            if (_remotePublicKey == null)
                System.Diagnostics.Debugger.Break();
            //var bytes = base.FormatMessageToSend(message);
            System.Diagnostics.Debug.WriteLine((IsServerSide ? "Server" : "Client") + " encrypting " + bytes.Length + " bytes");
            var msgBytes = W.Encryption.RSAMethods.Encrypt(bytes.AsString(), (RSAParameters)_remotePublicKey).AsBytes(); //Base64 encodes the message while encrypting //msg should be base64 encoded going into _rsa.Encrypt
            return base.Send(msgBytes);
            //base.Send()
        }
        /// <summary>
        /// Constructs a new SecureBaseClient
        /// </summary>
        public SecureClient() : base()
        {
            //no need to compress because encrypted is barely compressable
            //Socket.UseCompression = true;
            _keys = W.Encryption.RSAMethods.CreateKeyPair(2048); //= new W.Encryption.RSA();
        }
        /// <summary>
        /// Constructs a new SecureBaseClient
        /// </summary>
        /// <param name="client">An existing connected TcpClient</param>
        /// <param name="rsa">The RSA object used to initialize this client</param>
        public SecureClient(TcpClient client) : base(client)
        {
            //no need to compress because encrypted is barely compressable
            //Socket.UseCompression = true;
            _keys = W.Encryption.RSAMethods.CreateKeyPair(2048);
            OnConnected(client.Client.RemoteEndPoint.As<IPEndPoint>());
        }
        private void SendPublicKey()
        {
            string publicKey = string.Empty;
            try
            {
                publicKey = Newtonsoft.Json.JsonConvert.SerializeObject(_keys.PublicKey);
                var bytes = publicKey.AsBase64().AsBytes();
                System.Diagnostics.Debug.WriteLine((IsServerSide ? "Server" : "Client") + " sending public key: " + bytes.Length.ToString() + " bytes");
                //if (TcpHelpers.IsBase64Encoded(bytes.AsString()))
                //    System.Diagnostics.Debugger.Break();
                //Socket.Send(bytes, false, true); //don't send exact (allow compression of the key)
                Socket.Send(bytes); //don't send exact (allow compression of the key)
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debugger.Break();
            }
        }
    }

    /// <summary>
    /// A generic form of XClientSecure
    /// </summary>
    public class SecureClient<TMessageType> : SecureClient, IMessageSocket<SecureClient<TMessageType>, TMessageType>
    {
        /// <summary>
        /// Called when a message has been received
        /// </summary>
        public Action<SecureClient<TMessageType>, TMessageType> MessageReceived { get; set; }
        //public Action<SecureClient<TMessageType>, TMessageType> MessageSent { get; set; }

        /// <summary>
        /// Calls SecureDataReceived with the decrypted data
        /// </summary>
        /// <param name="bytes">The secure data received from the remote</param>
        /// <returns>The decrypted data</returns>
        protected override byte[] OnSecureDataReceived(byte[] bytes)
        {
            var result = base.OnSecureDataReceived(bytes); //decrypts the data
            var message = StreamHelpers.FormatReceivedMessage<TMessageType>(result, SerializationSettings);
            MessageReceived?.Invoke(this, message);
            return result;
        }
        //protected override byte[] OnSecureDataSent(byte[] bytes)
        //{
        //    var result = base.OnSecureDataSent(bytes); //decrypted by calling the base implementation
        //    var message = TcpHelpers.FormatReceivedMessage<TMessageType>(result, SerializationSettings);
        //    MessageSent?.Invoke(this, message);
        //    return result;
        //}

        /// <summary>
        /// Sends the message to the remote
        /// </summary>
        /// <param name="message">The message to send to the remote</param>
        public ulong Send(TMessageType message)
        {
            var bytes = StreamHelpers.FormatMessageToSend<TMessageType>(message, SerializationSettings);
            return base.Send(bytes);
        }

        /// <summary>
        /// Constructs a new XClientSecure
        /// </summary>
        public SecureClient() : base()
        {
        }
        /// <summary>
        /// Constructs a new XClientSecure
        /// </summary>
        /// <param name="client">An existing connected TcpClient</param>
        /// <param name="rsa">The existing RSA encryption object</param>
        public SecureClient(TcpClient client) : base(client)
        {
        }
    }
}
