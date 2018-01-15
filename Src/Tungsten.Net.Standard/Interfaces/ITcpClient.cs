using System;
using System.Net;
using System.Threading.Tasks;

namespace W.Net
{
    /// <summary>
    /// The interface for a tcp client
    /// </summary>
    public interface ITcpClient
    {
        /// <summary>
        /// Allow the client to configure itself from an existing connection on the server
        /// </summary>
        /// <param name="server">The ITcpServer which instantiated this ITcpClient</param>
        /// <param name="existingSocket">The TcpClient which was allocated by the TcpServer</param>
        /// <param name="cpuProfile">The preferred CPU profile</param>
        void ConfigureForServerSide(ITcpServer server, System.Net.Sockets.Socket existingSocket, W.Threading.CPUProfileEnum cpuProfile);
        /// <summary>
        /// The IPEndPoint of the remote machine
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }
    }
}