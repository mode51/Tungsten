using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using W;
using W.AsExtensions;
using W.Logging;
using W.Net;
using W.Net.SocketExtensions;

namespace W.Net.RPC
{
    /// <summary>
    /// Encapsulates information related to making the RPC call and the return value
    /// </summary>
    public class RPCResponse
    {
        /// <summary>
        /// The name of the method called
        /// </summary>
        public string Method { get; set; } = "";
        /// <summary>
        /// True if the call was successful, otherwise False
        /// </summary>
        /// <remarks>Note that this is different than the return value from the method, which can be of any value or type</remarks>
        public bool Success { get; set; } = false;
        /// <summary>
        /// The return value from the method
        /// </summary>
        public object Response { get; set; } = null;
        /// <summary>
        /// May contain exception information if there was an exception making or as a result of the call
        /// </summary>
        public string Exception { get; set; } = "";

        /// <summary>
        /// Useful for debugging or displaying information quickly.
        /// </summary>
        /// <returns>Returns a string representation of class members and their values</returns>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, "Name = " + Method, "Success = " + Success.ToString(), "Exception = " + Exception, "Response = " + Response?.ToString());
        }
    }
    /// <summary>
    /// Encapsulates information related to making the RPC call and the return value
    /// </summary>
    /// <typeparam name="TResponseType">The Type expected as a return value from the method call</typeparam>
    public class RPCResponse<TResponseType> : RPCResponse
    {
        /// <summary>
        /// The return value from the method
        /// </summary>
        public new TResponseType Response
        {
            get
            {
                return (TResponseType)base.Response;
            }
            set
            {
                base.Response = value;
            }
        }
    }

    /// <summary>
    /// Make calls into a Tungsten.Net.RPC.Server over Tcp
    /// </summary>
    /// <remarks>
    /// <para>Assymetric encryption is used to discourage sniffing.</para>
    /// <para>The server must be a valid instance of W.Net.RPC.Server</para>
    /// </remarks>
    public class Client : IDisposable
    {
        private Tcp.Generic.SecureTcpClient<Message> _client;// = new Tcp.Generic.SecureTcpClient<Message>(2048);

        /// <summary>
        /// The IPEndPoint of the server (The server must be a valid instance of W.Net.RPC.Server)
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; set; }

        /// <summary>
        /// The maximum amount of time, in milliseconds, that a call should wait for a response
        /// </summary>
        public int CallTimeout { get; set; }

        private CallResult<Message> MakeTheCallAndWait(string methodName, params object[] args)
        {
            var result = new CallResult<Message>();
            if (_client == null || (!_client?.Socket.Connected ?? false))
                throw new IOException("Socket is not connected");

            var msg = new Message();
            msg.Method = methodName;
            if (args != null)
                msg.Parameters.AddRange(args);
            if (msg.Id == Guid.Empty)
                msg.Id = Guid.NewGuid();
            msg.ExpireDateTime = DateTime.Now.AddMilliseconds(10000); //10 second expiration

            Log.i("W.Net.RPC: Attempting to call {0} with {1} parameters", methodName, args?.Length);
            try
            {
                if (_client.Socket.SendAndWaitForResponse(ref msg, out Message response, CallTimeout))
                {
                    result.Result = response;
                    result.Success = true;
                }
                else
                    result.Exception = new Exception("Call timed out");
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
            return result;
        }
        private void GetResult<TResponseType>(ref RPCResponse<TResponseType> result, object response, string exception)
        {
            try
            {
                var token = response as Newtonsoft.Json.Linq.JToken;
                //handle object deserialization
                if (token != null)
                    result.Response = token.ToObject<TResponseType>();
                //attempt exact type match deserialization (not sure we'll ever get here)
                else if (response is TResponseType)
                    result.Response = (TResponseType)response;
                //Newtonsoft always converts Int32 to Int64 when serializing (so we have to handle this case)
                else if (response is Int64 && typeof(TResponseType) == typeof(Int32))
                    result.Response = (TResponseType)Convert.ChangeType(response, typeof(TResponseType));

                result.Exception = exception;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                result.Response = default(TResponseType);
                result.Exception = e.Message;
            }
        }

        /// <summary>
        /// Calls a method on the server
        /// </summary>
        /// <param name="methodName">The name of the method to call</param>
        /// <param name="args">Any arguments to pass into the method</param>
        /// <returns>RPCResponse containing information related to the call and the return value</returns>
        public async Task<RPCResponse> CallAsync(string methodName, params object[] args)
        {
            return await Task.Run(() => { return Call(methodName, args); });
        }
        /// <summary>
        /// Calls a method on the server
        /// </summary>
        /// <param name="methodName">The name of the method to call</param>
        /// <param name="args">Any arguments to pass into the method</param>
        /// <returns>RPCResponse containing information related to the call and the return value</returns>
        public async Task<RPCResponse<TResponseType>> CallAsync<TResponseType>(string methodName, params object[] args)
        {
            return await Task.Run(() => { return Call<TResponseType>(methodName, args); });
        }
        /// <summary>
        /// Calls a method on the server
        /// </summary>
        /// <param name="methodName">The name of the method to call</param>
        /// <param name="args">Any arguments to pass into the method</param>
        /// <returns>RPCResponse containing information related to the call and the return value</returns>
        public RPCResponse Call(string methodName, params object[] args)
        {
            // MakeTheCallAndWait(methodName, args);
            var result = Call<object>(methodName, args);
            //var response = new CallResponse();
            //response.Method = result.Method;
            //response.Success = result.Success;
            //response.Response = result.Response;
            //response.Exception = result.Exception;
            return result;
        }
        /// <summary>
        /// Calls a method on the server
        /// </summary>
        /// <param name="methodName">The name of the method to call</param>
        /// <param name="args">Any arguments to pass into the method</param>
        /// <returns>RPCResponse containing information related to the call and the return value</returns>
        public RPCResponse<TResponseType> Call<TResponseType>(string methodName, params object[] args)
        {
            var response = new RPCResponse<TResponseType>() { Method = methodName };
            
            //make sure we make the call on the right server
            if ((_client?.Socket.Connected ?? false) && (_client.Socket.RemoteEndPoint?.ToString() != RemoteEndPoint?.ToString()))
            {
                _client.Dispose();
                _client = null;
            }

            try
            {
                //make sure we're connected
                if (!_client?.Socket.Connected ?? true)
                {
                    if (RemoteEndPoint == null)
                    {
                        response.Exception = "Server IPEndPoint has not been specified";
                        return response;
                    }
                    _client.Connect(RemoteEndPoint);
                    {
                        response.Exception = "Failed to connect to the server";
                        return response;
                    }
                }
            }
            catch (Exception e)
            {
                response.Exception = e.Message;
                return response;
            }

            var result = MakeTheCallAndWait(methodName, args);
            response.Method = methodName;
            response.Success = result.Success;
            response.Exception = result.Exception?.ToString() ?? result.Result?.Exception?.ToString();
            try
            {
                GetResult<TResponseType>(ref response, result.Result?.Response, result.Result?.Exception);
                //response.Response = (TResponseType)result.Result?.Response;
            }
            catch (InvalidCastException e)
            {
                response.Exception = e.Message;
            }
            return response;
        }

        /// <summary>
        /// Disposes the Client and release resources
        /// </summary>
        public void Dispose()
        {
            _client?.Dispose();
            _client = null;
        }
        /// <summary>
        /// Constructs a new Client
        /// </summary>
        public Client()
        {
            CallTimeout = -1;
        }
        /// <summary>
        /// Constructs a new Client, initialized with the specified values
        /// </summary>
        public Client(IPEndPoint remoteEndPoint, int msCallTimeout = -1)
        {
            RemoteEndPoint = remoteEndPoint;
            CallTimeout = msCallTimeout;
        }
    }

    ///// <summary>
    ///// Supports calling methods exposed by a Tungsten.Net.RPC Server
    ///// </summary>
    ///// <remarks>Greatly simplified implementation of W.Net.RPC.Client</remarks>
    //public class Client
    //{
    //    private SecureClient<Message> _client;
    //    private bool _useCompression = true;

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="methodName"></param>
    //    /// <param name="args"></param>
    //    /// <returns></returns>
    //    public RPCHandle Call(string methodName, params object[] args)
    //    {
    //        if (_client == null || (!_client?.IsConnected ?? false))
    //            throw new IOException("Socket is not connected");
    //        var msg = new Message();
    //        msg.Method = methodName;
    //        if (args != null)
    //            msg.Parameters.AddRange(args);
    //        if (msg.Id == Guid.Empty)
    //            msg.Id = Guid.NewGuid();
    //        msg.ExpireDateTime = DateTime.Now.AddMilliseconds(10000); //10 second expiration

    //        Log.i("W.Net.RPC: Attempting to call {0} with {1} parameters", methodName, args?.Length);
    //        var handler = new RPCHandle(_client, msg);
    //        try
    //        {
    //            _client.Send(ref msg);
    //        }
    //        catch (Exception e)
    //        {
    //            handler.CallException = e;
    //        }
    //        return handler;
    //    }

    //    /// <summary>
    //    /// Called when a connection has been established
    //    /// </summary>
    //    public Action<Client, IPEndPoint> Connected { get; set; }
    //    /// <summary>
    //    /// Called when the connection has been terminated
    //    /// </summary>
    //    public Action<Client, IPEndPoint, Exception> Disconnected { get; set; }
    //    /// <summary>
    //    /// True if the client is connected to the server, otherwise False
    //    /// </summary>
    //    public bool IsConnected => _client?.IsConnected ?? false;

    //    /// <summary>
    //    /// The remote IPEndPoint for this socket
    //    /// </summary>
    //    public IPEndPoint RemoteEndPoint { get; private set; }

    //    /// <summary>
    //    /// Attempts to connect to a local or remote Tungsten RPC Server
    //    /// </summary>
    //    /// <param name="remoteEndPoint">The IP address and port of the Tungsten RPC Server</param>
    //    /// <returns>A Task specifying success/failure of the connection</returns>
    //    public async Task<bool> ConnectAsync(IPEndPoint remoteEndPoint)
    //    {
    //        //8.14.2017 - moved this into the task
    //        //if (_client?.Socket?.IsConnected ?? false)
    //        //    return true;
    //        return await Task.Run(async () =>
    //        {
    //            //8.14.2017 - moved the below check into the task
    //            if (_client?.IsConnected ?? false)
    //                return true;
    //            //try
    //            //{
    //            RemoteEndPoint = remoteEndPoint;
    //            _client = new SecureClient<Message>();
    //            _client.Connected += (s, ep) => { Connected?.Invoke(this, ep); };
    //            _client.Disconnected += (s, ep, e) => { Disconnected?.Invoke(this, ep, e); };
    //            var result = await _client.ConnectAsync(remoteEndPoint);
    //            //if (result)
    //            //    result = _client.WaitForConnected(60000);
    //            if (result && _client.UseCompression != _useCompression)
    //                _client.UseCompression = _useCompression;
    //            return result;
    //            //}
    //            //catch (TaskCanceledException) { }
    //            //catch (TimeoutException) { }
    //            //return false;
    //        });
    //    }

    //    internal void Disconnect(Exception e)
    //    {
    //        if (_client == null)
    //            return;
    //        RemoteEndPoint = null;
    //        _client?.Disconnect(e);
    //        _client = null;
    //    }
    //    /// <summary>
    //    /// Disconnects the socket from the server
    //    /// </summary>
    //    public void Disconnect()
    //    {
    //        Disconnect(null);
    //    }
    //    /// <summary>
    //    /// Constructs a new Client class
    //    /// </summary>
    //    /// <param name="useCompression">If True, data compression will be used during transmission.</param>
    //    /// <remarks>The server must be declared with the same value.</remarks>
    //    public Client(bool useCompression = true)
    //    {
    //        _useCompression = useCompression;
    //    }
    //}
}
