using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace W.Encryption
{
    /// <summary>
    /// Provides RSA encryption functionality
    /// </summary>
    public class RSA : IDisposable
    {
        /// <summary>
        /// The encryption key size
        /// </summary>
        public int KeySize { get; private set; }

        /// <summary>
        /// The private key used to decrypt data(do not share)
        /// </summary>
        public System.Security.Cryptography.RSAParameters PrivateKey;
        /// <summary>
        /// The public key used to encrypt data (should be shared)
        /// </summary>
        public System.Security.Cryptography.RSAParameters PublicKey;
        /// <summary>
        /// Gets the key sizes that are supported by the asymmetric algorithm
        /// </summary>
        /// <returns>An enumeration of the supported key sizes</returns>
        public KeySizes[] LegalKeySizes
        {
            get
            {
                return RSAMethods.LegalKeySizes();
            }
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="text">The string to be encrypted</param>
        /// <returns>A string containing the encrypted text</returns>
        public string Encrypt(string text) { return Encrypt(text, PublicKey); }
        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="text">The text to encrypt</param>
        /// <param name="publicKey">The public key used to encrypt the text</param>
        /// <returns>A string containing the encrypted text</returns>
        public string Encrypt(string text, RSAParameters publicKey) { return RSAMethods.Encrypt(text, publicKey); }
        /// <summary>
        /// Decrypts a string (previously encrypted with the Encrypt method)
        /// </summary>
        /// <param name="text">The encrypted string</param>
        /// <returns>A string containing the decrypted value</returns>
        public string Decrypt(string text) { return Decrypt(text, PrivateKey); }
        /// <summary>
        /// Decrypts a string (previously encrypted with the Encrypt method)
        /// </summary>
        /// <param name="text">The encrypted string</param>
        /// <param name="privateKey">The private key used to decrypt the string</param>
        /// <returns>A string containing the decrypted value</returns>
        public string Decrypt(string text, RSAParameters privateKey) { return RSAMethods.Decrypt(text, PrivateKey); }

        /// <summary>
        /// Disposes the instance and releases resources
        /// </summary>
        public virtual void Dispose()
        {
            //RSA.As<IDisposable>()?.Dispose();
        }
        /// <summary>
        /// Constructs a new RSA
        /// </summary>
        /// <param name="keySize"></param>
        public RSA(int keySize)
        {
            KeySize = keySize;
            RSAMethods.CreateKeyPair(KeySize, out PrivateKey, out PublicKey);
        }
    }

//#if NET45
//    /// <summary>
//    /// Provides RSA encryption functionality
//    /// </summary>
//    public class RSA : RSA<RSACryptoServiceProvider>
//    {
//        /// <summary>
//        /// constructs a new RSA object
//        /// </summary>
//        public RSA(int keySize = 2048) : base(keySize)
//        {
//            //RSA = new RSACryptoServiceProvider(keySize);
//        }
//    }
//#else
//    public class RSAStandard : RSA<System.Security.Cryptography.RSA>
//    {
//        public RSAStandard(int keySize) : base(keySize)
//        {
//            //RSA = System.Security.Cryptography.RSA.Create();
//        }
//    }

////    /// <summary>
////    /// Provides RSA encryption functionality
////    /// </summary>
////    /// <remarks>
////    /// Adapted from an online sample http://digitalsquid.co.uk/2009/01/rsa-crypto/
////    /// </remarks>
////    public class RSA : IDisposable
////    {
////        private System.Security.Cryptography.RSA _rsa = System.Security.Cryptography.RSA.Create();
////        private int _keySize;

////        /// <summary>
////        /// The private key used to decrypt data(do not share)
////        /// </summary>
////        public System.Security.Cryptography.RSAParameters PrivateKey { get; set; }
////        /// <summary>
////        /// The public key used to encrypt data (should be shared)
////        /// </summary>
////        public System.Security.Cryptography.RSAParameters PublicKey { get; set; }

////        /// <summary>
////        /// Gets the key sizes that are supported by the asymmetric algorithm
////        /// </summary>
////        /// <returns>An enumeration of the supported key sizes</returns>
////        public IEnumerable<System.Security.Cryptography.KeySizes> LegalKeySizes
////        {
////            get
////            {
////                foreach (var value in _rsa.LegalKeySizes)
////                    yield return value;
////            }
////        }

////        /// <summary>
////        /// constructs a new RSA object
////        /// </summary>
////        public RSA(int keySize = -1)
////        {
////            _keySize = keySize != -1 ? keySize : _rsa.LegalKeySizes?[0].MaxSize ?? _rsa.KeySize;
////            //foreach(var s in _rsa.LegalKeySizes)
////            //    Log.i("Min:{0}, Max:{1}, Skip:{2}", s.MinSize, s.MaxSize, s.SkipSize);
////            _rsa.KeySize = _keySize;
////            PrivateKey = _rsa.ExportParameters(true);
////            PublicKey = _rsa.ExportParameters(false);
////        }

////        /// <summary>
////        /// Destructs the RSA object and calls Dispose 
////        /// </summary>
////        ~RSA()
////        {
////            Dispose();
////        }

////        /// <summary>
////        /// Encrypts a string
////        /// </summary>
////        /// <param name="text">The string to encrypt</param>
////        /// <returns>A string containing the encrypted value</returns>
////        public string Encrypt(string text)
////        {
////            return Encrypt(text.AsBytes(), PublicKey);
////        }
////        /// <summary>
////        /// Encrypts a string
////        /// </summary>
////        /// <param name="text">The string to encrypt</param>
////        /// <param name="publicKey">The public key used to encrypt the string</param>
////        /// <returns>A string containing the encrypted value</returns>
////        public string Encrypt(string text, System.Security.Cryptography.RSAParameters publicKey)
////        {
////            return Encrypt(text.AsBytes(), publicKey);
////        }
////        /// <summary>
////        /// Encrypts a string
////        /// </summary>
////        /// <param name="byteData">The data to encrypt</param>
////        /// <param name="publicKey">The public key used to encrypt the data</param>
////        /// <returns>A string containing the encrypted data</returns>
////        public string Encrypt(byte[] byteData, System.Security.Cryptography.RSAParameters publicKey)
////        {
////            //Log.v("Encryption Hash={0}", MD5.GetMd5Hash(publicKey.AsXml<RSAParameters>()));

////            _rsa.ImportParameters(publicKey);
////            //byte[] byteData = Encoding.UTF32.GetBytes(text);
////            int maxLength = (_keySize / 8) - 42;// 214;
////            int dataLength = byteData.Length;
////            int iterations = dataLength / maxLength;

////            var sb = new StringBuilder();
////            for (int i = 0; i <= iterations; i++)
////            {
////                var tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
////                Buffer.BlockCopy(byteData, maxLength * i, tempBytes, 0, tempBytes.Length);
////                var encbyteData = _rsa.Encrypt(tempBytes, RSAEncryptionPadding.Pkcs1);
////                sb.Append(Convert.ToBase64String(encbyteData));
////            }
////            var result = sb.ToString();
////            return result;
////        }
////        /// <summary>
////        /// Encrypts data asynchronously
////        /// </summary>
////        /// <param name="data">The data to encrypt</param>
////        /// <param name="key">The public key used to encrypt the data</param>
////        /// <returns>A Task for the asynchronous operation</returns>
////        public async Task<string> EncryptAsync(byte[] data, System.Security.Cryptography.RSAParameters key)
////        {
////            var result = new StringBuilder();
////            _rsa.ImportParameters(key);

////            int maxLength = (_keySize / 8) - 42;// 214;
////            int dataLength = data.Length;
////            int iterations = dataLength / maxLength;

////            var tasks = new List<Task>();
////            var semaphore = new SemaphoreSlim(8);

////            for (var i = 0; i <= iterations; i++)
////            {
////                await semaphore.WaitAsync();

////                var tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
////                Buffer.BlockCopy(data, maxLength * i, tempBytes, 0, tempBytes.Length);
////                tasks.Add(EncryptBlock(semaphore, tempBytes, (s, iteration) =>
////                {
////                    result.Append(s);
////                    //Log.i("Iteration:{0}", iteration);
////                }, _rsa, i));
////            }
////            await Task.WhenAll(tasks);
////            return result.ToString();
////        }
////#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
////        private static async Task EncryptBlock(SemaphoreSlim semaphore, byte[] data, Action<string, int> result, System.Security.Cryptography.RSA rsa, int iteration)
////#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
////        {
////            try
////            {
////                var encbyteData = rsa.Encrypt(data, System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
////                var r = Convert.ToBase64String(encbyteData);
////                result?.Invoke(r, iteration);
////            }
////            catch (Exception e)
////            {
////                System.Diagnostics.Debug.WriteLine(e.ToString());
////                System.Diagnostics.Debugger.Break();
////                //Log.e(e);
////            }
////            finally
////            {
////                semaphore.Release();
////            }
////        }

////        /// <summary>
////        /// Decrypts a string (previously encrypted with the Encrypt method)
////        /// </summary>
////        /// <param name="base64String">The encrypted string</param>
////        /// <returns>A string containing the decrypted value</returns>
////        public string Decrypt(string base64String)
////        {
////            return Decrypt(base64String, PrivateKey);
////        }
////        /// <summary>
////        /// Decrypts a string (previously encrypted with the Encrypt method)
////        /// </summary>
////        /// <param name="base64String">The encrypted string</param>
////        /// <param name="privateKey">The private key used to decrypt the string</param>
////        /// <returns>A string containing the decrypted value</returns>
////        public string Decrypt(string base64String, System.Security.Cryptography.RSAParameters privateKey)
////        {
////            string result = null;
////            try
////            {
////                _rsa.ImportParameters(privateKey);
////                int base64BlockSize = (256 % 3 != 0) ? ((256 / 3) * 4) + 4 : (256 / 3) * 4;
////                int iterations = base64String.Length / base64BlockSize;
////                int l = 0;
////                var fullbytes = new byte[0];
////                for (int i = 0; i < iterations; i++)
////                {
////                    byte[] encBytes = Convert.FromBase64String(base64String.Substring(base64BlockSize * i, base64BlockSize));
////#if NET45
////                    byte[] bytes = _rsa.DecryptValue(encBytes);
////#else
////                    byte[] bytes = _rsa.Decrypt(encBytes, System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
////#endif
////                    Array.Resize(ref fullbytes, fullbytes.Length + bytes.Length);
////                    foreach (byte t in bytes)
////                    {
////                        fullbytes[l] = t;
////                        l++;
////                    }
////                }
////                result = fullbytes.AsString();// Encoding.UTF32.GetString(fullbytes);
////            }
////            catch (Exception e)
////            {
////                System.Diagnostics.Debug.WriteLine(e.ToString());
////                System.Diagnostics.Debugger.Break();
////                //Log.e(e);
////            }
////            return result;
////        }

////        /// <summary>
////        /// Decrypts a byte array (previously encrypted with the Encrypt method)
////        /// </summary>
////        /// <param name="cipher">The encrypted data</param>
////        /// <returns>A byte array containing the decrypted value</returns>
////        public byte[] Decrypt(byte[] cipher)
////        {
////            return Decrypt(cipher, PrivateKey);
////        }
////        /// <summary>
////        /// Decrypts a byte array (previously encrypted with the Encrypt method)
////        /// </summary>
////        /// <param name="cipher">The encrypted byte array</param>
////        /// <param name="privateKey">The private key used to decrypt the data</param>
////        /// <returns>A byte array containing the decrypted value</returns>
////        public byte[] Decrypt(byte[] cipher, System.Security.Cryptography.RSAParameters privateKey)
////        {
////            byte[] result = null;
////            try
////            {
////                _rsa.ImportParameters(privateKey);
////                int base64BlockSize = (256 % 3 != 0) ? ((256 / 3) * 4) + 4 : (256 / 3) * 4;
////                int iterations = cipher.Length / base64BlockSize;
////                int l = 0;
////                var fullbytes = new byte[0];
////                var encBytes = new byte[base64BlockSize];// {} cipher.Substring(base64BlockSize * i, base64BlockSize));
////                for (int i = 0; i < iterations; i++)
////                {
////                    Array.Copy(cipher, base64BlockSize * i, encBytes, 0, base64BlockSize);
////                    byte[] bytes = _rsa.Decrypt(encBytes, System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
////                    Array.Resize(ref fullbytes, fullbytes.Length + bytes.Length);
////                    foreach (byte t in bytes)
////                    {
////                        fullbytes[l] = t;
////                        l++;
////                    }
////                }
////                result = fullbytes;// Encoding.UTF32.GetString(fullbytes);
////            }
////            catch (Exception e)
////            {
////                System.Diagnostics.Debug.WriteLine(e.ToString());
////                System.Diagnostics.Debugger.Break();
////                //Log.e(e);
////            }
////            return result;
////        }
////        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
////        public void Dispose()
////        {
////            _rsa?.Dispose();
////            _rsa = null;
////        }
////    }
//#endif
}
