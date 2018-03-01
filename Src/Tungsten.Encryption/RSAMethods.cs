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
    public static class RSAMethods
    {
#pragma warning restore CS1658 // Warning is overriding an error
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute

        /// <summary>
        /// Returns an arrary containing the supported key sizes
        /// </summary>
        /// <returns>An array of supported key sizes</returns>
        public static KeySizes[] LegalKeySizes()
        {
#if NET45
            using (var rsa = RSACryptoServiceProvider.Create())
            {
                return rsa.LegalKeySizes;
            }
#else
            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                return rsa.LegalKeySizes;
            }
#endif
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
#if NET45
            using (var rsa = System.Security.Cryptography.RSACryptoServiceProvider.Create())
            {
                //rsa.KeySize = keySize;
                privateKey = rsa.ExportParameters(true);
                publicKey = rsa.ExportParameters(false);
            }
#else
            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                rsa.KeySize = keySize;
                privateKey = rsa.ExportParameters(true);
                publicKey = rsa.ExportParameters(false);
            }
#endif
        }

        /// <summary>
        /// Encrypts a string using the specified keysize and public key
        /// </summary>
        /// <param name="text">The data to encrypt</param>
        /// <param name="publicKey">The public key used to encrypt the data</param>
        /// <returns>A string containing the encrypted data</returns>
        public static string Encrypt(string text, RSAParameters publicKey)
        {
#if NET45
            StringBuilder stringBuilder = new StringBuilder();
            int maxLength = 0;
            int dataLength = 0;
            int iterations = 0;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                var bytes = text.AsBytes();
                maxLength = ((rsa.KeySize - 384) / 8) + 37; //+37 for PKCS padding, +7 for OAEP (see: https://stackoverflow.com/a/3253142)
                dataLength = bytes.Length;
                iterations = dataLength / maxLength;
                for (int i = 0; i <= iterations; i++)
                {
                    var multiple = i * maxLength;
                    byte[] tempBytes = new byte[(dataLength - multiple > maxLength) ? maxLength : dataLength - multiple];
                    Buffer.BlockCopy(bytes, multiple, tempBytes, 0, tempBytes.Length);
                    byte[] encryptedBytes = rsa.Encrypt(tempBytes, false);

                    //Array.Reverse(encryptedBytes); //only if to be decrypted by non-Microsoft api's
                    stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
                }
                return stringBuilder.ToString();
            }
#else
            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                rsa.ImportParameters(publicKey);

                byte[] bytes = text.AsBytes();
                int maxLength = ((rsa.KeySize / 8) % 3 != 0) ? (((rsa.KeySize / 8) / 3) * 4) + 4 : ((rsa.KeySize / 8) / 3) * 4;
                int dataLength = bytes.Length;
                int iterations = dataLength / maxLength;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i <= iterations; i++)
                {
                    byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                    Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                    byte[] encryptedBytes = rsa.Encrypt(tempBytes, RSAEncryptionPadding.Pkcs1); //8.29.2017 - seems we can only use Pkcs1, probably because it's multi-platform
                    // Be aware the RSACryptoServiceProvider reverses the order of 
                    // encrypted bytes. It does this after encryption and before 
                    // decryption. If you do not require compatibility with Microsoft 
                    // Cryptographic API (CAPI) and/or other vendors. Comment out the 
                    // next line and the corresponding one in the DecryptString function.
                    //Array.Reverse(encryptedBytes);
                    // Why convert to base 64?
                    // Because it is the largest power-of-two base printable using only 
                    // ASCII characters
                    stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
                }
                return stringBuilder.ToString();
            }
#endif
        }
        /// <summary>
        /// Decrypts a string which was previously encrypted with the Encrypt method
        /// </summary>
        /// <param name="text">The encrypted byte array</param>
        /// <param name="privateKey">The private key used to decrypt the data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public static string Decrypt(string text, RSAParameters privateKey)
        {
#if NET45
            using (var rsa = new System.Security.Cryptography.RSACryptoServiceProvider())
            {
                //ImportParameters sets the keySize according to the size used to create the privateKey
                rsa.ImportParameters(privateKey);
                int maxLength = ((rsa.KeySize / 8) % 3 != 0) ? (((rsa.KeySize / 8) / 3) * 4) + 4 : ((rsa.KeySize / 8) / 3) * 4;
                int iterations = text.Length / maxLength;
                var fullBytes = new byte[0];
                for (int i = 0; i < iterations; i++)
                {
                    byte[] encryptedBytes = Convert.FromBase64String(text.Substring(maxLength * i, maxLength));
                    //Array.Reverse(encryptedBytes); //only if not encrypted by Microsoft api's
                    var bytes = rsa.Decrypt(encryptedBytes, false);
                    var len = fullBytes.Length;
                    Array.Resize(ref fullBytes, len + bytes.Length);
                    Array.Copy(bytes, 0, fullBytes, len, bytes.Length);
                }
                return fullBytes.AsString();
            }
#else
            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                int base64BlockSize = ((rsa.KeySize / 8) % 3 != 0) ? (((rsa.KeySize / 8) / 3) * 4) + 4 : ((rsa.KeySize / 8) / 3) * 4;
                int iterations = text.Length / base64BlockSize;
                var fullBytes = new byte[0];
                for (int i = 0; i < iterations; i++)
                {
                    byte[] encryptedBytes = Convert.FromBase64String(text.Substring(base64BlockSize * i, base64BlockSize));
                    // Be aware the RSACryptoServiceProvider reverses the order of 
                    // encrypted bytes after encryption and before decryption.
                    // If you do not require compatibility with Microsoft Cryptographic 
                    // API (CAPI) and/or other vendors.
                    // Comment out the next line and the corresponding one in the 
                    // EncryptString function.
                    //Array.Reverse(encryptedBytes);
                    var bytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1); //8.29.2017 - seems we can only use Pkcs1, probably because it's multi-platform
                    var len = fullBytes.Length;
                    Array.Resize(ref fullBytes, len + bytes.Length);
                    Array.Copy(bytes, 0, fullBytes, len, bytes.Length);
                    //arrayList.AddRange(bytes);
                }
                return fullBytes.AsString();
            }
#endif
        }

        /// <summary>
        /// Asynchronously encrypts a string using the specified keysize and public key
        /// </summary>
        /// <param name="text">The data to encrypt</param>
        /// <param name="publicKey">The public key used to encrypt the data</param>
        /// <returns>A string containing the encrypted data</returns>
        public static async Task<string> EncryptAsync(string text, RSAParameters publicKey)
        {
#if NET45
            return await Task.Run(() =>
            {
                return Encrypt(text, publicKey);
            });
#else
            return await Task.Run(() =>
            {
                return Encrypt(text, publicKey);
            });
#endif
        }
        /// <summary>
        /// Asynchronously decrypts a string which was previously encrypted with the Encrypt method
        /// </summary>
        /// <param name="text">The encrypted byte array</param>
        /// <param name="privateKey">The private key used to decrypt the data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public static async Task<string> DecryptAsync(string text, RSAParameters privateKey)
        {
#if NET45
            return await Task.Run(() =>
            {
                return Decrypt(text, privateKey);
            });
#else
            return await Task.Run(() =>
            {
                return Decrypt(text, privateKey);
            });
#endif
        }
    }
}
