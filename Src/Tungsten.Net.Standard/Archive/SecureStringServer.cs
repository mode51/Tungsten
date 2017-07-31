using W.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// A Tcp server which uses assymetric encryption to send/receive string data
    /// </summary>
    public class SecureStringServer : SecureServer<SecureClient<string>> { }
}