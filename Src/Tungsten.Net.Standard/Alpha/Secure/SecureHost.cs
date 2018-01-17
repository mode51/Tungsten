using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using W.AsExtensions;

namespace W.Net.Alpha
{
    public class SecureHost<TMessage> where TMessage : class, new()
    {
        private System.Collections.Concurrent.ConcurrentDictionary<string, SecureServer<TMessage>> _servers = null;
        private System.Net.Sockets.TcpListener _listener = null;
        private W.Threading.Lockers.SpinLocker<bool> _started = new Threading.Lockers.SpinLocker<bool>();
        private volatile bool _stopping = false;
        private W.Threading.ThreadSlim _listenThread = null;

        /// <summary>
        /// Raised when a message is received from a client
        /// </summary>
        public event Action<SecureHost<TMessage>, SecureServer<TMessage>, TMessage> MessageReceived;// { get; set; }
        /// <summary>
        /// Raised when a client connects to a local Host
        /// </summary>
        public event Action<SecureHost<TMessage>, SecureServer<TMessage>> Connected;
        /// <summary>
        /// Raised when a client disconnects from a local server Host
        /// </summary>
        public event Action<SecureHost<TMessage>, SecureServer<TMessage>> Disconnected;

        private void AddServer(SecureServer<TMessage> server)
        {
            _servers.TryAdd(server.Socket.Id, server);

        }
        private void RemoveServer(SecureServer<TMessage> server)
        {
            _servers.TryRemove(server.Socket.Id, out SecureServer<TMessage> item);
        }
        private void RemoveDisconnectedClients()
        {
            //remove disconnected servers
            var servers = _servers.ToArray();
            for (int t = servers.Length - 1; t >= 0; t--)
            {
                var server = servers[t].Value; //as SecureServer<TMessage>;
                if (server.IsConnected)
                {
                    server.Dispose(); //may not raise Server.Disconnected?
                    RemoveServer(server);
                    Disconnected?.Invoke(this, server);
                }
                W.Threading.Thread.Sleep(1);
            }
        }
        private void StartBase(IPEndPoint ipEndPoint, int backlog) //should only be called from within a _started.InLock call
        {
            try
            {
                _listener = new TcpListener(ipEndPoint);
                if (backlog > 0)
                    _listener.Start(backlog);
                else
                    _listener.Start();
                _listenThread.Start();
            }
            catch (System.IO.IOException e)
            {
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ToString());
                StopBase();
            }
        }
        private void StopBase() //should only be called from within a _started.InLock call
        {
            //Cancel the thread
            _listenThread.Stop();
            _listener?.Stop();
            _listener = null;
            //and disconnect all servers
            var servers = _servers.ToArray();
            for (int t = 0; t < servers.Length; t++)
                servers[t].Value.Disconnect();
            //RemoveDisconnectedClients();//.Dispose();
        }
        private void Start(IPEndPoint ipEndpoint, int backlog)
        {
            _started.InLock(() =>
            {
                if (_listener != null)
                    return;
                StartBase(ipEndpoint, backlog);
            });
        }
        private void Stop()
        {
            _started.InLock(started =>
            {
                _stopping = true;
                StopBase();
                _stopping = false;
                return false;
            });
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
                    var server = new SecureServer<TMessage>();
                    server.Connected += s => Connected?.Invoke(this, server);
                    server.MessageReceived += (sender, message) =>
                    {
                        MessageReceived?.Invoke(this, sender, message);
                    };
                    server.InitializeServer(socket);
                    server.Socket.UseCompression = this.UseCompression;
                    AddServer(server);
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

        /// <summary>
        /// Gets or sets a value indicating that the data should be compressed before transmission and decompressed after reception
        /// </summary>
        public bool UseCompression { get; set; }

        /// <summary>
        /// Constructs a new Host
        /// </summary>
        public SecureHost()
        {
            _servers = new System.Collections.Concurrent.ConcurrentDictionary<string, SecureServer<TMessage>>(); ;
            _listenThread = new W.Threading.ThreadSlim(ThreadProc);
        }
    }
}
