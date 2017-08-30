using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using W.Logging;

namespace W.Encryption
{
#if NET45
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
        public static string Encrypt(string text, System.Security.Cryptography.RSAParameters key, int keySize = 2048)
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
            using (var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(keySize))
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
        public static string Encrypt(byte[] byteData, System.Security.Cryptography.RSAParameters key, int keySize = 2048)
        {
            using (var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(keySize))
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
                using (var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(keySize))
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
        public static string Decrypt(string text, System.Security.Cryptography.RSAParameters key, int keySize = 2048)
        {
            string result = null;
            try
            {
                using (var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(keySize))
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
#elif NETSTANDARD1_3 || NETCOREAPP1_0
    using System.Security.Cryptography;

#pragma warning disable CS1584
#pragma warning disable CS1658
    /// Warning is overriding an error
    /// XML comment has syntactically incorrect cref attribute
    /// <summary><para>
    /// Replaces RSA.  This code was adaptd for NetStandard from an article published on CodeProject by Mathew John Schlabaugh in 2007.  It is less complicated but works more often than my initial RSA implementation. See: https://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET
    /// </para></summary>
    /// <see cref="https://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET"/>
    public class RSAMethods
#pragma warning restore CS1658 // Warning is overriding an error
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute
    {
        /// <summary>
        /// Contains a public/private key pair
        /// </summary>
        public class PublicKeyPrivateKeyPair
        {
            /// <summary>
            /// The private key used to decrypt
            /// </summary>
            public RSAParameters PrivateKey { get; set; }
            /// <summary>
            /// The public key used to encrypt
            /// </summary>
            public RSAParameters PublicKey { get; set; }
        }
        /// <summary>
        /// Returns the arrary of legal key sizes
        /// </summary>
        public static KeySizes[] LegalKeySizes()
        {
            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                return rsa.LegalKeySizes;
            }
        }
        /// <summary>
        /// Generates a public/private key pair
        /// </summary>
        /// <param name="keySize"></param>
        /// <returns></returns>
        public static PublicKeyPrivateKeyPair CreateKeyPair(int keySize)
        {
            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                rsa.KeySize = keySize;
                return new PublicKeyPrivateKeyPair() { PrivateKey = rsa.ExportParameters(true), PublicKey = rsa.ExportParameters(false) };
            }
        }
        /// <summary>
        /// Encrypts a string using the specified keysize and public key
        /// </summary>
        /// <param name="inputString">The data to encrypt</param>
        /// <param name="dwKeySize">The keysize to use when encrypting</param>
        /// <param name="publicKey">The public key used to encrypt the data</param>
        /// <returns>A string containing the encrypted data</returns>
        public static string Encrypt(string inputString, int dwKeySize, RSAParameters publicKey)
        {
            var rsa = System.Security.Cryptography.RSA.Create();
            rsa.KeySize = dwKeySize;
            rsa.ImportParameters(publicKey);

            int keySize = dwKeySize / 8;
            byte[] bytes = Encoding.UTF32.GetBytes(inputString);
            // The hash function in use by the .NET RSACryptoServiceProvider here 
            // is SHA1
            // int maxLength = ( keySize ) - 2 - 
            //              ( 2 * SHA1.Create().ComputeHash( rawBytes ).Length );
            int maxLength = keySize - 42;
            int dataLength = bytes.Length;
            int iterations = dataLength / maxLength;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= iterations; i++)
            {
                byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                byte[] encryptedBytes = rsa.Encrypt(tempBytes, RSAEncryptionPadding.Pkcs1); //8.29.2017 - seems we can only use Pkcs1, no idea why
                                                                                            // Be aware the RSACryptoServiceProvider reverses the order of 
                                                                                            // encrypted bytes. It does this after encryption and before 
                                                                                            // decryption. If you do not require compatibility with Microsoft 
                                                                                            // Cryptographic API (CAPI) and/or other vendors. Comment out the 
                                                                                            // next line and the corresponding one in the DecryptString function.
                Array.Reverse(encryptedBytes);
                // Why convert to base 64?
                // Because it is the largest power-of-two base printable using only 
                // ASCII characters
                stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Asynchronously encrypts a string using the specified keysize and public key
        /// </summary>
        /// <param name="inputString">The data to encrypt</param>
        /// <param name="dwKeySize">The keysize to use when encrypting</param>
        /// <param name="publicKey">The public key used to encrypt the data</param>
        /// <returns>A string containing the encrypted data</returns>
        public static async Task<string> EncryptAsync(string inputString, int dwKeySize, RSAParameters publicKey)
        {
            return await Task.Run(() =>
            {
                return Encrypt(inputString, dwKeySize, publicKey);
            });
        }

        /// <summary>
        /// Decrypts a string which was previously encrypted with the Encrypt method
        /// </summary>
        /// <param name="cipher">The encrypted byte array</param>
        /// <param name="dwKeySize">The keysize to use when decrypting</param>
        /// <param name="privateKey">The private key used to decrypt the data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public static string Decrypt(string cipher, int dwKeySize, RSAParameters privateKey)
        {
            var rsa = System.Security.Cryptography.RSA.Create();
            rsa.KeySize = dwKeySize;
            rsa.ImportParameters(privateKey);
            int base64BlockSize = ((dwKeySize / 8) % 3 != 0) ? (((dwKeySize / 8) / 3) * 4) + 4 : ((dwKeySize / 8) / 3) * 4;
            int iterations = cipher.Length / base64BlockSize;
            //var arrayList = new ArrayList();
            var fullBytes = new byte[0];
            for (int i = 0; i < iterations; i++)
            {
                byte[] encryptedBytes = Convert.FromBase64String(cipher.Substring(base64BlockSize * i, base64BlockSize));
                // Be aware the RSACryptoServiceProvider reverses the order of 
                // encrypted bytes after encryption and before decryption.
                // If you do not require compatibility with Microsoft Cryptographic 
                // API (CAPI) and/or other vendors.
                // Comment out the next line and the corresponding one in the 
                // EncryptString function.
                Array.Reverse(encryptedBytes);
                var bytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1); //8.29.2017 - seems we can only use Pkcs1, no idea why
                var len = fullBytes.Length;
                Array.Resize(ref fullBytes, len + bytes.Length);
                Array.Copy(bytes, 0, fullBytes, len, bytes.Length);
                //arrayList.AddRange(bytes);
            }
            return Encoding.UTF32.GetString(fullBytes);
            //return Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
        }
        /// <summary>
        /// Asynchronously decrypts a string which was previously encrypted with the Encrypt method
        /// </summary>
        /// <param name="cipher">The encrypted byte array</param>
        /// <param name="dwKeySize">The keysize to use when decrypting</param>
        /// <param name="privateKey">The private key used to decrypt the data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public static async Task<string> DecryptAsync(string cipher, int dwKeySize, RSAParameters privateKey)
        {
            return await Task.Run(() =>
            {
                return Decrypt(cipher, dwKeySize, privateKey);
            });
        }
    }
#endif
}
