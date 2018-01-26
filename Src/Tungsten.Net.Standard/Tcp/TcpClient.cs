using System;
using System.Net;
using System.Net.Sockets;
using W.AsExtensions;
using W.Net.SocketExtensions;

namespace W.Net
{
    public static partial class Tcp
    {
        public class TcpClient : IInitialize, IClient
        {
            private W.Threading.ThreadMethod _thread;
            private volatile bool _exitNow = false;

            public Socket Socket { get; private set; }

            public EventTemplate<IClient, byte[]> BytesReceived { get; private set; } = new EventTemplate<IClient, byte[]>();
            public EventTemplate<IClient> Disconnected { get; private set; } = new EventTemplate<IClient>();
            public EventTemplate<IClient> Connected { get; private set; } = new EventTemplate<IClient>();

            private void ThreadProc(params object[] args)
            {
                while (!_exitNow)
                {
                    if (Socket.Available > 0)
                    {
                        Socket.GetResponse(out byte[] bytes);
                        OnReceived(ref bytes);
                    }
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                }
            }
            protected virtual void OnSend(ref byte[] bytes)
            {
                Socket.SendMessage(ref bytes);
            }
            protected virtual void OnReceived(ref byte[] bytes)
            {
                BytesReceived.Raise(this, bytes);
            }
            protected virtual void OnConnect(params object[] args)
            {
                if (Socket != null && Socket.Connected)
                    throw new InvalidOperationException("The socket is already connected");
                var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                socket.Connect((IPEndPoint)args[0]);

                args[0] = socket;
                this.As<IInitialize>().Initialize(args);
                Connected.Raise(this);
            }
            protected virtual bool OnInitialize(params object[] args)
            {
                Socket = (Socket)args[0];
                return true;
            }

            public void Write(byte[] bytes) { OnSend(ref bytes); }
            public void Connect(IPEndPoint ep)
            {
                OnConnect(ep);
            }
            public virtual void Dispose()
            {
                _exitNow = true;
                _thread?.Wait();
                _thread?.Dispose();
#if NET45
            Socket?.Close();
#endif
                Socket?.Dispose();
            }
            bool IInitialize.Initialize(params object[] args)
            {
                var result = OnInitialize(args);
                if (result)
                {
                    _thread = new Threading.ThreadMethod(ThreadProc);
                    _thread.Start();
                }
                return result;
            }
        }
    }
}