using System;
using System.Net;
using W.Logging;
using System.Threading;
using W;
using W.AsExtensions;

namespace W.Net.RPC
{
    /// <summary>
    /// Supports remote instances of Tungsten.Net.RPC.Client to call local methods.
    /// </summary>
    public class Server : IDisposable
    {
        private W.Net.Server<W.Net.SecureClient<Message>> _host;
        private ManualResetEventSlim _mreIsListening;

        /// <summary>
        /// Exposes the dictionary of methods.  Custom, non-attributed methods may be added to this dictionary.
        /// </summary>
        public MethodDictionary Methods = new MethodDictionary();
        ///// <summary>
        ///// Multi-cast delegate will be called with the appropriate value whenever the server successfully starts or stops listening.
        ///// </summary>
        ///// <remarks>The value will be True if the server is listening, otherwise False</remarks>
        //public Action<bool> IsListeningChanged { get; set; }

        private bool OnMessageReceived(ref Message message)
        {
            try
            {
                var result = Methods.Call(message.Method, message.Parameters.ToArray());
                message.Exception = result.Exception?.ToString();
                message.Response = result.Result;
                return result.Success;
            }
            catch (Exception e)
            {
                message.Exception = e.ToString();
            }
            return false;
        }

        /// <summary>
        /// Blocks the calling thread until the server starts or fails to start within the allotted timeout period.
        /// </summary>
        /// <param name="msTimeout">The number of milliseconds to wait before a timeout occurs.</param>
        /// <returns>True if the server starts within the specified timeout period, otherwise False.</returns>
        public bool WaitForIsListening(int msTimeout = 10000)
        {
            //return TimeoutFunc<bool>.Create(msTimeout, ct => 
            //{
            //    var result = false;
            //    while (!ct.IsCancellationRequested)
            //    {
            //        if (_mreIsListening != null)
            //        {
            //            result = _mreIsListening.WaitOne(1);
            //            if (result)
            //                break;
            //        }
            //        System.Threading.Thread.Sleep(1);
            //    }
            //    return result;
            //}).Start();
            return _mreIsListening?.Wait(msTimeout) ?? false;
        }
        /// <summary>
        /// Starts listening for client connections on the specified network interface and port
        /// </summary>
        /// <param name="ep">The IPEndpoint on which to bind and listen for clients</param>
        public void Start(IPEndPoint ep)
        {
            Stop();
            _mreIsListening = new ManualResetEventSlim(false);
            _host = new Server<SecureClient<Message>>();
            //_host.IsListeningChanged += (isListening) => { IsListeningChanged?.Invoke(isListening); _mreIsListening?.Set(); };
            _host.ClientConnected += (client) =>
            {
                //Log.i("Client Connected: {0}", client.Name);
                client.Disconnected += (c, remoteEndPoint, exception) =>
                {
                    //var name = (c as GenericClient<Message>)?.Socket.Name ?? "Unknown";
                    Log.i($"Client Disconnected: {remoteEndPoint.ToString()}");
                };
                client.MessageReceived += (c, message) =>
                {
                    try
                    {
                        //call the appropriate RPC method
                        OnMessageReceived(ref message);
                        try
                        {
                            //send the result back to the client
                            c.As<SecureClient<Message>>().Send(ref message);
                        }
                        catch (Exception e)
                        {
                            Log.e(e);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.e(e);
                    }
                };
            };
            _host.Start(ep);
        }
        /// <summary>
        /// Stops listening for client connections
        /// </summary>
        public void Stop()
        {
            _mreIsListening?.Dispose();
            _mreIsListening = null;
            _host?.Stop();
            _host = null;
        }

        /// <summary>
        /// Initializes the Tungsten.Net.RPC.Server and loads the RPC methods
        /// </summary>
        /// <param name="useCompression">If True, data compression will be used during transmission.</param>
        /// <remarks>The client must be declared with the same value.</remarks>
        public Server(bool useCompression = true)
        {
            Methods.Refresh();
        }
        /// <summary>
        /// Calls Dispose and deconstructs the Tungsten.Net.RPC.Server
        /// </summary>
        ~Server()
        {
            Dispose();
        }
        /// <summary>
        /// Disposes the Tungsten.Net.RPC.Server and releases resources
        /// </summary>
        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }
    }
}
