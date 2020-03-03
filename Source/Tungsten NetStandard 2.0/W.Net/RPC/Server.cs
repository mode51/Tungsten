using System;
using System.Net;
using System.Threading;
using W.Net;

namespace W.Net.RPC
{
    /// <summary>
    /// Allows remote instances of Tungsten.Net.RPC.Client to call local methods.
    /// </summary>
    /// <remarks>Note: Due to the way Newtonsoft.Json deserializes integers, do NOT use int (Int32) in your api's as parameters or return types; use longs instead.</remarks>
    public class Server : IDisposable
    {
        private Tcp.Generic.SecureTcpHost<RPCMessage> _host;
        private int _keySize;
        private MethodDictionary _methods = new MethodDictionary();

        /// <summary>
        /// Exposes the dictionary of methods.  Custom, non-attributed methods may be added to this dictionary.
        /// </summary>
        public MethodDictionary API { get => _methods; }

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
        /// <param name="rpcAssembly">The root assembly in which to scan for RPC methods</param>
        /// <param name="scanReferences">If True, referenced assemblies will also be scanned for RPC methods</param>
        public void Start(IPEndPoint ep, System.Reflection.Assembly rpcAssembly, bool scanReferences = false)
        {
            Stop();
            _methods.Refresh(scanReferences, rpcAssembly);
            _host = new Tcp.Generic.SecureTcpHost<RPCMessage>(_keySize);
            //_host.IsListeningChanged += (isListening) => { IsListeningChanged?.Invoke(isListening); _mreIsListening?.Set(); };
            //_host.BytesReceived += (h, s, bytes) =>
            //{
            //    System.Diagnostics.Debug.WriteLine($"Server Received {bytes.Length} bytes");
            //};
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
        /// <param name="encryptionKeySize">The encryption key size (typically 2048 or 4096; 384 to 16384 in increments of 8)</param>
        /// <remarks>The client must be declared with the same value.</remarks>
        public Server(int encryptionKeySize)
        {
            _keySize = encryptionKeySize;
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
