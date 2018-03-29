using System;

namespace W.Net
{
    public static partial class Tcp
    {
        public static partial class Generic
        {
            public class SecureTcpClient<TMessage> : SecureTcpClient
            {
                public event Action<SecureTcpClient<TMessage>, TMessage> MessageReceived;// { get; private set; } = new EventTemplate<SecureTcpClient<TMessage>, TMessage>();
                protected override void OnReceived(ref byte[] bytes)
                {
                    base.OnReceived(ref bytes);
                    var message = SerializationMethods.Deserialize<TMessage>(ref bytes);
                    MessageReceived?.Invoke(this, message);
                }
                public void Write(TMessage message)
                {
                    var bytes = SerializationMethods.Serialize(message).AsBytes();
                    //base.OnSend(ref bytes);
                    Write(bytes);
                }
                public SecureTcpClient(int keySize) : base(keySize) { }
            }
        }
    }
}