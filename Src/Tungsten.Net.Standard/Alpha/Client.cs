using System;
using System.Net;
using System.Net.Sockets;
using W.AsExtensions;
using W.DelegateExtensions;

#if NET45
using System.Security.Principal;
#endif

namespace W.Net.Alpha
{
    /// <summary>
    /// The base class for a network client
    /// </summary>
    public class ClientBase<TClient> : SocketBase<TClient> where TClient : ClientBase<TClient>
    {
        //private System.Net.Sockets.Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Connects to a server
        /// </summary>
        /// <param name="ipEndPoint">The server IP address and port of the server</param>
        /// <returns>True if a connection was established, otherwise False</returns>
        public bool Connect(IPEndPoint ipEndPoint)
        {
            return Connect(ipEndPoint, -1);
        }
        /// <summary>
        /// Connects to a server
        /// </summary>
        /// <param name="address">The IP address of the server</param>
        /// <param name="port">The port on which the server is listening</param>
        /// <param name="msTimeout">The number of milliseconds to wait for a connection before timing out</param>
        /// <returns>True if a connection was established, otherwise False</returns>
        public bool Connect(string address, int port, int msTimeout = -1)
        {
            return Connect(new IPEndPoint(IPAddress.Parse(address), port), msTimeout);
        }
        /// <summary>
        /// Connects to a server
        /// </summary>
        /// <param name="ipEndPoint">The IP address and port on which the server is listening</param>
        /// <param name="msTimeout">The number of milliseconds to wait for a connection before timing out</param>
        /// <returns>True if a connection was established, otherwise False</returns>
        public bool Connect(IPEndPoint ipEndPoint, int msTimeout = -1)
        {
            //11.15.2017 - So I'm using TcpClient because reconnecting isn't really an option once the socket's been disconnected (particulary in NetStandard)
            // in NET45, a reconnection requires BeginConnect, but this isn't available for NetStandard
            // So for simplicity, I'll just create a new TcpClient for each connect

            System.Threading.Monitor.Enter(this);
            try
            {
                if (IsConnected)
                    return true;

                var result = false;
                var client = new System.Net.Sockets.TcpClient();

                try
                {
                    if (msTimeout == -1)
                        client.ConnectAsync(ipEndPoint.Address, ipEndPoint.Port).Wait();
                    else
                        client.ConnectAsync(ipEndPoint.Address, ipEndPoint.Port).Wait(msTimeout);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
                if (client.Connected)
                {
                    result = client.Connected;
                    base.InitializeConnection(client.Client);
                }
                else
                {
                    if (client.Client.Connected)
                        client.Client.Shutdown(SocketShutdown.Both);
                    client.Client.Dispose();
#if NETSTANDARD1_3
                    client.Dispose();
#elif NET45
                    client.Close();
#endif
                }
                return result;
            }
            finally
            {
                System.Threading.Monitor.Exit(this);

            }
        }

        ///// <summary>
        ///// Disconnects from the server
        ///// </summary>
        //public new void Disconnect()
        //{
        //    base.Disconnect();
        //}
        /// <summary>
        /// Constructs a new ClientBase
        /// </summary>
        internal ClientBase()
        {
        }
    }

    /// <summary>
    /// A Client which can send/receive array sof bytes
    /// </summary>
    public class Client : ClientBase<Client>
    {
        /// <summary>
        /// Constructs a new Client
        /// </summary>
        public Client() : base()
        {
        }
    }
    /// <summary>
    /// A generic Client which can send/receive serializable objects
    /// </summary>
    /// <typeparam name="TMessage">The Type of object which will be send and received</typeparam>
    public class Client<TMessage> : ClientBase<Client<TMessage>> where TMessage : class, new()
    {
        /// <summary>
        /// Called when a new message has been received
        /// </summary>
        public event Action<Client<TMessage>, TMessage> MessageReceived;

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="message">The object to be serialized and written to the network</param>
        public void Write(TMessage message)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            var bytes = json.AsBytes();
            base.Write(bytes);
        }
        /// <summary>
        /// Constructs a new Client
        /// </summary>
        public Client() : base()
        {
            BytesReceived += (client, bytes) =>
            {
                var json = bytes.AsString();
                var message = Newtonsoft.Json.JsonConvert.DeserializeObject<TMessage>(json);
                //MessageReceived?.BeginInvoke(this, message, ar => MessageReceived.EndInvoke(ar), null);
                MessageReceived?.Raise(this, message);
            };
        }
    }
}
