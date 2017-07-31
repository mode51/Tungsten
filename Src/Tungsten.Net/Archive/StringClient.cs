using System.Net.Sockets;
using W.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// A Tungsten Socket client which sends and receives text
    /// </summary>
    public class StringClient : FormattedSocket<string>
    {
        /// <summary>
        /// Constructs a new StringClient
        /// </summary>
        public StringClient() : base() { }
        /// <summary>
        /// Constructs a new StringClient
        /// </summary>
        public StringClient(TcpClient client) : base(client)
        {
        }

        /// <summary>
        /// Formats the data as text
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override string FormatReceivedMessage(byte[] message)
        {
            return message.AsString();
        }
        /// <summary>
        /// Converts the string into a byte array
        /// </summary>
        /// <param name="message">The string to be sent</param>
        /// <returns>A byte array containing the converted string</returns>
        protected override byte[] FormatMessageToSend(string message)
        {
            return message.AsBytes();
        }
    }
}