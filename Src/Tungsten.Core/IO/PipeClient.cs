using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using W;
using W.Logging;
using W.Threading;

namespace W.Pipes
{
    /// <summary>
    /// Wraps a NamedPipeClientStream for easier use
    /// </summary>
    public class PipeClient : IDisposable
    {
        private Lockable<System.IO.Pipes.NamedPipeClientStream> Client { get; } = new Lockable<NamedPipeClientStream>(null);
        private CancellationTokenSource _cts;
        private readonly string _name;
        private W.Threading.Thread _thread;

        /// <summary>
        /// Called if an exception occurs
        /// </summary>
        public Action<PipeClient, Exception> Exception { get; set; }
        /// <summary>
        /// Called when a message has been received from the server
        /// </summary>
        public Action<PipeClient, string> MessageReceived { get; set; }

        /// <summary>
        /// Gets the underlying NamedPipeClientStream as a PipeStream
        /// </summary>
        /// <returns>Gets the underlying NamedPipeClientStream as a PipeStream</returns>
        public PipeStream GetPipeStream() => Client.Value as PipeStream;
        /// <summary>
        /// Constructs a new PipeClient
        /// </summary>
        /// <param name="name">The name of the pipe to use</param>
        public PipeClient(string name)
        {
            _name = name;
        }
        /// <summary>
        /// Disposes the PipeClient
        /// </summary>
        ~PipeClient()
        {
            Dispose();
        }

        /// <summary>
        /// Creates the underlying NamedPipeClientStream and connects to the server
        /// </summary>
        /// <returns>True if a connection is established, otherwise false</returns>
        public bool Start()
        {
            Stop();
            try
            {
                Client.Value = new NamedPipeClientStream(".", _name, PipeDirection.InOut);
                Client.Value.Connect(1000);
                Client.Value.ReadMode = System.IO.Pipes.PipeTransmissionMode.Message; //it appears this must be done immediately after connecting
                _cts = new CancellationTokenSource();

                _thread = W.Threading.Thread.Create(cts =>
                {
                    try
                    {
                        var p = new GenericPipeTransmitter<string>(GetPipeStream());
                        while (!cts.IsCancellationRequested)
                        {
                            if (!Client.Value?.IsConnected ?? false)
                            {
                                while (Client.Value?.IsConnected ?? false)
                                {
                                    try
                                    {
                                        Client.ExecuteInLock(server =>
                                        {
                                            if (server == null)
                                                return;
                                            System.Diagnostics.Debugger.Break(); //uncomment the lines below to get working again
                                            //var msg = p.Read().Result;
                                            //if (!string.IsNullOrEmpty(msg))
                                            //    MessageReceived?.Invoke(this, msg);
                                        });
                                    }
                                    catch (System.IO.IOException e)
                                    {
                                        Exception?.Invoke(this, e);
                                    }
                                    catch (Exception e)
                                    {
                                        Exception?.Invoke(this, e);
                                    }
                                    System.Threading.Thread.Sleep(1); //play nice with other threads
                                }
                            }
                            System.Threading.Thread.Sleep(1); //play nice with other threads
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        Exception?.Invoke(this, e);
                    }
                    catch (Exception e)
                    {
                        Exception?.Invoke(this, e);
                    }
                });
            }
            catch (TimeoutException e)
            {
                Exception?.Invoke(this, e);
            }
            catch (Exception e)
            {
                Exception?.Invoke(this, e);
            }
            return Client.Value.IsConnected;
        }
        /// <summary>
        /// Stops the client and frees resources
        /// </summary>
        public void Stop()
        {
            _cts?.Cancel();
            _cts = null;
            Client.Value?.Dispose();
            Client.Value = null;
        }

        //public void Send<T>(T item)
        //{
        //    Send(item.AsXml<T>());
        //}
        //public void Send(string message)
        //{
        //    Send(message.AsBytes());
        //}
        //public void Send(byte[] data)
        //{
        //    Client.ExecuteInLock(value =>
        //    {
        //        if (value == null)
        //            return;
        //        PipeHelpers.SendMessageAsync(value, value.OutBufferSize, data, exception =>
        //        {
        //            try
        //            {
        //                if (exception != null)
        //                    Exception?.Invoke(this, exception);
        //            }
        //            catch
        //            {
        //                // ignored
        //            }
        //        }, _cts).Wait();
        //    });
        //}

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Stop();
        }
    }

    //internal class PipeHelpers
    //{
    //    private static bool SendChunk(PipeStream client, ref byte[] dataChunk, int offset = 0, int length = -1, Action<Exception> onComplete = null)
    //    {
    //        Exception ex = null;
    //        try
    //        {
    //            //send the message
    //            var dataLength = length == -1 ? dataChunk.Length : length;
    //            Log.v("Sending {0} bytes", dataLength);
    //            client.Write(dataChunk, offset, dataLength);
    //            //client.WaitForPipeDrain();
    //        }
    //        catch (ArgumentNullException e) //the buffer is null
    //        {
    //            ex = e;
    //        }
    //        catch (ArgumentOutOfRangeException e) //numberOfBytesSent < 0, numberOfBytesSent > message.length, bytesToSend < 0, bytesToSend > (message.Length - numberOfBytesSent)
    //        {
    //            ex = e;
    //        }
    //        catch (IOException e) // failure while writing to the network or error occurred while accessing the socket
    //        {
    //            ex = e;
    //        }
    //        catch (SocketException e)
    //        {
    //            var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
    //            Log.e("TcpHelpers.SendChunk Socket Exception({0}): {1}", errorCode, e.Message);
    //            ex = e;
    //        }
    //        if (ex != null)
    //        {
    //            onComplete?.Invoke(ex);
    //            return false;
    //        }
    //        return true;
    //    }
    //    /// <summary>
    //    /// Writes data to the specified NetworkStream
    //    /// </summary>
    //    /// <param name="stream">The stream on which to write data</param>
    //    /// <param name="sendBufferSize">The data will be written in chunks this size, or less</param>
    //    /// <param name="message">The data to write to the network stream</param>
    //    /// <param name="onComplete">Delegate called once the data has been written or an error occurs</param>
    //    /// <param name="cts">Can be used to cancel the write operation</param>
    //    /// <returns>The Task on which this method executes</returns>
    //    public static Task SendMessageAsync(PipeStream stream, int sendBufferSize, byte[] message, Action<Exception> onComplete = null, CancellationTokenSource cts = null)
    //    {
    //        return System.Threading.Tasks.Task.Run(() =>
    //        {
    //            int length = message.Length;
    //            int numberOfBytesSent = 0;
    //            Log.v("Send Message Size = {0}", length);

    //            //send the size
    //            var sizeBuffer = BitConverter.GetBytes(length); //4 bytes
    //            if (SendChunk(stream, ref sizeBuffer, 0, 4, onComplete))
    //            {

    //                //send the message
    //                while (!(cts?.IsCancellationRequested ?? false) && numberOfBytesSent < length)
    //                {
    //                    var bytesToSend = 0;
    //                    if (numberOfBytesSent < length)
    //                        bytesToSend = Math.Min(sendBufferSize, length - numberOfBytesSent);
    //                    if (!SendChunk(stream, ref message, numberOfBytesSent, bytesToSend, onComplete))
    //                        break;
    //                    numberOfBytesSent += bytesToSend;
    //                }
    //                if (numberOfBytesSent == length)
    //                    onComplete?.Invoke(null);
    //            }
    //        }, cts?.Token ?? CancellationToken.None);
    //    }

    //    private static int GetMessageSize(PipeStream stream, Action<byte[], Exception> onComplete = null, CancellationTokenSource cts = null)
    //    {
    //        int length = 0;
    //        int position = 0;
    //        var messageSizeBuffer = new byte[4];

    //        while (!(cts?.IsCancellationRequested ?? false))
    //        {
    //            try
    //            {
    //                //using (var ns = stream.GetStream())
    //                {
    //                    if (stream.Length > 0)//.DataAvailable)
    //                    {
    //                        var b = stream.Read(messageSizeBuffer, position, 1);
    //                        if (b == 0) //the TcpClient closed or shut down
    //                        {
    //                            onComplete?.Invoke(null, null);
    //                            break;
    //                        }
    //                        position += 1;
    //                        if (position == 4)
    //                        {
    //                            length = BitConverter.ToInt32(messageSizeBuffer, 0);
    //                            break;
    //                        }
    //                    }
    //                }
    //            }
    //            catch (SocketException e) //TcpClient closed or shut down
    //            {
    //                var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
    //                Log.e("TcpHelpers.GetMessageSize Socket Exception({0}): {1}", errorCode, e.Message);
    //                onComplete?.Invoke(null, e);
    //                break;
    //            }
    //            System.Threading.Thread.Sleep(1); //play nice with other threads
    //        }
    //        Log.v("Receive Message Size = {0}", length);
    //        return length;
    //    }
    //    private static void ReadMessage(PipeStream stream, int length, int receiveBufferSize, Action<byte[], Exception> onComplete = null, CancellationTokenSource cts = null)
    //    {
    //        var buffer = new byte[length];
    //        var numberOfBytesRead = 0;
    //        var read = 0;
    //        //var bufferSize = stream.ReceiveBufferSize;
    //        while (!(cts?.IsCancellationRequested ?? false) && numberOfBytesRead < length)
    //        {
    //            //using (var ns = stream.GetStream())
    //            if (stream.Length > 0) //.DataAvailable
    //            {
    //                var bytesToRead = 0;
    //                if (numberOfBytesRead < length)
    //                    bytesToRead = Math.Min(receiveBufferSize, length - numberOfBytesRead);
    //                try
    //                {
    //                    read = stream.Read(buffer, numberOfBytesRead, bytesToRead);
    //                }
    //                catch (SocketException e)
    //                {
    //                    var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
    //                    Log.e("TcpHelpers.ReadMessage Socket Exception({0}): {1}", errorCode, e.Message);
    //                    onComplete?.Invoke(null, e);
    //                    break;
    //                }
    //                if (read == 0) //TcpClient closed or shut down
    //                {
    //                    onComplete?.Invoke(null, null);
    //                    break;
    //                }
    //                Log.v("Read {0} bytes", read);
    //                numberOfBytesRead += read;
    //            }

    //            System.Threading.Thread.Sleep(1); //play nice with other threads
    //        }
    //        if (!(cts?.IsCancellationRequested ?? false))
    //            onComplete?.Invoke(buffer, null);
    //    }

    //    /// <summary>
    //    /// Reads data from the network stream.  The first 4 bytes should indicate the message size.
    //    /// </summary>
    //    /// <param name="stream">The network stream from which to read data</param>
    //    /// <param name="receiveBufferSize">Data will be read in chunks this size, or less</param>
    //    /// <param name="onComplete">Delegate called when a full message has been received, or an error occurs</param>
    //    /// <param name="cts">Can be used to cancel the read operation</param>
    //    /// <returns>The Task on which this method executes</returns>
    //    public static Task ReadMessageAsync(PipeStream stream, int receiveBufferSize, Action<byte[], Exception> onComplete = null, CancellationTokenSource cts = null)
    //    {
    //        return Task.Run(() =>
    //        {
    //            var size = GetMessageSize(stream, onComplete, cts);
    //            if (size > 0 && !(cts?.IsCancellationRequested ?? false))
    //                ReadMessage(stream, size, receiveBufferSize, onComplete, cts);
    //        }, cts?.Token ?? CancellationToken.None);
    //    }
    //}
}
