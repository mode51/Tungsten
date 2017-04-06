using System;
using System.Net;
using System.Threading.Tasks;
using W.Logging;
using System.Threading;

namespace W.Net.RPC
{
    public class Server : IDisposable
    {
        private GenericServer<Message> _host;
        private ManualResetEvent _mreIsListening;

        public MethodDictionary Methods = new MethodDictionary();
        public Action<bool> IsListeningChanged { get; set; }

        private bool OnMessageReceived(ref Message message)
        {
            var result = Methods.Call(message.Method, message.Parameters.ToArray());
            message.Exception = result.Exception;
            message.Response = result.Result;
            return result.Success;
        }

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
            return _mreIsListening?.WaitOne(msTimeout) ?? false;
        }
        public void Start(IPAddress ipAddress, int port)
        {
            Stop();
            _mreIsListening = new ManualResetEvent(false);
            _host = new GenericServer<Message>();
            _host.IsListeningChanged += (isListening) => { IsListeningChanged?.Invoke(isListening); _mreIsListening?.Set(); };
            _host.ClientConnected += (client) =>
            {
                //Log.i("Client Connected: {0}", client.Name);
                client.Disconnected += (c, exception) =>
                {
                    var name = (c as GenericClient<Message>)?.Socket.Name ?? "Unknown";
                    Log.i($"Client Disconnected: {name}");
                };
                client.GenericMessageReceived += (c, message) =>
                {
                    try
                    {
                        //call the appropriate RPC method
                        OnMessageReceived(ref message);
                        try
                        {
                            //Put this in a task to multi-thread the encryption and compression
                            Task.Run(() =>
                            {
                                //send the result back to the client
                                c.As<GenericClient<Message>>().Send(message);
                            });
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
        public void Stop()
        {
            _mreIsListening?.Dispose();
            _mreIsListening = null;
            _host?.Stop();
            _host = null;
        }

        public Server()
        {
            Methods.Refresh();
        }
        ~Server()
        {
            Dispose();
        }
        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }
    }
}
