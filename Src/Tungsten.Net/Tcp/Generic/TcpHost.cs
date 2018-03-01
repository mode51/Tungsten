using System;

namespace W.Net
{
    public static partial class Tcp
    {
        public static partial class Generic
        {
            public class TcpHost<TMessage> : TcpHost
            {
                public event Action<TcpHost<TMessage>, TcpClient<TMessage>, TMessage> MessageReceived;// { get; private set; } = new EventTemplate<TcpHost<TMessage>, TcpClient<TMessage>, TMessage>();

                protected override void OnBytesReceived(IClient client, byte[] bytes)
                {
                    base.OnBytesReceived(client, bytes);
                    var message = SerializationMethods.Deserialize<TMessage>(ref bytes);
                    MessageReceived?.Invoke(this, (TcpClient<TMessage>)client, message);
                }
                public TcpHost()
                {
                    OnCreateServer = s => { var server = new TcpClient<TMessage>(); server.As<IInitialize>().Initialize(s); return server; };
                }
            }
        }
    }
}