using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using W.Logging;

namespace W.Net
{
    /// <summary>
    /// Helper methods for TcpClient and NetworkStream
    /// </summary>
    public class TcpHelpers
    {
        /// <summary>
        /// If True, messages will be logged
        /// </summary>
        public static bool LogMessages { get; set; } = false;
        private static bool SendChunk(NetworkStream client, ref byte[] dataChunk, int offset = 0, int length = -1, Action<Exception> onComplete = null)
        {
            Exception ex = null;
            try
            {
                //send the message
                var dataLength = length == -1 ? dataChunk.Length : length;
                if (LogMessages) Log.v("Sending {0} bytes", dataLength);
                client?.Write(dataChunk, offset, dataLength);
            }
            catch (ArgumentNullException e) //the buffer is null
            {
                ex = e;
            }
            catch (ArgumentOutOfRangeException e) //numberOfBytesSent < 0, numberOfBytesSent > message.length, bytesToSend < 0, bytesToSend > (message.Length - numberOfBytesSent)
            {
                ex = e;
            }
            catch (IOException e) // failure while writing to the network or error occurred while accessing the socket
            {
                ex = e;
            }
            catch (SocketException e)
            {
                var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
                Log.e("TcpHelpers.SendChunk Socket Exception({0}): {1}", errorCode, e.Message);
                ex = e;
            }
            if (ex != null)
            {
                onComplete?.Invoke(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Writes data to the specified NetworkStream
        /// </summary>
        /// <param name="stream">The stream on which to write data</param>
        /// <param name="sendBufferSize">The data will be written in chunks this size, or less</param>
        /// <param name="message">The data to write to the network stream</param>
        /// <param name="onComplete">Delegate called once the data has been written or an error occurs</param>
        /// <param name="cts">Can be used to cancel the write operation</param>
        /// <returns>The Task on which this method executes</returns>
        public static Task SendMessageAsync(NetworkStream stream, int sendBufferSize, byte[] message, Action<Exception> onComplete = null, CancellationTokenSource cts = null)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                int length = message.Length;
                int numberOfBytesSent = 0;
                if (LogMessages) Log.v("Send Message Size = {0}", length);

                //send the size
                var sizeBuffer = BitConverter.GetBytes(length); //4 bytes
                if (SendChunk(stream, ref sizeBuffer, 0, 4, onComplete))
                {

                    //send the message
                    while (!(cts?.IsCancellationRequested ?? false) && numberOfBytesSent < length)
                    {
                        var bytesToSend = 0;
                        if (numberOfBytesSent < length)
                            bytesToSend = Math.Min(sendBufferSize, length - numberOfBytesSent);
                        if (!SendChunk(stream, ref message, numberOfBytesSent, bytesToSend, onComplete))
                            break;
                        numberOfBytesSent += bytesToSend;
                    }
                    if (numberOfBytesSent == length)
                        onComplete?.Invoke(null);
                }
            }, cts?.Token ?? CancellationToken.None);
        }

        private static int GetMessageSize(NetworkStream stream, Action<byte[], Exception> onComplete = null, CancellationTokenSource cts = null)
        {
            int length = 0;
            int position = 0;
            var messageSizeBuffer = new byte[4];

            while (!(cts?.IsCancellationRequested ?? false))
            {
                try
                {
                    //using (var ns = stream.GetStream())
                    {
                        if (stream.DataAvailable)
                        {
                            var b = stream.Read(messageSizeBuffer, position, 1);
                            if (b == 0) //the TcpClient closed or shut down
                            {
                                onComplete?.Invoke(null, null);
                                break;
                            }
                            position += 1;
                            if (position == 4)
                            {
                                length = BitConverter.ToInt32(messageSizeBuffer, 0);
                                break;
                            }
                        }
                    }
                }
                catch (SocketException e) //TcpClient closed or shut down
                {
                    var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
                    Log.e("TcpHelpers.GetMessageSize Socket Exception({0}): {1}", errorCode, e.Message);
                    onComplete?.Invoke(null, e);
                    break;
                }
                W.Threading.Thread.Sleep(1); //play nice with other threads
            }
            if (LogMessages) Log.v("Receive Message Size = {0}", length);
            return length;
        }
        private static void ReadMessage(NetworkStream stream, int length, int receiveBufferSize, Action<byte[], Exception> onComplete = null, CancellationTokenSource cts = null)
        {
            var buffer = new byte[length];
            var numberOfBytesRead = 0;
            var read = 0;
            //var bufferSize = stream.ReceiveBufferSize;
            while (!(cts?.IsCancellationRequested ?? false) && numberOfBytesRead < length)
            {
                //using (var ns = stream.GetStream())
                if (stream.DataAvailable)
                {
                    var bytesToRead = 0;
                    if (numberOfBytesRead < length)
                        bytesToRead = Math.Min(receiveBufferSize, length - numberOfBytesRead);
                    try
                    {
                        read = stream.Read(buffer, numberOfBytesRead, bytesToRead);
                    }
                    catch (SocketException e)
                    {
                        var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
                        Log.e("TcpHelpers.ReadMessage Socket Exception({0}): {1}", errorCode, e.Message);
                        onComplete?.Invoke(null, e);
                        break;
                    }
                    if (read == 0) //TcpClient closed or shut down
                    {
                        onComplete?.Invoke(null, null);
                        break;
                    }
                    if (LogMessages) Log.v("Read {0} bytes", read);
                    numberOfBytesRead += read;
                }

                W.Threading.Thread.Sleep(1); //play nice with other threads
            }
            if (!(cts?.IsCancellationRequested ?? false))
                onComplete?.Invoke(buffer, null);
        }

        /// <summary>
        /// Reads data from the network stream.  The first 4 bytes should indicate the message size.
        /// </summary>
        /// <param name="stream">The network stream from which to read data</param>
        /// <param name="receiveBufferSize">Data will be read in chunks this size, or less</param>
        /// <param name="onComplete">Delegate called when a full message has been received, or an error occurs</param>
        /// <param name="cts">Can be used to cancel the read operation</param>
        /// <returns>The Task on which this method executes</returns>
        public static Task ReadMessageAsync(NetworkStream stream, int receiveBufferSize, Action<byte[], Exception> onComplete = null, CancellationTokenSource cts = null)
        {
            return Task.Run(() =>
            {
                var size = GetMessageSize(stream, onComplete, cts);
                if (size > 0 && !(cts?.IsCancellationRequested ?? false))
                    ReadMessage(stream, size, receiveBufferSize, onComplete, cts);
            }, cts?.Token ?? CancellationToken.None);
        }
        /// <summary>
        /// Checks the NetworkStream.DataAvailable
        /// </summary>
        /// <param name="stream">The NetworkStream to check</param>
        /// <param name="onException">Called if a SocketException occurs</param>
        /// <returns></returns>
        public static bool IsMessageAvailable(NetworkStream stream, Action<Exception> onException)
        {
            try
            {
                if (stream == null)
                    return false;
                return stream.DataAvailable;
            }
            catch (SocketException e) //if remote host has been shut down or closed the connection
            {
                var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
                Log.e("Socket Exception({0}): {1}", errorCode, e.Message);
                onException?.Invoke(e);
            }
            return false;
        }
        /// <summary>
        /// Checks the TcpClient.Available value
        /// </summary>
        /// <param name="client">The TcpClient to check</param>
        /// <param name="onException">Called if a SocketException occurs</param>
        /// <returns>True if at least 4 bytes of data are available</returns>
        public static bool IsMessageAvailable(TcpClient client, Action<Exception> onException)
        {
            try
            {
                return client?.Available > 3;
            }
            catch (SocketException e) //if remote host has been shut down or closed the connection
            {
                var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
                Log.e("Socket Exception({0}): {1}", errorCode, e.Message);
                onException?.Invoke(e);
            }
            return false;
        }

        /// <summary>
        /// Determines if a string contains only Base64 characters
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value contains only Base64 characters, otherwise False</returns>
        /// <remarks>Obtained from: <see ref="https://stackoverflow.com/questions/8571501/how-to-check-whether-the-string-is-base64-encoded-or-not"/></remarks> 
        internal static bool IsBase64Encoded(string value)
        {
            var result = false;
            var regex = new System.Text.RegularExpressions.Regex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$");
            result = regex.IsMatch(value);
            return result;
        }
        /// <summary>
        /// Converts the object to json and then to a byte array
        /// </summary>
        /// <param name="message">The object to convert</param>
        /// <param name="settings">The JSON serialization settings to use during serialization</param>
        /// <returns>A byte array containing the serialized object</returns>
        internal static byte[] FormatMessageToSend<TLocalMessageType>(TLocalMessageType message, Newtonsoft.Json.JsonSerializerSettings settings)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(message, typeof(TLocalMessageType), settings);
            var bytes = msg.AsBytes();
            return bytes;
        }
        /// <summary>
        /// Converts a byte array into a deserialized object
        /// </summary>
        /// <param name="bytes">The byte array to convert</param>
        /// <param name="settings">The JSON serialization settings to use during serialization</param>
        /// <returns>The deserialized object</returns>
        internal static TLocalMessageType FormatReceivedMessage<TLocalMessageType>(byte[] bytes, Newtonsoft.Json.JsonSerializerSettings settings)
        {
            var message = bytes.AsString();
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TLocalMessageType>(message, settings);
            return obj;
        }
    }
}