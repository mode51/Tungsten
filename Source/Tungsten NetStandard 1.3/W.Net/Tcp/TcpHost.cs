using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace W.Net
{
    public static partial class Tcp
    {
        public class TcpHost : IDisposable// where TServer : IInitialize, IClient, new()
        {
            private TcpListener _host;
            private W.Threading.ThreadMethod _thread;
            private volatile bool _exitNow = false;
            //private W.Threading.Lockers.MonitorLocker _serversLocker = new W.Threading.Lockers.MonitorLocker();
            //private List<IClient> _servers = new List<IClient>();

            public System.Collections.Concurrent.ConcurrentDictionary<string, IClient> Servers { get; } = new System.Collections.Concurrent.ConcurrentDictionary<string, IClient>();

            public event Action<TcpHost, IClient, byte[]> BytesReceived;// { get; private set; } = new EventTemplate<TcpHost, IClient, byte[]>();
            public bool IsListening => _host != null;
            //public List<IClient> Servers
            //{
            //    get
            //    {
            //        var result = new List<IClient>();
            //        foreach (var server in Servers.Values)
            //            result.Add(server);
            //        return result;
            //    }
            //}
            public event Action<TcpHost, IClient> ClientConnected;
            public event Action<TcpHost, IClient> ClientDisconnected;

            protected virtual Func<Socket, IClient> OnCreateServer { get; set; } = s => { var server = new TcpClient() { IsServerSide = true }; server.As<IInitialize>().Initialize(s); return server; };

            private void BytesReceived_OnRaised(IClient sender, byte[] bytes)
            {
                OnBytesReceived(sender, bytes);
            }
            private void OnServerDisconnected(IClient server)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Host removing disconnected server");
                    //System.Diagnostics.Debugger.Break();

                    while (!Servers.TryRemove(server.Id.ToString(), out IClient value))
                        W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                    try
                    {
                        ClientDisconnected?.Invoke(this, server);
                    }
                    finally
                    {
                        server.Dispose();
                    }

                    //if (_servers.TryRemove(server.Id.ToString(), out IClient value))
                    //{
                    //    try
                    //    {
                    //        ClientDisconnected?.Invoke(this, server);
                    //    }
                    //    finally
                    //    {
                    //        server.Dispose();
                    //    }
                    //}
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    System.Diagnostics.Debugger.Break();
                }
            }
            private void ThreadProc(CancellationToken token, params object[] args)
            {
                while (!_exitNow)
                {
                    try
                    {
                        if (_host.Pending())
                        {
                            System.Net.Sockets.Socket socket = null;
#if NET45 || NETSTANDARD2_0 || NETCOREAPP2_0
                            socket = _host.AcceptSocket();
#elif NETSTANDARD1_3
                            socket = _host.AcceptSocketAsync().Result;
#endif
                            var server = OnCreateServer(socket);

                            server.Disconnected += OnServerDisconnected;
                            server.BytesReceived += BytesReceived_OnRaised;
                            //_serversLocker.InLock(() =>
                            {
                                while (!Servers.TryAdd(server.Id.ToString(), server))
                                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                                try
                                {
                                    ClientConnected?.Invoke(this, server);
                                }
                                finally
                                {
                                    System.Diagnostics.Debug.WriteLine("Host adding connected server");
                                }
                                //foreach (var s in _servers)
                                //{
                                //    if (!s.Value.Socket.IsConnected())
                                //        OnServerDisconnected(s);
                                //}
                            }//);
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        System.Diagnostics.Debugger.Break();
                    }
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                }
            }

            protected virtual void OnBytesReceived(IClient client, byte[] bytes)
            {
                BytesReceived?.Invoke(this, client, bytes);
            }
            public void Listen(IPEndPoint ep)
            {
                _host = new TcpListener(ep);
                _host.Start();
                _thread = W.Threading.ThreadMethod.Create(ThreadProc);
                _thread.Start();
            }
            public void Listen(IPEndPoint ep, int backlog)
            {
                _host = new TcpListener(ep);
                _host.Start(backlog);
                _thread = W.Threading.ThreadMethod.Create(ThreadProc);
                _thread.Start();
            }
            public void Dispose()
            {
                _exitNow = true;
                //_thread?.Wait();
                _thread?.Dispose();
                _host?.Stop();
                _host = null;
                //don't mess with _servers until the thread completes
                //_serversLocker.InLock(() =>
                {
                    foreach (var server in Servers)
                    {
                        server.Value.Disconnected -= OnServerDisconnected; //remove this so we don't receive it
                        server.Value.Dispose();
                    }
                    Servers.Clear();
                }//);
            }
        }
    }
}