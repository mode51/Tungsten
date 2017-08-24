using System;
using System.Net;
using System.Threading.Tasks;

namespace W.Net
{
    public interface ISocketClient : IDisposable //Disposable so the server can terminate the
    {
        void Configure(ISocketServer server, System.Net.Sockets.TcpClient existingClient = null);
        IPEndPoint RemoteEndPoint { get; }
    }

    public interface ISocketServer
    {
        void DisconnectClient(ISocketClient client, IPEndPoint remoteEndPoint, Exception e = null);
    }

    public interface ISecureSocketServer : ISocketServer
    {
        W.Encryption.RSA RSA { get; }
    }
}