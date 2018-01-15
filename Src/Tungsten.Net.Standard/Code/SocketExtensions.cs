using System;
using System.Net;
using System.Net.Sockets;
using W.Threading.ThreadExtensions;

namespace W.Net
{
    /// <summary>
    /// Helper methods for System.Net.Sockets.Socket
    /// </summary>
    internal static class SocketExtensions
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
                    result.Exception = new Exception("SocketTransceiver.GetMessageSize.Receive: " + errorCode.ToString());
                if (byteCount != 4)
                    result.Exception = new Exception("Unable to read enough bytes from the network", result.Exception);
                result.Result = BitConverter.ToInt32(buffer, 0);
                result.Success = byteCount == 4;
            }
            catch (Exception e)
            {
                Debug.e(e);
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
                        throw new Exception("SocketTransceiver.GetMessage.Receive: " + errorCode.ToString());
                    if (read == 0) //socket closed?
                        throw new Exception("SocketTransceiver.GetMessage.Receive: Socket Closed");
                    bytesRead += read;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.e(e);
            }
            bytes = null;
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
                    Debug.i(string.Format("Sent {0} bytes in {1} chunks", length, numberOfChunks));
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
                Debug.e(e);
            }
            return false;
        }
        /// <summary>
        /// Verifies that a socket is still connected
        /// </summary>
        /// <param name="socket">The socket to check</param>
        /// <returns>True if the socket is still connected, otherwise False</returns>
        public static bool IsConnected(this Socket socket)
        {
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
                Debug.i("Receiving bytes");
                var length = socket?.GetMessageSize(false);
                if (length.Success)
                {
                    Debug.i(string.Format("Data Size = {0}", length.Result));
                    if (socket?.GetMessage(length.Result, out response) ?? false)
                    //if (response != null)
                    {
                        Debug.i(string.Format("Received {0} bytes", response.Length));
                        return true;
                    }
                }
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
        /// Sends a message over the socket.  Does not wait for a response.
        /// </summary>
        /// <param name="socket">The socket on which to send data</param>
        /// <param name="bytes">The byte array containing the message</param>
        public static void SendAndForget(this Socket socket, ref byte[] bytes)
        {
            socket.SendMessage(ref bytes);
            Debug.i(string.Format("Sent {0} bytes", bytes.Length));
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

        public static CallResult<bool> Connect(this Socket socket, System.Net.IPEndPoint ipEndPoint, int msTimeout)
        {
            var result = new CallResult<bool>(false);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
#if NETSTANDARD1_3
            try
            {
                //var cts = new System.Threading.CancellationTokenSource(msTimeout + 10);
                //System.Threading.Tasks.Task.Run(() =>
                //{
                result.Success =  socket.ConnectAsync(ipEndPoint).Wait(msTimeout);
                //}, cts.Token).Wait();
                //result.Success = socket.Connected;
            }
            catch (OperationCanceledException e) //timed out
            {
                result.Exception = e;
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
#elif NET45
            try
            {
                var ar = socket.BeginConnect(ipEndPoint, null, socket);
                result.Success = ar.AsyncWaitHandle.WaitOne(msTimeout, true); 

                if (socket.Connected)
                {
                    socket.EndConnect(ar);
                }
            }
            catch (ArgumentNullException e) //socket.BeginConnect
            {
                result.Exception = e;
            }
            catch (ArgumentOutOfRangeException e) //AsyncWaitHandle.WaitOne
            {
                result.Exception = e;
            }
            catch (System.Threading.AbandonedMutexException e) //AsyncWaitHandle.WaitOne
            {
                result.Exception = e;
            }
            catch (SocketException e) //socket.BeginConnect/EndConnect
            {
                result.Exception = e;
            }
            catch (ObjectDisposedException e) //socket
            {
                result.Exception = e;
            }
            catch (System.Security.SecurityException e) //socket.BeginConnect
            {
                result.Exception = e;
            }
            catch (InvalidOperationException e) //socket.BeginConnect/EndConnect/AsyncWaitHandle.WaitOne
            {
                result.Exception = e;
            }
#endif
            return result;

        }
   }
}