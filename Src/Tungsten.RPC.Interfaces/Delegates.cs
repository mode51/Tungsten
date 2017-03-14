using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using W.RPC.Interfaces;

namespace W.RPC
{
    /// <summary>
    /// Delegates used by Tungsten.RPC.Server and Tungsten.RPC.Client
    /// </summary>
    public class Delegates
    {
        public delegate void ConnectionTimeoutDelegate(ISocketClient client, IPAddress remoteAddress);
        /// <summary>
        /// Delegate to notify the programmer when the Client has connected to the Server
        /// </summary>
        /// <param name="client">A reference to the Client which has connected</param>
        /// <param name="remoteAddress">The IP Address of the Server</param>
        public delegate void ConnectedDelegate(ISocketClient client, IPAddress remoteAddress);
        /// <summary>
        /// Delegate to notify the programmer when the Client has disconnected from the Server
        /// </summary>
        /// <param name="client">A reference to the Client which has disconnected</param>
        /// <param name="exception">Specifies the exception which caused the disconnection.  If no exception ocurred, this value is null.</param>
        public delegate void DisconnectedDelegate(ISocketClient client, Exception exception);
    }
}
