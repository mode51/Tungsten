using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;
using W.AsExtensions;
using W.Encryption;

namespace W.Net.Alpha
{
    ///// <summary>
    ///// The type of secure message
    ///// </summary>
    //public enum MessageTypeEnum
    //{
    //    /// <summary>
    //    /// Unspecified message data
    //    /// </summary>
    //    None = 0,
    //    /// <summary>
    //    /// Configuration related data
    //    /// </summary>
    //    Config = 1,
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    Data = 2
    //}
    ///// <summary>
    ///// The message class used by SecureClient and SecureServer
    ///// </summary>
    //public class SecureMessage
    //{
    //    /// <summary>
    //    /// The type of secure message
    //    /// </summary>
    //    public MessageTypeEnum Type;
    //    /// <summary>
    //    /// The message data
    //    /// </summary>
    //    public byte[] Data;
    //}

    /// <summary>
    /// The shared implementation for SecureClient and SecureServer
    /// </summary>
    /// <typeparam name="TType">The inherting type</typeparam>
    /// <typeparam name="TSocket">The type of SocketBase (client or server)</typeparam>
    /// <typeparam name="TMessage">The message Type</typeparam>
    public partial class SecureSocketBase<TType, TSocket, TMessage> : IDisposable
        where TType : SecureSocketBase<TType, TSocket, TMessage>
        where TSocket : SocketBase<TSocket>
        where TMessage : class, new()
    {
        /// <summary>
        /// Used to configure encryption and encrypt/decrypt data
        /// </summary>
        protected AssymetricEncryption Encryption = null;
        /// <summary>
        /// The key size to use when encrypting/decrypting data
        /// </summary>
        protected int KeySize;
        /// <summary>
        /// A SpinLock Disposer which can be used to lock cleanup resources 
        /// </summary>
        protected W.Disposer Disposer = new Disposer();
        /// <summary>
        /// The underlying socket on which to send/read data
        /// </summary>
        public TSocket Socket = default(TSocket);

        /// <summary>
        /// Called when the client connects to the server
        /// </summary>
        public event Action<TType> Connected;// { get; set; }
        /// <summary>
        /// Called when the client disconnects from the server
        /// </summary>
        public event Action<TType> Disconnected;// { get; set; }
        ///// <summary>
        ///// Raised when a message is received from a client
        ///// </summary>
        //public event Action<TType, byte[]> BytesReceived;// { get; set; }
        /// <summary>
        /// Raised when a message is received from a client
        /// </summary>
        public event Action<TType, TMessage> MessageReceived;
        /// <summary>
        /// A unique identifier for this SecureBase instance
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();
        /// <summary>
        /// True if the socket is currently connected, otherwise False
        /// </summary>
        public bool IsConnected => Socket.Socket.Connected;
        private bool Secure()
        {
            var result = Encryption.ExchangeKeys((myPublicKey) =>
            {
                var bytes = SerializationMethods.Serialize(myPublicKey).AsBytes();
                //var msg = new SecureMessage() { Type = MessageTypeEnum.Config, Data = bytes };
                //var msgData = Newtonsoft.Json.JsonConvert.SerializeObject(msg).AsBytes();
                byte[] response;
                if (SocketExtensions.SendAndWaitForResponse(Socket.Socket, ref bytes, out response, 5000))
                {
                    //var responseMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<SecureMessage>(response.AsString());
                    //RSAParameters? remotePublicKey = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(responseMsg.Data.AsString());
                    RSAParameters? remotePublicKey = SerializationMethods.Deserialize<RSAParameters>(ref response);
                    if (remotePublicKey != null)
                    {
                        //Socket.Connected -= HandleSocketConnected; //remove the handler
                        RaiseConnected((TType)this); //notify the owner
                        return remotePublicKey;
                    }
                }
                return null;
            });
            return result;
        }

        /// <summary>
        /// Raise the Connected event
        /// </summary>
        /// <param name="sender">The object raising the event</param>
        protected void RaiseConnected(TType sender) { Connected?.Invoke(sender); }
        /// <summary>
        /// Raise the Disconnected event
        /// </summary>
        /// <param name="sender">The object raising the event</param>
        protected void RaiseDisconnected(TType sender) { Disconnected?.Invoke(sender); }
        //protected void RaiseBytesReceived(TType sender, byte[] bytes) { BytesReceived?.Invoke(sender, bytes); }
        /// <summary>
        /// Raise the MessageReceived event
        /// </summary>
        /// <param name="sender">The object raising the event</param>
        /// <param name="message">The related SecureMessage object</param>
        protected void RaiseMessageReceived(TType sender, TMessage message) { MessageReceived?.Invoke(sender, message); }
        /// <summary>
        /// Executes the action from within the SpinLock Disposer
        /// </summary>
        /// <param name="action">The action to run</param>
        protected void Cleanup(Action action)
        {
            Disposer.Cleanup(() =>
            {
                action.Invoke();
            });
        }
        /// <summary>
        /// Deciphers the received bytes, then deserializes them into a SecureMessage
        /// </summary>
        /// <param name="s">The socket which received the data</param>
        /// <param name="bytes">The encrypted data</param>
        protected virtual void OnBytesReceived(TSocket s, byte[] bytes)
        {
            Encryption.Decrypt(ref bytes);
            var json = bytes.AsString();
            var message = SerializationMethods.Deserialize<TMessage>(json);
            RaiseMessageReceived((TType)this, message);
        }
        /// <summary>
        /// Serializes the message to bytes, encrypts the bytes, then writes them to the underlying socket
        /// </summary>
        /// <param name="message">The message to encrypt and write to the socket</param>
        protected virtual void OnWrite(TMessage message)
        {
            var json = SerializationMethods.Serialize(message);
            var bytes = json.AsBytes();
            var cipher = Encryption.Encrypt(ref bytes);
            Socket.Write(bytes);
        }
        /// <summary>
        /// Disconnects the underlying socket and releases related resources
        /// </summary>
        protected virtual void OnDisconnect()
        {
            Cleanup(() =>
            {
                if (Socket != null)
                {
                    Socket.Dispose();
                    Encryption = null;
                    Socket = null;
                    RaiseDisconnected((TType)this);
                }
            });
        }
        /// <summary>
        /// Initialize the underlying socket
        /// </summary>
        protected virtual void Connect(Socket socket = null)
        {
            Encryption = new AssymetricEncryption(KeySize);
            Socket = Activator.CreateInstance<TSocket>();
            if (socket != null)
                Socket.InitializeConnection(socket); //clients call this upon Connect, servers need to assign it here
            Socket.UseCompression = false; //you can't compress encrypted data
            Socket.Connected += s => Secure();
            Socket.Disconnected += s => OnDisconnect();
            Socket.BytesReceived += OnBytesReceived;
            //(s, bytes) =>
            //{
            //    if (Encryption.RemotePublicKey != null)
            //    {
            //        Encryption.Decrypt(ref bytes);
            //        BytesReceived?.Invoke((TType)this, bytes);
            //    }
            //};
        }

        /// <summary>
        /// Writes a SecureMessage to the underlying socket
        /// </summary>
        /// <param name="message">The message to send to the remote socket</param>
        public void Write(TMessage message)
        {
            OnWrite(message);
        }
        /// <summary>
        /// Disconnects the underlying socket and releases related resources
        /// </summary>
        public void Disconnect()
        {
            OnDisconnect();
        }
        /// <summary>
        /// Disposes the object and releases resources
        /// </summary>
        public void Dispose()
        {
            Cleanup(() => Disconnect());
        }

        /// <summary>
        /// Constructs a new SecureBase
        /// </summary>
        public SecureSocketBase() : this(2048) { }
        /// <summary>
        /// Constructs a new SecureBase
        /// </summary>
        /// <param name="keySize">The key size to use for encryption</param>
        public SecureSocketBase(int keySize)
        {
            KeySize = keySize;
        }
    }
}
