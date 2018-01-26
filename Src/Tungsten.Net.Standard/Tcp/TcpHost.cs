using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using W.AsExtensions;

namespace W.Net
{
    public static partial class Tcp
    {
        public class TcpHost : IDisposable// where TServer : IInitialize, IClient, new()
        {
            private TcpListener _host;
            private W.Threading.ThreadMethod _thread;
            private volatile bool _exitNow = false;
            private W.Threading.Lockers.MonitorLocker _serversLocker = new Threading.Lockers.MonitorLocker();
            private List<IClient> _servers = new List<IClient>();

            public EventTemplate<TcpHost, IClient, byte[]> BytesReceived { get; private set; } = new EventTemplate<TcpHost, IClient, byte[]>();
            public bool IsListening => _host != null;

            protected Func<Socket, IClient> OnCreateServer { get; set; } = s => { var server = new TcpClient(); server.As<IInitialize>().Initialize(s); return server; };

            private void BytesReceived_OnRaised(IClient sender, byte[] bytes)
            {
                OnBytesReceived(sender, bytes);
            }
            private void OnServerDisconnected(IClient server)
            {
                _serversLocker.InLock(() =>
                {
                    _servers.Remove(server);
                });
            }
            private void ThreadProc(params object[] args)
            {
                while (!_exitNow)
                {
                    if (_host.Pending())
                    {
#if NET45
                    var socket = _host.AcceptSocket();
#elif NETSTANDARD1_3
                        var socket = _host.AcceptSocketAsync().Result;
#endif
                        var server = OnCreateServer(socket);

                        server.Disconnected.OnRaised += OnServerDisconnected;
                        server.BytesReceived.OnRaised += BytesReceived_OnRaised;
                        _serversLocker.InLock(() =>
                        {
                            _servers.Add(server);
                        });
                    }
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                }
            }

            protected virtual void OnBytesReceived(IClient client, byte[] bytes)
            {
                BytesReceived.Raise(this, client, bytes);
            }
            public void Listen(IPEndPoint ep, int backlog)
            {
                _host = new TcpListener(ep);
                _host.Start(backlog);
                _thread = new W.Threading.ThreadMethod(ThreadProc);
                _thread.Start();
            }
            public void Dispose()
            {
                _exitNow = true;
                _thread?.Wait();
                _thread?.Dispose();
                _host?.Stop();
                _host = null;
                //don't mess with _servers until the thread completes
                foreach (var server in _servers)
                {
                    server.Disconnected.OnRaised -= OnServerDisconnected; //remove this so we don't receive it
                    server.Dispose();
                }
            }
        }
    }
}