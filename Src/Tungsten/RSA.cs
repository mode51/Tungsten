using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W.Logging;

namespace W.Encryption
{
    /// <summary>
    /// The public and private RSA keys
    /// </summary>
    public class PublicPrivateKeyPair
    {
        /// <summary>
        /// The private RSA key.  Should never be shared.
        /// </summary>
        public RSAParameters PrivateKey { get; set; }
        /// <summary>
        /// the public RSA key. Should be shared.
        /// </summary>
        public RSAParameters PublicKey { get; set; }
    }
    /// <summary>
    /// Provides RSA encryption functionality
    /// </summary>
    public class RSA : IDisposable
    {
        //private RSACryptoServiceProvider _rsa = null;
        private int _keySize;

        /// <summary>
        /// The private key used to decrypt data(do not share)
        /// </summary>
        public RSAParameters PrivateKey { get; set; }
        /// <summary>
        /// The public key used to encrypt data (should be shared)
        /// </summary>
        public RSAParameters PublicKey { get; set; }

        /// <summary>
        /// constructs a new RSA object
        /// </summary>
        public RSA(int keySize = 2048)
        {
            _keySize = keySize;// != -1 ? keySize : _rsa.KeySize;
            using (var _rsa = new RSACryptoServiceProvider(_keySize))
            {
                PrivateKey = _rsa.ExportParameters(true);
                PublicKey = _rsa.ExportParameters(false);
            }
        }

        /// <summary>
        /// Destructs the RSA object and calls Dispose 
        /// </summary>
        ~RSA()
        {
            Dispose();
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="text">The string to encrypt</param>
        /// <returns>A string containing the encrypted value</returns>
        public string Encrypt(string text)
        {
            return Encrypt(text.AsBytes(), PublicKey);
        }
        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="text">The string to encrypt</param>
        /// <param name="publicKey">The public key used to encrypt the string</param>
        /// <returns>A string containing the encrypted value</returns>
        public string Encrypt(string text, RSAParameters publicKey)
        {
            return Encrypt(text.AsBytes(), publicKey);
        }
        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="byteData">The data to encrypt</param>
        /// <param name="publicKey">The public key used to encrypt the data</param>
        /// <returns>A string containing the encrypted data</returns>
        public string Encrypt(byte[] byteData, RSAParameters publicKey)
        {
            return RSAMethods.Encrypt(byteData, publicKey, _keySize);
        }

        /// <summary>
        /// Decrypts a string (previously encrypted with the Encrypt method)
        /// </summary>
        /// <param name="base64String">The encrypted string</param>
        /// <returns>A string containing the decrypted value</returns>
        public string Decrypt(string base64String)
        {
            return Decrypt(base64String, PrivateKey);
        }
        /// <summary>
        /// Decrypts a string (previously encrypted with the Encrypt method)
        /// </summary>
        /// <param name="base64String">The encrypted string</param>
        /// <param name="privateKey">The private key used to decrypt the string</param>
        /// <returns>A string containing the decrypted value</returns>
        public string Decrypt(string base64String, RSAParameters privateKey)
        {
            return RSAMethods.Decrypt(base64String, privateKey, _keySize);
        }

        //public byte[] Decrypt(byte[] cipher)
        //{
        //    return Decrypt(cipher, PrivateKey);
        //}
        //public byte[] Decrypt(byte[] cipher, RSAParameters privateKey)
        //{
        //    return RSA_Old.Decrypt(cipher, privateKey, _keySize);
        //}
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            //_rsa?.Dispose();
            //_rsa = null;
        }
    }

    /// <summary>
    /// Creates MD5 hashes
    /// </summary>
    public class MD5
    {
        /// <summary>
        /// Create an MD5 hash for the specified string
        /// </summary>
        /// <param name="input">The value for which to create an MD5 hash</param>
        /// <returns>The MD5 hash of the specified value</returns>
        public static string GetMd5Hash(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return MD5.GetMd5Hash(input, md5);
            }
        }
        /// <summary>
        /// Create an MD5 hash for the specified string
        /// </summary>
        /// <param name="input">The value for which to create an MD5 hash</param>
        /// <param name="md5">The MD5 object to use to create the hash</param>
        /// <returns>The MD5 hash of the specified value</returns>
        public static string GetMd5Hash(string input, System.Security.Cryptography.MD5 md5)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Verify a hash against a string
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="hash">The MD5 hash to use to verify the input string</param>
        /// <returns>True if the input string is verified</returns>
        public static bool VerifyMd5Hash(string input, string hash)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                // Hash the input.
                string hashOfInput = GetMd5Hash(input, md5);

                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, hash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    /// <summary>
    /// Static Encrypt and Decrypt methods
    /// </summary>
    public class RSAMethods
    {
        //adapted from an online sample (http://digitalsquid.co.uk/2009/01/rsa-crypto/)
        /// <summary>
        /// Encrypts a string with RSA encryption
        /// </summary>
        /// <param name="text">The string to encrypt</param>
        /// <param name="key">The public key to use to encrypt the string</param>
        /// <param name="keySize">The keysize, in bits, of the public key</param>
        /// <returns>The encrypted value of the specified data</returns>
        public static string Encrypt(string text, string key, int keySize = 2048)
        {
            return Encrypt(Encoding.UTF8.GetBytes(text), key, keySize);
        }
        /// <summary>
        /// Encrypts a string with RSA encryption
        /// </summary>
        /// <param name="text">The string to encrypt</param>
        /// <param name="key">The public key to use to encrypt the string</param>
        /// <param name="keySize">The keysize, in bits, of the public key</param>
        /// <returns>The encrypted value of the specified data</returns>
        public static string Encrypt(string text, RSAParameters key, int keySize = 2048)
        {
            return Encrypt(Encoding.UTF8.GetBytes(text), key, keySize);
        }
        /// <summary>
        /// Encrypts a byte array with RSA encryption
        /// </summary>
        /// <param name="byteData">The byte array to encrypt</param>
        /// <param name="key">The public key to use to encrypt the byte array</param>
        /// <param name="keySize">The keysize, in bits, of the public key</param>
        /// <returns>The encrypted byte array of the specified data</returns>
        public static string Encrypt(byte[] byteData, string key, int keySize = 2048)
        {
            using (var rsa = new RSACryptoServiceProvider(keySize))
            {

                rsa.FromXmlString(key);
                //byte[] byteData = Encoding.UTF32.GetBytes(text);
                const int maxLength = 214;
                int dataLength = byteData.Length;
                int iterations = dataLength / maxLength;

                var sb = new StringBuilder();
                for (int i = 0; i <= iterations; i++)
                {
                    var tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                    Buffer.BlockCopy(byteData, maxLength * i, tempBytes, 0, tempBytes.Length);

                    var encbyteData = rsa.Encrypt(tempBytes, false);
                    sb.Append(Convert.ToBase64String(encbyteData));
                }
                var result = sb.ToString();
                return result;
            }
        }
        /// <summary>
        /// Encrypts a byte array with RSA encryption
        /// </summary>
        /// <param name="byteData">The byte array to encrypt</param>
        /// <param name="key">The public key to use to encrypt the byte array</param>
        /// <param name="keySize">The keysize, in bits, of the public key</param>
        /// <returns>The encrypted byte array of the specified data</returns>
        public static string Encrypt(byte[] byteData, RSAParameters key, int keySize = 2048)
        {
            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.ImportParameters(key);
                //byte[] byteData = Encoding.UTF32.GetBytes(text);
                const int maxLength = 214;
                int dataLength = byteData.Length;
                int iterations = dataLength / maxLength;

                var sb = new StringBuilder();
                for (int i = 0; i <= iterations; i++)
                {
                    var tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                    Buffer.BlockCopy(byteData, maxLength * i, tempBytes, 0, tempBytes.Length);

                    var encbyteData = rsa.Encrypt(tempBytes, false);
                    sb.Append(Convert.ToBase64String(encbyteData));
                }
                var result = sb.ToString();
                return result;
            }
        }

        //adapted from an online sample (http://digitalsquid.co.uk/2009/01/rsa-crypto/)
        /// <summary>
        /// Decrypts a string previously encrypted with RSA encryption
        /// </summary>
        /// <param name="text">The RSA encrypted string</param>
        /// <param name="key">The private key to use for decrypting</param>
        /// <param name="keySize">The keysize, in bits, of the private key</param>
        /// <returns>The decrypted string</returns>
        public static string Decrypt(string text, string key, int keySize = 2048)
        {
            string result = null;
            try
            {
                using (var rsa = new RSACryptoServiceProvider(keySize))
                {
                    rsa.FromXmlString(key);
                    int base64BlockSize = (256 % 3 != 0) ? ((256 / 3) * 4) + 4 : (256 / 3) * 4;
                    int iterations = text.Length / base64BlockSize;
                    int l = 0;
                    var fullbytes = new byte[0];
                    for (int i = 0; i < iterations; i++)
                    {
                        byte[] encBytes = Convert.FromBase64String(text.Substring(base64BlockSize * i, base64BlockSize));
                        byte[] bytes = rsa.Decrypt(encBytes, false);
                        Array.Resize(ref fullbytes, fullbytes.Length + bytes.Length);
                        foreach (byte t in bytes)
                        {
                            fullbytes[l] = t;
                            l++;
                        }
                    }
                    result = Encoding.UTF8.GetString(fullbytes);
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return result;
        }
        /// <summary>
        /// Decrypts a string previously encrypted with RSA encryption
        /// </summary>
        /// <param name="text">The RSA encrypted string</param>
        /// <param name="key">The private key to use for decrypting</param>
        /// <param name="keySize">The keysize, in bits, of the private key</param>
        /// <returns>The decrypted string</returns>
        public static string Decrypt(string text, RSAParameters key, int keySize = 2048)
        {
            string result = null;
            try
            {
                using (var rsa = new RSACryptoServiceProvider(keySize))
                {
                    rsa.ImportParameters(key);
                    int base64BlockSize = (256 % 3 != 0) ? ((256 / 3) * 4) + 4 : (256 / 3) * 4;
                    int iterations = text.Length / base64BlockSize;
                    int l = 0;
                    var fullbytes = new byte[0];
                    for (int i = 0; i < iterations; i++)
                    {
                        byte[] encBytes = Convert.FromBase64String(text.Substring(base64BlockSize * i, base64BlockSize));
                        byte[] bytes = rsa.Decrypt(encBytes, false);
                        Array.Resize(ref fullbytes, fullbytes.Length + bytes.Length);
                        foreach (byte t in bytes)
                        {
                            fullbytes[l] = t;
                            l++;
                        }
                    }
                    result = Encoding.UTF8.GetString(fullbytes);
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return result;
        }
        ///// <summary>
        ///// Decrypts a string previously encrypted with RSA encryption
        ///// </summary>
        ///// <param name="text">The RSA encrypted string</param>
        ///// <param name="key">The private key to use for decrypting</param>
        ///// <param name="keySize">The keysize, in bits, of the private key</param>
        ///// <returns>The decrypted string</returns>
        //public static string Decrypt(string text, System.Security.SecureString key, int keySize = 2048)
        //{
        //    string result = null;
        //    try
        //    {
        //        using (var rsa = new RSACryptoServiceProvider(keySize))
        //        {
        //            rsa.FromXmlString(SecureStringMethods.ConvertToUnsecureString(key));
        //            int base64BlockSize = (256 % 3 != 0) ? ((256 / 3) * 4) + 4 : (256 / 3) * 4;
        //            int iterations = text.Length / base64BlockSize;
        //            int l = 0;
        //            var fullbytes = new byte[0];
        //            for (int i = 0; i < iterations; i++)
        //            {
        //                byte[] encBytes = Convert.FromBase64String(text.Substring(base64BlockSize * i, base64BlockSize));
        //                byte[] bytes = rsa.Decrypt(encBytes, false);
        //                Array.Resize(ref fullbytes, fullbytes.Length + bytes.Length);
        //                foreach (byte t in bytes)
        //                {
        //                    fullbytes[l] = t;
        //                    l++;
        //                }
        //            }
        //            result = Encoding.UTF8.GetString(fullbytes);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Log.e(e);
        //    }
        //    return result;
        //}
    }

    /// <summary>
    /// Provides methods for RSA Encryption
    /// </summary>
    /// <remarks>This class was adapted from an online sample at http://digitalsquid.co.uk/2009/01/rsa-crypto/ </remarks>
    public class RSA_Old
    {
        /// <summary>
        /// Creates a public/private key pair which can be used for RSA encryption
        /// </summary>
        /// <param name="publicKey">The public key</param>
        /// <param name="privateKey">The private key</param>
        /// <param name="keySize">The keysize, in bits, for the keys</param>
        /// <returns>True if successful, otherwise False</returns>
        public static bool CreatePublicPrivateKeyPair(out string publicKey, out string privateKey, int keySize = 2048)
        {
            bool result = false;
            try
            {
                using (var rsa = new RSACryptoServiceProvider(keySize))
                {
                    privateKey = rsa.ToXmlString(true);
                    publicKey = rsa.ToXmlString(false);
                }
                result = true;
            }
            catch
            {
                publicKey = null;
                privateKey = null;
            }
            return result;
        }

        /// <summary>
        /// Used to store a public/private key pair for RSA encryption
        /// </summary>
        public class RSAPublicPrivateKeyPair
        {
            /// <summary>
            /// The public key used to encrypt data
            /// </summary>
            public string PublicKey { get; set; }
            /// <summary>
            /// The private key used to decrypt data
            /// </summary>
            public string PrivateKey { get; set; }

            /// <summary>
            /// Creates a new public/private key pair for use with RSA encryption
            /// </summary>
            /// <param name="keySize">The keysize, in bits, of the key to use</param>
            /// <returns></returns>
            public static RSAPublicPrivateKeyPair Create(int keySize = 2048)
            {
                var result = new RSAPublicPrivateKeyPair();
                try
                {
                    using (var rsa = new RSACryptoServiceProvider(keySize))
                    {
                        result.PrivateKey = rsa.ToXmlString(true);
                        result.PublicKey = rsa.ToXmlString(false);
                    }
                    return result;
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
                return null;
            }
        }
    }
}