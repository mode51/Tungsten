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
    public class CallResultTests
    {
        //private ITestOutputHelper output;
        //public CallResultTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}
        [TestMethod]
        public void CallResult_DefaultConstructorTest()
        {
            var r = new CallResult();
            Assert.IsFalse(r.Success);
            Assert.IsNull(r.Exception);
        }
        [TestMethod]
        public void CallResult_Constructor_Success_Test()
        {
            var r = new CallResult(true);
            Assert.IsTrue(r.Success);
            Assert.IsNull(r.Exception);
        }
        [TestMethod]
        public void CallResult_Constructor_Failure_With_Exception_Test()
        {
            var r = new CallResult(false, new ArgumentNullException());
            Assert.IsFalse(r.Success);
            Assert.IsTrue(r.Exception != null);
            Assert.IsTrue(r.Exception is ArgumentNullException);
        }

        [TestMethod]
        public void CallResult_Generic_DefaultConstructorTest()
        {
            var r = new CallResult<int>();
            Assert.IsFalse(r.Success);
            Assert.IsNull(r.Exception);
        }
        [TestMethod]
        public void CallResult_Generic_Constructor_Success_Test()
        {
            var r = new CallResult<int>(true);
            Assert.IsTrue(r.Success);
            Assert.IsNull(r.Exception);
        }
        [TestMethod]
        public void CallResult_Generic_Constructor_Success_And_Result_Test()
        {
            var r = new CallResult<int>(true, 22);
            Assert.IsTrue(r.Success);
            Assert.IsTrue(r.Result == 22);
            Assert.IsNull(r.Exception);
        }
        [TestMethod]
        public void CallResult_Generic_Constructor_Failure_With_Exception_Test()
        {
            var r = new CallResult<int>(false, 22, new ArgumentNullException());
            Assert.IsFalse(r.Success);
            Assert.IsTrue(r.Result == 22);
            Assert.IsTrue(r.Exception != null);
            Assert.IsTrue(r.Exception is ArgumentNullException);
        }
    }
}
