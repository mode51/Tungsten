using System;
using System.Net.Sockets;

namespace W.Net.SocketExtensions
{
    /// <summary>
    /// Helper methods for System.Net.Sockets.Socket
    /// </summary>
    public static class SocketExtensions
    {
        /// <summary>
        /// Obtain the size of the next message
        /// </summary>
        /// <param name="socket">The socket on which to read the data</param>
        /// <param name="peek">If true, the data containing the size is not removed from the socket buffer</param>
        /// <returns>A CallResult containing the call success/failure, the message size and any exception information</returns>
        public static CallResult<int> GetMessageSize(this Socket socket, bool peek)
        {
            var result = new CallResult<int>(false, 0);
            try
            {
                var buffer = new byte[4];
                int byteCount = 0;
                SocketError errorCode;
                if (peek)
                    byteCount = socket.Receive(buffer, 0, 4, SocketFlags.Peek, out errorCode);
                else
                    byteCount = socket.Receive(buffer, 0, 4, SocketFlags.None, out errorCode);
                if (errorCode != SocketError.Success)
                    result.Exception = new Exception("GetMessageSize.Receive: " + errorCode.ToString());
                if (byteCount != 4)
                    result.Exception = new Exception("Unable to read enough bytes from the network", result.Exception);
                result.Result = BitConverter.ToInt32(buffer, 0);
                result.Success = byteCount == 4;
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
                if (length.Success)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("GetResponse.Data Size = {0}", length.Result));
                    if (socket?.GetMessage(length.Result, out response) ?? false)
                    //if (response != null)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("GetResponse.Received {0} bytes", response.Length));
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
    }
}
