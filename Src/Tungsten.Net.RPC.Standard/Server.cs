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
        private W.Net.Tcp.Generic.SecureTcpHost<Message> _host;

        /// <summary>
        /// Exposes the dictionary of methods.  Custom, non-attributed methods may be added to this dictionary.
        /// </summary>
        public MethodDictionary Methods = new MethodDictionary();

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
        /// Starts listening for client connections on the specified network interface and port
        /// </summary>
        /// <param name="ep">The IPEndpoint on which to bind and listen for clients</param>
        public void Start(IPEndPoint ep)
        {
            Stop();
            _host = new Tcp.Generic.SecureTcpHost<Message>(2048);
            //_host.IsListeningChanged += (isListening) => { IsListeningChanged?.Invoke(isListening); _mreIsListening?.Set(); };
            _host.MessageReceived.OnRaised += (h, s, message) =>
            {
                try
                {
                    //call the appropriate RPC method
                    OnMessageReceived(ref message);
                    try
                    {
                        //send the result back to the client
                        s.Write(message);
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
            _host.Listen(ep, 20);
        }
        /// <summary>
        /// Stops listening for client connections
        /// </summary>
        public void Stop()
        {
            _host?.Dispose();
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
