using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.Logging;

namespace W.RPC
{
    internal class EncryptedClient : ClientBase
    {
        private RSA.RSAPublicPrivateKeyPair _keypair = RSA.RSAPublicPrivateKeyPair.Create();
        private string _remotePublicKey = String.Empty;
        private bool _initializeEncryption = true;

        public delegate void SecuredDelegate(EncryptedClient client);
        public new delegate void MessageArrivedDelegate(EncryptedClient client, string message);

        public event SecuredDelegate Secured;
        public new event MessageArrivedDelegate MessageArrived;
        public Property<bool> IsSecure { get; } = new Property<bool>(false);

        private void RaiseSecured()
        {
            var evt = Secured;
            try
            {
                evt?.Invoke(this);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }
        private void RaiseMessageArrived(string message)
        {
            var evt = this.MessageArrived;
            try
            {
                evt?.Invoke(this, message);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }

        protected override void OnMessageSend(ref string message)
        {
            if (!String.IsNullOrEmpty(_remotePublicKey))
                message = RSA.Encrypt(message, _remotePublicKey);
            base.OnMessageSend(ref message);
        }
        protected override void OnMessageReceived(ref string message)
        {
            if (_initializeEncryption)
            {
                _remotePublicKey = message;
                Log.i("{0}: Received Remote's Public Key", Name);
                message = String.Empty;
                _initializeEncryption = false;
                IsSecure.Value = true;
                RaiseSecured();
                return;
            }
            else
            {
                message = RSA.Decrypt(message, _keypair.PrivateKey);
            }
            base.OnMessageReceived(ref message);
        }

        public new void Post(string message)
        {
            base.Post(message);
        }

        public EncryptedClient()
        {
            base.Connected += (sender, address) =>
            {
                base.Post(_keypair.PublicKey);
            };
            base.Disconnected += (client, address, exception) =>
            {
                _remotePublicKey = "";
                _initializeEncryption = true;
            };
            base.MessageArrived += (sender, message) => RaiseMessageArrived(message);
        }
    }
}
