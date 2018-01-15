using System;
using System.Threading.Tasks;
using System.Threading;

namespace W.Net.RPC
{
    /// <summary>
    /// Handles obtaining a response from the RPC call to the server
    /// </summary>
    public class RPCHandle : IDisposable
    {
        private SecureClient<Message> _client;
        private ManualResetEventSlim _mreResponseReceived = new ManualResetEventSlim(false);
        private Message Message { get; set; }
        private Lockable<bool> _isDisposed = new Lockable<bool>(false);
        private object _lock = new object();

        private RPCResult<TResponseType> GetResult<TResponseType>(object response, Exception exception)
        {
            return GetResult<TResponseType>(response, new ExceptionInformation(exception));
        }
        private RPCResult<TResponseType> GetResult<TResponseType>(object response, ExceptionInformation exception)
        {
            var result = new RPCResult<TResponseType>();
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

                //if (typeof(TResponseType) == typeof(object))
                //{
                //    result.Response = (response != null) ? (TResponseType)response : default(TResponseType);
                //}
                //else
                //{
                //    result.Response = Activator.CreateInstance<TResponseType>();
                //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                //    Newtonsoft.Json.JsonConvert.PopulateObject(json, result.Response);
                //}
                result.Exception = exception;// new ExceptionInformation(exception)
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                result.Response = default(TResponseType);
                result.Exception = new ExceptionInformation(e);
            }
            return result;
        }

        /// <summary>
        /// Waits for a response from the server
        /// </summary>
        /// <returns>A Task which returns a CallResult<object></object></returns>
        public async Task<RPCResult<object>> WaitForResponse()
        {
            var result = await WaitForResponse<object>();
            return result;
        }
        /// <summary>
        /// Waits for a response from the server.
        /// </summary>
        /// <returns>A Task which returns a CallResult&lt;TResponseType&gt;</returns>
        public async Task<RPCResult<TResponseType>> WaitForResponse<TResponseType>()
        {
            var result = await Task.Run(() =>
            {
                Exception ex = null;
                try
                {
                    _mreResponseReceived?.Wait();
                    return GetResult<TResponseType>(Message.Response, string.IsNullOrEmpty(Message.Exception) ? null : new Exception(Message.Exception));
                }
                catch (NullReferenceException) { ex = new ObjectDisposedException("CallHandler", "The CallHandler has been disposed.  The response cannot be trusted."); } //ignore this because someone called Dispose()
                    catch (ObjectDisposedException) { ex = new ObjectDisposedException("CallHandler", "The CallHandler has been disposed.  The response cannot be trusted."); }//ignore this because someone called Dispose()
                    catch (TaskCanceledException)
                {
                    System.Diagnostics.Debugger.Break();
                }
                return GetResult<TResponseType>(null, ex);
            });
            return result;
        }

        /// <summary>
        /// The exception, if one occurred while making the call.  If this value is null, then the programmer may use WaitForResponse to obtain a return value.
        /// </summary>
        /// <remarks>Note that this value should be tested before calling WaitForResponse.  This value is not a return value from the call.  It is only used if making the call raised an exception.</remarks>
        public Exception CallException { get; internal set; }

        internal RPCHandle(SecureClient<Message> client, Message message)
        {
            Message = message;
            _client = client;
            _client.MessageReceived += OnMessageReceived;
        }
        /// <summary>
        /// Deconstructs the CallHandler and calls Dispose
        /// </summary>
        ~RPCHandle()
        {
            Dispose();
        }
        private void OnMessageReceived(object sender, Message message)
        {
            if (message.Id == Message.Id)
            {
                lock (_lock)
                {
                    //if (message.Response != null)
                    try
                    {
                        this.Message = message;
                        //_response = message?.Response ?? null;
                        _mreResponseReceived?.Set();
                    }
                    catch (NullReferenceException) { } //ignore this because someone called Dispose()
                    catch (ObjectDisposedException) { } //ignore this because someone called Dispose()
                }
            }
        }
        /// <summary>
        /// Disposes the CallHandler and releases resources
        /// </summary>
        public void Dispose()
        {
            lock (_lock)
            {
                if (_client != null && !_isDisposed.Value)
                {
                    _isDisposed.Value = true;
                    _client.MessageReceived -= OnMessageReceived;
                    _client = null;

                    _mreResponseReceived.Dispose();
                    //_mreResponseReceived = null;

                    GC.SuppressFinalize(this);
                }
            }
        }
    }
}
