using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using W;
using W.Logging;
using W.Net;
using W.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// Adds logging via a SecureStringClient
    /// </summary>
    public class SecureStringClientLogger : W.Logging.CustomLogger
    {
        private SecureClient<string> _client;

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
            if (_client?.Socket?.IsConnected ?? false) //because the handler is added before the client connects
            {
                message = FormatLogMessage(category, message);
                _client.Send(message);
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
                _client.Dispose();
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
            _client = new SecureClient<string>();

            if (!_client.Socket.ConnectAsync(serverEndPoint.Address, serverEndPoint.Port).Result)
            {
                this.SocketError = System.Net.Sockets.SocketError.ConnectionRefused;
                System.Diagnostics.Debug.WriteLine("SecureStringLogger failed to connect to the server.");
                return;
            }
            if (!_client.WaitForConnected(System.Diagnostics.Debugger.IsAttached ? -1 : 5000))
            {
                this.SocketError = System.Net.Sockets.SocketError.TimedOut;
                //throw new TimeoutException("Connection timed out waiting for the server");
                return;
            }
        }
    }
    ///// <summary>
    ///// Adds support for sending log messages to an IP host
    ///// </summary>
    //public class Logging
    //{
    //    private static List<SecureStringLogger> _clients = new List<SecureStringLogger>();
    //    private static object _lockObj = new object();

    //    /// <summary>
    //    /// Configures W.Logging.Log to send information to an IPEndPoint
    //    /// </summary>
    //    /// <param name="ipAddress"></param>
    //    /// <param name="port"></param>
    //    /// <param name="autoAddTimestamp">If true, each log message is automatically prefixed with a timestamp</param>
    //    public static void Add(IPAddress ipAddress, int port, bool autoAddTimestamp = true)
    //    {
    //        var client = _clients.FirstOrDefault(c => c.IpAddress.Equals(ipAddress) && c.Port == port));
    //        if (client == null)
    //        {
    //            var newClient = new SecureStringLogger(ipAddress, port, autoAddTimestamp);
    //            _clients.Add(newClient);
    //        }
    //    }

    //    /// <summary>
    //    /// Disconnects and disposes all SecureStringClient loggers
    //    /// </summary>
    //    public static void RemoveAll()
    //    {
    //        lock (_lockObj)
    //        {
    //            while (_clients.Count > 0)
    //            {
    //                Remove(_clients[0].IpAddress, _clients[0].Port);
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// Disconnects and disposes all SecureStringClient loggers
    //    /// </summary>
    //    public static void Remove(IPAddress ipAddress, int port)
    //    {
    //        lock (_lockObj)
    //        {
    //            var client = _clients.FirstOrDefault(c => c.IpAddress.Equals(ipAddress) && c.Port == port);
    //            if (client != null)
    //            {
    //                client.Dispose();
    //                _clients.Remove(client);
    //            }
    //        }
    //    }
    //}
}
