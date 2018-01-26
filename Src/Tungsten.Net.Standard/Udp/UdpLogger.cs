using System.Net;
using W.AsExtensions;

namespace W.Net
{
    public static partial class Udp
    {
        /// <summary>
        /// A Udp network logger for W.Logging
        /// </summary>
        public class UdpLogger : W.Logging.CustomLogger
        {
            private IPEndPoint _remoteEndPoint;
            private System.Net.Sockets.UdpClient _client;

            /// <summary>
            /// Log a message to the custom logger
            /// </summary>
            /// <param name="category">The log message category</param>
            /// <param name="message">The log message</param>
            protected override void LogMessage(W.Logging.Log.LogMessageCategory category, string message)
            {
                message = FormatLogMessage(category, message);
                var bytes = message.AsBytes();
                _client.SendAsync(bytes, bytes.Length, _remoteEndPoint).Wait();
            }
            /// <summary>
            /// Constructs a new Udp Client Logger
            /// </summary>
            /// <param name="remoteIpEndPoint">The IPEndPoint of the remote log server</param>
            /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
            public UdpLogger(IPEndPoint remoteIpEndPoint, bool addTimestamp) : base(remoteIpEndPoint.ToString(), addTimestamp)
            {
                _remoteEndPoint = remoteIpEndPoint;
                _client = new System.Net.Sockets.UdpClient();
            }
        }
    }
}
