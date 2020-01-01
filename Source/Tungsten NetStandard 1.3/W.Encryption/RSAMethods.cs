using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace W.Encryption
{
    using System.Security.Cryptography;

    internal static class AsExtensions
    {
        public static byte[] AsBytes(this string @this)
        {
            return System.Text.Encoding.UTF8.GetBytes(@this);
        }
        public static string AsString(this byte[] @this)
        {
            return System.Text.Encoding.UTF8.GetString(@this, 0, @this.Length);
        }
    }
#pragma warning disable CS1584
#pragma warning disable CS1658
    /// Warning is overriding an error
    /// XML comment has syntactically incorrect cref attribute
    /// <summary><para>
    /// Replaces RSA.  This code was adapted for NetStandard from an article published on CodeProject by Mathew John Schlabaugh in 2007.  It is less complicated but works more often than my initial RSA implementation. See: https://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET
    /// </para></summary>
    /// <see cref="https://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET"/>
#pragma warning restore CS1658 // Warning is overriding an error
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute
    public static class RSAMethods
    {
#if NET45
        private static System.Security.Cryptography.RSA GetRSA(int? keySize = null)
        {
            if (keySize != null)
                return new System.Security.Cryptography.RSACryptoServiceProvider((int)keySize);
            return System.Security.Cryptography.RSACryptoServiceProvider.Create();
        }
#else
        private static System.Security.Cryptography.RSA GetRSA()
        {
            return System.Security.Cryptography.RSA.Create();
        }
#endif

        /// <summary>
        /// Returns an arrary containing the supported key sizes
        /// </summary>
        /// <returns>An array of supported key sizes</returns>
        public static KeySizes[] LegalKeySizes()
        {
            using (var rsa = GetRSA())
                return rsa.LegalKeySizes;
        }
        /// <summary>
        /// Generates a public/private key pair
        /// </summary>
        /// <param name="keySize">The key size to use when creating the public and private keys</param>
        /// <param name="privateKey">The generated private key</param>
        /// <param name="publicKey">The generated public key</param>
        /// <returns>A newly created PublicPrivateKeyPair containing the public and private keys</returns>
        public static void CreateKeyPair(int keySize, out RSAParameters privateKey, out RSAParameters publicKey)
        {
            using (var rsa = GetRSA())
            {
                rsa.KeySize = keySize;
                privateKey = rsa.ExportParameters(true);
                publicKey = rsa.ExportParameters(false);
            }
        }

        ///// <summary>
        ///// Encrypts a string using the specified keysize and public key
        ///// </summary>
        ///// <param name="text">The data to encrypt</param>
        ///// <param name="key">The public key used to encrypt the data</param>
        ///// <returns>A string containing the encrypted data</returns>
        //public static string Encrypt(string text, RSAParameters key)
        //{
        //    return Encrypt(text.AsBytes(), key).AsString();
        //}
        /// <summary>
        /// Encrypts a byte array using the specified keysize and public key
        /// </summary>
        /// <param name="bytes">The data to encrypt</param>
        /// <param name="key">The key used to encrypt the data</param>
        /// <returns>A byte array containing the encrypted data</returns>
        public static byte[] Encrypt(byte[] bytes, RSAParameters key)
        {
            byte[] result = new byte[0];
            using (var rsa = GetRSA())
            {
                rsa.ImportParameters(key);
                //byte[] bytes = text.AsBytes();
                int maxLength = (rsa.KeySize / 8) - 42;// ((rsa.KeySize / 8) % 3 != 0) ? (((rsa.KeySize / 8) / 3) * 4) + 4 : ((rsa.KeySize / 8) / 3) * 4;
                int dataLength = bytes.Length;
                int iterations = dataLength / maxLength;
                var stringBuilder = new StringBuilder();
                for (int i = 0; i <= iterations; i++)
                {
                    byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                    Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
#if NET45
                    byte[] encryptedBytes = ((RSACryptoServiceProvider)rsa).Encrypt(tempBytes, false);
#else
                    byte[] encryptedBytes = rsa.Encrypt(tempBytes, RSAEncryptionPadding.Pkcs1); //8.29.2017 - seems we can only use Pkcs1, probably because it's multi-platform
#endif
                    // Be aware the RSACryptoServiceProvider reverses the order of 
                    // encrypted bytes. It does this after encryption and before 
                    // decryption. If you do not require compatibility with Microsoft 
                    // Cryptographic API (CAPI) and/or other vendors. Comment out the 
                    // next line and the corresponding one in the DecryptString function.
                    //Array.Reverse(encryptedBytes);
                    // Why convert to base 64?
                    // Because it is the largest power-of-two base printable using only 
                    // ASCII characters
                    var len = result.Length;
                    Array.Resize(ref result, len + encryptedBytes.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, result, len, encryptedBytes.Length);
                }
            }
            return result;
        }
        ///// <summary>
        ///// Decrypts a string which was previously encrypted with the Encrypt method
        ///// </summary>
        ///// <param name="text">The encrypted data</param>
        ///// <param name="key">The key to decrypt the data</param>
        ///// <returns>A byte array containing the decrypted value</returns>
        //public static string Decrypt(string text, RSAParameters key)
        //{
        //    return Decrypt(text.AsBytes(), key).AsString();
        //}
        /// <summary>
        /// Decrypts a byte array which was previously encrypted with the Encrypt method
        /// </summary>
        /// <param name="bytes">The encrypted data</param>
        /// <param name="key">The key to decrypt the data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public static byte[] Decrypt(byte[] bytes, RSAParameters key)
        {
            using (var rsa = GetRSA())
            {
                rsa.ImportParameters(key);
                int blockSize = rsa.KeySize / 8;
                //int base64BlockSize = (rsa.KeySize / 8) - 42;// ((rsa.KeySize / 8) % 3 != 0) ? (((rsa.KeySize / 8) / 3) * 4) + 4 : ((rsa.KeySize / 8) / 3) * 4;
                //int iterations = bytes.Length / base64BlockSize;
                int iterations = bytes.Length / blockSize;
                var fullBytes = new byte[0];

                for (int i = 0; i < iterations; i++)
                {
                    byte[] encryptedBytes = new byte[blockSize];
                    Array.Copy(bytes, blockSize * i, encryptedBytes, 0, blockSize);
                    // Be aware the RSACryptoServiceProvider reverses the order of 
                    // encrypted bytes after encryption and before decryption.
                    // If you do not require compatibility with Microsoft Cryptographic 
                    // API (CAPI) and/or other vendors.
                    // Comment out the next line and the corresponding one in the 
                    // EncryptString function.
                    //Array.Reverse(encryptedBytes);
#if NET45
                    var decryptedBytes = ((RSACryptoServiceProvider)rsa).Decrypt(encryptedBytes, false);
#else
                    var decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1); //8.29.2017 - seems we can only use Pkcs1, probably because it's multi-platform
#endif
                    var len = fullBytes.Length;
                    Array.Resize(ref fullBytes, len + decryptedBytes.Length);
                    Array.Copy(decryptedBytes, 0, fullBytes, len, decryptedBytes.Length);
                    //arrayList.AddRange(bytes);
                }
                return fullBytes;
            }
        }

//        /// <summary>
//        /// Asynchronously encrypts a string using the specified keysize and public key
//        /// </summary>
//        /// <param name="text">The string to encrypt</param>
//        /// <param name="key">The key to encrypt the data</param>
//        /// <returns>A string containing the encrypted data</returns>
//        public static async Task<string> EncryptAsync(string text, RSAParameters key)
//        {
//#if NET45
//            return await Task.Run(() =>
//            {
//                return Encrypt(text, key);
//            });
//#else
//            return await Task.Run(() =>
//            {
//                return Encrypt(text, key);
//            });
//#endif
//        }
//        /// <summary>
//        /// Asynchronously decrypts a string which was previously encrypted with the Encrypt method
//        /// </summary>
//        /// <param name="text">The encrypted string</param>
//        /// <param name="key">The key to decrypt the data</param>
//        /// <returns>A string containing the decrypted value</returns>
//        public static async Task<string> DecryptAsync(string text, RSAParameters key)
//        {
//#if NET45
//            return await Task.Run(() =>
//            {
//                return Decrypt(text, key);
//            });
//#else
//            return await Task.Run(() =>
//            {
//                return Decrypt(text, key);
//            });
//#endif
//        }
        /// <summary>
        /// Asynchronously encrypts a string using the specified keysize and public key
        /// </summary>
        /// <param name="bytes">The data to encrypt</param>
        /// <param name="key">The key to encrypt the data</param>
        /// <returns>A byte array containing the encrypted data</returns>
        public static async Task<byte[]> EncryptAsync(byte[] bytes, RSAParameters key)
        {
#if NET45
            return await Task.Run(() =>
            {
                return Encrypt(bytes, key);
            });
#else
            return await Task.Run(() =>
            {
                return Encrypt(bytes, key);
            });
#endif
        }
        /// <summary>
        /// Asynchronously decrypts a string which was previously encrypted with the Encrypt method
        /// </summary>
        /// <param name="bytes">The encrypted byte array</param>
        /// <param name="key">The key to decrypt the data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public static async Task<byte[]> DecryptAsync(byte[] bytes, RSAParameters key)
        {
#if NET45
            return await Task.Run(() =>
            {
                return Decrypt(bytes, key);
            });
#else
            return await Task.Run(() =>
            {
                return Decrypt(bytes, key);
            });
#endif
        }
    }
    internal static class Logging
    {
        public static System.Net.IPEndPoint LOCALUDPLOGCONSOLE = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 2114);
        public static async void Log(string message)
        {
            try
            {
                //using (var client = new System.Net.Sockets.UdpClient("127.0.0.1", 2114))
                using (var client = new System.Net.Sockets.UdpClient())
                {
                    var bytes = message.AsBytes();
                    await client.SendAsync(bytes, bytes.Length, LOCALUDPLOGCONSOLE);
                }
            }
            finally { }

        }
    }
}
