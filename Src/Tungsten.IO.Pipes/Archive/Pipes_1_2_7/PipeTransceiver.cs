using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using W;
using W.Logging;

namespace W.IO.Pipes
{
    /// <summary>
    /// Synchronizes the process of transmitting and receiving data over a named pipe
    /// </summary>
    public class PipeTransceiver<TDataType> : Disposable where TDataType : class
    {
        private const int RECEIVE_BUFFER_SIZE = 256;
        private const int SEND_BUFFER_SIZE = 256;
        private ManualResetEventSlim _mreWriteComplete = new ManualResetEventSlim(true);

        private readonly ConcurrentQueue<byte[]> _writeQueue = new ConcurrentQueue<byte[]>();
        private W.Threading.Thread _thread = null;

        /// <summary>
        /// The PipeStream associated with this PipeTransceiver
        /// </summary>
        protected Lockable<PipeStream> Stream { get; } = new Lockable<PipeStream>();
        /// <summary>
        /// Override to customize received data before exposing it via the MessageReceived callback
        /// </summary>
        /// <param name="message">The received data</param>
        /// <returns>The formatted data</returns>
        protected virtual TDataType FormatReceivedMessage(byte[] message)
        {
            return message.As<TDataType>();
        }
        /// <summary>
        /// Override to customize the data before transmission
        /// </summary>
        /// <param name="message">The unaltered data to send</param>
        /// <returns>The formatted data</returns>
        protected virtual byte[] FormatMessageToSend(TDataType message)
        {
            return message.As<byte[]>();
        }
        /// <summary>
        /// Override to handle a disconnect
        /// </summary>
        /// <param name="e">The exception, if one occurred</param>
        /// <remarks>This method is called when a disconnect has been detected by a failed ReadAsync</remarks>
        protected virtual void OnDisconnected(Exception e = null)
        {
        }

        /// <summary>
        /// Called when a message has been received
        /// </summary>
        public Action<object, TDataType> MessageReceived { get; set; }

        /// <summary>
        /// Set to True if the client is running server-side
        /// </summary>
        /// <remarks>This value is informational only; no logic depends on it</remarks>
        protected bool IsServerSide { get; set; } = false;
        /// <summary>
        /// Can be useful for large data sets.  Set to True to use compression, otherwise False
        /// </summary>
        /// <remarks>Make sure both server and client have the same value</remarks>
        public bool UseCompression { get; set; }

        private void SendMessages(CancellationToken token)
        {
            //write all available messages
            while (!_writeQueue.IsEmpty)
            {
                Debug.WriteLine("Sending message(s)");
                if (_writeQueue.TryDequeue(out byte[] message))
                {
                    System.Diagnostics.Debug.WriteLine("Sending: " + message.AsString());
                    if (UseCompression)
                        message = message.AsCompressed();
                    PipeStreamMethods.WriteMessageAsync(Stream.Value, message, SEND_BUFFER_SIZE, token).Wait();
                    //WriteMessageToStream(message, SEND_BUFFER_SIZE);
                }
            }
            _mreWriteComplete.Set(); //can be null if the thread was aborted
        }
        private void ReadMessages(CancellationToken token)
        {
            //read a message if one is available
            var messageSize = PipeStreamMethods.GetMessageSize(Stream.Value, 1000);
            //while (messageSize > 0 )//&& !token.IsCancellationRequested)
            if (messageSize > 0)
            {
                //try
                //{
                if (PipeStreamMethods.ReadMessage(Stream.Value, messageSize, RECEIVE_BUFFER_SIZE, out byte[] message, 5000))
                //if (message != null)
                {
                    if (UseCompression)
                        message = message.FromCompressed();
                    var formattedMessage = FormatReceivedMessage(message);
                    MessageReceived?.BeginInvoke(this, formattedMessage, ar => MessageReceived.EndInvoke(ar), null);
                }
                //}
                //catch (OperationCanceledException) //the read timed out
                //{
                //    System.Diagnostics.Debugger.Break();
                //}
            }
        }
        private void ThreadProc(CancellationToken token)
        {
            while (token != null && !token.IsCancellationRequested)
            {
                try
                {
                    //W.Threading.Thread.Sleep(1);
                    SendMessages(token);
                    ReadMessages(token);
                }
                //#if NET45 || NETSTANDARD1_4 || NETCOREAPP1_0 || WINDOWS_UWP
                //            catch (System.MissingMethodException e)
                //            {
                //                System.Diagnostics.Debug.WriteLine(e.ToString());
                //                System.Diagnostics.Debugger.Break();
                //            }
                //#endif
                //#if NETSTANDARD1_4 //Tungsten.IO.Pipes.Standard is 1.4
                //                catch (System.Threading.Tasks.TaskCanceledException)
                //                {
                //                    //do nothing
                //                }
                //#else
                //                catch (System.Threading.ThreadAbortException)
                //                {
                //                    System.Threading.Thread.ResetAbort();
                //                }
                //#endif
                //catch (ObjectDisposedException)
                //{
                //    //ignore - this can happen if the PipeTransceiver is disposed after ReadMessageSize or ReadMessage are initiated
                //}
                //catch (AggregateException)
                //{
                //    //ignore - this happens when ReadMessageSize times out
                //}
                catch (InvalidOperationException e)
                {
                    //ignore - pipe was closed
                    OnDisconnected(e); //added 10.29.2017
                }
                catch (System.IO.IOException e) //Pipe closed or shut down?
                {
                    //System.Diagnostics.Debug.WriteLine("SynchronizedReadWrite.GetMessageSize Exception: {0}", e.Message);
                    OnDisconnected(e);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                    OnDisconnected(e);
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    System.Diagnostics.Debugger.Break();
                    //Log.e(e);
                }
                finally
                {
                    _mreWriteComplete.Set(); //in case an exception occurred while sending messages
                }
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
            }
        }

        /// <summary>
        /// Disposes the PipeTransceiver and release resources
        /// </summary>
        protected override void OnDispose()
        {
            _mreWriteComplete.Wait();
            _mreWriteComplete.Dispose();
            _thread?.Dispose(); //check for null just in case it was never assigned
        }

        /// <summary>
        /// Waits for all the messages to be written
        /// </summary>
        public void WaitForWriteComplete()
        {
            _mreWriteComplete.Wait();
        }
        /// <summary>
        /// Waits for all the messages to be written or times out after the specified time
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait before a timeout occurs</param>
        /// <returns>True if all messages were sent, otherwise False</returns>
        public bool WaitForWriteComplete(int msTimeout)
        {
            return _mreWriteComplete.Wait(msTimeout);
        }
        /// <summary>
        /// Queues a message to send over the pipe
        /// </summary>
        /// <param name="message"></param>
        public void Write(TDataType message)
        {
            _mreWriteComplete.Reset();//10.29.2017 - moved this above the FormatMessageToSend
            var formatted = FormatMessageToSend(message);
            _writeQueue.Enqueue(formatted);
            //Console.WriteLine("There are {0} items to write", _writeQueue.Count);
        }
        /// <summary>
        /// Initialize the PipeTransceiver with the given PipeStream
        /// </summary>
        /// <param name="stream">The PipeStream to use</param>
        /// <param name="isServerSide">If True, this indicates a server-side client (primarily used for debugging purposes)</param>
        public void Initialize(PipeStream stream, bool isServerSide)
        {
            IsServerSide = isServerSide;
            Stream.Value = stream;

            //NetStandard doesn't support Read Timeouts, so we can't use that.  
            //Our ReadMessageSize uses the Async method and CancellationTokenSource to timeout

            _thread = W.Threading.Thread.Create(ThreadProc);
        }

        /// <summary>
        /// Deconstructs the PipeTransceiver and calls Dispose
        /// </summary>
        ~PipeTransceiver()
        {
            Dispose();
        }
    }
}