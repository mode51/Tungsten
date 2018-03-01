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
        //private ITestOutputHelper output;
        //public RSATests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}
        [TestMethod]
        public void RSA_EncryptAndDecrypt()
        {
            const int keySize = 2048;
            const string value = "Jordan                                                                                                            Duerksen";
            
            W.Encryption.RSAMethods.CreateKeyPair(keySize, out RSAParameters privateKey, out RSAParameters publicKey);
            {
                try
                {
                    var encrypted = W.Encryption.RSAMethods.Encrypt(value, publicKey);
                    Console.WriteLine("Encrypted = {0}", encrypted);
                    var decrypted = W.Encryption.RSAMethods.Decrypt(encrypted, privateKey);
                    Assert.IsTrue(decrypted == value);
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
