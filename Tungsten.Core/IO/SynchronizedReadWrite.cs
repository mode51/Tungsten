using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace W.Pipes
{
    /// <summary>
    /// Synchronizes two threads
    /// </summary>
    public class SynchronizedReadWrite : IDisposable
    {
        private PipeStream _stream;
        private Lockable<int> _readSize = new Lockable<int>(0);

        private W.Threading.Thread _watchThread;
        private W.Threading.Gate _gateReadMessageSize;
        private W.Threading.Gate _gateReadMessage;
        private W.Threading.Gate _gateWrite;

        private readonly ConcurrentQueue<byte[]> _writeQueue = new ConcurrentQueue<byte[]>();

        public Action<object, Exception> Exception { get; set; }
        public Action<object, byte[]> MessageReceived { get; set; }

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
                    Debug.WriteLine("GenericPipeTransmitter.ReadMessage Exception: {0}", e.Message);
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

                Thread.Sleep(1); //play nice with other threads
            }
            return numberOfBytesRead == length ? buffer : null;
        }
        private async void GetMessageSize(PipeStream stream, CancellationTokenSource cts)
        {
            try
            {
                var messageSizeBuffer = new byte[4];
                _readSize.Value = 0;
                var b = await stream.ReadAsync(messageSizeBuffer, 0, 4, cts.Token);
                //stream.ReadAsync(messageSizeBuffer, 0, 4, cts.Token).Wait();
                {
                    var length = BitConverter.ToInt32(messageSizeBuffer, 0);
                    System.Diagnostics.Debug.WriteLine("Receive Message Size = {0}", length);
                    _readSize.Value = length;
                }
                //await stream.ReadAsync(messageSizeBuffer, 0, 4, cts.Token).ContinueWith(task =>
                //{
                //    var length = BitConverter.ToInt32(messageSizeBuffer, 0);
                //    _readSize.Value = length;
                //});
            }
            catch (TaskCanceledException e)
            {
                //ignore
                //Exception?.Invoke(this, e);
            }
            catch (OperationCanceledException e)
            {
                //ignore - canceled to write or dispose
            }
            catch (System.IO.IOException e) //Pipe closed or shut down?
            {
                System.Diagnostics.Debug.WriteLine("SynchronizedReadWrite.GetMessageSize Exception: {0}", e.Message);
                Exception?.Invoke(this, e);
            }
            catch (Exception e)
            {
                Exception?.Invoke(this, e);
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
                    WriteMessage(_stream, item, 256);
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
            _gateReadMessageSize = new Threading.Gate(cts =>
            {
                try
                {
                    GetMessageSize(_stream, cts);
                    if (_readSize.Value <= 0) //read was cancelled (either to write or dispose)
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
                try
                {
                    var bytes = ReadMessage(_stream, _readSize.Value, 256);
                    if (bytes != null)
                    {
                        MessageReceived?.Invoke(this, bytes);
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
                try
                {
                    while (!_writeQueue.IsEmpty)
                        WriteProc(); //writes all queued messages
                    _gateReadMessageSize.Run();
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
            }, (b, exception) =>
            {
                if (exception != null)
                    Exception?.Invoke(this, exception);
            });
        }

        /// <summary>
        /// Constructs a Synchronizer
        /// </summary>
        public SynchronizedReadWrite(PipeStream stream)
        {
            _stream = stream;
            StartReadMessageSizeGate();
            StartReadMessageGate();
            StartWriteGate();

            _gateReadMessageSize.Run();

            _watchThread = W.Threading.Thread.Create(cts =>
            {
                while (!cts?.IsCancellationRequested ?? false)
                {
                    if (!_writeQueue.IsEmpty)
                    {
                        _gateReadMessageSize.Cancel();
                        _gateReadMessageSize.Join(1000); // wait 1000ms for debugging purposes
                        _gateWrite.Run();
                    }
                    System.Threading.Thread.Sleep(1); //play nice with other threads
                }
            });
        }

        /// <summary>
        /// Disposes the Synchronizer and releases resources
        /// </summary>
        ~SynchronizedReadWrite()
        {
            Dispose();
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _watchThread?.Dispose();
            _watchThread = null;

            _gateReadMessageSize?.Dispose();
            _gateReadMessageSize = null;

            _gateReadMessage?.Dispose();
            _gateReadMessage = null;

            _gateWrite?.Dispose();
            _gateWrite = null;
        }

        //public bool IsEmpty
        //{
        //    get { return _readQueue.Count == 0; }
        //}
        //public byte[] Read()
        //{
        //    if (_readQueue.IsEmpty)
        //        return default(byte[]);
        //    byte[] result = default(byte[]);
        //    return _readQueue.TryDequeue(out result) ? result : default(byte[]);
        //}
        public void Write(byte[] message)
        {
            _writeQueue.Enqueue(message);
        }
    }
}