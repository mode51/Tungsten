using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// A secure TCP client which connects to a SecureServer and transceives strings.  Assymetric encryption is used to secure the transmitted data.
    /// </summary>
    public class SecureStringClient : SecureClient<string>
    {
        /// <summary>
        /// Constructs a new SecureStringClient
        /// </summary>
        public SecureStringClient() : base() { }
        /// <summary>
        /// Constructs a new SecureStringClient (used by SecureServer)
        /// </summary>
        /// <param name="client">An existing connected TcpClient</param>
        /// <param name="rsa">An existing instance of RSA to be used for encryption</param>
        public SecureStringClient(TcpClient client, Encryption.RSA rsa) : base(client, rsa) { }
    }
}
