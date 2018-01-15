using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using W.Logging;

#if NET45 || NETSTANDARD1_3 || NETCOREAPP1_0
namespace W.Encryption
{
    using System.Security.Cryptography;

#pragma warning disable CS1584
#pragma warning disable CS1658
    /// Warning is overriding an error
    /// XML comment has syntactically incorrect cref attribute
    /// <summary><para>
    /// Replaces RSA.  This code was adaptd for NetStandard from an article published on CodeProject by Mathew John Schlabaugh in 2007.  It is less complicated but works more often than my initial RSA implementation. See: https://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET
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
            using (var rsa = new RSACryptoServiceProvider())
            {
                return rsa.LegalKeySizes;
            }
#elif NETSTANDARD1_3 || NETCOREAPP1_0
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
        /// <returns>A newly created PublicPrivateKeyPair containing the public and private keys</returns>
        public static PublicPrivateKeyPair CreateKeyPair(int keySize)
        {
#if NET45
            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                return new PublicPrivateKeyPair() { PrivateKey = rsa.ExportParameters(true), PublicKey = rsa.ExportParameters(false) };
            }
#elif NETSTANDARD1_3 || NETCOREAPP1_0
            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                rsa.KeySize = keySize;
                return new PublicPrivateKeyPair() { PrivateKey = rsa.ExportParameters(true), PublicKey = rsa.ExportParameters(false) };
            }
#endif
        }
#if NET45
        /// <summary>
        /// Encrypts a string using the specified keysize and public key
        /// </summary>
        /// <param name="inputString">The data to encrypt</param>
        /// <param name="publicKey">The public key used to encrypt the data</param>
        /// <returns>A string containing the encrypted data</returns>
        public static string Encrypt(string inputString, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                //ImportParameters sets the keySize according to the size used to create the publicKey
                rsa.ImportParameters(publicKey);

                int keySize = rsa.KeySize / 8;
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
                    byte[] encryptedBytes = rsa.Encrypt(tempBytes, true);

                    //8.29.2017 - seems we can only use Pkcs1, no idea why
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
        }
        /// <summary>
        /// Decrypts a string which was previously encrypted with the Encrypt method
        /// </summary>
        /// <param name="cipher">The encrypted byte array</param>
        /// <param name="privateKey">The private key used to decrypt the data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public static string Decrypt(string cipher, RSAParameters privateKey)
        {
            using (var rsa = new System.Security.Cryptography.RSACryptoServiceProvider())
            {
                //ImportParameters sets the keySize according to the size used to create the privateKey
                rsa.ImportParameters(privateKey);
                int base64BlockSize = ((rsa.KeySize / 8) % 3 != 0) ? (((rsa.KeySize / 8) / 3) * 4) + 4 : ((rsa.KeySize / 8) / 3) * 4;
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
                    var bytes = rsa.Decrypt(encryptedBytes, true);
                    var len = fullBytes.Length;
                    Array.Resize(ref fullBytes, len + bytes.Length);
                    Array.Copy(bytes, 0, fullBytes, len, bytes.Length);
                    //arrayList.AddRange(bytes);
                }
                return Encoding.UTF32.GetString(fullBytes);
                //return Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
            }
        }
#elif NETSTANDARD1_3 || NETCOREAPP1_0
        /// <summary>
        /// Encrypts a string using the specified keysize and public key
        /// </summary>
        /// <param name="inputString">The data to encrypt</param>
        /// <param name="publicKey">The public key used to encrypt the data</param>
        /// <returns>A string containing the encrypted data</returns>
        public static string Encrypt(string inputString, RSAParameters publicKey)
        {
            var rsa = System.Security.Cryptography.RSA.Create();
            //rsa.KeySize = dwKeySize;
            rsa.ImportParameters(publicKey);

            int keySize = rsa.KeySize / 8;
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
                byte[] encryptedBytes = rsa.Encrypt(tempBytes, RSAEncryptionPadding.Pkcs1); //8.29.2017 - seems we can only use Pkcs1, probably because it's multi-platform
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
        /// Decrypts a string which was previously encrypted with the Encrypt method
        /// </summary>
        /// <param name="cipher">The encrypted byte array</param>
        /// <param name="privateKey">The private key used to decrypt the data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public static string Decrypt(string cipher, RSAParameters privateKey)
        {
            var rsa = System.Security.Cryptography.RSA.Create();
            //rsa.KeySize = dwKeySize;
            rsa.ImportParameters(privateKey);
            int base64BlockSize = ((rsa.KeySize / 8) % 3 != 0) ? (((rsa.KeySize / 8) / 3) * 4) + 4 : ((rsa.KeySize / 8) / 3) * 4;
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
                var bytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1); //8.29.2017 - seems we can only use Pkcs1, probably because it's multi-platform
                var len = fullBytes.Length;
                Array.Resize(ref fullBytes, len + bytes.Length);
                Array.Copy(bytes, 0, fullBytes, len, bytes.Length);
                //arrayList.AddRange(bytes);
            }
            return Encoding.UTF32.GetString(fullBytes);
            //return Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
        }
#endif
        /// <summary>
        /// Asynchronously encrypts a string using the specified keysize and public key
        /// </summary>
        /// <param name="inputString">The data to encrypt</param>
        /// <param name="publicKey">The public key used to encrypt the data</param>
        /// <returns>A string containing the encrypted data</returns>
        public static async Task<string> EncryptAsync(string inputString, RSAParameters publicKey)
        {
            return await Task.Run(() =>
            {
                return Encrypt(inputString, publicKey);
            });
        }
        /// <summary>
        /// Asynchronously decrypts a string which was previously encrypted with the Encrypt method
        /// </summary>
        /// <param name="cipher">The encrypted byte array</param>
        /// <param name="privateKey">The private key used to decrypt the data</param>
        /// <returns>A byte array containing the decrypted value</returns>
        public static async Task<string> DecryptAsync(string cipher, RSAParameters privateKey)
        {
            return await Task.Run(() =>
            {
                return Decrypt(cipher, privateKey);
            });
        }
    }
}
#endif
