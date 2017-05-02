using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using W.Logging;
using W.Threading;

namespace W.Net
{
    /// <summary>
    /// Encapsulates safe TcpClient reading.  Supports data larger than the ReceiveBufferSize.
    /// </summary>
    public class TcpClientReader
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly int _receiveBufferSize;
        private readonly ConcurrentQueue<byte[]> _messages = new ConcurrentQueue<byte[]>();
        private W.Threading.Thread _thread;
        private W.Threading.Thread _messageHandlerThread;

        /// <summary>
        /// Delegate called whenever an exception occurs
        /// </summary>
        public Action<Exception> OnException; //use as multi-cast delegate
        /// <summary>
        /// Delegate called when a message is received
        /// </summary>
        public Action<byte[]> OnMessageReceived; //use as multi-cast delegate

        /// <summary>
        /// Constructs the TcpClientReader
        /// </summary>
        /// <param name="client"></param>
        public TcpClientReader(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (!client.Connected)
                throw new ArgumentOutOfRangeException(nameof(client), "The TcpClient must be connected to a remote server");
            _client = client;
            _stream = client?.GetStream();
            _receiveBufferSize = client?.ReceiveBufferSize ?? 8192;
        }
        /// <summary>
        /// Starts waiting for messages
        /// </summary>
        public void Start()
        {
            Stop();
            _messageHandlerThread = new W.Threading.Thread(MessageReceivedHandlerProc);
            _thread = W.Threading.Thread.Create(ThreadProc);
        }
        /// <summary>
        /// Stops waiting for messages
        /// </summary>
        public void Stop()
        {
            _messageHandlerThread?.Cancel();
            _messageHandlerThread = null;

            _thread?.Cancel();
            _thread = null;
        }

        private void ThreadProc(CancellationTokenSource cts)
        {
            try
            {
                while (!cts?.IsCancellationRequested ?? false)
                {
                    if (TcpHelpers.IsMessageAvailable(_client, OnException))
                    {
                        TcpHelpers.ReadMessageAsync(_stream, _receiveBufferSize, (bytes, exception) =>
                        {
                            if (exception != null)
                                OnException?.Invoke(exception);
                            if (bytes != null)
                                _messages.Enqueue(bytes);
                        }, cts).Wait();
                    }
                    W.Threading.Thread.Sleep(1); //play nice with other threads
                }
                //cts.Dispose();
            }
            catch (Exception e)
            {
                W.Logging.Log.e("MessageReader Exception: {0}", e.Message);
            }
            _thread = null;
        }
        private void MessageReceivedHandlerProc(CancellationTokenSource cts)
        {
            try
            {
                while (!cts?.IsCancellationRequested ?? false)
                {
                    byte[] message;
                    if (_messages.TryDequeue(out message))
                    {
                        if (message != null)
                        {
                            try
                            {
                                OnMessageReceived?.Invoke(message);
                            }
                            catch (Exception e)
                            {
                                Log.e(e);
                            }
                        }
                    }
                    W.Threading.Thread.Sleep(1); //play nice with other threads
                }
                //cts.Dispose();
            }
            catch (Exception e)
            {
                Log.e("MessageReader Exception: {0}", e.Message);
            }
        }
    }
}