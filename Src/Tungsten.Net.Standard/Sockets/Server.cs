using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using W.Logging;

namespace W.Net.Sockets
{
    /// <summary>
    /// A socket server which listens for and handles client connections 
    /// </summary>
    /// <typeparam name="TSocket">The type of Socket client to use</typeparam>
    public class Server<TSocket> : IDisposable where TSocket : class, IFormattedSocket
    {
        private System.Net.Sockets.TcpListener _server;
        private object _lock = new object();
        private readonly List<TSocket> _clients = new List<TSocket>();
        private W.Threading.Thread _listenProc;
        private bool _isListening;

        ///<summary>
        /// Called when a client connects to the server
        /// </summary>
        public Action<TSocket> ClientConnected { get; set; }
        /// <summary>
        /// Called when a client disconnects normally or by exception
        /// </summary>
        public Action<TSocket, IPEndPoint, Exception> ClientDisconnected { get; set; }
        /// <summary>
        /// Called when the value of IsListening changes to true or false
        /// </summary>
        public Action<bool> IsListeningChanged { get; set; }

        /// <summary>
        /// True if the server is listening for clients, otherwise false
        /// </summary>
        public bool IsListening
        {
            get
            {
                lock (_lock)
                {
                    return _isListening;
                }
            }
            set
            {
                lock (_lock)
                {
                    _isListening = value;
                }
            }
        }

        /// <summary>
        /// Constructs a new Server
        /// </summary>
        public Server()
        {
        }
        /// <summary>
        /// Disposes and deconstructs the Server instance
        /// </summary>
        ~Server()
        {
            Dispose();
        }

        private void ListenForClientsProc_OnComplete(bool success, Exception e)
        {
            Log.i("Tungsten.Net.SecureServer Shutdown Complete(result={0}", success);
            if (e != null)
                Log.e(e);
            if (IsListening)
                Stop();
        }

        private void ListenForClientsProc(CancellationTokenSource cts)
        {
            IsListeningChanged?.Invoke(true);
            try
            {
                while (!cts?.Token.IsCancellationRequested ?? false && (_server != null))
                {
                    W.Threading.Thread.Sleep(1);
                    if (IsListening && !_server.Pending())
                        continue;

                    TcpClient client = null;
                    try
                    {
                        client = _server.AcceptTcpClientAsync().Result;
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
                            Log.e("SocketException: Socket Error Code {0}: {1}\n{2}", e.SocketErrorCode,
                                Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode),
                                e.ToString());
                        break;
                    }
                    catch (Exception e)
                    {
                        Log.e(e);
                        break;
                    }
                    OnCreateClientHandler(client);
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            finally
            {
                IsListening = false;
                IsListeningChanged?.Invoke(IsListening);
            }
        }
        /// <summary>
        /// Configures a new server-side client connection
        /// </summary>
        /// <param name="client">The new server-side client connection</param>
        protected virtual void OnCreateClientHandler(TcpClient client)
        {
            //var handler = new TClientType(client);
            var handler = (TSocket)Activator.CreateInstance(typeof(TSocket), client);
            //Notifications.ClientCreated?.Invoke(handler);
            handler.As<IFormattedSocket>().Disconnected += (s, remoteEndPoint, exception) =>
            {
                var proxy = s.As<TSocket>();
                if (proxy == null)
                    throw new ArgumentOutOfRangeException(nameof(s), "Parameter s should have been a legitimate instance of SecureByteClient");
                if (_clients.Contains(proxy))
                    _clients.Remove(proxy);
                ClientDisconnected?.Invoke(proxy, remoteEndPoint, exception);
            };
            _clients.Add(handler);
            ClientConnected?.Invoke(handler);
            Log.i("Server created client handler");
        }

        /// <summary>
        /// Starts listening for clients
        /// </summary>
        /// <param name="ipAddress">The IP address to use</param>
        /// <param name="port">The port on which to listen</param>
        public void Start(IPAddress ipAddress, int port)
        {
            _server = new TcpListener(ipAddress, port);
            _server.Start();
            _listenProc = W.Threading.Thread.Create(ListenForClientsProc, ListenForClientsProc_OnComplete);
            IsListening = true;
        }

        /// <summary>
        /// Stops listening for and disconnects all current connections
        /// </summary>
        public void Stop()
        {
            while (_clients.Count > 0)
                _clients[0].Socket.Disconnect();
            _listenProc?.Cancel();
            _listenProc?.Join(1000);
            _listenProc = null;
            _server?.Stop();
            IsListening = false;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }

    }
}