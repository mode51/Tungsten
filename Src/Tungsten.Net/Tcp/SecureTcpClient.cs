using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using W.Encryption;

namespace W.Net
{
    public static partial class Tcp
    {
        public class SecureTcpClient : TcpClient
        {
            private int _keySize;
            public event Action<SecureTcpClient> SecureFailed;// { get; private set; } = new EventTemplate<SecureTcpClient>();

            protected AssymetricEncryption Encryption { get; private set; }

            protected override void OnSend(ref byte[] bytes)
            {
                if (Encryption.RemotePublicKey != null)
                    Encryption.Encrypt(ref bytes);
                base.OnSend(ref bytes);
            }
            protected override void OnReceived(ref byte[] bytes)
            {
                if (Encryption.RemotePublicKey != null)
                    Encryption.Decrypt(ref bytes);
                base.OnReceived(ref bytes);
            }
            protected override bool OnInitialize(params object[] args)
            {
                base.OnInitialize(args);

                Encryption = new AssymetricEncryption((int)args[1]);
                var result = Encryption.ExchangeKeys(myPublicKey =>
                {
                    RSAParameters? remotePublicKey = null;
                    byte[] response;
                    var bytes = SerializationMethods.Serialize(myPublicKey).AsBytes();
                    if (Socket.SendAndWaitForResponse(ref bytes, out response, 5000))
                        remotePublicKey = SerializationMethods.Deserialize<RSAParameters>(ref response);

                    if (remotePublicKey == null)
                        SecureFailed?.Invoke(this);
                    return remotePublicKey;
                });
                return result;
            }
            public new void Connect(IPEndPoint ep)
            {
                OnConnect(ep, _keySize); //keySize is used here when it's a Client, and in Initialize when it's a server
            }
            public SecureTcpClient(int keySize)
            {
                _keySize = keySize;
            }
        }
    }
}