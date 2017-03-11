using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
    /// <summary>
    /// Synchronizes two threads
    /// </summary>
    public class PipeTransceiver<TDataType> : IDisposable where TDataType : class
    {
        private Lockable<int> _readSize = new Lockable<int>(0);

        private W.Threading.Thread _watchThread;
        private W.Threading.Gate _gateReadMessageSize;
        private W.Threading.Gate _gateReadMessage;
        private W.Threading.Gate _gateWrite;

        private readonly ConcurrentQueue<byte[]> _writeQueue = new ConcurrentQueue<byte[]>();

        /// <summary>
        /// The PipeStream associated with this PipeTransceiver
        /// </summary>
        protected Lockable<PipeStream> Stream { get; } = new Lockable<PipeStream>();
        /// <summary>
        /// Set to True if the client is running server-side
        /// </summary>
        /// <remarks>This value is informational only; no logic depends on it</remarks>
        protected bool IsServerSide { get; set; } = false;

        /// <summary>
        /// Called when an exception occurs
        /// </summary>
        public Action<object, Exception> Exception { get; set; }
        /// <summary>
        /// Called when a message has been received
        /// </summary>
        public Action<object, TDataType> MessageReceived { get; set; }

        private void WriteMessage(System.IO.Pipes.PipeStream stream, byte[] bytes, int writeBufferSize)
        {
            try
            {
                //var bytes = FormatMessageToSend(message);
                int length = bytes.Length;

                //send the size
                var sizeBuffer = BitConverter.GetBytes(length); //4 bytes
                stream.Write(sizeBuffer, 0, 4);

                int outBufferSize = writeBufferSize;//stream.OutBufferSize == 0 ? 256 : stream.OutBufferSize;
                int numberOfBytesSent = 0;
                while (numberOfBytesSent < length)
                {
                    var bytesToSend = 0;
                    if (numberOfBytesSent < length)
                        bytesToSend = Math.Min(outBufferSize, length - numberOfBytesSent);
                    stream.Write(bytes, numberOfBytesSent, bytesToSend);
                    stream.WaitForPipeDrain();
                    numberOfBytesSent += bytesToSend;
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
        }
        private byte[] ReadMessage(PipeStream stream, int length, int receiveBufferSize)
        {
            //no need to cancel a read, just let it finish
            var buffer = new byte[length];
            var numberOfBytesRead = 0;
            while (numberOfBytesRead < length)
            {
                var read = 0;
                var bytesToRead = 0;
                if (numberOfBytesRead < length)
                    bytesToRead = Math.Min(receiveBufferSize, length - numberOfBytesRead);
                try
                {
                    read = stream.Read(buffer, numberOfBytesRead, bytesToRead);
                    //read = ReadMessage(numberOfBytesRead, bytesToRead);
                }
                catch (System.IO.IOException e)
                {
                    Debug.WriteLine("PipeTransmitter.ReadMessage Exception: {0}", e.Message);
                    Exception?.Invoke(this, e);
                    break;
                }
                //if (read == 0) //pipe closed or shut down
                //{
                //    Exception?.Invoke(null, null);
                //    break;
                //}
                Debug.WriteLine("Read {0} bytes", read);
                numberOfBytesRead += read;

                System.Threading.Thread.Sleep(1); //play nice with other threads
            }
            return numberOfBytesRead == length ? buffer : null;
        }
        private async Task GetMessageSize(PipeStream stream, CancellationTokenSource cts)
        {
            //3.8.2017 - this code seems to work.  blocks and cancels correctly
            try
            {
                var messageSizeBuffer = new byte[4];
                _readSize.Value = 0;
                stream.ReadAsync(messageSizeBuffer, 0, 4, cts.Token).ContinueWith(task =>
                {
                    var length = BitConverter.ToInt32(messageSizeBuffer, 0);
                    //Console.WriteLine("Receive Message Size = {0}", length);
                    _readSize.Value = length;
                }).Wait(cts.Token);
            }
            catch (TaskCanceledException e)
            {
                //ignore
                //Exception?.Invoke(this, e);
            }
            catch (OperationCanceledException e)
            {
                //ignore - canceled to write or dispose
                //Console.WriteLine("ReadMessageSize was canceled");
            }
            catch (System.IO.IOException e) //Pipe closed or shut down?
            {
                //System.Diagnostics.Debug.WriteLine("SynchronizedReadWrite.GetMessageSize Exception: {0}", e.Message);
                Exception?.Invoke(this, e);
            }
            catch (Exception e)
            {
                Exception?.Invoke(this, e);
            }
            finally
            {
                //Console.WriteLine("Leaving GetMessageSize");
            }
        }
        private void WriteProc()
        {
            try
            {
                if (_writeQueue.IsEmpty)
                    return;
                byte[] item;
                if (_writeQueue.TryDequeue(out item))
                {
                    WriteMessage(Stream.Value, item, 256);
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
        }

        private void StartReadMessageSizeGate()
        {
            _gateReadMessageSize = new Threading.Gate(async cts =>
            {
                //Console.WriteLine("-> ReadMessageSizeGate");
                try
                {
                    await GetMessageSize(Stream.Value, cts);
                    if (_readSize.Value <= 0) //read was cancelled (either to write or dispose)
                    {
                        if (!Stream.Value?.IsConnected ?? false)
                            return;
                    }
                    if (cts == null || cts.IsCancellationRequested)
                        return;
                    _gateReadMessage.Run(); //read the message
                    _gateReadMessage.Join();
                }
                catch (TaskCanceledException e)
                {
                    //ignore
                }
                catch (OperationCanceledException e)
                {
                    //ignore - the read was cancelled
                }
                catch (AggregateException e)
                {
                    Exception?.Invoke(this, e);
                }
                catch (Exception e)
                {
                    Exception?.Invoke(this, e);
                }
                finally
                {
                    //Console.WriteLine("<- ReadMessageSizeGate");
                }
            },
            (b, exception) =>
            {
                if (exception != null)
                    Exception?.Invoke(this, exception);
            });
        }
        private void StartReadMessageGate()
        {
            _gateReadMessage = new Threading.Gate(cts =>
            {
                //Console.WriteLine("-> ReadMessageGate");
                try
                {
                    if (_readSize.Value > 0)
                    {
                        var bytes = ReadMessage(Stream.Value, _readSize.Value, 256);
                        if (bytes != null)
                        {
                            var formatted = FormatReceivedMessage(bytes);
                            MessageReceived?.Invoke(this, formatted);
                        }
                    }
                }
                catch (OperationCanceledException e)
                {
                    //ignore - the read was cancelled
                }
                catch (AggregateException e)
                {
                    Exception?.Invoke(this, e);
                }
                catch (Exception e)
                {
                    Exception?.Invoke(this, e);
                }
                //Console.WriteLine("<- ReadMessageGate");
            },
            (b, exception) =>
            {
                if (exception != null)
                    Exception?.Invoke(this, exception);
            });
        }
        private void StartWriteGate()
        {
            _gateWrite = new W.Threading.Gate(cts =>
            {
                //Console.WriteLine("-> WriteGate");
                try
                {
                    while (!_writeQueue.IsEmpty)
                        WriteProc(); //writes all queued messages
                }
                catch (OperationCanceledException e)
                {
                    //ignore - the read was cancelled
                }
                catch (AggregateException e)
                {
                    Exception?.Invoke(this, e);
                }
                catch (System.IO.IOException e)
                {
                    Exception?.Invoke(this, e);
                }
                //Console.WriteLine("<- WriteGate");
            }, (b, exception) =>
            {
                if (exception != null)
                    Exception?.Invoke(this, exception);
            });
        }

        /// <summary>
        /// Constructs a Synchronizer
        /// </summary>
        public void Initialize(PipeStream stream, bool isServerSide)
        {
            IsServerSide = isServerSide;
            Stream.Value = stream;
            StartReadMessageSizeGate();
            StartReadMessageGate();
            StartWriteGate();

            _gateReadMessageSize.Run();

            _watchThread = W.Threading.Thread.Create(cts =>
            {
                while (!cts?.IsCancellationRequested ?? false)
                {
                    if (Stream.Value != null && !Stream.Value.IsConnected)
                    {
                        OnDisconnected();
                    }
                    if (!_writeQueue.IsEmpty)
                    {
                        _gateReadMessageSize.Cancel();
                        //Console.WriteLine("Joining gateReadMessageSize");
                        _gateReadMessageSize.Join(); // wait 1000ms for debugging purposes
                        _gateWrite.Run();
                        _gateWrite.Join(); //join until it completes
                        _gateReadMessageSize.Run();  //restart listening for messages
                    }
                    System.Threading.Thread.Sleep(1); //play nice with other threads
                }
            });
        }

        /// <summary>
        /// Disposes the Synchronizer and releases resources
        /// </summary>
        ~PipeTransceiver()
        {
            Dispose();
        }

        /// <summary>
        /// Queues a message to send over the pipe
        /// </summary>
        /// <param name="message"></param>
        public void Write(TDataType message)
        {
            var formatted = FormatMessageToSend(message);
            _writeQueue.Enqueue(formatted);
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

        #region IDisposable Support
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        protected virtual void OnDispose()
        {
            _watchThread?.Dispose();
            _watchThread = null;

            _gateReadMessageSize?.Cancel();
            _gateReadMessageSize?.Dispose();
            _gateReadMessageSize = null;

            _gateReadMessage?.Cancel();
            _gateReadMessage?.Dispose();
            _gateReadMessage = null;

            _gateWrite?.Cancel();
            _gateWrite?.Dispose();
            _gateWrite = null;

            //_stream = null; //disposed by PipeClient
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            OnDispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}