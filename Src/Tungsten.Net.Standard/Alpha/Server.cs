using System;
using W.AsExtensions;
using W.DelegateExtensions;

//This Server class is only used on the server side.  It is created by Host.

namespace W.Net.Alpha
{
    /// <summary>
    /// Base class for a server-side client
    /// </summary>
    public class ServerBase<TServer> : SocketBase<TServer> where TServer : ServerBase<TServer>
    {
        ///// <summary>
        ///// Disconnects from the Client
        ///// </summary>
        //public new void Disconnect()
        //{
        //    base.Disconnect();
        //}
        /// <summary>
        /// Constructs a new ServerBase
        /// </summary>
        internal ServerBase()
        {
            IsServerSide = true;
        }
    }

    /// <summary>
    /// Network server which can send/receive arrays of bytes
    /// </summary>
    public class Server : ServerBase<Server>
    {
        public Server() : base()
        {
        }
    }
    /// <summary>
    /// Network server which can send/receive serialiable objects
    /// </summary>
    /// <typeparam name="TMessage">The Type of object which will be send and received</typeparam>
    public class Server<TMessage> : ServerBase<Server<TMessage>> where TMessage: class, new()
    {
        /// <summary>
        /// Called when a new message has been received
        /// </summary>
        public event Action<Server<TMessage>, TMessage> MessageReceived;// { get; set; }

        /// <summary>
        /// Calls the BytesReceived delegate
        /// </summary>
        /// <param name="bytes">The bytes received</param>
        protected override void OnBytesReceived(byte[] bytes)
        {
            base.OnBytesReceived(bytes);
            var json = bytes.AsString();
            var message = Newtonsoft.Json.JsonConvert.DeserializeObject<TMessage>(json);
            //MessageReceived?.BeginInvoke(this, message, ar => MessageReceived.EndInvoke(ar), null);
            MessageReceived?.Raise(this, message);
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="message">The object to be serialized and written to the network</param>
        public void Write(TMessage message)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            var bytes = json.AsBytes();
            base.Write(bytes);
        }
        /// <summary>
        /// Constructs a new Server
        /// </summary>
        public Server() : base()
        {
        }
    }
}
