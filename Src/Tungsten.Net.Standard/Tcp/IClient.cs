using System;
using System.Net;
using System.Net.Sockets;

namespace W.Net
{
    public static partial class Tcp
    {
        public interface IClient : IDisposable
        {
            Socket Socket { get; }
            EventTemplate<IClient, byte[]> BytesReceived { get; }
            EventTemplate<IClient> Connected { get; }
            EventTemplate<IClient> Disconnected { get; }
            void Write(byte[] bytes);
            void Connect(IPEndPoint ep);
        }
    }
}
