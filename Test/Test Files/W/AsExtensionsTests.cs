using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using W;

namespace W.Tests
{
    [TestClass]
    public class AsExtensionsTests
    {
        //private ITestOutputHelper output;
        //public AsExtensionsTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}

        [TestMethod]
        public void AsExtensions_CompressDecompress()
        {
            const string value = "Jordan                                                                                                            Duerksen";
            var compressed = value.AsBytes().AsCompressed();
            Console.WriteLine("Compressed Size = {0}", compressed.Length);
            var decompressed = compressed.FromCompressed();
            Console.WriteLine("Decompressed Size = {0}", decompressed.Length);
            Assert.IsTrue(decompressed.AsString() == value);
        }
        //[TestMethod]
        //public void CompressDecompressString()
        //{
        //    const string value = "Jordan                                                                                                            Duerksen";
        //    var compressed = value.AsCompressed();
        //    Console.WriteLine("Compressed Size = {0}", compressed.Length);
        //    var decompressed = compressed.AsDecompressed();
        //    Console.WriteLine("Decompressed Size = {0}", decompressed.Length);
        //    Assert.IsTrue(decompressed == value);
        //}
    }
}
