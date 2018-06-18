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

        private void Test_AsBytes_AsString_Encoding(System.Text.Encoding encoding)
        {
            const string value = "Jordan Duerksen";
            Console.WriteLine($"Initial: {encoding} Size = {value.Length}, value={value}");
            var bytes = value.AsBytes(encoding);
            Console.WriteLine($"Encoded: {encoding} Size = {bytes.Length}, AsBase64={bytes.AsBase64()}");
            var decoded = bytes.AsString(encoding);
            Console.WriteLine($"Decoded: {encoding} Size = {decoded.Length}, decoded={decoded}");
            Assert.IsTrue(decoded == value);
        }
        [TestMethod]
        public void AsExtensions_UTF32()
        {
            foreach (var encoding in System.Text.Encoding.GetEncodings())
                Test_AsBytes_AsString_Encoding(encoding.GetEncoding());
        }

        private void Test_AsBase64_FromBase64_Encoding(System.Text.Encoding encoding)
        {
            Console.WriteLine($"Encoding: {encoding.GetType().Name}");
            const string value = "Jordan Duerksen";
            Console.WriteLine($"Intial: Size = {value.Length}, value={value}");
            var encoded = value.AsBase64(encoding);
            Console.WriteLine($"Encoded: Size = {encoded.Length}, AsBase64={encoded}");
            var decoded = encoded.FromBase64(encoding);
            Console.WriteLine($"Decoded: Size = {decoded.Length}, decoded={decoded}");
            Assert.IsTrue(decoded == value);
            Console.WriteLine();
        }
        [TestMethod]
        public void AsExtensions_Base64_AllEncodings()
        {
            foreach(var encoding in System.Text.Encoding.GetEncodings())
                Test_AsBase64_FromBase64_Encoding(encoding.GetEncoding());
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
