using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class GateTests
    {
        //private ITestOutputHelper output;
        //public GateTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}
        [TestMethod]
        public void Gate_CreateAndDispose()
        {
            using (var gate = new W.Threading.Gate(ct => { }))
            {
                Assert.IsTrue(true);
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void Gate_RunOnce()
        {
            var t = 0;
            using (var gate = new W.Threading.Gate(token => { t += 1; }))
            {
                gate.Run();
                var complete = gate.Join(1000);
                Assert.IsTrue(complete);
                Assert.IsTrue(t == 1);
            }
        }
        [TestMethod]
        public void Gate_RunTenTimes()
        {
            var t = 0;
            using (var gate = new W.Threading.Gate(token => { t += 1; /*W.Threading.Thread.Sleep(500);*/ }))
            {
                for (int i = 0; i < 10; i++)
                {
                    gate.Run();
                    var complete = gate.Join(100);
                    Assert.IsTrue(complete);
                    Console.WriteLine("t = {0}", t);
                    Assert.IsTrue(gate.IsComplete);
                }
                Assert.IsTrue(t == 10);
            }
        }
    }
}
