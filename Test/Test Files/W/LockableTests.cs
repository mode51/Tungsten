using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class LockableTests
    {
        //private ITestOutputHelper output;
        //public LockableTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}
        [TestMethod]
        public void Lockable_InitializeAndRead()
        {
            var value = new Lockable<int>(5);
            Assert.IsTrue(value.Value == 5);
        }
        [TestMethod]
        public void Lockable_InitializeAndModify()
        {
            var value = new Lockable<int>(3);
            value.Value = 5;
            Assert.IsTrue(value.Value == 5);
        }
        [TestMethod]
        public void Lockable_InitializeAndReadMany()
        {
            var value = new Lockable<int>(5);
            for(int t=0; t<100000; t++)
                Assert.IsTrue(value.Value == 5);
        }
        [TestMethod]
        public void Lockable_InitializeAndModifyMany()
        {
            var value = new Lockable<int>(3);
            for (int t = 0; t < 100000; t++)
            {
                value.Value = t;
                Assert.IsTrue(value.Value == t);
            }
        }
    }
}
