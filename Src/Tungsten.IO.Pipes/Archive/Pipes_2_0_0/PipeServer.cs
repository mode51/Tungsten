using System;
using System.Collections.Generic;
using System.IO.Pipes;

namespace W.IO.Pipes
{
    /// <summary>
    /// Listens for a single PipeClient to connect
    /// </summary>
    /// <remarks>Pipes need to have a distinct PipeServer for each client connection.  See PipeHost to support multiple client connections.</remarks>
    public class PipeServerBase : PipeClient
    {
        private bool _listenOk = true;
        private bool _exited = false;
        private PipeHostServer _host = null;

        /// <summary>
        /// If True, data will be compressed before sending and decompressed upon reception
        /// </summary>
        protected bool UseCompression { get; set; }

        internal int ServerIndex { get; set; }

        /// <summary>
        /// The listen loop
        /// </summary>
        protected void Listen()
        {
            try
            {
                //while ((_host?.ListenOk ?? false) && _listenOk && (Stream?.IsConnected ?? false))
                while (_listenOk && (Stream?.IsConnected ?? false))
                {
                    OnListenOnce();
                }
            }
            catch (AggregateException)
            {
                //ingore 
            }
            catch (ObjectDisposedException)
            {
                //ignore
            }
#if NET45
            catch (System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
            }
#endif
            finally
            {
                //if (!_suppressDisconnectedEvent)
                //    RaiseDisconnected();
                _exited = true;
                _listenOk = false; //it can be true if the stream disconnected
            }
        }
        /// <summary>
        /// Listens for data on the pipe for a short amount of time
        /// </summary>
        protected virtual void OnListenOnce()
        {
        }

        /// <summary>
        /// Disposes the PipeServer and releases resources
        /// </summary>
        public new void Dispose()
        {
            _listenOk = false;
            while (!_exited)
                System.Threading.Thread.Sleep(1);
            base.Dispose();
        }
        /// <summary>
        /// Constructs a new PipeServer
        /// </summary>
        /// <param name="stream">The NamedPipeClientServer stream on which to listen for client messages</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        public PipeServerBase(PipeStream stream, bool useCompression) : base(stream)
        {
            UseCompression = useCompression;
        }

        /// <summary>
        /// Constructs a new PipeServer
        /// </summary>
        /// <param name="host">The PipeHost hosting the PipeServer</param>
        /// <param name="stream">The NamedPipeClientServer stream on which to listen for client messages</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        internal PipeServerBase(PipeHostServer host, PipeStream stream, bool useCompression) : this (stream, useCompression)
        {
            _host = host;
        }
    }

    /// <summary>
    /// Listens for a single PipeClient to connect
    /// </summary>
    /// <remarks>Pipes need to have a distinct PipeServer for each client connection.  See PipeHost to support multiple client connections./></remarks>
    public partial class PipeServer : PipeServerBase
    {
        /// <summary>
        /// Raised when the server receives a message from the client
        /// </summary>
        public event Action<PipeServer, byte[]> BytesReceived;

        /// <summary>
        /// Listens for data on the pipe for a short amount of time
        /// </summary>
        protected override void OnListenOnce()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var bytes = WaitForMessageAsync(UseCompression, 100).Result;
            Console.WriteLine($"{ServerIndex}. OnListenOnce: Elapsed = {sw.ElapsedMilliseconds}ms");
            if (bytes != null)
                BytesReceived?.Invoke(this, bytes);
        }
        /// <summary>
        /// Constructs a new PipeServer
        /// </summary>
        /// <param name="stream">The NamedPipeClientServer stream on which to listen for client messages</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        public PipeServer(PipeStream stream, bool useCompression) : base(stream, useCompression) { }

        /// <summary>
        /// Creates a new PipeServer instance, initializing the NamedPipeServerStream instance with the specified parameters
        /// </summary>
        /// <param name="pipeName">The name of the pipe to create.  The name must follow typical Named Pipe naming rules.</param>
        /// <param name="maxConnections">The maximum number of connections.  This is often limited by your platform.</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        /// <param name="onConnected">Action to call after a client has connected to the server</param>
        /// <returns>A CallResult instance containing success or failure of the call, an exception if one occurred and the resulting PipeServer.  Note that if the call fails, the Result member will be null.</returns>
        public static CallResult<PipeServer> CreateServer(string pipeName, int maxConnections, bool useCompression, Action<PipeServer> onConnected = null)
        {
            var result = new CallResult<PipeServer>();
            Helpers.CreateServer(pipeName, maxConnections, s =>
            {
                result.Result = new PipeServer(s, useCompression);
            }, s =>
            {
                result.Result.Listen();
                onConnected?.Invoke(result.Result);
            });
            return result;
        }
    }

    /// <summary>
    /// Listens for a single PipeClient to connect
    /// </summary>
    /// <remarks>Pipes need to have a distinct PipeServer for each client connection.  See PipeHost to support multiple client connections./></remarks>
    public class PipeServer<TType> : PipeServerBase where TType : class
    {
        /// <summary>
        /// Raised when the server receives a message from the client
        /// </summary>
        public event Action<PipeServer<TType>, TType> MessageReceived;

        /// <summary>
        /// Listens for data on the pipe for a short amount of time
        /// </summary>
        protected override void OnListenOnce()
        {
            var bytes = WaitForMessageAsync<TType>(UseCompression, 100).Result;
            if (bytes != null)
                MessageReceived?.Invoke(this, bytes);
        }

        /// <summary>
        /// Constructs a new PipeServer
        /// </summary>
        /// <param name="stream">The NamedPipeClientServer stream on which to listen for client messages</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        public PipeServer(PipeStream stream, bool useCompression) : base(stream, useCompression) { }

        /// <summary>
        /// Creates a new PipeServer instance, initializing the NamedPipeServerStream instance with the specified parameters
        /// </summary>
        /// <param name="pipeName">The name of the pipe to create.  The name must follow typical Named Pipe naming rules.</param>
        /// <param name="maxConnections">The maximum number of connections.  This is often limited by your platform.</param>
        /// <param name="useCompression">If True, data will be compressed before sending and decompressed upon reception</param>
        /// <param name="onConnected">Action to call after a client has connected to the server</param>
        /// <returns>A CallResult containing success or failure of the call, an exception if one occurred and the resulting PipeServer.  Note that if the call fails, the Result member will be null.</returns>
        public static CallResult<PipeServer<TType>> CreateServer(string pipeName, int maxConnections, bool useCompression, Action<PipeServer<TType>> onConnected = null)
        {
            var result = new CallResult<PipeServer<TType>>();
            Helpers.CreateServer(pipeName, maxConnections, s =>
            {
                result.Result = new PipeServer<TType>(s, useCompression);
            }, s =>
            {
                result.Result.Listen();
                onConnected?.Invoke(result.Result);
            });
            return result;
        }
    }
}
