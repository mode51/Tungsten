using System;
using System.Net;
using System.Threading.Tasks;
using W.Logging;
using System.Threading;

namespace W.Net.RPC
{
    /// <summary>
    /// Supports remote instances of Tungsten.Net.RPC.Client to call local methods.
    /// </summary>
    public class RPCServer : IDisposable
    {
        private XServer<XClient<Message>> _host;
        private ManualResetEventSlim _mreIsListening;

        /// <summary>
        /// Exposes the dictionary of methods.  Custom, non-attributed methods may be added to this dictionary.
        /// </summary>
        public MethodDictionary Methods = new MethodDictionary();
        /// <summary>
        /// Multi-cast delegate will be called with the appropriate value whenever the server successfully starts or stops listening.
        /// </summary>
        /// <remarks>The value will be True if the server is listening, otherwise False</remarks>
        public Action<bool> IsListeningChanged { get; set; }

        private bool OnMessageReceived(ref Message message)
        {
            var result = Methods.Call(message.Method, message.Parameters.ToArray());
            message.Exception = new ExceptionInformation(result.Exception);
            message.Response = result.Result;
            return result.Success;
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
        /// <param name="ipAddress">The IP address on which to bind and listen for clients</param>
        /// <param name="port">The port on which to listen.  Must be a positive value.</param>
        public void Start(IPAddress ipAddress, int port)
        {
            if (port <= 0)
                throw new ArgumentOutOfRangeException(nameof(port));
            Stop();
            _mreIsListening = new ManualResetEventSlim(false);
            _host = new XServer<XClient<Message>>();
            _host.IsListeningChanged += (isListening) => { IsListeningChanged?.Invoke(isListening); _mreIsListening?.Set(); };
            _host.ClientConnected += (client) =>
            {
                client.Socket.UseCompression = true;
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
                            //Put this in a task to multi-thread the encryption and compression
                            //Task.Run(() =>
                            //{
                            //send the result back to the client
                            c.SerializationSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                            c.SerializationSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                            c.Send(message);
                            //});
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
            _host.Start(ipAddress, port);
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
        public RPCServer()
        {
            Methods.Refresh();
        }
        /// <summary>
        /// Calls Dispose and deconstructs the Tungsten.Net.RPC.Server
        /// </summary>
        ~RPCServer()
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
