using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            const string value = "Jordan                                                                                                            Duerksen";
            var keys = W.Encryption.RSAMethods.CreateKeyPair(2048);
            {
                try
                {
                    var encrypted = W.Encryption.RSAMethods.Encrypt(value, keys.PublicKey);
                    Console.WriteLine("Encrypted = {0}", encrypted);
                    var decrypted = W.Encryption.RSAMethods.Decrypt(encrypted, keys.PrivateKey);
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
