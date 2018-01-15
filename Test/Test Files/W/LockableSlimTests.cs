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
    public class LockableSlimTests
    {
        //private ITestOutputHelper output;
        //public LockableSlimTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}
        [TestMethod]
        public void LockableSlim_InitializeAndRead()
        {
            var value = new LockableSlim<int>(5);
            Assert.IsTrue(value.Value == 5);
        }
        [TestMethod]
        public void LockableSlim_InitializeAndModify()
        {
            var value = new LockableSlim<int>(3);
            value.Value = 5;
            Assert.IsTrue(value.Value == 5);
        }
        [TestMethod]
        public void LockableSlim_InitializeAndReadMany()
        {
            var value = new LockableSlim<int>(5);
            for (int t = 0; t < 100000; t++)
                Assert.IsTrue(value.Value == 5);
        }
        [TestMethod]
        public void LockableSlim_InitializeAndModifyMany()
        {
            var value = new LockableSlim<int>(3);
            for (int t = 0; t < 100000; t++)
            {
                value.Value = t;
                Assert.IsTrue(value.Value == t);
            }
        }
    }
}
