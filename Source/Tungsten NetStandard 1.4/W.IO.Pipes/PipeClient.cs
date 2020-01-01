using System;
using System.Threading.Tasks;

namespace W.IO.Pipes
{

    /// <summary>
    /// A pipe client.  This class sends and receives byte arrays.
    /// </summary>
    public class PipeClient : PipeClient<byte[]> { }

    /// <summary>
    /// The generic version of PipeClient.  This class expects all messages to be of the specified type.
    /// </summary>
    /// <typeparam name="TMessage">The message type to send and receive</typeparam>
    public class PipeClient<TMessage> : Pipe<TMessage>
    {
        /// <summary>
        /// Raised when a connection attemp fails
        /// </summary>
        public event Action<Pipe, Exception> ConnectionFailed;
        /// <summary>
        /// Raised when a connection attempt succeeds
        /// </summary>
        public event Action<Pipe> Connected;
        /// <summary>
        /// Attempts to connect the pipe to a pipe server
        /// </summary>
        /// <param name="serverName">The name or ip of the machine hosting the server pipe</param>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="tokenImpersonationLevel">The impersonation type for the pipe to use</param>
        /// <param name="msTimeout">The maximum amount of time, in milliseconds, to wait for the server to connect</param>
        /// <returns>True if a connection was established, otherwise False</returns>
        public bool Connect(string serverName, string pipeName, System.Security.Principal.TokenImpersonationLevel tokenImpersonationLevel, int msTimeout)
        {
            Stream = Helpers.CreateClientAndConnect(serverName, pipeName, tokenImpersonationLevel, msTimeout, out Exception e);
            if (e != null)
                ConnectionFailed?.Invoke(this, e);
            else
                Connected?.Invoke(this);
            return (Stream != null);
        }
        /// <summary>
        /// Attempts to asynchronously connect the pipe to a pipe server
        /// </summary>
        /// <param name="serverName">The name or ip of the machine hosting the server pipe</param>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="tokenImpersonationLevel">The impersonation type for the pipe to use</param>
        /// <param name="msTimeout">The maximum amount of time, in milliseconds, to wait for the server to connect</param>
        /// <returns>True if a connection was established, otherwise False</returns>
        public async Task<bool> ConnectAsync(string serverName, string pipeName, System.Security.Principal.TokenImpersonationLevel tokenImpersonationLevel, int msTimeout)
        {
            return await Task.Run(() =>
            {
                return Connect(serverName, pipeName, tokenImpersonationLevel, msTimeout);
            });
        }

        /// <summary>
        /// Creates a new PipeClient and attempts to connect the pipe to a pipe server
        /// </summary>
        /// <param name="serverName">The name or ip of the machine hosting the server pipe</param>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="tokenImpersonationLevel">The impersonation type for the pipe to use</param>
        /// <param name="msTimeout">The maximum amount of time, in milliseconds, to wait for the server to connect</param>
        /// <returns>True if a connection was established, otherwise False</returns>
        public static PipeClient Create(string serverName, string pipeName, System.Security.Principal.TokenImpersonationLevel tokenImpersonationLevel, int msTimeout)
        {
            var result = new PipeClient();
            if (!result.Connect(serverName, pipeName, tokenImpersonationLevel, msTimeout))
            {
                result.Dispose();
                result = null;
            }
            return result;
        }
        /// <summary>
        /// Creates a new PipeClient and attempts to asynchronously connect the pipe to a pipe server
        /// </summary>
        /// <param name="serverName">The name or ip of the machine hosting the server pipe</param>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="tokenImpersonationLevel">The impersonation type for the pipe to use</param>
        /// <param name="msTimeout">The maximum amount of time, in milliseconds, to wait for the server to connect</param>
        /// <returns>True if a connection was established, otherwise False</returns>
        public static async Task<Pipe> CreateAsync(string serverName, string pipeName, System.Security.Principal.TokenImpersonationLevel tokenImpersonationLevel, int msTimeout)
        {
            var result = new PipeClient();
            var connected = await result.ConnectAsync(serverName, pipeName, tokenImpersonationLevel, msTimeout);
            if (!connected)
            {
                result.Dispose();
                result = null;
            }
            return result;
        }
    }
}