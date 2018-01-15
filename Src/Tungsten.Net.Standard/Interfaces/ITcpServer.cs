using System;
using System.Net;

namespace W.Net
{
    /// <summary>
    /// The interface for a tcp server
    /// </summary>
    public interface ITcpServer
    {
        /// <summary>
        /// The ITcpClient calls this method so the server knows that this particular client has disconnected
        /// </summary>
        /// <param name="client">The ITcpClient which has disconnected</param>
        /// <param name="remoteEndPoint">The remote IPEndPoint of the remote machine</param>
        /// <param name="e">An exception if one caused the disconnection</param>
        void DisconnectClient(ITcpClient client, IPEndPoint remoteEndPoint, Exception e = null);
    }
}