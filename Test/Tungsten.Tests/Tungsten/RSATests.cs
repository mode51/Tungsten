using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using W;

using Assert = NUnit.Framework.Assert;


namespace W.Tests.Tungsten
{
    [TestFixture]
    internal class RSATests
    {
        [Test]
        public void EncryptAndDecrypt()
        {
            const string value = "Jordan                                                                                                            Duerksen";
            using (var rsa = new W.Encryption.RSA(2048))
            {
                var encrypted = rsa.Encrypt(value, rsa.PublicKey);
                Console.WriteLine("Encrypted = {0}", encrypted);
                var decrypted = rsa.Decrypt(encrypted, rsa.PrivateKey);
                Assert.IsTrue(decrypted == value);
            }
        }
    }
}
