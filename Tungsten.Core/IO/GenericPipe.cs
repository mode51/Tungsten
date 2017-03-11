using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W.Pipes
{
    /// <summary>
    /// A Pipe reader which uses Json to serialize and deserialize data
    /// </summary>
    /// <typeparam name="TType">The class type used in serialization</typeparam>
    public class GenericPipeReader<TType> : GenericPipeBase<TType> where TType : class
    {
        private CancellationTokenSource _cts;
        private W.Threading.Thread _thread = null;
        private readonly byte[] _readBuffer = new byte[256];

        /// <summary>
        /// Called when a message has been received
        /// </summary>
        public Action<GenericPipeReader<TType>, TType> MessageReceived { get; set; }

        /// <summary>
        /// Constructs a new PipeBase
        /// </summary>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="direction">The direction of the stream (read, write or both)</param>
        /// <param name="mode">The readmode for the pipe</param>
        public override bool Connect(string pipeName, PipeDirection direction = PipeDirection.In, PipeTransmissionMode mode = PipeTransmissionMode.Message)
        {
            base.Connect(pipeName, direction, mode);
            Initialize();
            return true;
        }
        /// <summary>
        /// Disconnects from the server and releases resources
        /// </summary>
        public override void Disconnect()
        {
            _cts?.Cancel();
            _cts = null;
            _thread?.Dispose();
            _thread = null;
            base.Disconnect();
        }

        private void Initialize()
        {
            _cts = new CancellationTokenSource();
            _thread = W.Threading.Thread.Create(ReadMessagesProc, (b, exception) =>
            {
                if (exception != null)
                    Exception?.Invoke(this, exception);
            });
        }
        private TType Read()
        {
            var result = new StringBuilder();
            try
            {
                do
                {
                    int count = 0;
                    try
                    {
                        Stream.ExecuteInLock(s =>
                        {
                            //count = Stream.Value.Read(buffer, 0, buffer.Length);
                            //Stream.Value.ReadAsync(_readBuffer, 0, _readBuffer.Length, cts.Token).Result;
                            using (var cts = new CancellationTokenSource(10))
                                count = Stream.Value.ReadAsync(_readBuffer, 0, _readBuffer.Length, cts.Token).Result;
                        });
                    }
                    catch (TaskCanceledException)
                    {
                        //ignore
                    }
                    catch (AggregateException e)
                    {
                        //Exception?.Invoke(this, e);
                    }
                    catch (Exception e)
                    {
                        Exception?.Invoke(this, e);
                    }
                    if (count > 0)
                        result.Append(_readBuffer.AsString(0, count));
                    else
                    {
                        //Console.WriteLine("Looping");
                        break;
                    }
                } while (!Stream.Value.IsMessageComplete);
            }
            catch (Exception e)
            {
                Exception?.Invoke(this, e);
                return null;
            }
            if (typeof(TType) == typeof(string))
                return result.Length> 0 ? (TType)(object)result.ToString() : null;
            return result.ToString().FromJson<TType>();
        }
        private void ReadMessagesProc(CancellationTokenSource cts)
        {
            while (!cts.IsCancellationRequested)
            {
                try
                {
                    var msg = Read();
                    if (msg != null)
                        MessageReceived?.Invoke(this, msg);
                }
                catch (Exception e)
                {
                    Exception?.Invoke(this, e);
                }
                System.Threading.Thread.Sleep(1); //play nice with other threads
            }
        }

        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <remarks>Used when creating a client and calling Connect</remarks>
        public GenericPipeReader() : base() { }

        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <param name="stream">A previously connected PipeStream</param>
        /// <remarks>Calling Connect will disconnect this PipeStream</remarks>
        public GenericPipeReader(PipeStream stream) : base(stream)
        {
            Initialize();
        }
    }

        /// <summary>
    /// A Pipe writer which uses Json to serialize and deserialize data
    /// </summary>
    /// <typeparam name="TType">The class type used in serialization</typeparam>
    public class GenericPipeWriter<TType> : GenericPipeBase<TType> where TType : class
    {
        /// <summary>
        /// Writes a message to the PipeStream in chunks of 256 bytes or less
        /// </summary>
        /// <param name="message">The message to write to the connected PipeStream</param>
        public void Send(TType message)
        {
            var bytes = message.AsJson<TType>().AsBytes();
            int length = bytes.Length;
            int outBufferSize = 256;// stream.OutBufferSize == 0 ? 256 : stream.OutBufferSize;
            int numberOfBytesSent = 0;
            while (numberOfBytesSent < length)
            {
                var bytesToSend = 0;
                if (numberOfBytesSent < length)
                    bytesToSend = Math.Min(outBufferSize, length - numberOfBytesSent);
                try
                {
                    Stream.ExecuteInLock(s =>
                    {
                        Stream.Value.Write(bytes, numberOfBytesSent, bytesToSend);
                        Stream.Value.WaitForPipeDrain();
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
                numberOfBytesSent += bytesToSend;
            }
            //if (numberOfBytesSent == length)
            //    return true;
        }

        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <remarks>Used when creating a client and calling Connect</remarks>
        public GenericPipeWriter() : base() { }

        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <param name="stream">A previously connected PipeStream</param>
        /// <remarks>Calling Connect will disconnect this PipeStream</remarks>
        public GenericPipeWriter(PipeStream stream) : base(stream) { }
    }

    /// <summary>
    /// A Pipe which uses Json to serialize and deserialize data
    /// </summary>
    /// <typeparam name="TType">The class type used in serialization</typeparam>
    public class GenericPipeBase<TType> : IDisposable where TType : class
    {
        /// <summary>
        /// The PipeStream associated with this GenericPipe
        /// </summary>
        protected Lockable<PipeStream> Stream { get; } = new Lockable<PipeStream>();

        /// <summary>
        /// Called if an exception occurs
        /// </summary>
        public Action<object, Exception> Exception { get; set; }

        /// <summary>
        /// Constructs a new PipeBase
        /// </summary>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="direction">The direction of the stream (read, write or both)</param>
        public virtual bool Connect(string pipeName, PipeDirection direction = PipeDirection.Out, PipeTransmissionMode mode = PipeTransmissionMode.Message)
        {
            Disconnect();
            var client = TryConnect(pipeName, direction, mode);
            if (client == null)
                return false;
            Stream.Value = client;
            return true;
        }
        /// <summary>
        /// Disconnects from the server and releases resources
        /// </summary>
        public virtual void Disconnect()
        {
            Stream.Value?.Dispose();
            Stream.Value = null;
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public virtual void Dispose()
        {
            Disconnect();
        }

        private NamedPipeClientStream TryConnect(string pipeName, PipeDirection direction, PipeTransmissionMode mode = PipeTransmissionMode.Message)
        {
            try
            {
                var client = new NamedPipeClientStream(".", pipeName, direction);
                client.Connect(1000);
                client.ReadMode = mode; //it appears this must be done immediately after connecting
                return client;
            }
            catch (TimeoutException e)
            {
                Exception?.Invoke(this, e);
            }
            catch (System.IO.IOException e)
            {
                Exception?.Invoke(this, e);
            }
            catch (Exception e)
            {
                Exception?.Invoke(this, e);
            }
            return null;
        }

        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <remarks>Used when creating a client and calling Connect</remarks>
        public GenericPipeBase() { }

        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <param name="stream">A previously connected PipeStream</param>
        /// <remarks>Calling Connect will disconnect this PipeStream</remarks>
        public GenericPipeBase(PipeStream stream)
        {
            Stream.Value = stream;
        }
        /// <summary>
        /// Disposes the Pipe
        /// </summary>
        ~GenericPipeBase()
        {
            Dispose();
        }
    }

        /// <summary>
    /// A Pipe which uses Json to serialize and deserialize data
    /// </summary>
    /// <typeparam name="TType">The class type used in serialization</typeparam>
    public class GenericPipe<TType> : IDisposable where TType : class
    {
        private CancellationTokenSource _cts;
        private W.Threading.Thread _thread = null;
        //const int inBufferSize = 256;//Stream.InBufferSize == 0 ? 256 : Stream.InBufferSize;
        private readonly byte[] _readBuffer = new byte[256];
        protected Lockable<PipeStream> Stream { get; } = new Lockable<PipeStream>();

        /// <summary>
        /// Called if an exception occurs
        /// </summary>
        public Action<GenericPipe<TType>, Exception> Exception { get; set; }
        /// <summary>
        /// Called when a message has been received
        /// </summary>
        public Action<GenericPipe<TType>, TType> MessageReceived { get; set; }

        /// <summary>
        /// Writes a message to the PipeStream in chunks of 256 bytes or less
        /// </summary>
        /// <param name="message">The message to write to the connected PipeStream</param>
        public void Send(TType message)
        {
            var bytes = message.AsJson<TType>().AsBytes();
            int length = bytes.Length;
            int outBufferSize = 256;// stream.OutBufferSize == 0 ? 256 : stream.OutBufferSize;
            int numberOfBytesSent = 0;
            while (numberOfBytesSent < length)
            {
                var bytesToSend = 0;
                if (numberOfBytesSent < length)
                    bytesToSend = Math.Min(outBufferSize, length - numberOfBytesSent);
                try
                {
                    Stream.ExecuteInLock(s =>
                    {
                        Stream.Value.Write(bytes, numberOfBytesSent, bytesToSend);
                        Stream.Value.WaitForPipeDrain();
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
                numberOfBytesSent += bytesToSend;
            }
            //if (numberOfBytesSent == length)
            //    return true;
        }

        /// <summary>
        /// Constructs a new PipeBase
        /// </summary>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="direction">The direction of the stream (read, write or both)</param>
        public bool Connect(string pipeName, PipeDirection direction = PipeDirection.InOut)
        {
            Disconnect();
            var client = TryConnect(pipeName, direction);
            if (client == null)
                return false;
            Stream.Value = client;
            Initialize();
            return true;
        }
        /// <summary>
        /// Disconnects from the server and releases resources
        /// </summary>
        public void Disconnect()
        {
            _cts?.Cancel();
            _cts = null;
            _thread?.Dispose();
            _thread = null;
            Stream.Value?.Dispose();
            Stream.Value = null;
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Disconnect();
        }

        private NamedPipeClientStream TryConnect(string pipeName, PipeDirection direction)
        {
            try
            {
                var client = new NamedPipeClientStream(".", pipeName, direction);
                client.Connect(1000);
                client.ReadMode = System.IO.Pipes.PipeTransmissionMode.Byte; //it appears this must be done immediately after connecting
                return client;
            }
            catch (TimeoutException e)
            {
                Exception?.Invoke(this, e);
            }
            catch (System.IO.IOException e)
            {
                Exception?.Invoke(this, e);
            }
            catch (Exception e)
            {
                Exception?.Invoke(this, e);
            }
            return null;
        }

        private void Initialize()
        {
            _cts = new CancellationTokenSource();
            _thread = W.Threading.Thread.Create(ReadMessagesProc, (b, exception) =>
            {
                if (exception != null)
                    Exception?.Invoke(this, exception);
            });

        }
        private TType Read()
        {
            var result = new StringBuilder();
            try
            {
                do
                {
                    int count = 0;
                    try
                    {
                        Stream.ExecuteInLock(s =>
                        {
                            //count = Stream.Value.Read(buffer, 0, buffer.Length);
                            //Stream.Value.ReadAsync(_readBuffer, 0, _readBuffer.Length, cts.Token).Result;
                            using (var cts = new CancellationTokenSource(10))
                                count = Stream.Value.Read(_readBuffer, 0, _readBuffer.Length);
                        });
                    }
                    catch (TaskCanceledException)
                    {
                        //ignore
                    }
                    catch (AggregateException e)
                    {
                        //Exception?.Invoke(this, e);
                    }
                    catch (Exception e)
                    {
                        Exception?.Invoke(this, e);
                    }
                    if (count > 0)
                        result.Append(_readBuffer.AsString(0, count));
                    else
                    {
                        //Console.WriteLine("Looping");
                        break;
                    }
                } while (!Stream.Value.IsMessageComplete);
            }
            catch (Exception e)
            {
                Exception?.Invoke(this, e);
                return null;
            }
            return result.ToString().FromJson<TType>();
        }
        private void ReadMessagesProc(CancellationTokenSource cts)
        {
            while (!cts.IsCancellationRequested)
            {
                try
                {
                    var msg = Read();
                    if (msg != null)
                        MessageReceived?.Invoke(this, msg);
                }
                catch (Exception e)
                {
                    Exception?.Invoke(this, e);
                }
                System.Threading.Thread.Sleep(1); //play nice with other threads
            }
        }

        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <remarks>Used when creating a client and calling Connect</remarks>
        public GenericPipe() { }

        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <param name="stream">A previously connected PipeStream</param>
        /// <remarks>Calling Connect will disconnect this PipeStream</remarks>
        public GenericPipe(PipeStream stream)
        {
            Stream.Value = stream;
            Initialize();
        }
        /// <summary>
        /// Disposes the Pipe
        /// </summary>
        ~GenericPipe()
        {
            Dispose();
        }
    }

}