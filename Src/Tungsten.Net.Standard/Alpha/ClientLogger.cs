using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using W;
using W.AsExtensions;
using W.Logging;

namespace W.Net.Alpha
{
    /// <summary>
    /// A network logger for W.Logging
    /// </summary>
    public class ClientLogger : W.Logging.CustomLogger
    {
        private Client _client;

        /// <summary>
        /// Log a message to the custom logger
        /// </summary>
        /// <param name="category">The log message category</param>
        /// <param name="message">The log message</param>
        protected override void LogMessage(Log.LogMessageCategory category, string message)
        {
            message = FormatLogMessage(category, message);
            _client.Write(message.AsBytes());
        }
        /// <summary>
        /// Disposes the CustomLogger, releases resources and supresses the finalizer
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
        /// <summary>
        /// Constructs a new ClientLogger
        /// </summary>
        /// <param name="address">The IP address of the server</param>
        /// <param name="port">The port on which the server is listening</param>
        /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
        public ClientLogger(string address, int port, bool addTimestamp = true) : this(new IPEndPoint(IPAddress.Parse(address), port), addTimestamp)
        {
        }
        /// <summary>
        /// Constructs a new ClientLogger
        /// </summary>
        /// <param name="ipEndPoint">The IPEndPoint of the server</param>
        /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
        public ClientLogger(IPEndPoint ipEndPoint, bool addTimestamp = true) : base(ipEndPoint.ToString(), addTimestamp)
        {
            _client = new Client();
            _client.Connect(ipEndPoint);
        }
    }
}
