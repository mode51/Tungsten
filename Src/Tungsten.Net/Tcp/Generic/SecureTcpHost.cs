using System;
using System.Linq;
using System.Text;

namespace W.Net
{
    public static partial class Tcp
    {
        public static partial class Generic
        {
            public class SecureTcpHost<TMessage> : TcpHost
            {
                public event Action<SecureTcpHost<TMessage>, SecureTcpClient<TMessage>, TMessage> MessageReceived;// { get; private set; } = new EventTemplate<SecureTcpHost<TMessage>, SecureTcpClient<TMessage>, TMessage>();

                protected override void OnBytesReceived(IClient client, byte[] bytes)
                {
                    base.OnBytesReceived(client, bytes);
                    var message = SerializationMethods.Deserialize<TMessage>(ref bytes);
                    MessageReceived?.Invoke(this, (SecureTcpClient<TMessage>)client, message);
                }
                public SecureTcpHost(int keySize)
                {
                    OnCreateServer = s => { var server = new SecureTcpClient<TMessage>(keySize); server.As<IInitialize>().Initialize(s, keySize); return server; };
                }
            }
        }
    }
}