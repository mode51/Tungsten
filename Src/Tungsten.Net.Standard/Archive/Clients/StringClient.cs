using System.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// A TCP client to be used with W.Net.Server which transmits and receives strings
    /// </summary>
    public class StringClient : Client<string>
    {
        /// <summary>
        /// Constructs a new Stringclient
        /// </summary>
        public StringClient() : base() { }
        /// <summary>
        /// Constructs a new StringClient
        /// </summary>
        /// <param name="client"></param>
        public StringClient(TcpClient client) : base(client) { }
    }
}