using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using W;

namespace W.Tests
{
    [TestClass]
    public class ArrayMethodsTests
    {
        private byte[] complete = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        [TestMethod]
        public void PeekStart()
        {
            var result = ArrayMethods.Peek(complete, 0, 5);
            for(int t=0; t<result.Length; t++)
                Assert.IsTrue(result[t] == complete[t]);
        }
        [TestMethod]
        public void PeekStart_Nothing()
        {
            var result = ArrayMethods.Peek(complete, 0, 0);
            Assert.IsTrue(result.Length == 0);
            for (int t = 0; t < result.Length; t++)
                Assert.IsTrue(result[t] == complete[t]);
        }
        [TestMethod]
        public void Peek()
        {
            var result = ArrayMethods.Peek(complete, 2, 8);
            Assert.IsTrue(result.Length == 8);
            for (int t = 0; t < result.Length; t++)
                Assert.IsTrue(result[t] == complete[t + 2]);
        }
        [TestMethod]
        public void Peek_Nothing()
        {
            var result = ArrayMethods.Peek(complete, 2, 0);
            Assert.IsTrue(result.Length == 0);
        }
        [TestMethod]
        public void PeekEnd()
        {
            var result = ArrayMethods.PeekEnd(complete, 5);
            Assert.IsTrue(result.Length == 5);
            for (int t = 0; t < result.Length; t++)
                Assert.IsTrue(result[t] == complete[t + 5]);
        }
        [TestMethod]
        public void PeekEnd_Nothing()
        {
            var result = ArrayMethods.PeekEnd(complete, 0);
            Assert.IsTrue(result.Length == 0);
        }

        [TestMethod]
        public void TakeFromStart()
        {
            var data = (byte[])complete.Clone();
            var result = ArrayMethods.TakeFromStart(ref data, 5);
            Assert.AreNotEqual(data, result);
            Assert.IsTrue(data.Length == 5);
            Assert.IsTrue(result.Length == 5);
            for (int t = 0; t < data.Length; t++)
                Assert.IsTrue(data[t] == complete[t+5]);
            for (int t = 0; t < result.Length; t++)
                Assert.IsTrue(result[t] == complete[t]);
        }
        [TestMethod]
        public void Take()
        {
            var data = (byte[])complete.Clone();
            var result = ArrayMethods.Take(ref data, 2, 6);
            Assert.AreNotEqual(data, result);
            Assert.IsTrue(data.Length == 4);
            Assert.IsTrue(result.Length == 6);
            Assert.IsTrue(data[0] == complete[0]);
            Assert.IsTrue(data[1] == complete[1]);
            Assert.IsTrue(data[2] == complete[8]);
            Assert.IsTrue(data[3] == complete[9]);
            for (int t = 0; t < result.Length; t++)
                Assert.IsTrue(result[t] == complete[t+2]);
        }
        [TestMethod]
        public void TakeFromEnd()
        {
            var data = (byte[])complete.Clone();
            var result = ArrayMethods.TakeFromEnd(ref data, 5);
            Assert.AreNotEqual(data, result);
            Assert.IsTrue(data.Length == 5);
            Assert.IsTrue(result.Length == 5);
            for (int t = 0; t < data.Length; t++)
                Assert.IsTrue(data[t] == complete[t]);
            for (int t = 0; t < result.Length; t++)
                Assert.IsTrue(result[t] == complete[t+5]);
        }

        [TestMethod]
        public void TrimStart()
        {
            var data = (byte[])complete.Clone();
            var result = ArrayMethods.TrimStart(ref data, 5);
            Assert.AreEqual(data, result);
            Assert.IsTrue(data.Length == 5);
            Assert.IsTrue(result.Length == 5);
            for (int t = 0; t < data.Length; t++)
                Assert.IsTrue(data[t] == complete[t + 5]);
            for (int t = 0; t < result.Length; t++)
                Assert.IsTrue(result[t] == complete[t + 5]);
        }
        [TestMethod]
        public void Trim()
        {
            var data = (byte[])complete.Clone();
            var result = ArrayMethods.Trim(ref data, 2, 6);
            Assert.AreEqual(data, result);
            Assert.IsTrue(data.Length == 4);
            Assert.IsTrue(result.Length == 4);
            Assert.IsTrue(data[0] == complete[0]);
            Assert.IsTrue(data[1] == complete[1]);
            Assert.IsTrue(data[2] == complete[8]);
            Assert.IsTrue(data[3] == complete[9]);
            Assert.IsTrue(result[0] == complete[0]);
            Assert.IsTrue(result[1] == complete[1]);
            Assert.IsTrue(result[2] == complete[8]);
            Assert.IsTrue(result[3] == complete[9]);
        }
        [TestMethod]
        public void TrimEnd()
        {
            var data = (byte[])complete.Clone();
            var result = ArrayMethods.TrimEnd(ref data, 5);
            Assert.AreEqual(data, result);
            Assert.IsTrue(data.Length == 5);
            Assert.IsTrue(result.Length == 5);
            for (int t = 0; t < data.Length; t++)
                Assert.IsTrue(data[t] == complete[t]);
            for (int t = 0; t < result.Length; t++)
                Assert.IsTrue(result[t] == complete[t]);
        }

        [TestMethod]
        public void Append()
        {
            var data = new byte[] { 0, 1, 2, 3, 4 };
            var result = W.ArrayMethods.Append(ref data, new byte[] { 5, 6, 7, 8, 9 });
            Assert.AreEqual(data, result);
            Assert.IsTrue(data.Length == 10);
            Assert.IsTrue(result.Length == 10);
            Assert.AreEqual(complete.Length, data.Length);
            for (int t = 0; t < complete.Length; t++)
                Assert.IsTrue(complete[t] == data[t]);
        }
        [TestMethod]
        public void Append_Nothing()
        {
            var data = new byte[] { 0, 1, 2, 3, 4 };
            var result = W.ArrayMethods.Append(ref data, new byte[0]);
            Assert.AreEqual(data, result);
            Assert.IsTrue(data.Length == 5);
            Assert.IsTrue(result.Length == 5);
            for (int t = 0; t < data.Length; t++)
                Assert.IsTrue(complete[t] == data[t]);
        }

        [TestMethod]
        public void Insert()
        {
            var data = new byte[] { 0, 1, 2, 3, 6, 7, 8, 9 };
            var result = ArrayMethods.Insert(ref data, new byte[] { 4, 5 }, 4);
            Assert.AreEqual(result, data);
            for (int t = 0; t < result.Length; t++)
                Assert.IsTrue(result[t] == complete[t]);
        }
        [TestMethod]
        public void Insert_Nothing()
        {
            var data = new byte[] { 0, 1, 2, 3, 6, 7, 8, 9 };
            var result = ArrayMethods.Insert(ref data, new byte[] {}, 4);
            Assert.AreEqual(result, data);
            for (int t = 0; t < 4; t++)
                Assert.IsTrue(result[t] == complete[t]);
            for (int t = 4; t < result.Length; t++)
                Assert.IsTrue(result[t] == complete[t+2]);
        }
    }
}