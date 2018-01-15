using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using W;
using W.AsExtensions;
using W.Logging;
using W.Net;

namespace W.Net
{
    /// <summary>
    /// Adds logging via a SecureStringClient
    /// </summary>
    public class SecureStringClientLogger : W.Logging.CustomLogger
    {
        private W.Net.SecureClient _client;

        /// <summary>
        /// The IPEndPoint for the remote server
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// Set to a non-Success value if a connection could not be established 
        /// </summary>
        public System.Net.Sockets.SocketError SocketError { get; private set; }

        /// <summary>
        /// Log a message to the custom logger
        /// </summary>
        /// <param name="category">The log message category</param>
        /// <param name="message">The log message</param>
        protected override void LogMessage(W.Logging.Log.LogMessageCategory category, string message)
        {
            if (_client?.IsConnected ?? false) //because the handler is added before the client connects
            {
                message = FormatLogMessage(category, message);
                var bytes = message.AsBytes();
                _client.Send(bytes);
            }
        }
        /// <summary>
        /// Disposes the CustomLogger, releases resources and supresses the finalizer
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
            if (_client != null)
            {
                if (_client.IsConnected)
                    _client.Disconnect();
                _client = null;
            }
        }
        /// <summary>
        /// Constructs a new SecureStringLogger
        /// </summary>
        /// <param name="ipAddress">The IP address for the server</param>
        /// <param name="port">The Port for the server</param>
        /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
        public SecureStringClientLogger(IPAddress ipAddress, int port, bool addTimestamp = true) : this(new IPEndPoint(ipAddress, port), addTimestamp)
        {
        }
        /// <summary>
        /// Constructs a new SecureStringLogger
        /// </summary>
        /// <param name="serverEndPoint">The IP address for the server</param>
        /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
        public SecureStringClientLogger(IPEndPoint serverEndPoint, bool addTimestamp = true) : base(serverEndPoint.ToString(), addTimestamp)
        {
            RemoteEndPoint = serverEndPoint;
            _client = new SecureClient();

            if (!_client.Connect(serverEndPoint))//.ContinueWith(task =>
            {
                //if (!_client.IsConnected)
                {
                    this.SocketError = System.Net.Sockets.SocketError.ConnectionRefused;
                    System.Diagnostics.Debug.WriteLine("SecureStringLogger failed to connect to the server.");
                    return;
                }
            }//);
        }
    }
}
