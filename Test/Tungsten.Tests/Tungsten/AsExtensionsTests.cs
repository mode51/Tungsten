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
    internal class AsExtensionsTests
    {
        [Test]
        public void AsCompressedDecompressed()
        {
            const string value = "Jordan                                                                                                            Duerksen";
            var compressed = value.AsCompressed();
            Console.WriteLine("Compressed Size = {0}", compressed.Length);
            var decompressed = compressed.AsDecompressed();
            Console.WriteLine("Decompressed Size = {0}", decompressed.Length);
            Assert.IsTrue(decompressed == value);
        }
    }
}
