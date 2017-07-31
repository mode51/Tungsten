using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using W.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// A socket server which listens for and handles client connections 
    /// </summary>
    /// <typeparam name="TClientType">The type of Socket client to use</typeparam>
    public class Server<TClientType> : IDisposable where TClientType : class, IDataSocket
    {
        private System.Net.Sockets.TcpListener _server;
        private readonly object _lock = new object();
        private readonly List<TClientType> _clients = new List<TClientType>();
        private W.Threading.Thread _listenProc;
        private bool _isListening;

        ///<summary>
        /// Called when a client connects to the server
        /// </summary>
        public Action<TClientType> ClientConnected { get; set; }
        /// <summary>
        /// Called when a client disconnects normally or by exception
        /// </summary>
        public Action<TClientType, IPEndPoint, Exception> ClientDisconnected { get; set; }
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
            System.Diagnostics.Debug.WriteLine("Tungsten.Net.SecureServer Shutdown Complete(result={0}", success);
            if (e != null)
                System.Diagnostics.Debug.WriteLine(e.ToString());
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
                            System.Diagnostics.Debug.WriteLine(string.Format("SocketException: Socket Error Code {0}: {1}\n{2}", e.SocketErrorCode,
                                Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode),
                                e.ToString()));
                        break;
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        break;
                    }
                    OnCreateClientHandler(client);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            finally
            {
                IsListening = false;
                IsListeningChanged?.Invoke(IsListening);
            }
        }
        /// <summary>
        /// Creates a new client of type TClientType
        /// </summary>
        /// <param name="client">The TcpClient used to initialize the client</param>
        /// <returns>The new client of type TClientType</returns>
        protected virtual TClientType CreateClient(TcpClient client)
        {
            var handler = (TClientType)Activator.CreateInstance(typeof(TClientType), client);
            return handler;
        }
        /// <summary>
        /// Configures a new server-side client connection
        /// </summary>
        /// <param name="client">The new server-side client connection</param>
        protected virtual void OnCreateClientHandler(TcpClient client)
        {
            //var handler = new TClientType(client);
            var handler = CreateClient(client);// (TClientType)Activator.CreateInstance(typeof(TClientType), client);
            handler.Disconnected += (s, remoteEndPoint, exception) =>
            {
                if (_clients.Contains(handler))
                    _clients.Remove(handler);
                ClientDisconnected?.Invoke(handler, remoteEndPoint, exception);
            };
            //Notifications.ClientCreated?.Invoke(handler);
            //handler.As<IFormattedSocket>().Disconnected += (s, remoteEndPoint, exception) =>
            //{
            //    var proxy = s.As<TClientType>();
            //    if (proxy == null)
            //        throw new ArgumentOutOfRangeException(nameof(s), "Parameter s should have been a legitimate instance of SecureByteClient");
            //    if (_clients.Contains(proxy))
            //        _clients.Remove(proxy);
            //    ClientDisconnected?.Invoke(proxy, remoteEndPoint, exception);
            //};
            _clients.Add(handler);
            ClientConnected?.Invoke(handler);
            System.Diagnostics.Debug.WriteLine("Server created client handler");
        }

        /// <summary>
        /// Starts listening for clients
        /// </summary>
        /// <param name="ipAddress">The IP address to use</param>
        /// <param name="port">The port on which to listen</param>
        public void Start(IPAddress ipAddress, int port)
        {
            Start(new IPEndPoint(ipAddress, port));
        }
        /// <summary>
        /// Starts listening for clients
        /// </summary>
        /// <param name="ipEndPoint">The IP address and port to use</param>
        public void Start(IPEndPoint ipEndPoint)
        {
            try
            {
                _server = new TcpListener(ipEndPoint.Address, ipEndPoint.Port);
                _server.Start();
                _listenProc = W.Threading.Thread.Create(ListenForClientsProc, ListenForClientsProc_OnComplete);
                IsListening = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
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
