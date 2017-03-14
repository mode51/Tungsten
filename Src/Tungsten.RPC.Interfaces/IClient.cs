using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.RPC
{
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
    public interface IClient : ISocketClient
    {
        /// <summary>
        /// Calls a method on the Tungsten RPC Server
        /// </summary>
        /// <param name="methodName">The name of the method to call, including full namespace and class name</param>
        /// <param name="onResponse">A callback where </param>
        /// <typeparam name="T">The result from the call</typeparam>
        /// <returns>A ManualResetEvent which can be joined (with or without a timeout) to block the calling thread until a respoonse is received.</returns>
        ManualResetEvent MakeRPCCall<T>(string methodName, Action<T> onResponse);

        /// <summary>
        /// Calls a method on the Tungsten RPC Server
        /// </summary>
        /// <param name="methodName">The name of the method to call, including full namespace and class name</param>
        /// <param name="onResponse">A callback where </param>
        /// <param name="args">Optional parameters to be passed into the method</param>
        /// <typeparam name="T">The result from the call</typeparam>
        /// <returns>A ManualResetEvent which can be joined (with or without a timeout) to block the calling thread until a respoonse is received.</returns>
        ManualResetEvent MakeRPCCall<T>(string methodName, Action<T> onResponse, params object[] args);

        /// <summary>
        /// Not sure I should keep this method.  Shouldn't all RPC calls have a result?  Otherwise, the client wouldn't know if it succeeded.
        /// </summary>
        /// <param name="onResponse"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        ManualResetEvent MakeRPCCall(string methodName, Action onResponse, params object[] args);

        /// <summary>Disconnects from a Tungsten RPC Server if connected.  Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        void Dispose();
    }
}
