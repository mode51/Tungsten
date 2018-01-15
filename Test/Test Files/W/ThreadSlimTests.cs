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
    public class ThreadSlimTests
    {
        //private ITestOutputHelper output;
        //public ThreadSlimTests(ITestOutputHelper output)
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
        private void ThreadProc1(System.Threading.CancellationToken token)
        {
            Console.WriteLine("Inside ThreadProc1");
            while (!token.IsCancellationRequested)
            {
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
            }
            Console.WriteLine("Leaving ThreadProc1");
        }
        private void ThreadProc2(System.Threading.CancellationToken token, params object[] args)
        {
            Console.WriteLine("Inside ThreadProc2(" + string.Join(", ", args) + ")");
            while (!token.IsCancellationRequested)
            {
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
            }
            Console.WriteLine("Leaving ThreadProc2");
        }

        [TestMethod]
        public void ThreadSlim_Create()
        {
            using (var thread = new W.Threading.ThreadSlim(ThreadProc1))
            {
                Assert.IsTrue(thread != null);
                Assert.IsTrue(!thread.IsRunning);
                Assert.IsTrue(!thread.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadSlim_Create2()
        {
            using (var thread = new W.Threading.ThreadSlim(ThreadProc2))
            {
                Assert.IsTrue(thread != null);
                Assert.IsTrue(!thread.IsRunning);
                Assert.IsTrue(!thread.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadSlim_StartAndStop()
        {
            using (var thread = new W.Threading.ThreadSlim(ThreadProc1))
            {
                Assert.IsTrue(thread != null);
                thread.Start();
                Assert.IsTrue(thread.IsRunning);
                thread.Wait(1); //this should allow the proc to run at least once
                thread.Stop();
                Assert.IsTrue(!thread.IsRunning);
                Assert.IsTrue(thread.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadSlim_StartAndStop2()
        {
            using (var thread = new W.Threading.ThreadSlim(ThreadProc2))
            {
                Assert.IsTrue(thread != null);
                thread.Start(24);
                Assert.IsTrue(thread.IsRunning);
                thread.Wait(1); //this should allow the proc to run at least once
                thread.Stop();
                Assert.IsTrue(!thread.IsRunning);
                Assert.IsTrue(thread.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadSlim_Restart()
        {
            using (var thread = new W.Threading.ThreadSlim(ThreadProc1))
            {
                Assert.IsTrue(thread != null);
                for (int t = 0; t < 10; t++)
                {
                    thread.Start();
                    Assert.IsTrue(thread.IsRunning);
                    thread.Wait(1); //this should allow the proc to run at least once
                    thread.Stop();
                    Assert.IsTrue(!thread.IsRunning);
                    Assert.IsTrue(thread.IsComplete);
                }
            }
        }
        [TestMethod]
        public void ThreadSlim_SignalToStopAndWait_1sec()
        {
            using (var thread = new W.Threading.ThreadSlim(ThreadProc1))
            {
                Assert.IsTrue(thread != null);
                thread.Start();
                Assert.IsTrue(thread.IsRunning);
                thread.SignalToStop();
                Assert.IsTrue(thread.Wait(1000));
                Assert.IsTrue(!thread.IsRunning);
                Assert.IsTrue(thread.IsComplete);
            }
        }
        [TestMethod]
        public void ThreadSlim_SignalToStopAndWait_ForCancel()
        {
            var cts = new System.Threading.CancellationTokenSource();
            using (var thread = new W.Threading.ThreadSlim(ThreadProc1))
            {
                Assert.IsTrue(thread != null);
                thread.Start();
                Assert.IsTrue(thread.IsRunning);
                thread.SignalToStop();
                cts.CancelAfter(1000);
                Assert.IsTrue(thread.Wait(cts.Token));
                Assert.IsTrue(!thread.IsRunning);
                Assert.IsTrue(thread.IsComplete);
            }
        }
        [TestMethod]
        public async Task ThreadSlim_SignalToStopAndWaitIndefinitelyAsync()
        {
            var cts = new System.Threading.CancellationTokenSource();
            using (var thread = new W.Threading.ThreadSlim(ThreadProc1))
            {
                Assert.IsTrue(thread != null);
                thread.Start();
                Assert.IsTrue(thread.IsRunning);
                //this should allow the proc to run at least once
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                thread.SignalToStop();
                cts.CancelAfter(1000);
                await Task.Run(() => { thread.Wait(); }, cts.Token).ContinueWith(task =>
                {
                    Assert.IsTrue(!task.IsCanceled);
                    Assert.IsTrue(!task.IsFaulted);
                    Assert.IsTrue(!thread.IsRunning);
                    Assert.IsTrue(thread.IsComplete);
                });
            }
        }
    }
}
