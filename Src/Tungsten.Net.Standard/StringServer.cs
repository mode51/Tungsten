using System.Net.Sockets;
using W.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// A Tcp server which sends and receives string data
    /// </summary>
    public class StringServer : Server<StringClient> { }
}