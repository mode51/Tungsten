using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace W.RPC.Interfaces
{
    /// <summary>
    /// Defines the interface for an RPC Server
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Starts the server on the specified network IP address and port
        /// </summary>
        /// <param name="address">The IP address on which to listen</param>
        /// <param name="port">The port on which to listen</param>
        void Start(IPAddress address, int port);
        /// <summary>
        /// Stops listening
        /// </summary>
        void Stop();
        /// <summary>
        /// Disposes the Server and releases resources
        /// </summary>
        void Dispose();
    }
}
