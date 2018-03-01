
namespace W.Net
{
    public static partial class Tcp
    {
        public class SecureTcpHost : TcpHost
        {
            public SecureTcpHost(int keySize)
            {
                OnCreateServer = s => { var server = new SecureTcpClient(keySize); server.As<IInitialize>().Initialize(s, keySize); return server; };
            }
        }
    }
}
