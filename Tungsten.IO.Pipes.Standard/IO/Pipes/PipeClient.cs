using System.Collections.Generic;
using System.Text;
using W;

namespace W.IO.Pipes
{

    /// <summary>
    /// A pipe client which sends and receives byte arrays
    /// </summary>
    public class PipeClient : FormattedPipeClient<byte[]> { }

    /// <summary>
    /// Used to send/receive any class type over a named pipe
    /// </summary>
    /// <typeparam name="TDataType">The class type of the data</typeparam>
    public class PipeClient<TDataType> : FormattedPipeClient<TDataType> where TDataType : class
    {
        /// <summary>
        /// Customized formatting for the received message
        /// </summary>
        /// <param name="message">The byte[] containing the unformatted message</param>
        /// <returns>The formatted message</returns>
        protected override TDataType FormatReceivedMessage(byte[] message)
        {
            var xml = message.AsString();
            var result = xml.FromXml<TDataType>();
            return result;
        }
        /// <summary>
        /// Customized formatting for sending a message
        /// </summary>
        /// <param name="message">The message to convert to a byte array</param>
        /// <returns>The message converted to a byte array</returns>
        protected override byte[] FormatMessageToSend(TDataType message)
        {
            var xml = message.AsXml<TDataType>();
            var bytes = xml.AsBytes();
            return bytes;
        }
    }
}
