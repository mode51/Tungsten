using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using W.Logging;

namespace W.RPC
{
    public class Client
    {
        private EncryptedClient<Message> _client;
        private IPAddress _remoteAddress;

        public delegate void ConnectedDelegate(Client client, IPAddress remoteAddress);
        public event ConnectedDelegate Connected;
        public delegate void DisconnectedDelegate(Client client, Exception exception);
        public event DisconnectedDelegate Disconnected;
        public Lockable<bool> IsConnected = new Lockable<bool>(false);

        public ManualResetEvent MakeRPCCall<T>(string methodName, Action<T> onResponse)
        {
            return MakeRPCCall(methodName, onResponse, null);
        }
        public ManualResetEvent MakeRPCCall<T>(string methodName, Action<T> onResponse, params object[] args)
        {
            var msg = new Message();
            var mre = new ManualResetEvent(false);
            msg.Method = methodName;
            if (args != null)
                msg.Parameters.AddRange(args);

            _client.Post(msg, (client, response, isExpired) =>
            {
                T result = default(T);
                if (response?.Response != null)
                {
                    var token = response.Response as JToken;
                    if (token != null)
                        result = token.ToObject<T>();
                    else if (response.Response is T)
                        result = (T)response.Response;
                }
                onResponse?.Invoke(result);
                mre.Set(); //this needs to happen after the call to onResponse
            }, 30000);
            return mre;
        }
        /// <summary>
        /// Not sure I should keep this method.  Shouldn't all RPC calls have a result?  Otherwise, the client wouldn't know if it succeeded.
        /// </summary>
        /// <param name="onResponse"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public ManualResetEvent MakeRPCCall(string methodName, Action onResponse, params object[] args)
        {
            var msg = new Message();
            var mre = new ManualResetEvent(false);
            msg.Method = methodName;
            if (args != null)
                msg.Parameters.AddRange(args);

            _client.Post(msg, (client, response, isExpired) =>
            {
                onResponse?.Invoke();
                mre.Set(); //this needs to happen after the call to onResponse
            }, 30000);
            return mre;
        }

        public void Disconnect()
        {
            Disconnect(null);
        }
        internal void Disconnect(Exception e)
        {
            _client.Disconnect(e);
        }
        internal CallResult Connect(System.Net.Sockets.TcpClient client) //called by Server
        {
            if (IsConnected.Value)
                return new CallResult(true);

            var result = _client.Connect(client);
            if (result.Success)
            {
                if (!_client.IsSecure.WaitForChanged(10000)) //default timeout
                {
                    Log.w("Connection timed out while securing");
                    Disconnect(new Exception("Server failed to secure the connection"));
                }
                else
                    Log.i("Client Connected");
            }
            else
                Disconnect(new Exception("Server failed to respond"));
            return result;
        }
        public CallResult Connect(string remoteAddress, int remotePort, int msTimeout = 10000)
        {
            if (IsConnected.Value)
                return new CallResult(true);

            var result = _client.Connect(remoteAddress, remotePort, msTimeout);
            if (result.Success)
            {
                if (!_client.IsSecure.WaitForChanged(msTimeout))
                {
                    Log.w("Connection timed out while securing");
                    Disconnect(new Exception("Server failed to secure the connection"));
                }
                else
                    Log.i("Client Connected");
            }
            else
                Disconnect(new Exception("Server failed to respond"));
            return result;
        }
        public CallResult Connect(IPAddress remoteAddress, int remotePort, int msTimeout = 10000)
        {
            if (IsConnected.Value)
                return new CallResult(true);
            return Connect(remoteAddress.ToString(), remotePort, msTimeout);
        }
        public async Task ConnectAsync(string remoteAddress, int remotePort, int msTimeout = 10000)
        {
            if (IsConnected.Value)
                return;
            await _client.ConnectAsync(remoteAddress, remotePort, msTimeout).ContinueWith(f => _client.IsSecure.WaitForChanged(msTimeout));
        }
        public async Task ConnectAsync(IPAddress remoteAddress, int remotePort, int msTimeout = 10000)
        {
            if (IsConnected.Value)
                return;
            await _client.ConnectAsync(remoteAddress, remotePort, msTimeout).ContinueWith(f => _client.IsSecure.WaitForChanged(msTimeout));
        }

        public Client()
        {
            _client = new EncryptedClient<Message>();
            _client.Connected += (client, address) =>
            {
                //don't care when we connect, we care when we're secured
                _remoteAddress = address;
            };
            _client.Secured += client =>
            {
                IsConnected.Value = true;
                var evt = this.Connected;
                evt?.Invoke(this, _remoteAddress);
            };
            _client.Disconnected += (client, address, exception) =>
            {
                IsConnected.Value = false;

                Log.i("Client Disconnected: {0}", client.Name);
                if (exception != null)
                    Log.i("Disconnect Reason: {0}", exception.Message);
                var evt = this.Disconnected;
                evt?.Invoke(this, exception);
            };
        }
    }
}