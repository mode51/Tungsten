using System;

namespace W.Net
{
    public static partial class Tcp
    {
        public static partial class Generic
        {
            public class TcpClient<TMessage> : TcpClient
            {
                public event Action<TcpClient<TMessage>, TMessage> MessageReceived;// { get; private set; } = new EventTemplate<TcpClient<TMessage>, TMessage>();

                protected override void OnReceived(ref byte[] bytes)
                {
                    base.OnReceived(ref bytes);
                    var message = SerializationMethods.Deserialize<TMessage>(ref bytes);
                    MessageReceived?.Invoke(this, message);
                }
                public void Write(TMessage message)
                {
                    base.Write(SerializationMethods.Serialize(message).AsBytes());
                }
            }
        }
    }
}