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
        private static object _locker = new object();
        private int _counter = 0;
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
            Output("Inside Method1");
            Output("Leaving Method1");
        }
        private void Method2(CancellationToken token, params object[] args)
        {
            if (args?.Length > 0)
                Output("Inside Method2(" + string.Join(", ", args) + ")");
            else
                Output("Inside Method2");
            Output("Leaving Method2");
            _counter += 1;
            //for (int t = 0; t < args.Length; t++)
            //    Console.WriteLine("Parameter = " + args[t].ToString());
        }
        private static void Output(string format, params object[] args)
        {
            try
            {
                //lock (outputLock)
                    Console.WriteLine(format, args);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Break();
            }
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
        public void ThreadMethod_RunManyTimes()
        {
            var iterations = 1000;
            var proc = W.Threading.ThreadMethod.Create(Method2);
            Assert.IsTrue(!proc.IsComplete);
            _counter = 0;
            for (int t = 1; t <= iterations; t++)
            {
                proc.Start(t);
                Assert.IsTrue(proc.Wait(1000), $"Iteration #{t} failed to complete");
                //proc.Wait();
                //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                Assert.IsTrue(proc.IsComplete, $"Exception at iteration #{t}");
                Assert.IsTrue(_counter == t, $"_counter != t, _counter/t == {_counter}/{t}");
            }
            Assert.IsTrue(_counter == iterations, $"_counter != iterations, _counter == {_counter}");
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
                while (!token.IsCancellationRequested)
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
