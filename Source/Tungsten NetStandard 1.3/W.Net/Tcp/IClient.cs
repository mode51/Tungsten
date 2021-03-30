using System;
using System.Net;
using System.Net.Sockets;

namespace W.Net
{
    public static partial class Tcp
    {
        public interface IClient : IDisposable
        {
            Guid Id { get; }
            Socket Socket { get; }
            event Action<IClient, byte[]> BytesReceived;
            event Action<IClient> Disconnected;
            event Action<IClient> Connected;
            //EventTemplate<IClient, byte[]> BytesReceived { get; }
            //EventTemplate<IClient> Connected { get; }
            //EventTemplate<IClient> Disconnected { get; }
            void Write(byte[] bytes);
            void Connect(IPEndPoint ep);
        }
    }
}
