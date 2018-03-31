using System;
using System.Net.Sockets;

//3.30.2018 - these need to be internal because they extend Socket, not W.Net.TcpClient/Host, etc (and therefore cause concurrency blocks)
//          - and there's really no need to make them public

namespace W.Net
{
    /// <summary>
    /// Helper methods for System.Net.Sockets.Socket
    /// </summary>
    internal static class SocketExtensions
    {
        /// <summary>
        /// Closes the socket and disposes it
        /// </summary>
        /// <param name="socket"></param>
        public static void ShutdownAndDispose(this Socket socket)
        {
            try
            {
                socket?.Shutdown(SocketShutdown.Both);
#if NET45
                if (socket?.Connected ?? false)
                    socket?.Disconnect(true);
#endif
                socket?.Dispose();
            }
            catch { }
        }
        /// <summary>
        /// Obtain the size of the next message
        /// </summary>
        /// <param name="socket">The socket on which to read the data</param>
        /// <param name="peek">If true, the data containing the size is not removed from the socket buffer</param>
        /// <returns>The message size or null if no value was obtained</returns>
        public static int? GetMessageSize(this Socket socket, bool peek)
        {
            int? result = null;
            try
            {
                var buffer = new byte[4];
                int byteCount = 0;
                SocketError errorCode;
                if (peek)
                    byteCount = socket.Receive(buffer, 0, 4, SocketFlags.Peek, out errorCode);
                else
                    byteCount = socket.Receive(buffer, 0, 4, SocketFlags.None, out errorCode);
                //if (errorCode != SocketError.Success)
                //    result.Exception = new Exception("GetMessageSize.Receive: " + errorCode.ToString());
                //if (byteCount != 4)
                //    result.Exception = new Exception("Unable to read enough bytes from the network", result.Exception);
                if (byteCount == 4)
                    result = BitConverter.ToInt32(buffer, 0);
                //result.Success = byteCount == 4;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return result;
        }
        /// <summary>
        /// Uses blocking calls to receive data from the socket
        /// </summary>
        /// <param name="socket">The socket from which to receive data</param>
        /// <param name="length">The number of bytes the method should expect to receive</param>
        /// <param name="bytes">The byte array to store the data</param>
        /// <returns>A byte array containing the data</returns>
        public static bool GetMessage(this Socket socket, int length, out byte[] bytes)
        {
            try
            {
                var bufferSize = socket.ReceiveBufferSize;
                bytes = new byte[length];
                var bytesRead = 0;

                var numberOfChunks = length % bufferSize;
                while (bytesRead < length)
                {
                    SocketError errorCode;
                    var bytesToRead = 0;

                    if (bytesRead < length)
                        bytesToRead = Math.Min(bufferSize, length - bytesRead);

                    var read = socket.Receive(bytes, bytesRead, bytesToRead, SocketFlags.None, out errorCode);
                    if (errorCode != SocketError.Success)
                        throw new Exception("GetMessage.Receive: " + errorCode.ToString());
                    if (read == 0) //socket closed?
                        throw new Exception("GetMessage.Receive: Socket Closed");
                    bytesRead += read;
                }
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            bytes = null;
            return false;
        }
        /// <summary>
        /// Reads a response from the socket
        /// </summary>
        /// <param name="socket">The socket from which to read</param>
        /// <param name="response">The response received from the socket</param>
        /// <returns>True if a response was received, otherwise False</returns>
        public static bool GetResponse(this Socket socket, out byte[] response)
        {
            response = null;
            if (socket?.Available >= 4)
            {
                //receive any incoming data
                System.Diagnostics.Debug.WriteLine("GetResponse.Receiving bytes");
                var length = socket?.GetMessageSize(false);
                if (length != null && length > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"GetResponse.Data Size = {length}");
                    if (socket?.GetMessage((int)length, out response) ?? false)
                    //if (response != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"GetResponse.Received {response.Length} bytes");
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Sends data over the socket
        /// </summary>
        /// <param name="socket">The socket on which to send data</param>
        /// <param name="bytes">The data to send</param>
        public static bool SendBytes(this Socket socket, ref byte[] bytes)
        {
            SocketError errorCode;
            try
            {
                if (socket.Connected)
                {
                    var numberOfChunks = 0;
                    var bufferSize = socket.SendBufferSize;
                    var bytesSent = 0;
                    var length = bytes.Length;

                    while (bytesSent < length)
                    {
                        numberOfChunks += 1;
                        int bytesToSend = 0;
                        if (bytesSent < length)
                            bytesToSend = Math.Min(bufferSize, length - bytesSent);
                        var sent = socket.Send(bytes, bytesSent, bytesToSend, SocketFlags.None, out errorCode);
                        if (errorCode != SocketError.Success)
                            return false;
                        if (sent == 0) //socket closed?
                            return false;
                        bytesSent += sent;
                    }
                    System.Diagnostics.Debug.WriteLine(string.Format("SendBytes: Sent {0} bytes in {1} chunks", length, numberOfChunks));
                    return true;
                }
            }
            catch (ObjectDisposedException)
            {
                //socket has been disposed
            }
            catch (System.Net.Sockets.SocketException e)
            {
                if (e.SocketErrorCode != SocketError.ConnectionAborted)
                    System.Diagnostics.Debugger.Break();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return false;
        }
        /// <summary>
        /// Sends data over the socket
        /// </summary>
        /// <param name="socket">The socket on which to send data</param>
        /// <param name="bytes">The data to send</param>
        public static bool SendMessage(this Socket socket, ref byte[] bytes)
        {
            SocketError errorCode;
            try
            {
                if (socket.Connected)
                {
                    var numberOfChunks = 0;
                    var bufferSize = socket.SendBufferSize;
                    var bytesSent = 0;
                    var length = bytes.Length;

                    var lengthBuffer = BitConverter.GetBytes(length); //4 bytes
                    socket.Send(lengthBuffer);
                    while (bytesSent < length)
                    {
                        numberOfChunks += 1;
                        int bytesToSend = 0;
                        if (bytesSent < length)
                            bytesToSend = Math.Min(bufferSize, length - bytesSent);
                        var sent = socket.Send(bytes, bytesSent, bytesToSend, SocketFlags.None, out errorCode);
                        if (errorCode != SocketError.Success)
                            return false;
                        //    throw new Exception("SocketTransceiver.SendMessage.Send: " + errorCode.ToString());
                        if (sent == 0) //socket closed?
                            return false;
                        //    throw new Exception("SocketTransceiver.SendMessage.Send: Socket Closed");
                        bytesSent += sent;
                    }
                    System.Diagnostics.Debug.WriteLine(string.Format("SendMessage: Sent {0} bytes in {1} chunks", length, numberOfChunks));
                    return true;
                }
            }
            catch (ObjectDisposedException)
            {
                //socket has been disposed
            }
            catch (System.Net.Sockets.SocketException e)
            {
                if (e.SocketErrorCode != SocketError.ConnectionAborted)
                    System.Diagnostics.Debugger.Break();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return false;
        }
        /// <summary>
        /// Waits the given amount of time (in milliseconds) for a message to arrive on the socket
        /// </summary>
        /// <param name="socket">The socket on which to wait</param>
        /// <param name="response">The response, if one is received</param>
        /// <param name="msTimeout">The timeout, in milliseconds</param>
        /// <returns>True if a message was received, otherwise False</returns>
        public static bool WaitForResponse(this Socket socket, out byte[] response, int msTimeout = -1)
        {
            try
            {
                bool available = System.Threading.SpinWait.SpinUntil(() => socket.Available > 4, msTimeout);
                //bool available = socket.WaitForValue(s => s.Available > 4, msTimeout);
                if (available)
                    return socket.GetResponse(out response);
            }
#if NET45
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
            }
#endif
            catch (AggregateException)
            {
                System.Diagnostics.Debugger.Break();
            }
            catch (Exception)
            {
                System.Diagnostics.Debugger.Break();
            }
            response = null;
            return false;
        }
        /// <summary>
        /// Sends a message over the socket and waits, for the given amount of time (in milliseconds), for a response.
        /// </summary>
        /// <param name="socket">The socket on which to send data</param>
        /// <param name="bytes">The byte array containing the message</param>
        /// <param name="response">The byte array to contain the response, if one is received</param>
        /// <param name="msTimeout">The timeout, in milliseconds, to wait for a response</param>
        /// <returns>True if a response was received, otherwise False</returns>
        public static bool SendAndWaitForResponse(this Socket socket, ref byte[] bytes, out byte[] response, int msTimeout = -1)
        {
            socket.SendMessage(ref bytes);
            return socket.WaitForResponse(out response, msTimeout);
        }
        /// <summary>
        /// Sends a message over the socket and waits, for the given amount of time (in milliseconds), for a response.
        /// </summary>
        /// <param name="socket">The socket on which to send data</param>
        /// <param name="message">The message</param>
        /// <param name="response">The response, if one is received</param>
        /// <param name="msTimeout">The timeout, in milliseconds, to wait for a response</param>
        /// <returns>True if a response was received, otherwise False</returns>
        public static bool SendAndWaitForResponse<TMessage>(this Socket socket, ref TMessage message, out TMessage response, int msTimeout = -1)
        {
            response = default(TMessage);
            var bytes = SerializationMethods.Serialize(message).AsBytes();
            socket.SendMessage(ref bytes);
            byte[] responseBytes;
            var result = socket.WaitForResponse(out responseBytes, msTimeout);
            if (result)
                response = SerializationMethods.Deserialize<TMessage>(ref responseBytes);
            return result;
        }
        /// <summary>
        /// Verifies that a socket is still connected
        /// </summary>
        /// <param name="socket">The socket to check</param>
        /// <returns>True if the socket is still connected, otherwise False</returns>
        public static bool IsConnected(this Socket socket)
        {
            //try
            //{
            //    return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            //}
            //catch (SocketException) { return false; }
            try
            {
                if (!socket.Connected)
                    return false;
                var canRead = socket.Poll(1, SelectMode.SelectRead);// ?? true;
                var canWrite = socket.Poll(1, SelectMode.SelectWrite);
                var isError = socket.Poll(1, SelectMode.SelectError);
                var available = socket.Available;// ?? 0;
                if (canRead)
                {
                    var buffer = new byte[1];
                    //var read = socket.Receive(buffer, SocketFlags.Peek);
                    //if (read == 0)
                    //    return false;
                    var read = socket.Receive(buffer, 0, 1, SocketFlags.Peek, out SocketError errorCode);
                    if (errorCode != SocketError.Success || read == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Socket.IsConnected yielded: {0}", Enum.GetName(typeof(SocketError), errorCode));
                        return false;
                    }
                }
                //if (socket.Connected && (canRead && (available == 0))) //socket is closed if true yet Available is 0
                //    result = false;
                //else
                //    result = true;

                //if (!canWrite)
                //    Exception.Invoke(Socket, new System.Exception("Can't write to the socket"));
                //if (isError)
                //    Exception.Invoke(Socket, new System.Exception("The socket is in an excepted state"));
            }
            catch (NotSupportedException)
            {
                System.Diagnostics.Debugger.Break();
                return false;
            }
            catch (SocketException)
            {
                System.Diagnostics.Debugger.Break(); //disconnect?
                return false;
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Break();
                return false;
            }
            catch (Exception)
            {
                System.Diagnostics.Debugger.Break();
            }
            return true;
        }
    }
    internal enum HeaderTypeEnum : Int32
    {
        Disconnect = 0,
        KeepAlive = 1,
        Data = 2
    }
    internal static class SocketExtensions2
    {
        /// <summary>
        /// Read and Int32 value from the socket
        /// </summary>
        /// <param name="socket">The socket on which to read the data</param>
        /// <param name="value">The Int32 variable to receive the value</param>
        /// <param name="peek">If true, the data containing the size is not removed from the socket buffer</param>
        /// <returns>True if a value was read, otherwise False</returns>
        private static bool GetInt32(this Socket socket, out Int32 value, bool peek)
        {
            var result = false;
            value = 0;
            try
            {
                var buffer = new byte[4];
                Int32 byteCount = 0;
                SocketError errorCode;
                if (peek)
                    byteCount = socket.Receive(buffer, 0, 4, SocketFlags.Peek, out errorCode);
                else
                    byteCount = socket.Receive(buffer, 0, 4, SocketFlags.None, out errorCode);
                //if (errorCode != SocketError.Success)
                //    result.Exception = new Exception("GetMessageSize.Receive: " + errorCode.ToString());
                //if (byteCount != 4)
                //    result.Exception = new Exception("Unable to read enough bytes from the network", result.Exception);
                value = BitConverter.ToInt32(buffer, 0);
                result = byteCount == 4;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return result;
        }
        public static bool GetResponseHeader(this Socket socket, out HeaderTypeEnum type, out Int32 length)
        {
            var result = false;
            type = 0;
            length = 0;
            if (socket.Available >= 8)
            {
                if (socket.GetInt32(out Int32 iType, false))
                    result = socket.GetInt32(out length, false);
                type = (HeaderTypeEnum)iType;
            }
            return result;
        }
        public static bool GetResponseWithHeader(this Socket socket, out HeaderTypeEnum type, out byte[] bytes)
        {
            var result = false;
            Int32 length;
            bytes = null;
            if (socket.GetResponseHeader(out type, out length))
            {
                result = socket.GetMessage(length, out bytes);
            }
            return result;
        }
        public static bool SendMessageWithHeader(this Socket socket, HeaderTypeEnum type, ref byte[] bytes)
        {
            var result = false;
            var headerBytes = new byte[8];
            Buffer.BlockCopy(BitConverter.GetBytes((Int32)type), 0, headerBytes, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(bytes?.Length ?? 0), 0, headerBytes, 4, 4);
            //if (socket.SendBytes(ref typeBytes) && socket.SendBytes(ref lengthBytes))
            if (socket.SendBytes(ref headerBytes))
            {
                if (bytes == null)
                    result = true;
                else
                {
                    result = socket.SendBytes(ref bytes);
                    if (result)
                        System.Diagnostics.Debug.WriteLine($"SendMessageWithHeader: Sent {bytes.Length} bytes");
                    else
                        System.Diagnostics.Debug.WriteLine($"SendMessageWithHeader: Send bytes failed");
                }
            }
            return result;
        }
        public static bool SendDisconnect(this Socket socket)
        {
            System.Diagnostics.Debug.WriteLine("SendDisconnect: Sending Disconnect");
            byte[] disconnectBytes = null;
            return socket.SendMessageWithHeader(HeaderTypeEnum.Disconnect, ref disconnectBytes);
        }
        public static bool SendKeepAlive(this Socket socket)
        {
            System.Diagnostics.Debug.WriteLine("SendKeepAlive: Sending KeepAlive");
            byte[] keepAliveBytes = null;
            return socket.SendMessageWithHeader(HeaderTypeEnum.KeepAlive, ref keepAliveBytes);
        }
    }
}
