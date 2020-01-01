using System.Threading.Tasks;
using System.Net;

namespace W.Net
{
    public static partial class Tcp
    {
        /// <summary>
        /// Sends log messages to a remote server via Tcp
        /// </summary>
        /// <example>
        ///     Log.LogTheMessage += (category, message) => W.Net.Tcp.TcpLogger.LogTheMessage("127.0.0.1", 5555, true, category, message);
        /// </example>
        public static class TcpLogger
        {
            private static IPEndPoint _ep = null;
            private static TcpClient _client = null;
            private static TcpClient GetInstance(IPEndPoint ep)
            {
                try
                {
                    if (_client != null && _ep != ep)
                    {
                        _client.Dispose();
                        _client = null;
                    }
                    if (_client == null)
                    {
                        _client = new TcpClient();
                        _client.Connect(ep);
                    }
                }
                catch
                {
                    _client?.Dispose();
                    _client = null;
                }
                return _client;
            }
            /// <summary>
            /// Log a message to the custom logger.  Note that this message is slower due to parsing the remoteIP with each call
            /// </summary>
            /// <param name="remoteIP">The IP address or name of the remote server</param>
            /// <param name="remotePort">The port of the remote Udp server</param>
            /// <param name="message">The log message</param>
            public static void LogTheMessage(string remoteIP, int remotePort, string message)
            {
                var ep = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
                LogTheMessage(ep, message);
            }
            /// <summary>
            /// Log a message to the remote machine.  Note that this message is slower due to parsing the remoteIP with each call
            /// </summary>
            /// <param name="remoteIPEndPoint">The IPEndPoint of the remote server</param>
            /// <param name="message">The log message</param>
            public static void LogTheMessage(IPEndPoint remoteIPEndPoint, string message)
            {
                var instance = GetInstance(remoteIPEndPoint);
                if (instance != null)
                {
                    try
                    {
                        instance?.Write(message.AsBytes());
                    }
                    catch { }
                }
            }
        }
    }
}