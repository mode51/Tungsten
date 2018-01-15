using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W;
using W.Threading.ThreadExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class ExtensionMethodsTests
    {
        //private ITestOutputHelper output;
        //public ExtensionMethodsTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}
        [TestMethod]
        public async Task ExtensionMethods_WaitForValueAsync_WithTimeout_Failure()
        {
            var mre = new System.Threading.ManualResetEventSlim(false);
            var result = false;
            var thread = W.Threading.Thread.Create(token => { mre.Wait(1000); result = true; });
            thread.Start();
            var r = System.Threading.SpinWait.SpinUntil(() => (result == true), 100);
            //var r = await result.WaitForValueAsync(value => value == true, 100); //don't wait long enough
            Assert.IsFalse(result);
            Assert.IsFalse(r);
            Assert.IsTrue(thread.Join(2000));
            Assert.IsTrue(result);
        }
        [TestMethod]
        public async Task ExtensionMethods_WaitForValueAsync_WithTimeout_Success()
        {
            var mre = new System.Threading.ManualResetEventSlim(false);
            var result = new Lockable<bool>();
            var thread = W.Threading.Thread.Create(token => { mre.Wait(500); result.Value = true; });
            thread.Start();
            System.Threading.SpinWait.SpinUntil(() => (result.Value == true), 1000);
            //await result.WaitForValueAsync(r => r.Value == true, 1000); //wait long enough
            Assert.IsTrue(result.Value);
            Assert.IsTrue(thread.Join(1000));
        }
        [TestMethod]
        public void ExtensionMethods_WaitForValue_WithTimeout_Failure()
        {
            var mre = new System.Threading.ManualResetEventSlim(false);
            var value = new Lockable<bool>();
            var thread = W.Threading.Thread.Create(token =>  
            {
                mre.Wait(1000);
                value.Value = true;
            });
            thread.Start();
            var result = System.Threading.SpinWait.SpinUntil(() => value.Value == true, 100);
            //var result = value.WaitForValue(r => r.Value == true, 100);
            Assert.IsFalse(result);
            Assert.IsTrue(thread.Join(1000));
            Assert.IsTrue(value.Value);
        }
        [TestMethod]
        public void ExtensionMethods_WaitForValue_WithTimeout_Success()
        {
            var mre = new System.Threading.ManualResetEventSlim(false);
            var result = new Lockable<bool>();
            var thread = W.Threading.Thread.Create(token => { mre.Wait(100); result.Value = true; });
            thread.Start();
            System.Threading.SpinWait.SpinUntil(() => result.Value == true, 200);
            //result.WaitForValue(r => r.Value == true, 500);
            Assert.IsTrue(result.Value);
            Assert.IsTrue(thread.Join(500));
        }
        [TestMethod]
        public async Task ExtensionMethods_WaitForValueAsync_Indefinitely()
        {
            var mre = new System.Threading.ManualResetEventSlim(false);
            var result = new Lockable<bool>();
            var thread = W.Threading.Thread.Create(token => { mre.Wait(500); result.Value = true; });
            thread.Start();
            System.Threading.SpinWait.SpinUntil(() => result.Value == true);
            //await result.WaitForValueAsync(r => r.Value == true);
            Assert.IsTrue(result.Value);
            Assert.IsTrue(thread.Join(1000));
        }
        [TestMethod]
        public void ExtensionMethods_WaitForValue_Indefinitely()
        {
            var mre = new System.Threading.ManualResetEventSlim(false);
            var result = new Lockable<bool>();
            var thread = W.Threading.Thread.Create(token => { mre.Wait(500); result.Value = true; });
            thread.Start();
            System.Threading.SpinWait.SpinUntil(() => result.Value == true);
            //result.WaitForValue(r => r.Value == true);
            Assert.IsTrue(result.Value);
            Assert.IsTrue(thread.Join(1000));
        }
    }
}
