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
            private bool _isInitialized = false;
            public event Action<SecureTcpClient> SecureFailed;// { get; private set; } = new EventTemplate<SecureTcpClient>();

            protected AssymetricEncryption Encryption { get; private set; }
            protected bool IsSecure => Encryption?.RemotePublicKey != null;

            protected override void OnSend(ref byte[] bytes)
            {
                var temp = (byte[])bytes.Clone();
                if (Encryption.RemotePublicKey != null)
                    Encryption.Encrypt(ref bytes);
                if (Encryption.RemotePublicKey == null)
                    System.Diagnostics.Debugger.Break();
                base.OnSend(ref bytes);
            }
            protected override void OnReceived(ref byte[] bytes)
            {
                var temp = (byte[])bytes.Clone();
                if (Encryption.RemotePublicKey != null)
                    Encryption.Decrypt(ref bytes);
                if (Encryption.RemotePublicKey == null)
                    System.Diagnostics.Debugger.Break();
                base.OnReceived(ref bytes);
            }
            protected override bool OnInitialize(params object[] args)
            {
                bool result = base.OnInitialize(args);

                //Encryption = new AssymetricEncryption((int)args[1]);
                result = Encryption.ExchangeKeys(myPublicKey =>
                {
                    RSAParameters? remotePublicKey = null;
                    byte[] response;
                    var bytes = SerializationMethods.Serialize(myPublicKey).AsBytes();
                    if (Socket.SendAndWaitForResponse(ref bytes, out response, 60000))
                        remotePublicKey = SerializationMethods.Deserialize<RSAParameters>(ref response);

                    if (remotePublicKey == null)
                        SecureFailed?.Invoke(this);
                    else
                        _isInitialized = true;
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
                Encryption = new AssymetricEncryption(_keySize); //create the RSA keys here
            }
        }
    }
}