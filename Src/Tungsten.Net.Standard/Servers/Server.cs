using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using W;
using W.AsExtensions;

namespace W.Net
{
    /// <summary>
    /// A socket server which listens for and handles client connections to automatically echo incoming messages
    /// </summary>
    /// <typeparam name="TClientType">The type of Socket client to use.  This type must derrive from W.Net.ClientBase</typeparam>
    public class EchoServer<TClientType> : Server<TClientType> where TClientType : Client, new()
    {
        /// <summary>
        /// Constructs an EchoServer
        /// </summary>
        public EchoServer()
        {
            base.ClientConnected += c =>
            {
                var client = c as Client;
                client.DataReceived += (o, bytes) =>
                {
                    var client2 = o as Client;
                    if (client2?.IsConnected ?? false) //because SecureClient won't be connected until after _remotePublicKey has been set
                    {
                        client2.Send(bytes);
                        Console.WriteLine("Server Echoed {0} bytes", bytes.Length);
                    }
                };
            };
        }
    }

    /// <summary>
    /// A socket server which listens for and handles client connections 
    /// </summary>
    /// <typeparam name="TClientType">The type of Socket client to use</typeparam>
    public class Server<TClientType> : ITcpServer, IDisposable where TClientType : ITcpClient, new()
    {
        private int _backlog;
        //private readonly System.Collections.Concurrent.ConcurrentDictionary<string, TClientType> _clients = new ConcurrentDictionary<string, TClientType>();
        private readonly List<TClientType> _clients = new List<TClientType>();
        private object _clientsLock = new object();
        private W.Threading.ParameterizedThread _listenProc;
        private ManualResetEventSlim _mreServerListenComplete = new ManualResetEventSlim(false);

        /// <summary>
        /// Select the CPU profile to choose your needs
        /// </summary>
        public W.Threading.CPUProfileEnum CPUProfile { get; set; } = W.Threading.CPUProfileEnum.SpinWait1;

        ///<summary>
        /// Called when a client connects to the server
        /// </summary>
        public Action<TClientType> ClientConnected { get; set; }
        /// <summary>
        /// Called when a client disconnects normally or by exception
        /// </summary>
        public Action<TClientType, IPEndPoint, Exception> ClientDisconnected { get; set; }

        /// <summary>
        /// True if the server is listening for clients, otherwise false
        /// </summary>
        public bool IsListening => (_listenProc?.IsStarted ?? false);
        //{
        //    get
        //    {
        //        return _listenProc?.IsStarted ?? false;
        //    }
        //}
        /// <summary>
        /// If True, the socket will compress data before sending and decompress data when receiving
        /// </summary>
        public bool UseCompression { get; set; }

        private void RemoveDisconnectedClients()
        {
            lock (_clientsLock)
            {
                //remove disconnected clients
                for (int t = _clients.Count - 1; t >= 0; t--)
                {
                    var client = _clients[t] as Client;
                    if (!SocketExtensions.IsConnected(client.Socket))
                        client.Disconnect();
                }
            }
        }
        private void ListenForClientsProc(CancellationToken ct, params object[] args)
        {
            try
            {
                var ipEndPoint = (IPEndPoint)args[0];
                var server = new TcpListener(ipEndPoint.Address, ipEndPoint.Port);
                server.Start(_backlog);
                while (!ct.IsCancellationRequested)
                {
                    RemoveDisconnectedClients();//11.11.2017 - removing this call causes constantly increasing memory usage (_clients.Count continues to increase)
                    if (IsListening && !(server?.Pending() ?? false))
                    {
                        W.Threading.Thread.Sleep(CPUProfile);
                        continue;
                    }

                    try
                    {
                        //awaiting the Accept causes the thread (Task) to continue into the ContinueWith (which then sets _mreComplete)
                        //so we can't use the async method here
                        var socket = server.AcceptSocketAsync().Result;
                        if (socket != null)
                            CreateServerClient(socket);
                        //var client = server.AcceptTcpClientAsync().Result;
                        //if (client != null)
                        //    CreateServerClient(client);
                    }
                    catch (NullReferenceException)
                    {
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }
                    catch (InvalidOperationException) //no longer listening
                    {
                        break;
                    }
                    catch (SocketException e)
                    {
                        if (e.SocketErrorCode != SocketError.Interrupted) //the server was stopped
                        {
                            var msg = string.Format("SocketException: Socket Error Code {0}: {1}\n{2}", e.SocketErrorCode, Enum.GetName(typeof(System.Net.Sockets.SocketError), e.SocketErrorCode), e.ToString());
                            Debug.i(msg);
                        }
                        break;
                    }
                    catch (Exception e)
                    {
                        Debug.e(e);
                        break;
                    }
                }
                server.Stop();
                //_server.Server.Dispose();
            }
            catch (Exception e)
            {
                Debug.e(e);
            }
            finally
            {
                _mreServerListenComplete.Set();
            }
        }
        private void CreateServerClient(System.Net.Sockets.Socket socket)
        {
            try
            {
                var handler = new TClientType();
                //_clients.TryAdd(client.Client.RemoteEndPoint.As<IPEndPoint>().ToString(), handler);
                lock (_clientsLock)
                {
                    _clients.Add(handler);
                }
                handler.As<ITcpClient>().ConfigureForServerSide(this, socket, CPUProfile);
                handler.As<Client>().UseCompression = UseCompression;
                handler.As<Client>().Disconnected += (o, r, e) =>
                {
                    lock (_clientsLock)
                    {
                        if (_clients.Contains((TClientType)o))
                            _clients.Remove((TClientType)o);
                    }
                };
                Debug.i("Server created client handler");
                ClientConnected?.Invoke(handler);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Explicit ISocketServer
        void ITcpServer.DisconnectClient(ITcpClient client, IPEndPoint remoteEndPoint, Exception e)
        {
            try
            {
                var tc = (TClientType)client;
                lock (_clientsLock)
                {
                    if (_clients.Contains(tc))
                    {
                        _clients.Remove(tc);
                        ClientDisconnected?.Invoke(tc, tc.RemoteEndPoint, e);
                        Debug.i(string.Format("Removed {0}, Count = {1}", client.RemoteEndPoint, _clients.Count));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.e(ex);
                System.Diagnostics.Debugger.Break();
            }
        }
        #endregion

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
                _mreServerListenComplete.Reset();
                _listenProc = new W.Threading.ParameterizedThread(ListenForClientsProc);//, ListenForClientsProc_OnComplete);
                _listenProc.Start(ipEndPoint);
            }
            catch (SocketException e)
            {
                Debug.e(e);
                System.Diagnostics.Debugger.Break();
            }
            catch (Exception e)
            {
                Debug.e(e);
                System.Diagnostics.Debugger.Break();
            }
        }
        /// <summary>
        /// Stops listening for and disconnects all current connections
        /// </summary>
        public void Stop()
        {
            try
            {
                //if (!IsListening)
                //    return;
                if (_listenProc != null) //because Dispose calls Stop too
                {
                    _listenProc.Stop();
                    _mreServerListenComplete.Wait(); //redundant check because _listenProc.Stop should already be waiting for it to complete
                    _listenProc.Dispose();
                    _listenProc = null;
                }
                //if (_server != null) //because Dispose calls Stop too
                //{
                //    _server.Stop();
                //    _server = null;
                //}
            }
            catch (Exception e)
            {
                Debug.e(e);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Constructs a new Server
        /// </summary>
        public Server() : this(20)
        {
        }
        /// <summary>
        /// Constructs a new Server
        /// </summary>
        /// <param name="backlog">The number of connections the server should pre-allocate resources for</param>
        public Server(int backlog)
        {
            _backlog = backlog;
        }
        ///// <summary>
        ///// Disposes and deconstructs the Server instance
        ///// </summary>
        //~Server()
        //{
        //    Dispose();
        //}
    }
}