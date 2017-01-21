using System;
using System.Security.Cryptography;
using System.Text;
using W.Logging;

namespace W.RPC
{
    internal class RSA
    {
        //adapted from an online sample (http://digitalsquid.co.uk/2009/01/rsa-crypto/)
        public static string Encrypt(string text, string key, int keySize = 2048)
        {
            return Encrypt(Encoding.UTF32.GetBytes(text), key, keySize);
        }
        public static string Encrypt(byte[] byteData, string key, int keySize = 2048)
        {
            var rsa = new RSACryptoServiceProvider(keySize);
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
        //adapted from an online sample (http://digitalsquid.co.uk/2009/01/rsa-crypto/)
        public static string Decrypt(string text, string key, int keySize = 2048)
        {
            string result = null;
            try
            {
                var rsa = new RSACryptoServiceProvider(keySize);
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
                result = Encoding.UTF32.GetString(fullbytes);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return result;
        }
        public static string Decrypt(string text, System.Security.SecureString key, int keySize = 2048)
        {
            string result = null;
            try
            {
                var rsa = new RSACryptoServiceProvider(keySize);
                rsa.FromXmlString(SecureStringMethods.ConvertToUnsecureString(key));
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
                result = Encoding.UTF32.GetString(fullbytes);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return result;
        }
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

        public class RSAPublicPrivateKeyPair
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }

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