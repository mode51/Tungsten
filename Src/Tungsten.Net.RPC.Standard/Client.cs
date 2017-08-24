using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using W.Logging;
using W.Net;

namespace W.Net.RPC
{
    /// <summary>
    /// Supports calling methods exposed by a Tungsten.Net.RPC Server
    /// </summary>
    /// <remarks>Greatly simplified implementation of W.Net.RPC.Client</remarks>
    public class Client
    {
        private SecureClient<Message> _client;
        private bool _useCompression = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public RPCHandle Call(string methodName, params object[] args)
        {
            if (_client == null || (!_client?.Socket?.IsConnected ?? false))
                throw new IOException("Socket is not connected");
            var msg = new Message();
            msg.Method = methodName;
            if (args != null)
                msg.Parameters.AddRange(args);
            if (msg.Id == Guid.Empty)
                msg.Id = Guid.NewGuid();
            msg.ExpireDateTime = DateTime.Now.AddMilliseconds(10000); //10 second expiration

            Log.i("W.Net.RPC: Attempting to call {0} with {1} parameters", methodName, args?.Length);
            var handler = new RPCHandle(_client, msg);
            try
            {
                _client.Send(msg);
            }
            catch (Exception e)
            {
                handler.CallException = e;
            }
            return handler;
        }

        /// <summary>
        /// Called when a connection has been established
        /// </summary>
        public Action<Client, IPEndPoint> Connected { get; set; }
        /// <summary>
        /// Called when the connection has been terminated
        /// </summary>
        public Action<Client, IPEndPoint, Exception> Disconnected { get; set; }
        /// <summary>
        /// True if the client is connected to the server, otherwise False
        /// </summary>
        public bool IsConnected => _client?.Socket?.IsConnected ?? false;

        /// <summary>
        /// The remote IPEndPoint for this socket
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteEndPoint">The IP address and port of the Tungsten RPC Server</param>
        /// <returns>A Task specifying success/failure of the connection</returns>
        public async Task<bool> ConnectAsync(IPEndPoint remoteEndPoint)
        {
            //8.14.2017 - moved this into the task
            //if (_client?.Socket?.IsConnected ?? false)
            //    return true;
            return await Task.Run(async () =>
            {
                //8.14.2017 - moved the below check into the task
                if (_client?.Socket?.IsConnected ?? false)
                    return true;
                //try
                //{
                RemoteEndPoint = remoteEndPoint;
                _client = new SecureClient<Message>();
                _client.Connected += (s, ep) => { Connected?.Invoke(this, ep); };
                _client.Disconnected += (s, ep, e) => { Disconnected?.Invoke(this, ep, e); };
                var result = await _client.Socket.ConnectAsync(remoteEndPoint.Address, remoteEndPoint.Port);
                //if (result)
                //    result = _client.WaitForConnected(60000);
                if (result && _client.Socket.UseCompression != _useCompression)
                    _client.Socket.UseCompression = _useCompression;
                return result;
                //}
                //catch (TaskCanceledException) { }
                //catch (TimeoutException) { }
                //return false;
            });
        }

        internal void Disconnect(Exception e)
        {
            if (_client == null)
                return;
            RemoteEndPoint = null;
            _client?.Socket.Disconnect(e);
            _client = null;
        }
        /// <summary>
        /// Disconnects the socket from the server
        /// </summary>
        public void Disconnect()
        {
            Disconnect(null);
        }
        /// <summary>
        /// Constructs a new Client class
        /// </summary>
        /// <param name="useCompression">If True, data compression will be used during transmission.</param>
        /// <remarks>The server must be declared with the same value.</remarks>
        public Client(bool useCompression = true)
        {
            _useCompression = useCompression;
        }
    }
}
