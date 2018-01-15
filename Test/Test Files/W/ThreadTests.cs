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
    public class ThreadTests
    {
        //private ITestOutputHelper output;
        //public ThreadTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}
        [TestMethod]
        public void Thread_Simple()
        {
            var age = new Lockable<int>();

            var thread = W.Threading.Thread.Create((token) =>
            {
                while(age.Value < 47)
                {
                    age.Value += 1;
                    W.Threading.Thread.Sleep(0);
                }
            });
            thread.Start();
            var result = thread.Join(1000);

            Assert.IsTrue(age.Value == 47);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void Thread_Cancel()
        {
            var thread = new W.Threading.Thread(token =>
            //var thread = Thread.Create((token) =>
            {
                while (!token.IsCancellationRequested)
                    W.Threading.Thread.Sleep(CPUProfileEnum.Sleep);
            });
            thread.Start();
            W.Threading.Thread.Sleep(500);
            thread.Stop();
            var result = thread.Join(1000);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Thread_SimpleTyped()
        {
            var age = new Lockable<int>(25);
            var name = "Jordan";

            var thread = W.Threading.Thread.Create<int>((token, internalAge) =>
            {
                Assert.IsTrue(age.Value != internalAge);
                age.Value = internalAge;
                name += " Duerksen";
            });
            thread.Start(47);
            var result = thread.Join(60000);

            Assert.IsTrue(age.Value == 47, "Age = " + age.Value + " instead of 47");
            Assert.IsTrue(result);
            Assert.IsTrue(name == "Jordan Duerksen");
        }

        [TestMethod]
        public async Task Thread_ThreadSleep()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            await Task.Delay(1000);
            var elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine("Elapsed = {0}", elapsed);
            Assert.IsTrue(elapsed > 999, "Elapsed time was too short");
            Assert.IsTrue(elapsed < 1100, "Elapsed time was too long");
        }

        [TestMethod]
        public void Thread_StopWithNoStart()
        {
            bool complete = false;
            var thread = new W.Threading.Thread(token =>
            {
                while (!token.IsCancellationRequested)
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                complete = true;
            });
            thread.Stop();
            var joinComplete = thread.Join(1000);// cts.Token.WaitHandle.WaitOne(5000);

            Assert.IsFalse(joinComplete);
            Assert.IsFalse(complete);
        }
        [TestMethod]
        public void Thread_StopButCompleteOk()
        {
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            bool complete = false;
            var thread = new W.Threading.Thread(token =>
            {
                while (!token.IsCancellationRequested)
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                complete = true;
                mreContinue.Set();
            });
            thread.Start();
            
            var joinComplete = thread.Join(10);// cts.Token.WaitHandle.WaitOne(5000);
            Assert.IsFalse(joinComplete);
            thread.SignalToStop();
            joinComplete = thread.Join(5000);// cts.Token.WaitHandle.WaitOne(5000);
            Assert.IsTrue(joinComplete);
            if (!complete)
                Assert.IsTrue(mreContinue.Wait(1000));
            Assert.IsTrue(complete);
        }
        [TestMethod]
        public void Thread_CreateAndDispose()
        {
            using (var thread = new W.Threading.Thread(token => { }))
            {
                thread.Start();
                Assert.IsTrue(true);
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void Thread_RunForASecond()
        {
            var t = new LockableSlim<int>();
            using (var thread = new W.Threading.Thread(token => 
            {
                while (!token.IsCancellationRequested)
                {
                    t.Value += 1;
                    W.Threading.Thread.Sleep(CPUProfileEnum.Sleep);
                }
            }))
            {
                thread.Start();
                var isComplete = thread.Join(1000); //let it run for a second before returning
                Assert.IsFalse(isComplete);
                Console.WriteLine("t = {0}", t.Value);
                Assert.IsTrue(t.Value > 0);
                //thread.Stop();
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public async Task Thread_RunAndCancel()
        {
            var t = 0;
            using (var thread = new W.Threading.Thread(token => { while (!token.IsCancellationRequested) { t += 1; W.Threading.Thread.Sleep(CPUProfileEnum.Sleep); } }))
            {
                thread.Start();
                await Task.Delay(500);
                thread.Stop();
                var complete = thread.Join(1000);

                Console.WriteLine("t = {0}", t);

                Assert.IsTrue(t > 0);
                Assert.IsTrue(complete);
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void Thread_Sleep_Sleep()
        {
            var mre = new System.Threading.ManualResetEventSlim(false);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var thread = new W.Threading.Thread(token =>
            {
                mreContinue.Wait();
                for(int t=0; t<2500; t++)
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
            });
            thread.Start();
            Assert.IsTrue(thread.IsStarted);
            Assert.IsTrue(!thread.IsComplete);
            mreContinue.Set();
            Assert.IsTrue(thread.Join(10000));
            Assert.IsTrue(thread.IsComplete);
        }
        [TestMethod]
        public void Thread_Sleep_SpinWait0()
        {
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var thread = new W.Threading.Thread(token =>
            {
                mreContinue.Wait();
                var sw = System.Diagnostics.Stopwatch.StartNew();
                //this loop has to be short because we're using SpinWait0 which is extremly fast
                while (sw.ElapsedMilliseconds < 10)
                {
                    Console.WriteLine("Elapsed = {0}", sw.ElapsedMilliseconds);
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait0);
                }
                sw.Stop();
                Console.WriteLine("Elapsed = {0}", sw.ElapsedMilliseconds);
            });
            thread.Start();
            Assert.IsTrue(thread.IsStarted);
            Assert.IsTrue(!thread.IsComplete);
            mreContinue.Set();
            Assert.IsTrue(thread.Join(1000));
            Assert.IsTrue(thread.IsComplete);
        }
        [TestMethod]
        public void Thread_Sleep_SpinWait1()
        {
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var thread = new W.Threading.Thread(token =>
            {
                mreContinue.Wait();
                var sw = System.Diagnostics.Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds < 1000)
                {
                    Console.WriteLine("Elapsed = {0}", sw.ElapsedMilliseconds);
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                }
                sw.Stop();
                Console.WriteLine("Elapsed = {0}", sw.ElapsedMilliseconds);
            });
            thread.Start();
            Assert.IsTrue(thread.IsStarted);
            Assert.IsTrue(!thread.IsComplete);
            mreContinue.Set();
            Assert.IsTrue(thread.Join(20000));
            Assert.IsTrue(thread.IsComplete);
        }
#if NET45
        [TestMethod]
        public void Thread_Sleep_Yield()
        {
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var thread = new W.Threading.Thread(token =>
            {
                mreContinue.Wait();
                for (int t = 0; t < 2500; t++)
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Yield);
            });
            thread.Start();
            Assert.IsTrue(thread.IsStarted);
            Assert.IsTrue(!thread.IsComplete);
            mreContinue.Set();
            Assert.IsTrue(thread.Join(10000));
            Assert.IsTrue(thread.IsComplete);
        }
#endif

    }
}
