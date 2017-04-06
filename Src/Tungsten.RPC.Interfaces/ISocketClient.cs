using System;
using System.Net;
using System.Threading.Tasks;

namespace W.RPC
{
    /// <summary>
    /// Defines the interface for a Socket Client
    /// </summary>
    public interface ISocketClient
    {
        /// <summary>
        /// Raised when the Client has connected to the Server
        /// </summary>
        event Delegates.ConnectedDelegate Connected;

        /// <summary>
        /// Raised when the Client has disconnected from the Server
        /// </summary>
        event Delegates.DisconnectedDelegate Disconnected;

        /// <summary>
        /// True if the Client is currently connected to a Tungsten RPC Server, otherwise False
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        /// <param name="onConnection">This callback can be used instead of the Connected event</param>
        /// <param name="onException">This callback can be used to handle an exception, if one ocurrs</param>
        /// <returns>A bool specifying success/failure</returns>
        bool Connect(string remoteAddress, int remotePort, int msTimeout = Constants.DefaultConnectTimeout, Action<ISocketClient, IPAddress> onConnection = null, Action<Exception> onException = null);

        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        /// <param name="onConnection">This callback can be used instead of the Connected event</param>
        /// <param name="onException">This callback can be used to handle an exception, if one ocurrs</param>
        /// <returns>A bool specifying success/failure</returns>
        bool Connect(IPAddress remoteAddress, int remotePort, int msTimeout = Constants.DefaultConnectTimeout, Action<ISocketClient, IPAddress> onConnection = null, Action<Exception> onException = null);

        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server asynchronously
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        /// <returns>A Task which can be awaited</returns>
        Task ConnectAsync(string remoteAddress, int remotePort, int msTimeout = Constants.DefaultConnectTimeout);

        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server asynchronously
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        /// <returns>A Task which can be awaited</returns>
        Task ConnectAsync(IPAddress remoteAddress, int remotePort, int msTimeout = Constants.DefaultConnectTimeout);

        /// <summary>
        /// Disconnects from the Server
        /// </summary>
        void Disconnect();
    }
}
