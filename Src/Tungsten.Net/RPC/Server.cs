using System;
using System.Net;
using System.Threading;
using W.Net;

namespace W.Net.RPC
{
    /// <summary>
    /// Supports remote instances of Tungsten.Net.RPC.Client to call local methods.
    /// </summary>
    public class Server : IDisposable
    {
        private Tcp.Generic.SecureTcpHost<RPCMessage> _host;
        private MethodDictionary _methods = new MethodDictionary();

        /// <summary>
        /// Exposes the dictionary of methods.  Custom, non-attributed methods may be added to this dictionary.
        /// </summary>
        private System.Collections.Generic.Dictionary<string, System.Reflection.MethodInfo> Methods { get => _methods.Methods; }

        private bool OnMessageReceived(ref RPCMessage message)
        {
            try
            {
                var success = _methods.Call(out object result, out Exception exception, message.Method, message.Parameters.ToArray());
                message.Exception = exception?.ToString();
                message.Response = result;
                return success;
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
            _methods.Refresh();
            _host = new Tcp.Generic.SecureTcpHost<RPCMessage>(2048);
            //_host.IsListeningChanged += (isListening) => { IsListeningChanged?.Invoke(isListening); _mreIsListening?.Set(); };
            _host.MessageReceived += (h, s, message) =>
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
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        System.Diagnostics.Debugger.Break();
                        //Log.e(e);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    System.Diagnostics.Debugger.Break();
                    //Log.e(e);
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
        /// <remarks>The client must be declared with the same value.</remarks>
        public Server()
        {
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
