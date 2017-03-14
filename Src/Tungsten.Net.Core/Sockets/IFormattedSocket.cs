using System;

namespace W.Net.Sockets
{
    /// <summary>
    /// Implemented by FormattedSocket; required by W.Net.Sockets.Server and W.Net.Sockets.SecureServer.
    /// </summary>
    public interface IFormattedSocket
    {
        /// <summary>
        /// The underlying Tungsten Socket
        /// </summary>
        Socket Socket { get; }
        /// <summary>
        /// Called when the client disconnects
        /// </summary>
        Action<object, Exception> Disconnected { get; set; }
    }
}