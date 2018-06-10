using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using W;

namespace W.Encryption
{
    /// <summary>
    /// Facilitates two way (assymetric) encryption via RSA cryptography
    /// </summary>
    public class AssymetricEncryption
    {
        private RSA _rsa;
        //private int _keySize;
        //private RSAParameters _privateKey;
        //private RSAParameters _publicKey;

        ///// <summary>
        ///// A delegate used by ExchangeKeys to facilitate the exchange of the public keys
        ///// </summary>
        ///// <param name="localPublicKey">The local public key</param>
        ///// <returns>Return the remote public key</returns>
        //public delegate RSAParameters? ExchangeKeysDelegate(RSAParameters localPublicKey);

        /// <summary>
        /// The KeySize the object was created with
        /// </summary>
        public int CurrentKeySize => _rsa.KeySize;
        /// <summary>
        /// The local public key
        /// </summary>
        public RSAParameters PublicKey => _rsa.PublicKey;
        /// <summary>
        /// The remote's public key
        /// </summary>
        public RSAParameters? RemotePublicKey { get; set; }
        /// <summary><para>
        /// Calls the function which completes the exchange and sets RemotePublicKey to the result.
        /// This function must be implemented by the developer and is contextual to his or her scenario.
        /// In all cases however, the return value must be the remote public key upon success, or null to specify a failure.
        /// </para></summary>
        /// <param name="del">The function to call</param>
        /// <returns>True if RemotePublicKey was assigned a non-null value, otherwise False</returns>
        public bool ExchangeKeys(Func<RSAParameters, RSAParameters?> del)
        {
            RemotePublicKey = del.Invoke(_rsa.PublicKey);
            return (RemotePublicKey != null);
        }

        /// <summary>
        /// Encrypts data with the remote public key
        /// </summary>
        /// <param name="bytes">The data to encrypt with the remote public key</param>
        /// <returns></returns>
        public bool Encrypt(ref byte[] bytes)
        {
            var result = false;
            if (RemotePublicKey == null)
                throw new InvalidOperationException("Nothing can be encrypted when the RemotePublicKey is null");
            try
            {
                //var message = RSAMethods.Encrypt(bytes.AsString(), (System.Security.Cryptography.RSAParameters)RemotePublicKey); //msg should be base64 encoded (by a previous Encrypt call) going into _rsa.Decrypt
                //bytes = message.AsBytes();
                bytes = RSAMethods.EncryptToBytes(bytes, (System.Security.Cryptography.RSAParameters)RemotePublicKey);
                result = true;
            }
            catch (Exception e)
            {
                Logging.Log(e.ToString());
                System.Diagnostics.Debugger.Break();
            }
            return result;
        }
        /// <summary>
        /// Decrypts data with the local private key
        /// </summary>
        /// <param name="bytes">The data to decrypt</param>
        /// <returns>The decrypted data</returns>
        public bool Decrypt(ref byte[] bytes)
        {
            var result = false;
            try
            {
                //var message = RSAMethods.Decrypt(bytes.AsString(), _rsa.PrivateKey); //msg should be base64 encoded (by a previous Encrypt call) going into _rsa.Decrypt
                //bytes = message.AsBytes();
                var resultBytes = RSAMethods.DecryptToBytes(bytes, _rsa.PrivateKey);
                bytes = resultBytes;
                result = true;
            }
            catch (Exception e)
            {
                //Log.e(e);
                Logging.Log(e.ToString());
                System.Diagnostics.Debugger.Break();
            }
            return result;
        }

        /// <summary>
        /// The legal RSA key sizes supported by the platform
        /// </summary>
        public KeySizes[] LegalKeySizes { get { return RSAMethods.LegalKeySizes(); } }
        /// <summary>
        /// Constructs a new TwoWayEncryption instance
        /// </summary>
        /// <param name="keySize">The encryption key size</param>
        public AssymetricEncryption(int keySize = 2048)
        {
            _rsa = new RSA(keySize);
            //_keySize = keySize;
            //RSAMethods.CreateKeyPair(_keySize, out _privateKey, out _publicKey);
        }
    }
}
