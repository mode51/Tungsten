using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections.Concurrent;

namespace W.Net
{
    public class ClientBase
    {
        protected TcpClient TcpClient { get; set; }
        protected TcpClientReader StreamReader { get; set; }
        protected TcpClientWriter StreamWriter { get; set; }
        protected NetworkStream NetworkStream { get; set; }
        protected ConcurrentQueue<byte[]> ReceivedMessages { get; } = new ConcurrentQueue<byte[]>();
        protected ulong SentMessageCount { get; set; }

        protected virtual byte[] FormatReceivedMessage(byte[] receivedBytes)
        {
            return receivedBytes;
        }
        protected virtual byte[] FormatMessageToSend(byte[] bytesToSend)
        {
            return bytesToSend;
        }

        /// <summary>
        /// Gets or sets a Name property
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The remote IPEndPoint for this socket
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; protected set; }
    }

    public partial class ClientSlim //Events/Delegates
    {
        /// <summary>
        /// Called when the client connects to the server
        /// </summary>
        public Action<ClientSlim, IPEndPoint> Connected { get; set; }
        /// <summary>
        /// Called when the client disconnects from the server
        /// </summary>
        public Action<object, IPEndPoint, Exception> Disconnected { get; set; }

        /// <summary>
        /// Called when a message has arrived
        /// </summary>
        public Action<ClientSlim> MessageReady { get; set; }
    }

    public partial class ClientSlim //public properties
    {
        public bool UseCompression { get; set; }
        public bool IsConnected { get; protected set; }
    }

    public partial class ClientSlim //private methods
    {
        private void HandleMessageReceived(byte[] bytes)
        {
            var data = FormatReceivedMessage(bytes);
            ReceivedMessages.Enqueue(data);
            //Task.Run(() => { MessageReady?.Invoke(this); });
            MessageReady?.Invoke(this);
        }
        private void FinalizeConnection(IPEndPoint remoteEndPoint)
        {
            NetworkStream = TcpClient.GetStream();
            StreamReader = new TcpClientReader(TcpClient);
            StreamReader.OnMessageReceived += HandleMessageReceived;
            StreamReader.Start();

            StreamWriter = new TcpClientWriter(TcpClient);
            StreamWriter.Start();

            Name = RemoteEndPoint.ToString();
        }
    }
    public partial class ClientSlim //protected methods
    {
        protected override byte[] FormatReceivedMessage(byte[] receivedBytes)
        {
            if (UseCompression)
            {
                var decompressed = receivedBytes.FromCompressed();
                var bytes = base.FormatReceivedMessage(decompressed);
                return bytes;
            }
            return receivedBytes;
        }
        protected override byte[] FormatMessageToSend(byte[] bytesToSend)
        {
            if (UseCompression)
            {
                var compressed = bytesToSend.AsCompressed();
                var bytes = base.FormatMessageToSend(compressed);
                return bytes;
            }
            return bytesToSend;
        }
        protected virtual async Task OnConnectedAsync()
        {
            FinalizeConnection(RemoteEndPoint);
            IsConnected = true;
            if (Connected != null)
                //await Task.Run(() => Connected?.Invoke(this, RemoteEndPoint));
                Connected?.Invoke(this, RemoteEndPoint);
        }
        protected virtual async Task OnDisconnectedAsync(IPEndPoint remoteEndPoint, Exception e = null)
        {
            IsConnected = false;
            if (Disconnected != null)
                //await Task.Run(() => Disconnected?.Invoke(this, remoteEndPoint, e));
                Disconnected?.Invoke(this, remoteEndPoint, e);
        }
    }
    public partial class ClientSlim //public methods
    {
        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <returns>A bool specifying success/failure</returns>
        /// <remarks>If an exception occurs, the Disconnected delegate will be called with the specific exception</remarks>
        /// <code>
        /// await ConnectAsync(ep);
        /// </code>
        public virtual async Task ConnectAsync(IPEndPoint remoteEndPoint)
        {
            Exception ex = null;
            try
            {
                RemoteEndPoint = remoteEndPoint;
                TcpClient = new TcpClient();

                await TcpClient.ConnectAsync(RemoteEndPoint.Address, RemoteEndPoint.Port).ContinueWith(async task =>
                {
                    if (task.IsCanceled || task.IsFaulted)
                    {
                        await OnDisconnectedAsync(RemoteEndPoint, task.Exception);
                        return;
                    }
                    if (TcpClient.Connected)
                    {
                        await OnConnectedAsync();
                    }
                });
            }
            catch (ArgumentNullException e) //the address parameter is null
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine(string.Format("Argument Null Exception: {0}", e.Message));
            }
            catch (ArgumentOutOfRangeException e) //the port is not between MinPort and MaxPort
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine(string.Format("Argument Out of Range Exception: {0}", e.Message));
            }
            catch (SocketException e) //an error occured while accessing the socket.
            {
                ex = e;
                var errorCode = Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode);
                System.Diagnostics.Debug.WriteLine(string.Format("Socket Exception({0}): {1}", errorCode, e.Message));
            }
            catch (ObjectDisposedException e) //TcpClient is closed
            {
                ex = e;
                System.Diagnostics.Debug.WriteLine(string.Format("Object Disposed Exception: {0}", e.Message));
            }
            if (ex != null)
            {
                await DisconnectAsync(ex);
            }
        }
        /// <summary>
        /// Disconnects from the remote server and cleans up resources
        /// </summary>
        /// <code>
        /// await Disconnect();
        /// </code>
        public virtual async Task DisconnectAsync()
        {
            await DisconnectAsync(null);
        }

        /// <summary>
        /// Disconnects from the remote server and cleans up resources
        /// </summary>
        /// <param name="e">An exception if one occurred</param>
        /// <code>
        /// await Disconnect(new Exception("Some error ocurred"));
        /// </code>
        public virtual async Task DisconnectAsync(Exception e)
        {
            await Task.Run(async () =>
            {
                if (TcpClient == null)
                    return;

                StreamReader?.Stop();
                StreamReader = null;

                StreamWriter?.Stop();
                StreamWriter = null;

                NetworkStream?.Dispose();
                NetworkStream = null;

#if NETSTANDARD1_3
            TcpClient?.Dispose();
#else
                TcpClient?.Close();
#endif
                TcpClient = null;

                await OnDisconnectedAsync(RemoteEndPoint, e);
                RemoteEndPoint = null;
            });
        }

        /// <summary>
        /// Returns the number of received messages which haven't been dequeued
        /// </summary>
        public int MessageCount
        {
            get
            {
                return ReceivedMessages.Count;
            }
        }
        /// <summary>
        /// Dequeues a received message, if one is available
        /// </summary>
        /// <returns>The next message in the receive queue</returns>
        public byte[] GetNextMessage()
        {
            byte[] bytes;
            if (ReceivedMessages.TryDequeue(out bytes))
                return bytes;
            return null;
        }
        /// <summary>
        /// Dequeues a received message, if one is available, converts it to a string (assumes it's json), and deserializes the json into an object of type TItemType
        /// </summary>
        /// <returns>The next message in the receive queue</returns>
        public TItemType GetNextMessage<TItemType>()
        {
            var bytes = GetNextMessage();
            var json = bytes.AsString();
            var item = Newtonsoft.Json.JsonConvert.DeserializeObject<TItemType>(json);
            return item;
        }

        public async Task<bool> WaitForMessageReady(int msTimeout = -1)
        {
            System.Threading.CancellationTokenSource cts;
            if (msTimeout > 0)
                cts = new System.Threading.CancellationTokenSource(msTimeout);
            else
                cts = new System.Threading.CancellationTokenSource();
            var result = await Task.Run(() =>
            {
                while (MessageCount == 0)
                {
                    W.Threading.Thread.Sleep(1);
                }
            }, cts.Token).ContinueWith(task =>
            {
                return (!task.IsCanceled);
            });
            return result;
        }

        /// <summary>
        /// Sends data to the remote
        /// </summary>
        /// <param name="bytes">The data to send</param>
        public virtual void Send(byte[] bytes)
        {
            var data = FormatMessageToSend(bytes);
            SentMessageCount += 1;
            StreamWriter.Send(new SocketData() { Id = SentMessageCount, Data = data });
        }
        /// <summary>
        /// Serializes the item and sends it to the remote.
        /// </summary>
        /// <param name="bytes">The data to send</param>
        public virtual void Send<T>(T item) where T : class
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            var bytes = json.AsBytes();
            Send(bytes);
        }
    }

    public partial class ClientSlim : ClientBase, ISocketClient, IDisposable //constructors and finalizer
    {
        /// <summary>
        /// Constructs a SocketSlim
        /// </summary>
        public ClientSlim() : base()
        {
        }
        /// <summary>
        /// Constructs a SocketSlim
        /// </summary>
        /// <param name="tcpClient">A handle to an existing TcpClient</param>
        public ClientSlim(TcpClient tcpClient) : this()
        {
            if (tcpClient == null)
                throw new ArgumentNullException(nameof(tcpClient), "Client must not be null and must already be connected");
            if (!tcpClient.Connected)
                throw new ArgumentOutOfRangeException(nameof(tcpClient), "Client must already be connected");
            TcpClient = tcpClient;
            var ep = TcpClient.Client.RemoteEndPoint.As<IPEndPoint>();
            if (ep != null)
                RemoteEndPoint = new IPEndPoint(ep.Address, ep.Port);
            if (TcpClient != null)
                OnConnectedAsync().Wait();
        }

        /// <summary>
        /// Disposes and deconstructs the Socket instance
        /// </summary>
        ~ClientSlim()
        {
            Dispose();
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public virtual void Dispose()
        {
            DisconnectAsync().Wait();
            GC.SuppressFinalize(this);
        }
    }

    public partial class SecureClientSlim : ClientSlim
    {
        private W.Encryption.RSA _rsa;
        private RSAParameters? _remotePublicKey = null;

        protected override byte[] FormatMessageToSend(byte[] bytesToSend)
        {
            if (_remotePublicKey != null && _rsa != null)
            {
                var cipher = _rsa.Encrypt(bytesToSend, (RSAParameters)_remotePublicKey);
                var bytes = cipher.AsBytes();
                return base.FormatMessageToSend(bytes);
            }
            else
                return base.FormatMessageToSend(bytesToSend);
        }
        protected override byte[] FormatReceivedMessage(byte[] receivedBytes)
        {
            if (_rsa != null)
            {
                var cipher = receivedBytes.AsString();
                var decrypted = _rsa.Decrypt(cipher);
                var bytes = decrypted.AsBytes();
                return base.FormatReceivedMessage(bytes);
            }
            else
                return base.FormatMessageToSend(receivedBytes);
        }

        private void SendPublicKey()
        {
            var publicKey = Newtonsoft.Json.JsonConvert.SerializeObject(_rsa.PublicKey);
            var bytes = publicKey.AsBytes();
            Send(bytes);
        }
        private async Task<bool> SecureTheConnection()
        {
            SendPublicKey();
            var ready = await WaitForMessageReady(10000);
            if (ready)
            {
                var data = GetNextMessage();
                var json = data.AsString();
                _remotePublicKey = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(json);
            }
            return ready;
        }
        public SecureClientSlim() : base()
        {
            base.Connected += (client, remoteEndPoint) =>
            {
                _rsa = new Encryption.RSA();
                SecureTheConnection().Wait();
            };
            base.Disconnected += (client, remoteEndPoint, exception) =>
            {
                _remotePublicKey = null;
            };
        }
        public SecureClientSlim(TcpClient tcpClient, W.Encryption.RSA rsa) : base(tcpClient)
        {
            _rsa = rsa; //this won't work because the base class constructor will call OnConnected before this is assigned
            SecureTheConnection().Wait();
        }
    }
}