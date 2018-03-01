using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private void Method1(CancellationToken token)
        {
            Console.WriteLine("Inside Method1");
            Console.WriteLine("Leaving Method1");
        }
        private void Method2(CancellationToken token, params object[] args)
        {
            Console.WriteLine("Inside Method2(" + string.Join(", ", args) + ")");
            Console.WriteLine("Leaving Method2");
            //for (int t = 0; t < args.Length; t++)
            //    Console.WriteLine("Parameter = " + args[t].ToString());
        }

        [TestMethod]
        public void ThreadMethod_Create()
        {
            var proc = W.Threading.ThreadMethod.Create(Method1);
            {

                Assert.IsTrue(proc != null);
                Assert.IsTrue(!proc.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadMethod_Create2()
        {
            var proc = W.Threading.ThreadMethod.Create(Method2);
            {
                Assert.IsTrue(proc != null);
                Assert.IsTrue(!proc.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadMethod_Run()
        {
            var proc = W.Threading.ThreadMethod.Create(Method2);
            {
                proc.Start();
                //proc.Wait();
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                Assert.IsTrue(proc.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadMethod_RunWithParameters()
        {
            var proc = W.Threading.ThreadMethod.Create(Method2);
            {
                proc.Start(24);
                //proc.Wait();
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                Assert.IsTrue(proc.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadMethod_RunWithManyParameters()
        {
            var proc = W.Threading.ThreadMethod.Create(Method2);
            {
                proc.Start("Jordan", 24, "Duerksen", 0.5, new Customer() { First = "Jordan", Last = "Duerksen" });
                //proc.Wait();
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                Assert.IsTrue(proc.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadMethod_Cancel()
        {
            var proc = W.Threading.ThreadMethod.Create(token =>
            {
                while(!token.IsCancellationRequested)
                {
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                }
            });
            proc.Start();
            System.Threading.Thread.Sleep(10);
            proc.Dispose();
            Assert.IsTrue(proc.IsComplete);
        }
    }
}
