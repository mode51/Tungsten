using System;
using System.Net;

namespace W.Net.Sockets
{
    /// <summary>
    /// Implemented by FormattedSocket; required by W.Net.Sockets.Server and W.Net.Sockets.SecureServer.
    /// </summary>
    public interface ISocket
    {
        /// <summary>
        /// The underlying Tungsten Socket
        /// </summary>
        Socket Socket { get; }
        /// <summary>
        /// Called when the client disconnects
        /// </summary>
        Action<object, IPEndPoint, Exception> Disconnected { get; set; }
    }
}