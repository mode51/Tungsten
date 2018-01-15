using System;
using System.Collections.Generic;
using System.Threading;
using W.DelegateExtensions;
using System.Net;
using System.Net.Sockets;
using W.Threading;
#if NET45
using System.Security.Principal;
#endif

namespace W.Net.Alpha
{
    //private members
    public partial class HostBase<TServer> : Disposable where TServer : ServerBase<TServer>, new()
    {
        private System.Collections.Concurrent.ConcurrentDictionary<string, TServer> _servers = new System.Collections.Concurrent.ConcurrentDictionary<string, TServer>();
        //private List<TServer> _servers = new List<TServer>();
        private object _serversLock = new object();
        private LockableSlim<bool> _started = new LockableSlim<bool>();
        private LockableSlim<bool> _stopping = new LockableSlim<bool>();
        private ThreadMethod _stop = null;
        private ThreadMethod _start = null;
        private System.Net.Sockets.TcpListener _listener = null;
        private W.Threading.ThreadSlim _listenAT = null;

        private void AddServer(TServer server)
        {
            lock(_serversLock)
                _servers.TryAdd(server.Id, server);

        }
        private void RemoveServer(TServer server)
        {
            lock (_serversLock)
                _servers.TryRemove(server.Id, out TServer item);
        }
        private void RemoveDisconnectedClients()
        {
            lock (_serversLock)
            {
                //remove disconnected servers
                var servers = _servers.ToArray();
                for (int t = servers.Length - 1; t >= 0; t--)
                {
                    var server = servers[t].Value as TServer;
                    if (server.Socket != null && !server.Socket.IsConnected())
                    {
                        server.Dispose(); //may not raise Server.Disconnected?
                        RemoveServer(server);
                        ServerDisconnected?.Raise(this, server);
                    }
                    W.Threading.Thread.Sleep(1);
                }
            }
        }
        private void ThreadProc(CancellationToken token)
        {
            //wait for client connections
            try
            {
                while (!token.IsCancellationRequested)
                {
                    RemoveDisconnectedClients();//11.11.2017 - removing this call causes constantly increasing memory usage (_clients.Count continues to increase)
                    if (!_listener.Pending())
                    {
                        W.Threading.Thread.Sleep(1);// W.Threading.CPUProfileEnum.Sleep);
                        continue;
                    }

                    try
                    {
                        //var server = _listener.AcceptTcpClientAsync().Result;
                        var serverSocket = _listener.AcceptSocketAsync().Result;
                        if (serverSocket != null)
                            InitializeServer(serverSocket, token);
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
                } //while
            }
            catch (Exception e)
            {
                Debug.e(e);
            }
        }
        private void InitializeServer(System.Net.Sockets.Socket socket, CancellationToken token)
        {
            try
            {
                if (token.IsCancellationRequested)
                {
                    //TODO: do I really need to dispose this here?
                    //if (socket.IsConnected())
                        socket.Shutdown(SocketShutdown.Both);
                    //#if NETSTANDARD1_3
                    //                    socket.Dispose();
                    //#elif NET45
                    //                    socket.Close();
                    //#endif
#if NET45
                    socket.Close();
#endif
                    socket.Dispose();
                    return;
                }
                if (socket.Connected)
                {
                    var server = new TServer();
                    server.InitializeConnection(socket);
                    server.UseCompression = this.UseCompression;
                    //server.Disconnected += (s) =>
                    //{
                    //    try
                    //    {
                    //        if (s != null && s is TServer c)
                    //        {
                    //            RemoveServer(c);
                    //            ServerDisconnected?.Raise(this, c);
                    //            c.Dispose();
                    //        }
                    //    }
                    //    finally
                    //    {
                    //    }
                    //};
                    server.ReadProgress += (sender, current, total) =>
                    {
                        ReadProgress?.Raise(this, sender, current, total);
                    };
                    server.BytesReceived += (sender, message) =>
                    {
                        BytesReceived?.Raise(this, sender, message);
                    };
                    AddServer(server);
                    ServerConnected?.Raise(this, server);
                }
            }
            catch (System.IO.IOException)
            {
                System.Diagnostics.Debugger.Break(); //why are we here?
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                //ignore
                System.Diagnostics.Debugger.Break(); //why are we here?
                //Exception?.Invoke(this, e);
            }
            catch (System.OperationCanceledException) //happens when the server is closed
            {
                //TODO: do I really need to dispose this?
#if NETSTANDARD1_3
                socket.Dispose();
#elif NET45
                socket.Close();
#endif
                System.Diagnostics.Debugger.Break();
                //dispose the last allocated stream
                //ignore
                //Exception?.Invoke(this, e);
            }
            catch (Exception)
            {
                System.Diagnostics.Debugger.Break(); //why are we here?
            }
        }
        private void StartMethod(IPEndPoint ipEndpoint, int backlog)
        {
            if (_started.Value)
                return;
            try
            {
                _listener = new TcpListener(ipEndpoint);
                if (backlog > 0)
                    _listener.Start(backlog);
                else
                    _listener.Start();
                _listenAT.Start();
                _started.Value = true;
            }
            catch (System.IO.IOException e)
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
                _started.Value = true;
                Stop();
            }
        }
        private void StopMethod()
        {
            if (_started.Value && !_stopping.Value)
            {
                _stopping.Value = true;
                //Cancel the thread
                _listenAT.Stop();
                _listener?.Stop();
                _listener = null;
                //and disconnect all servers
                var servers = _servers.ToArray();
                for (int t = 0; t < servers.Length; t++)
                    servers[t].Value.Dispose();
                //RemoveDisconnectedClients();//.Dispose();
                _started.Value = false;
                _stopping.Value = false;
            }
        }
    }

    //protected members
    public partial class HostBase<TServer> : Disposable where TServer : ServerBase<TServer>, new()
    {
        /// <summary>
        /// Disposes the Host and releases resources
        /// </summary>
        protected override void OnDispose()
        {
            Stop();
            _listenAT.Dispose();
            //_start.Dispose();
            //_stop.Dispose();
            //_started.Dispose();
            //_stopping.Dispose();
            _serversLock = null;
            base.OnDispose();
        }
    }

    //public members
    public partial class HostBase<TServer> : Disposable where TServer : ServerBase<TServer>, new()
    {
        /// <summary>
        /// Gets or sets a value indicating that the data should be compressed before transmission and decompressed after reception
        /// </summary>
        public bool UseCompression { get; set; }

        /// <summary>
        /// Raised when a client connects to a local Host
        /// </summary>
        public event Action<HostBase<TServer>, TServer> ServerConnected;
        /// <summary>
        /// Raised when a client disconnects from a local server Host
        /// </summary>
        public event Action<HostBase<TServer>, TServer> ServerDisconnected;
        /// <summary>
        /// Raised when a message is received from a client
        /// </summary>
        public event Action<HostBase<TServer>, TServer, byte[]> BytesReceived;
        /// <summary>
        /// Reflects the status of reading data
        /// </summary>
        public event Action<HostBase<TServer>, TServer, int, int> ReadProgress;

        /// <summary>
        /// Start listening for clients
        /// </summary>
        /// <param name="address">The ip address on which to listen</param>
        /// <param name="port">The port on which to listen</param>
        public void Start(string address, int port)
        {
            Start(new IPEndPoint(IPAddress.Parse(address), port));
        }
        /// <summary>
        /// Start listening for clients
        /// </summary>
        /// <param name="address">The ip address on which to listen</param>
        /// <param name="port">The port on which to listen</param>
        /// <param name="backlog">The maximum number of concurrently connected clients</param>
        public void Start(string address, int port, int backlog)
        {
            Start(new IPEndPoint(IPAddress.Parse(address), port), backlog);
        }
        /// <summary>
        /// Start listening for clients
        /// </summary>
        /// <param name="ipEndPoint">The IP address and port of the host</param>
        public void Start(IPEndPoint ipEndPoint)
        {
            _start.RunSynchronously(ipEndPoint, 20);
        }
        /// <summary>
        /// Start listening for clients
        /// </summary>
        /// <param name="ipEndPoint">The IP address and port of the host</param>
        /// <param name="backlog">The maximum number of concurrently connected clients</param>
        public void Start(IPEndPoint ipEndPoint, int backlog)
        {
            _start.RunSynchronously(ipEndPoint, backlog);
        }
        /// <summary>
        /// Stops listening for clients
        /// </summary>
        public void Stop()
        {
            _stop.RunSynchronously();
        }
    }

    /// <summary>
    /// Base class of server hosts
    /// </summary>
    public partial class HostBase<TServer> : Disposable where TServer : ServerBase<TServer>, new()
    {
        /// <summary>
        /// Constructs a new HostBase
        /// </summary>
        internal HostBase()
        {
            //_listenThread = new Threading.Thread(WaitForClientsProc, false);
            _listenAT = new W.Threading.ThreadSlim(ThreadProc);
            _start = ThreadMethod.Create(args => { StartMethod((IPEndPoint)args[0], (int)args[1]); });
            _stop = ThreadMethod.Create(StopMethod);
        }
    }
}
