using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W;
using W.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class ThreadMethodTests
    {
        //private ITestOutputHelper output;
        //public ThreadMethodTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}

        private class Customer
        {
            public string First { get; set; }
            public string Last { get; set; }
            public override string ToString()
            {
                return Last + ", " + First;
            }
        }
        private void Method1()
        {
            Console.WriteLine("Inside Method1");
            Console.WriteLine("Leaving Method1");
        }
        private void Method2(params object[] args)
        {
            Console.WriteLine("Inside Method2(" + string.Join(", ", args) + ")");
            Console.WriteLine("Leaving Method2");
            //for (int t = 0; t < args.Length; t++)
            //    Console.WriteLine("Parameter = " + args[t].ToString());
        }

        [TestMethod]
        public void ThreadMethod_Create()
        {
            var proc = ThreadMethod.Create(Method1);
            {

                Assert.IsTrue(proc != null);
                Assert.IsTrue(!proc.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadMethod_Create2()
        {
            var proc = ThreadMethod.Create(Method2);
            {
                Assert.IsTrue(proc != null);
                Assert.IsTrue(!proc.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadMethod_Run()
        {
            var proc = ThreadMethod.Create(Method2);
            {
                proc.RunSynchronously();
                Assert.IsTrue(proc.IsComplete);
            }
        }
        [TestMethod]
        public async Task ThreadMethod_RunAsync()
        {
            var proc = ThreadMethod.Create(Method2);
            {
                await proc.StartAsync();
                Assert.IsTrue(proc.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadMethod_RunWithParameters()
        {
            var proc = ThreadMethod.Create(Method2);
            {
                proc.RunSynchronously(24);
                Assert.IsTrue(proc.IsComplete);
            }
        }
        [TestMethod]
        public async Task ThreadMethod_RunAsyncWithParameters()
        {
            var proc = ThreadMethod.Create(Method2);
            {
                await proc.StartAsync(24);
                Assert.IsTrue(proc.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadMethod_RunWithManyParameters()
        {
            var proc = ThreadMethod.Create(Method2);
            {
                proc.RunSynchronously("Jordan", 24, "Duerksen", 0.5, new Customer() { First = "Jordan", Last = "Duerksen" });
                Assert.IsTrue(proc.IsComplete);
            }
        }
        [TestMethod]
        public async Task ThreadMethod_RunAsyncWithManyParameters()
        {
            var proc = ThreadMethod.Create(Method2);
            {
                await proc.StartAsync("Jordan", 24, "Duerksen", 0.5, new Customer() { First = "Jordan", Last = "Duerksen" });
            }
        }
    }
}
