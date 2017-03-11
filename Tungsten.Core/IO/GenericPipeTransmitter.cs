using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace W.Pipes
{
    /// <summary>
    /// Generic PipeStream client.  Supports sending and receiving classes.
    /// </summary>
    /// <typeparam name="TType">The class type to send and receive as serialized json</typeparam>
    public class GenericPipeClient<TType> : IDisposable where TType : class
    {
        private Lockable<PipeStream> Stream { get; } = new Lockable<PipeStream>();

        /// <summary>
        /// Used to send and receive messages
        /// </summary>
        public GenericPipeTransmitter<TType> Transmitter { get; private set; }
        public SynchronizedReadWrite ReadWriter { get; private set; }

        /// <summary>
        /// Constructs a new PipeBase
        /// </summary>
        /// <param name="pipeName">The name of the pipe</param>
        /// <param name="direction">The direction of the stream (read, write or both)</param>
        public bool Connect(string pipeName, PipeDirection direction = PipeDirection.InOut, Action<GenericPipeClient<TType>, Exception> onException = null)
        {
            Disconnect();
            var client = TryConnect(pipeName, direction, onException);
            if (client == null)
                return false;
            Stream.Value = client;
            return true;
        }
        /// <summary>
        /// Disconnects from the server and releases resources
        /// </summary>
        public void Disconnect()
        {
            Stream.Value?.Dispose();
            Stream.Value = null;
        }
        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <remarks>Used when creating a client and calling Connect</remarks>
        public GenericPipeClient() { }

        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <param name="stream">A previously connected PipeStream</param>
        /// <remarks>Calling Connect will disconnect this PipeStream</remarks>
        public GenericPipeClient(PipeStream stream)
        {
            //Transmitter = new GenericPipeTransmitter<TType>(stream);
            ReadWriter = new SynchronizedReadWrite(stream);
            Stream.Value = stream;
        }
        /// <summary>
        /// Disposes the Pipe
        /// </summary>
        ~GenericPipeClient()
        {
            Dispose();
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Disconnect();
        }

        private NamedPipeClientStream TryConnect(string pipeName, PipeDirection direction, Action<GenericPipeClient<TType>, Exception> onException = null)
        {
            try
            {
                var client = new NamedPipeClientStream(".", pipeName, direction);
                client.Connect(1000);
                client.ReadMode = System.IO.Pipes.PipeTransmissionMode.Byte; //it appears this must be done immediately after connecting
                //Transmitter = new GenericPipeTransmitter<TType>(client);
                ReadWriter = new SynchronizedReadWrite(client);
                return client;
            }
            catch (TimeoutException e)
            {
                onException?.Invoke(this, e);
            }
            catch (System.IO.IOException e)
            {
                onException?.Invoke(this, e);
            }
            catch (Exception e)
            {
                onException?.Invoke(this, e);
            }
            return null;
        }
    }

    ///// <summary>
    ///// Transmits strings
    ///// </summary>
    //public class StringPipeTransmitter : GenericPipeTransmitter<string> {
    //    /// <summary>
    //    /// Override to provide specific conversion/formatting functionality
    //    /// </summary>
    //    /// <param name="message">The binary representation of the message</param>
    //    /// <returns>The message after conversion from a binary array</returns>
    //    protected override string FormatReceivedMessage(byte[] message)
    //    {
    //        if (message == null || message.Length == 0)
    //            return null;
    //        return message.AsString();
    //    }
    //    /// <summary>
    //    /// Override to provide specific conversion/formatting functionality
    //    /// </summary>
    //    /// <param name="message">The message to convert to a binary array</param>
    //    /// <returns>A binary representation of the message</returns>
    //    protected override byte[] FormatMessageToSend(string message)
    //    {
    //        return string.IsNullOrEmpty(message) ? null : message.AsBytes();
    //    }

    //    /// <summary>
    //    /// Constructs a new PipeTransmitter
    //    /// </summary>
    //    /// <param name="stream"></param>
    //    /// <param name="cts"></param>
    //    public StringPipeTransmitter(PipeStream stream, CancellationTokenSource cts = null) : base(stream, cts)
    //    {
    //    }
    //}
    /// <summary>
    /// A class which handles sending and receiving data via a PipeStream
    /// </summary>
    public class GenericPipeTransmitter<TType> : IDisposable where TType : class
    {
        private W.Threading.Thread _readThread = null;

        /// <summary>
        /// The PipeStream on which to send and receive data
        /// </summary>
        protected PipeStream Stream;
        /// <summary>
        /// If cancelled, any Send or WaitForResponse is 
        /// </summary>
        protected CancellationTokenSource Cts;

        /// <summary>
        /// Called if an exception occurs
        /// </summary>
        public Action<object, Exception> Exception { get; set; }

        /// <summary>
        /// Override to provide specific conversion/formatting functionality
        /// </summary>
        /// <param name="message">The binary representation of the message</param>
        /// <returns>The message after conversion from a binary array</returns>
        protected virtual TType FormatReceivedMessage(byte[] message)
        {
            if (message == null || message.Length == 0)
                return default(TType);
            if (typeof(TType) == typeof(string))
                return (TType)(object)message.AsString().FromBase64();
            var msg = message.AsString().FromBase64();
            return msg.FromJson<TType>();
        }
        /// <summary>
        /// Override to provide specific conversion/formatting functionality
        /// </summary>
        /// <param name="message">The message to convert to a binary array</param>
        /// <returns>A binary representation of the message</returns>
        protected virtual byte[] FormatMessageToSend(TType message)
        {
            if (typeof(TType) == typeof(string))
                return string.IsNullOrEmpty((string)(object)message) ? null : ((string)(object)message).AsBase64().AsBytes();
            return message?.AsJson<TType>().AsBase64().AsBytes();
        }

        /// <summary>
        /// Reads a message from the pipe
        /// </summary>
        /// <returns>The message as a string</returns>
        public TType Read()
        {
            //if (Cts == null || Cts.IsCancellationRequested)
            //    Cts = new CancellationTokenSource();
            return Read(Stream);
        }
        /// <summary>
        /// Reads a message from the pipe
        /// </summary>
        /// <param name="stream">The stream on which to read data</param>
        /// <returns>The message as a string</returns>
        public TType Read(PipeStream stream)
        {
            var size = GetMessageSize(stream);
            if (size == 0)
                return default(TType);
            var bytes = ReadMessage(stream, size, 256);
            var msg = FormatReceivedMessage(bytes);
            return msg;
        }

        /// <summary>
        /// Reads a message from the pipe
        /// </summary>
        /// <returns>The message as a string</returns>
        public TType ReadMessage()
        {
            return ReadMessage(Stream);
        }
        /// <summary>
        /// Reads a message from the pipe
        /// </summary>
        /// <param name="stream">The stream on which to read data</param>
        /// <returns>The message as a string</returns>
        public TType ReadMessage(PipeStream stream)
        {
            if (Cts == null || Cts.IsCancellationRequested)
                Cts = new CancellationTokenSource();
            var result = new byte[0];
            int resultLength = 0;
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            try
            {
                int inBufferSize = 256;//stream.InBufferSize == 0 ? 256 : stream.InBufferSize;
                var buffer = new byte[inBufferSize];
                for (int t = 0; t < 4; t++)
                    stream.ReadByte();
                do
                {
                    int count = stream.Read(buffer, 0, buffer.Length);
                    Array.Resize(ref result, resultLength + count);
                    Array.Copy(buffer, resultLength, result, resultLength, count);
                    resultLength += count;
                } while (!stream.IsMessageComplete);

                do
                {
                    int count = stream.Read(buffer, 0, buffer.Length);
                    Array.Resize(ref result, resultLength + count);
                    Array.Copy(buffer, resultLength, result, resultLength, count);
                    resultLength += count;
                } while (!stream.IsMessageComplete);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return default(TType);
            }
            return FormatReceivedMessage(result);
            //return FormatReceivedMessage(result.ToString().AsBytes());        
        }

        /// <summary>
        /// Writes a message to the PipeStream
        /// </summary>
        /// <param name="message">The message to write</param>
        public void Send(TType message)
        {
            if (Cts == null || Cts.IsCancellationRequested)
                Cts = new CancellationTokenSource();
            Send(Stream, message, Cts);
        }
        /// <summary>
        /// Writes a message to the PipeStream in chunks of 256 bytes or less
        /// </summary>
        /// <param name="stream">A connected PipeStream on which to write the message</param>
        /// <param name="message">The message to write to the connected PipeStream</param>
        /// <param name="cts">A CancellationTokenSource which can be used to cancel the operation</param>
        public void Send(System.IO.Pipes.PipeStream stream, TType message, CancellationTokenSource cts)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            try
            {
                var bytes = FormatMessageToSend(message);
                int length = bytes.Length;

                //send the size
                var sizeBuffer = BitConverter.GetBytes(length); //4 bytes
                Stream.Write(sizeBuffer, 0, 4);

                int outBufferSize = 256;//stream.OutBufferSize == 0 ? 256 : stream.OutBufferSize;
                int numberOfBytesSent = 0;
                while (!(cts?.IsCancellationRequested ?? false) && numberOfBytesSent < length)
                {
                    var bytesToSend = 0;
                    if (numberOfBytesSent < length)
                        bytesToSend = Math.Min(outBufferSize, length - numberOfBytesSent);
                    stream.Write(bytes, numberOfBytesSent, bytesToSend);
                    stream.WaitForPipeDrain();
                    numberOfBytesSent += bytesToSend;
                }
                //if (numberOfBytesSent == length)
                //    return true;
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

        /// <summary>
        /// Cancels 
        /// </summary>
        public void Cancel()
        {
            Cts?.Cancel();
            Cts = null;
        }

        /// <summary>
        /// Constructs a new PipeTransmitter
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="cts"></param>
        public GenericPipeTransmitter(PipeStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            Stream = stream;
            Cts = new CancellationTokenSource();
            //_readThread = W.Threading.Thread.Create(ReadProc);
        }

        /// <summary>
        /// Disposes the PipeTransmitter
        /// </summary>
        ~GenericPipeTransmitter()
        {
            Dispose();
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Cancel();
            _readThread?.Cancel();
            _readThread = null;
        }

        private int GetMessageSize(PipeStream stream)
        {
            int length = 0;
            int position = 0;
            var messageSizeBuffer = new byte[4];

            while (!(Cts?.IsCancellationRequested ?? false))
            {
                try
                {
                    var b = stream.Read(messageSizeBuffer, position, 1);
                    //var b = ReadMessage(position, 1);
                    if (b == 1)
                    {
                        position += 1;
                        if (position == 4)
                        {
                            length = BitConverter.ToInt32(messageSizeBuffer, 0);
                            break;
                        }
                    }
                }
                catch (System.IO.IOException e) //TcpClient closed or shut down
                {
                    System.Diagnostics.Debug.WriteLine("GenericPipeTransmitter.GetMessageSize Exception: {0}", e.Message);
                    Exception?.Invoke(this, e);
                    break;
                }
                Thread.Sleep(1); //play nice with other threads
            }
            if (length > 0)
                System.Diagnostics.Debug.WriteLine("Receive Message Size = {0}", length);
            return length;
        }
        private byte[] ReadMessage(PipeStream stream, int length, int receiveBufferSize)
        {
            var buffer = new byte[length];
            var numberOfBytesRead = 0;
            while (!(Cts?.IsCancellationRequested ?? false) && numberOfBytesRead < length)
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
        private int ReadMessage(int numberOfBytesRead, int bytesToRead)
        {
            var buffer = new byte[bytesToRead];
            try
            {
                //var read = await Stream.ReadAsync(buffer, numberOfBytesRead, bytesToRead, Cts.Token);
                var read = Stream.Read(buffer, numberOfBytesRead, bytesToRead);
                return read;
            }
            catch (System.IO.IOException e)
            {
                Exception?.Invoke(this, e);
            }
            catch (Exception e)
            {
                Exception?.Invoke(this, e);
            }
            return 0;
        }

        ///// <summary>
        ///// Called when a message has been received from the server
        ///// </summary>
        //public Action<GenericPipeTransmitter<TType>, TType> MessageReceived { get; set; }

        //private async void ReadProc(CancellationTokenSource cts)
        //{
        //    while (!cts.IsCancellationRequested)
        //    {
        //        if (Stream != null && Stream.IsConnected && (!CtsRead.Value?.IsCancellationRequested ?? false))
        //        {
        //            var msg = await Read();
        //            if (msg != null)
        //                MessageReceived?.Invoke(this, msg);
        //        }
        //        System.Threading.Thread.Sleep(1); //play nice with other threads
        //    }
        //}
    }
}
