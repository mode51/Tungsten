using System;
using System.Net;
using System.Net.Sockets;

namespace W.Net.Sockets
{
    /// <summary>
    /// Allows the programmer to customize data before sending and after reception
    /// </summary>
    /// <typeparam name="TDataType">The type of data</typeparam>
    public class FormattedClient<TDataType> : IFormattedSocket, IDisposable where TDataType : class
    {
        /// <summary>
        /// The underlying Tungsten Socket
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Called when a connection has been established
        /// </summary>
        public Action<object, IPEndPoint> Connected { get; set; }
        /// <summary>
        /// Called when the connection has been terminated
        /// </summary>
        public Action<object, IPEndPoint, Exception> Disconnected { get; set; }
        /// <summary>
        /// Called when data has been received and formatted
        /// </summary>
        public Action<object, TDataType> MessageReceived { get; set; }
        /// <summary>
        /// Called after data has been formatted and sent
        /// </summary>
        public Action<object> MessageSent { get; set; }

        /// <summary>
        /// Can be useful for large data sets.  Set to True to use compression, otherwise False.
        /// </summary>
        /// <remarks>Make sure both server and client have the same value</remarks>
        public bool UseCompression
        {
            get
            {
                return Socket.UseCompression;
            }
            set
            {
                Socket.UseCompression = value;
            }
        }

        /// <summary>
        /// Constructs a new FormattedSocket
        /// </summary>
        public FormattedClient()
        {
            this.Socket = new Socket();
            HookNotifications();
        }
        /// <summary>
        /// Constructs a new FormattedSocket.  Used internally by W.Net.Sockets.Server and W.Net.Sockets.SecureServer.
        /// </summary>
        /// <param name="client">An existing connected TcpClient</param>
        public FormattedClient(TcpClient client) //used by SecureServer
        {
            this.Socket = new Socket(client);
            HookNotifications();
        }
        /// <summary>
        /// Disposes and deconstructs the FormattedSocket instance
        /// </summary>
        ~FormattedClient()
        {
            Dispose();
        }
        private void HookNotifications()
        {
            Socket.Connected += (socket, remoteEndPoint) =>
            {
                Connected?.Invoke(this, remoteEndPoint);
            };
            Socket.Disconnected += (socket, remoteEndPoint, exception) =>
            {
                Disconnected?.Invoke(this, remoteEndPoint, exception);
            };
            Socket.MessageReceived += (s, message) =>
            {
                var msg = FormatReceivedMessage(message);
                if (msg != null)
                    MessageReceived?.Invoke(this, msg);
            };
            Socket.MessageSent += socket =>
            {
                MessageSent?.Invoke(this);
            };
        }

        /// <summary>
        /// Override to customize received data before exposing it via the MessageReceived callback
        /// </summary>
        /// <param name="message">The received data</param>
        /// <returns>The original or modified data</returns>
        protected virtual TDataType FormatReceivedMessage(byte[] message)
        {
            return message.As<TDataType>();
        }
        /// <summary>
        /// Override to customize the data before transmission
        /// </summary>
        /// <param name="message">The unaltered data to send</param>
        /// <returns>The original or modified data</returns>
        protected virtual byte[] FormatMessageToSend(TDataType message)
        {
            return message.As<byte[]>();
        }
        /// <summary>
        /// Queues data to send
        /// </summary>
        /// <param name="message"></param>
        /// <param name="immediate"></param>
        public void Send(TDataType message, bool immediate = false)
        {
            var msg = FormatMessageToSend(message);
            Socket.Send(msg, immediate);
        }
        /// <summary>
        /// Queues data to send
        /// </summary>
        /// <param name="message"></param>
        /// <param name="immediate"></param>
        public void Send(byte[] message, bool immediate = false)
        {
            var msg = FormatMessageToSend(message.As<TDataType>());
            Socket.Send(message, immediate);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Socket?.Dispose();
            Socket = null;
            GC.SuppressFinalize(this);
        }
    }
}