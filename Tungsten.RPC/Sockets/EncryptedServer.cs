using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W.Logging;

namespace W.RPC
{
    internal class EncryptedServer<TClientType> : IDisposable, INamed where TClientType : ClientBase, INamed
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private TcpListener _listener = null;
        private bool _isListening = false;
        private List<ISocketClient> _clients = new List<ISocketClient>();

        public delegate void ClientConnectedDelegate(object sender, TClientType client);
        public event ClientConnectedDelegate ClientConnected;

        public delegate void IsListeningChangedDelegate(object sender, bool isListening);
        public event IsListeningChangedDelegate IsListeningChanged;

        private void RaiseClientConnected(TClientType client)
        {
            var evt = ClientConnected;
            try
            {
                evt?.Invoke(this, client);
            }
            catch (Exception e)
            {
                Log.e("Exception in ClientConnected event handler: {0}", e.ToString());
            }
        }
        private void RaiseIsListeningChanged(bool value)
        {
            var evt = IsListeningChanged;
            try
            {
                _isListening = value;
                evt?.Invoke(this, value);
            }
            catch (Exception e)
            {
                Log.e("Exception in IsListeningChanged event handler: {0}", e.ToString());
            }
        }
        private void ListenForClients()
        {
            W.Threading.Thread.Create(cts =>
            {
                RaiseIsListeningChanged(true);
                try
                {
                    while (!_cts.Token.IsCancellationRequested && _listener != null)
                    {
                        System.Threading.Thread.Sleep(1);
                        if (_isListening && !_listener.Pending())
                            continue;

                        TcpClient client = null;
                        try
                        {
                            client = _listener.AcceptTcpClient();
                        }
                        catch (NullReferenceException)
                        {
                            break;
                        }
                        catch (ObjectDisposedException)
                        {
                            break;
                        }
                        catch (SocketException e)
                        {
                            if (e.SocketErrorCode != SocketError.Interrupted) //the server was stopped
                                Log.e("SocketException Error Code {0}, Socket Error Code {1}: {2}", e.ErrorCode,
                                    Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode),
                                    e.ToString());
                            break;
                        }
                        catch (Exception)
                        {
                            break;
                        }

                        //var handler = (TClientType)Activator.CreateInstance(typeof(TClientType), client);
                        var handler = Activator.CreateInstance<TClientType>();
                        handler.Name = client.Client.RemoteEndPoint.ToString(); // "Server Handler";
                        handler.Connect(client);
                        handler.Disconnected += OnClientDisconnected;
                        _clients.Add(handler);
                        RaiseClientConnected(handler);
                        Log.i("Server created client handler");
                    }
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
                finally
                {
                    RaiseIsListeningChanged(false);
                }
            });
        }

        private void OnClientDisconnected(ISocketClient client, Exception e)
        {
            if (_clients.Contains(client))
                _clients.Remove(client);
            //throw new NotImplementedException();
        }

        public TClientType this[string clientName]
        {
            get { return _clients.FirstOrDefault(c => (c as INamed)?.Name == clientName) as TClientType; }
        }

        public void Start(IPAddress address, int port)
        {
            Log.v("Listen on {0}, Port {1}", address.ToString(), port);
            if (_listener == null)
            {
                _listener = new TcpListener(address, port);
                _listener.Start(20);
                ListenForClients();
            }
            else
            {
                _listener.Start(20);
                RaiseIsListeningChanged(true);
            }
            Log.v("Listening on {0}, Port {1}", address.ToString(), port);
        }

        public void Stop()
        {
            _listener?.Stop();
            RaiseIsListeningChanged(false);
        }
        public string Name { get; set; }
        public void Dispose()
        {
            if (_isListening)
            {
                _isListening = false; //Stop the listener thread
                System.Threading.Thread.Sleep(10);
                _cts.Cancel();
                _listener.Stop();
                Log.i("Server Stopped");
            }
        }
    }
}
