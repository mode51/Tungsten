using System;
using System.IO.Pipes;

namespace W.IO.Pipes
{
    /// <summary>
    /// Listens for a single PipeClient to connect
    /// </summary>
    /// <remarks>Pipes need to have a distinct PipeServer for each client connection.  See PipeHost to support multiple client connections./></remarks>
    public class PipeServer : PipeClient
    {
        private bool _useCompression = false;

        /// <summary>
        /// Raised when the server receives a message from the client
        /// </summary>
        public event Action<PipeServer, byte[]> BytesReceived;

        private void Listen()
        {
            try
            {
                while (Stream?.IsConnected ?? false)
                {
                    var bytes = WaitForMessageAsync(_useCompression, 100).Result;
                    if (bytes != null)
                        BytesReceived?.Invoke(this, bytes);
                }
            }
#if NET45
            catch (System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
            }
#endif
            finally
            {
                RaiseDisconnected();
            }
        }
        /// <summary>
        /// Constructs a new PipeServer
        /// </summary>
        /// <param name="stream">The NamedPipeClientServer stream on which to listen for client messages</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        public PipeServer(PipeStream stream, bool useCompression) : base(stream) { _useCompression = useCompression; }

        /// <summary>
        /// Creates a new PipeServer instance, initializing the NamedPipeServerStream instance with the specified parameters
        /// </summary>
        /// <param name="pipeName">The name of the pipe to create.  The name must follow typical Named Pipe naming rules.</param>
        /// <param name="maxConnections">The maximum number of connections.  This is often limited by your platform.</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        /// <returns>A CallResult instance containing success or failure of the call, an exception if one occurred and the resulting PipeServer.  Note that if the call fails, the Result member will be null.</returns>
        public static CallResult<PipeServer> CreateServer(string pipeName, int maxConnections, bool useCompression)
        {
            var result = new CallResult<PipeServer>();
            Helpers.CreateServer(pipeName, maxConnections, s =>
            {
                result.Result = new PipeServer(s, useCompression);
            }, s =>
            {
                result.Result.Listen();
            });
            return result;
        }
    }

    /// <summary>
    /// Listens for a single PipeClient to connect
    /// </summary>
    /// <remarks>Pipes need to have a distinct PipeServer for each client connection.  See PipeHost to support multiple client connections./></remarks>
    public class PipeServer<TType> : PipeClient where TType : class
    {
        private bool _useCompression = false;

        /// <summary>
        /// Raised when the server receives a message from the client
        /// </summary>
        public event Action<PipeServer<TType>, TType> MessageReceived;

        private void Listen()
        {
            try
            {
                while (Stream?.IsConnected ?? false)
                {
                    var bytes = WaitForMessageAsync<TType>(_useCompression, 100).Result;
                    if (bytes != null)
                        MessageReceived?.Invoke(this, bytes);
                }
            }
            finally
            {
                RaiseDisconnected();
            }
        }

        /// <summary>
        /// Constructs a new PipeServer
        /// </summary>
        /// <param name="stream">The NamedPipeClientServer stream on which to listen for client messages</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        public PipeServer(PipeStream stream, bool useCompression) : base(stream) { _useCompression = useCompression; }

        /// <summary>
        /// Creates a new PipeServer instance, initializing the NamedPipeServerStream instance with the specified parameters
        /// </summary>
        /// <param name="pipeName">The name of the pipe to create.  The name must follow typical Named Pipe naming rules.</param>
        /// <param name="maxConnections">The maximum number of connections.  This is often limited by your platform.</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        /// <returns>A CallResult containing success or failure of the call, an exception if one occurred and the resulting PipeServer.  Note that if the call fails, the Result member will be null.</returns>
        public static CallResult<PipeServer<TType>> CreateServer(string pipeName, int maxConnections, bool useCompression)
        {
            var result = new CallResult<PipeServer<TType>>();
            Helpers.CreateServer(pipeName, maxConnections, s =>
            {
                result.Result = new PipeServer<TType>(s, useCompression);
            }, s =>
            {
                result.Result.Listen();
            });
            return result;
        }
    }
}
