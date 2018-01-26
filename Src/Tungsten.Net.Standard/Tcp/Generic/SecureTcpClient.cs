using W.AsExtensions;

namespace W.Net
{
    public static partial class Tcp
    {
        public static partial class Generic
        {
            public class SecureTcpClient<TMessage> : SecureTcpClient
            {
                public EventTemplate<SecureTcpClient<TMessage>, TMessage> MessageReceived { get; private set; } = new EventTemplate<SecureTcpClient<TMessage>, TMessage>();
                protected override void OnReceived(ref byte[] bytes)
                {
                    base.OnReceived(ref bytes);
                    var message = SerializationMethods.Deserialize<TMessage>(ref bytes);
                    MessageReceived.Raise(this, message);
                }
                public void Write(TMessage message)
                {
                    base.Write(SerializationMethods.Serialize(message).AsBytes());
                }
                public SecureTcpClient(int keySize) : base(keySize) { }
            }
        }
    }
}