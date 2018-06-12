using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using W;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class RSATests
    {
        private static int keySize = 2048;
        private static string valueString = "Jordan                                                                                                            Duerksen";
        private static byte[] valueBytes = valueString.AsBytes();// "Jordan                                                                                                            Duerksen".AsBytes();

        [TestMethod]
        public void RSA_EncryptAndDecrypt()
        {
            W.Encryption.RSAMethods.CreateKeyPair(keySize, out RSAParameters privateKey, out RSAParameters publicKey);
            {
                byte[] encrypted;
                byte[] decrypted;
                try
                {
                    encrypted = W.Encryption.RSAMethods.Encrypt(valueBytes, publicKey);
                    Console.WriteLine("Encrypted = {0}", encrypted);
                    decrypted = W.Encryption.RSAMethods.Decrypt(encrypted, privateKey);
                    Assert.IsTrue(decrypted.AsString() == valueString);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    throw e;
                }
            }
        }
        [TestMethod]
        public void RSA_LegalKeySizes()
        {
            var keySizes = W.Encryption.RSAMethods.LegalKeySizes();
            foreach (var ks in keySizes)
                Console.WriteLine($"Legal Key Size - Min:{ks.MinSize}, Max:{ks.MaxSize}, SkipSize:{ks.SkipSize}");
            var mid = Math.Max(keySizes[0].MinSize, (keySizes[0].MaxSize / keySizes[0].SkipSize / 2) * keySizes[0].SkipSize);
            W.Encryption.RSAMethods.CreateKeyPair(mid, out RSAParameters privateKey, out RSAParameters publicKey);
            {
                byte[] encrypted;
                byte[] decrypted;
                try
                {
                    encrypted = W.Encryption.RSAMethods.Encrypt(valueBytes, publicKey);
                    Console.WriteLine("Encrypted = {0}", encrypted);
                    decrypted = W.Encryption.RSAMethods.Decrypt(encrypted, privateKey);
                    Assert.IsTrue(decrypted.AsString() == valueString);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    throw e;
                }
            }
        }
        [TestMethod]
        public void RSA_EncryptAndDecryptToBytes()
        {
            W.Encryption.RSAMethods.CreateKeyPair(keySize, out RSAParameters privateKey, out RSAParameters publicKey);
            {
                byte[] encrypted;
                byte[] decrypted;
                try
                {
                    encrypted = W.Encryption.RSAMethods.Encrypt(valueBytes, publicKey);
                    Console.WriteLine("Encrypted = {0}", encrypted);
                    decrypted = W.Encryption.RSAMethods.Decrypt(encrypted, privateKey);
                    Assert.IsTrue(decrypted.Length == valueBytes.Length);
                    for(int t=0; t<decrypted.Length; t++)
                        Assert.IsTrue(decrypted[t] == valueBytes[t]);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    throw e;
                }
            }
        }
    }
}
