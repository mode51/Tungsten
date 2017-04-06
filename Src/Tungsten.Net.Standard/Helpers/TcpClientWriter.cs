using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using W.Logging;

namespace W.Net
{
    /// <summary>
    /// Encapsulates safe TcpClient writing.  Supports data larger than the SendBufferSize.
    /// </summary>
    public class TcpClientWriter
    {
        private readonly NetworkStream _stream;
        private readonly int _sendBufferSize;
        private W.Threading.Thread _thread;
        private readonly ConcurrentQueue<byte[]> _sendQueue = new ConcurrentQueue<byte[]>();

        /// <summary>
        /// Delegate called whenever a message is successfully sent
        /// </summary>
        public Action OnMessageSent; //use as multi-cast delegate
        /// <summary>
        /// Delegate called whenever an exception occurs
        /// </summary>
        public Action<Exception> OnException; //use as multi-cast delegate

        /// <summary>
        /// Constructs the TcpClientWriter
        /// </summary>
        /// <param name="client">The currently connected TcpClient</param>
        public TcpClientWriter(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (!client.Connected)
                throw new ArgumentOutOfRangeException(nameof(client), "The TcpClient must be connected to a remote server");
            _stream = client.GetStream();
            _sendBufferSize = client.SendBufferSize;
        }
        /// <summary>
        /// Constructs the TcpClientWriter
        /// </summary>
        /// <param name="stream">The currently connected NetworkStream</param>
        /// <param name="sendBufferSize">The SendBufferSize, in bytes; used to send the data in chunks</param>
        public TcpClientWriter(NetworkStream stream, int sendBufferSize)
        {
            _stream = stream;
            _sendBufferSize = sendBufferSize;
        }

        /// <summary>
        /// Starts sending 
        /// </summary>
        public void Start()
        {
            Stop();
            _thread = Threading.Thread.Create(ThreadProc);
        }
        /// <summary>
        /// Stops sending data (this may leave messages unsent)
        /// </summary>
        public void Stop()
        {
            _thread?.Cancel();
            _thread = null;
        }
        /// <summary>
        /// Enqueues data to be sent.
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            _sendQueue.Enqueue(data);
        }

        private void ThreadProc(CancellationTokenSource cts)
        {
            try
            {
                while (!cts?.IsCancellationRequested ?? false)
                {
                    if (!_sendQueue.IsEmpty && _stream != null)
                    {
                        byte[] message;
                        if (_sendQueue.TryDequeue(out message))
                        {
                            TcpHelpers.SendMessageAsync(_stream, _sendBufferSize, message, exception =>
                                {
                                    if (exception != null)
                                        OnException?.Invoke(exception);
                                    else
                                        OnMessageSent?.Invoke();
                                }, cts).Wait();
                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
                //cts?.Dispose();
            }
            catch (Exception e)
            {
                Log.e("MessageReader Thread Exception: {0}", e.Message);
            }
            _thread = null;
        }
    }
}