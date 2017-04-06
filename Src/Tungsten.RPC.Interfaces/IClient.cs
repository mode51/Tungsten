using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace W.RPC
{
    /// <summary>
    /// Defines the interface of an RPC client
    /// </summary>
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
