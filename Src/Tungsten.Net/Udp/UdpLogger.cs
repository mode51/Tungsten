using System.Net;

namespace W.Net
{
    public static partial class Udp
    {
        /// <summary>
        /// Sends log messages to a remote server via Udp
        /// </summary>
        /// <example>
        ///     Log.LogTheMessage += (category, message) => W.Net.Udp.UdpLogger.LogTheMessage("127.0.0.1", 5555, true, category, message);
        /// </example>
        public static class UdpLogger
        {
            private static System.Net.Sockets.UdpClient _client = new System.Net.Sockets.UdpClient();

            private static System.Net.Sockets.UdpClient GetInstance(IPEndPoint remoteIpEndPoint)
            {
                return _client;
            }
            /// <summary>
            /// Log a message to the remote machine.  Note that this message is slower due to parsing the remoteIP with each call
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
            /// Log a message to the remote machine
            /// </summary>
            /// <param name="remoteIPEndPoint">The IPEndPoint of the remote log server</param>
            /// <param name="message">The log message</param>
            public static void LogTheMessage(IPEndPoint remoteIPEndPoint, string message)
            {
                var instance = GetInstance(remoteIPEndPoint);
                if (instance != null)
                {
                    var bytes = message.AsBytes();
                    instance.SendAsync(bytes, bytes.Length, remoteIPEndPoint).Wait();
                }
            }
        }
    }
}
