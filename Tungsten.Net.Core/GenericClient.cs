using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using W.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// Extends SecureStringClient by supporting generics
    /// </summary>
    /// <typeparam name="TType">The type to transmit and receive</typeparam>
    public class GenericClient<TType> : W.Net.SecureStringClient
    {
        /// <summary>
        /// Called when data has been received and formatted
        /// </summary>
        public Action<object, TType> GenericMessageReceived { get; set; }

        /// <summary>
        /// Serialize an object and send it to the remote
        /// </summary>
        /// <param name="item">The item to serialize and transmit</param>
        public void Send(TType item)
        {
            var data = item.AsXml<TType>();
            base.Send(data);
        }

        /// <summary>
        /// Constructs a GenericClient
        /// </summary>
        public GenericClient() : base()
        {
            HookMessageReceived();
        }

        /// <summary>
        /// Constructs a new GenericClient
        /// </summary>
        /// <param name="client">An existing connected TcpClient</param>
        /// <param name="rsa">An existing instance of RSA to be used for encryption</param>
        public GenericClient(System.Net.Sockets.TcpClient client, W.Encryption.RSA rsa) : base(client, rsa)
        {
            HookMessageReceived();
        }

        private void HookMessageReceived()
        {
            MessageReceived += (o, s) =>
            {
                var item = s.FromXml<TType>();
                this.GenericMessageReceived?.Invoke(this, item);
            };
        }
    }

    /// <summary>
    /// Extends SecureServer by supporting generics
    /// </summary>
    /// <typeparam name="TType">The type to transmit and receive</typeparam>
    public class GenericServer<TType> : W.Net.Sockets.SecureServer<GenericClient<TType>>
    {
    }
}

