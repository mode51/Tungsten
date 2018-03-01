using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Pipes;
#if NET45
using System.Security.Principal;
#endif

namespace W.IO.Pipes
{
    /// <summary>
    /// Static utility methods for Pipe streams
    /// </summary>
    public static class PipeStreamMethods
    {
        /// <summary>
        /// Reads a chunk of bytes from a pipe stream
        /// </summary>
        /// <param name="stream">The stream from which to read data</param>
        /// <param name="length">The number of bytes to read</param>
        /// <param name="msTimeout">The maximum number of milliseconds to wait for the bytes</param>
        /// <returns>If successfull, a byte array containing the data, otherwise null</returns>
        public static byte[] ReadChunk(PipeStream stream, int length, int msTimeout)
        {
            int read = 0;
            var bytes = new byte[length];
            try
            {
                var cts = new CancellationTokenSource(msTimeout);
                Task.Run(async () =>
                {
                    read = await stream.ReadAsync(bytes, 0, length);
                }).Wait(cts.Token);
                if (read != length)
                    Array.Resize(ref bytes, read);
                return bytes;
            }
            catch (OperationCanceledException)
            {
                //System.Diagnostics.Debugger.Break();
                //System.Diagnostics.Debug.WriteLine(e.ToString());
                return null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return null;
            }
        }
        /// <summary>
        /// Reads the incoming message size from the specified pipe stream
        /// </summary>
        /// <param name="stream">The pipe stream from which to read bytes</param>
        /// <param name="msTimeout">The number of milliseconds to wait for a message</param>
        /// <returns>The size of the incoming message</returns>
        /// <remarks>The first 4 bytes received determine the message size</remarks>
        public static int GetMessageSize(PipeStream stream, int msTimeout = 100)
        {
            var bytes = ReadChunk(stream, 4, msTimeout);
            if (bytes?.Length == 4)
            {
                var len = BitConverter.ToInt32(bytes, 0);
                System.Diagnostics.Debug.WriteLine("Incoming Message Size: {0}", len);
                return len;
            }
            return 0;
            //var cts = new CancellationTokenSource(msTimeout);  //timeout after 100 milliseconds
            //var bytes = new byte[4];
            //var result = 0;
            //try
            //{
            //    Task.Run(() =>
            //    {
            //        stream.ReadAsync(bytes, 0, 4).ContinueWith(task =>
            //        {
            //            var read = task.Result;
            //            if (read > 0)
            //            {
            //                var len = BitConverter.ToInt32(bytes, 0);
            //                System.Diagnostics.Debug.WriteLine("Incoming Message Size: {0}", len);
            //                result = len;
            //            }
            //        }).Wait(cts.Token);
            //    }).Wait(cts.Token);
            //}
            //catch (OperationCanceledException)
            //{
            //    //ignore, task was cancelled
            //}
            //catch (TimeoutException e)
            //{
            //    //ignore? - this happens when the CancellationToken times out?
            //    System.Diagnostics.Debugger.Break();
            //    System.Diagnostics.Debug.WriteLine(e.ToString());
            //}
            ////if (ReadMessage(stream, 4, 4, msTimeout, out byte[] bytes) && bytes != null)
            ////{
            ////    var len = BitConverter.ToInt32(bytes, 0);
            ////    System.Diagnostics.Debug.WriteLine("Incoming Message Size: {0}", len);
            ////    return len;
            ////}
            //return result;
        }
        /// <summary>
        /// Reads the specified number of bytes from the pipe
        /// </summary>
        /// <param name="stream">The pipe stream from which to read bytes</param>
        /// <param name="length">The number of bytes to read</param>
        /// <param name="receiveBlockSize">The size of the receive buffer</param>
        /// <param name="msTimeout">The number of milliseconds allowed before the operation is cancelled</param>
        /// <param name="bytes">If successful, the bytes read from the pipe, otherwise null</param>
        /// <returns>True if the read was successful, otherwise False.</returns>
        public static bool ReadMessage(PipeStream stream, int length, int receiveBlockSize, out byte[] bytes, int msTimeout)
        {
            bytes = new byte[length];
            var numberOfBytesRead = 0;
            while (numberOfBytesRead < length)
            {
                var bytesToRead = 0;
                if (numberOfBytesRead < length)
                    bytesToRead = Math.Min(receiveBlockSize, length - numberOfBytesRead);
                var cache = ReadChunk(stream, bytesToRead, msTimeout);
                if (cache?.Length == 0)
                {
                    bytes = null;
                    return false;
                }
                Array.Copy(cache, 0, bytes, numberOfBytesRead, cache.Length);
                numberOfBytesRead += cache.Length;
            }
            return (numberOfBytesRead == length);
        }
        /// <summary>
        /// Writes the specified bytes to a pipe stream
        /// </summary>
        /// <param name="stream">The pipe stream on which to write bytes</param>
        /// <param name="bytes">The bytes to write to the pipe stream</param>
        /// <param name="writeBlockSize">The size of the write buffer</param>
        /// <param name="token">A CancellationToken which can be used to cancel the operation</param>
        /// <returns>The Task associated with this operation</returns>
        /// <remarks>The first 4 bytes sent will contain the message size so that the reader knows how many bytes to receive</remarks>
        public static async Task WriteMessageAsync(PipeStream stream, byte[] bytes, int writeBlockSize, CancellationToken token)
        {
            int length = bytes.Length;
            //send the size
            var sizeBuffer = BitConverter.GetBytes(length); //4 bytes
            await stream.WriteAsync(sizeBuffer, 0, 4);
            //stream.WaitForPipeDrain();

            //send the bytes
            int outBufferSize = writeBlockSize;
            int numberOfBytesSent = 0;
            while (numberOfBytesSent < length && !token.IsCancellationRequested)
            {
                var bytesToSend = 0;
                if (numberOfBytesSent < length)
                    bytesToSend = Math.Min(outBufferSize, length - numberOfBytesSent);
                await stream.WriteAsync(bytes, numberOfBytesSent, bytesToSend, token);
                //stream.WaitForPipeDrain();
                numberOfBytesSent += bytesToSend;
            }
            //await stream.FlushAsync();
            //stream.WaitForPipeDrain();
        }
    }
}
