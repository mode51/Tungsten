using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using W.Net;

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
        private Tcp.Generic.SecureTcpClient<RPCMessage> _client;// = new Tcp.Generic.SecureTcpClient<Message>(2048);
        private int _keySize;

        /// <summary>
        /// The IPEndPoint of the server (The server must be a valid instance of W.Net.RPC.Server)
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; set; }

        /// <summary>
        /// The maximum amount of time, in milliseconds, that a call should wait for a response
        /// </summary>
        public int CallTimeout { get; set; }

        private bool MakeTheCallAndWait(ref RPCMessage rpcMessage, ref Exception exception, string methodName, params object[] args)
        {
            var result = false;
            if (_client == null || (!_client?.Socket.Connected ?? false))
                throw new IOException("Socket is not connected");

            rpcMessage.Method = methodName;
            if (args != null)
                rpcMessage.Parameters.AddRange(args);
            if (rpcMessage.Id == Guid.Empty)
                rpcMessage.Id = Guid.NewGuid();
            //rpcMessage.ExpireDateTime = DateTime.Now.AddMilliseconds(30000); //10 second expiration

            //Log.i("W.Net.RPC: Attempting to call {0} with {1} parameters", methodName, args?.Length);
            try
            {
                //can't use the SocketExtensions because they don't go go through encryption
                _client.MessageReceived += _client_MessageReceived;
                _response = null;
                _client.Write(rpcMessage);
                _mreResponseReceived.Reset();
                if (_mreResponseReceived.Wait(CallTimeout))
                {
                    rpcMessage.Exception = _response.Exception;
                    rpcMessage.Response = _response.Response;
                    result = true;
                }
                else
                {
                    exception = new Exception("Call timed out");
                }

                //if (_client.Socket.SendAndWaitForResponse(ref rpcMessage, out RPCMessage response, CallTimeout))
                //{
                //    rpcMessage.Response = response;
                //    result = true;
                //}
                //else
                //    exception = new Exception("Call timed out");
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                _client.MessageReceived -= _client_MessageReceived;
            }
            return result;
        }
        private ManualResetEventSlim _mreResponseReceived = new ManualResetEventSlim(false);
        private RPCMessage _response;
        private void _client_MessageReceived(Tcp.Generic.SecureTcpClient<RPCMessage> client, RPCMessage response)
        {
            _response = response;
            _mreResponseReceived.Set();
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

                if (string.IsNullOrEmpty(result.Exception))
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
                if (!_client?.Socket?.Connected ?? true)
                {
                    if (RemoteEndPoint == null)
                    {
                        response.Exception = "Server IPEndPoint has not been specified";
                        return response;
                    }
                    if (_client == null)
                        _client = new Tcp.Generic.SecureTcpClient<RPCMessage>(_keySize);
                    _client.Connect(RemoteEndPoint);
                    if (!_client.Socket?.Connected ?? true)
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
            Exception ex = null;
            var rpcMessage = new RPCMessage();
            var result = MakeTheCallAndWait(ref rpcMessage, ref ex, methodName, args);
            response.Method = methodName;
            response.Success = result;
            response.Exception = ex?.ToString() ?? rpcMessage.Exception?.ToString();
            try
            {
                GetResult<TResponseType>(ref response, rpcMessage?.Response, rpcMessage?.Exception);
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
        /// <param name="encryptionKeySize">The encryption key size (typically 2048 or 4096; 384 to 16384 in increments of 8)</param>
        public Client(int encryptionKeySize)
        {
            _keySize = encryptionKeySize;
            CallTimeout = -1;
        }
        /// <summary>
        /// Constructs a new Client, initialized with the specified values
        /// </summary>
        /// <param name="remoteEndPoint">The server's IP address and port</param>
        /// <param name="encryptionKeySize">The encryption key size (typically 2048 or 4096; 384 to 16384 in increments of 8)</param>
        /// <param name="msCallTimeout">The maximum number of milliseconds to wait for a call to complete</param>
        public Client(IPEndPoint remoteEndPoint, int encryptionKeySize, int msCallTimeout = -1)
        {
            _keySize = encryptionKeySize;
            RemoteEndPoint = remoteEndPoint;
            CallTimeout = msCallTimeout;
        }
    }
}
