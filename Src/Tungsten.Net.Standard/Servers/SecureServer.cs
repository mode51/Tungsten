using System;
using System.Net.Sockets;
using W.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// A secure TCP server which hosts SecureClient connections.  Assymetric encryption is used to secure the transmitted data.
    /// </summary>
    public class SecureServer<TClientType> : Server<TClientType> where TClientType : class, IDataSocket
    {
        private W.Encryption.RSA _rsa = new W.Encryption.RSA();
        /// <summary>
        /// Creates a new instance of TClientType
        /// </summary>
        /// <param name="client">A TcpClient with an established connection</param>
        /// <returns>The new instance of TClientType</returns>
        protected override TClientType CreateClient(TcpClient client)
        {
            System.Diagnostics.Debug.WriteLine("Server creating new client");
            var result = (TClientType)Activator.CreateInstance(typeof(TClientType), client, _rsa);
            System.Diagnostics.Debug.WriteLine("Server created new client");
            return result;
        }
    }
    public class SecureEchoServer : SecureEchoServer<SecureClient<string>> { }
    public class SecureEchoServer<TClientType> : SecureServer<TClientType> where TClientType : Client
    {
        public SecureEchoServer()
        {
            ClientConnected += (client) =>
            {
                client.DataReceived += (c, data) =>
                {
                    var message = data.AsString();
                    if (!string.IsNullOrEmpty(message))
                    {
                        c.Send(message.ToUpper().AsBytes());
                    }
                };
            };
        }
    }
}
