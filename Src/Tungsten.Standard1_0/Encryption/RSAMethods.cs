#if NET45
using System;
using System.Collections.Generic;
using System.Text;
using W.Logging;

namespace W.Encryption
{
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
}
#endif
