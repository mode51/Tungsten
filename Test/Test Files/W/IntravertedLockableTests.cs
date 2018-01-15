//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using W;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace W.Tests
//{
//    public class StopwatchEx : IDisposable
//    {
//        private System.Diagnostics.Stopwatch _sw = null;
//        private Action<long> _onComplete = null;

//        public System.Diagnostics.Stopwatch Stopwatch => _sw;

//        public void Dispose()
//        {
//            var elapsed = _sw.ElapsedMilliseconds;
//            _onComplete?.Invoke(elapsed);
//        }
//        public StopwatchEx(Action<long> onComplete)
//        {
//            _onComplete = onComplete;
//            _sw = System.Diagnostics.Stopwatch.StartNew();
//        }

//        public static StopwatchEx Create(Action<long> onComplete) => new StopwatchEx(onComplete);
//        public static StopwatchEx SendToConsole(string format = "Elapsed: {0}") => new StopwatchEx(elapsed => { Console.WriteLine(format, elapsed); });
//        public static StopwatchEx DebugWriteLine(string format = "Elapsed: {0}") => new StopwatchEx(elapsed => { System.Diagnostics.Debug.WriteLine(string.Format(format, elapsed)); });
//        public static StopwatchEx TraceWriteLine(string format = "Elapsed: {0}") => new StopwatchEx(elapsed => { System.Diagnostics.Trace.WriteLine(string.Format(format, elapsed)); });
//    }
//    [TestClass]
//    public class IntravertedLockableTests
//    {
//        [TestMethod]
//        public void Create()
//        {
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 24;
//            Assert.IsTrue(lockable.Value == 24);
//        }

//        [TestMethod]
//        public void WaitForChange_Fail_WithTimeout()
//        {
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 24;
//            Assert.IsTrue(lockable.Value == 24);
//            Assert.IsFalse(lockable.WaitForChange(25));
//        }
//        [TestMethod]
//        public void WaitForChange_Fail_WithCancellationToken()
//        {
//            var cts = new CancellationTokenSource(25);
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 24;
//            Assert.IsTrue(lockable.Value == 24);
//            try
//            {
//                lockable.WaitForChange(cts.Token);
//            }
//            catch (OperationCanceledException)
//            {
//                Assert.IsTrue(lockable.Value == 24);
//            }
//        }
//        [TestMethod]
//        public void WaitForChange_Success()
//        {
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 24;
//            var thread1 = W.Threading.ThreadSlim.Create(async token =>
//            {
//                await Task.Delay(25);
//                lockable.Value = 25;
//            });
//            var thread2 = W.Threading.ThreadSlim.Create(token =>
//            {
//                var result = lockable.WaitForChange(1000);
//                Assert.IsTrue(result);
//            });

//            Assert.IsTrue(lockable.Value == 24);
//            thread2.Start();//begin waiting
//            thread1.Start(); //change the value
//            thread2.Wait();
//            Assert.IsTrue(lockable.Value == 25);

//            thread2.Dispose();
//            thread1.Dispose();
//        }
//        [TestMethod]
//        public void WaitForChange_Success_WithTimeout()
//        {
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 0;
//            using (var threadWait = W.Threading.ThreadMethod.Create(() =>
//            {
//                Console.WriteLine("Started");
//                Assert.IsTrue(lockable.Value == 0);
//                Console.WriteLine("Waiting");
//                using(StopwatchEx.SendToConsole())
//                    lockable.WaitForChange(1000);
//                Console.WriteLine("New Value = {0}", lockable.Value);
//                Assert.IsTrue(lockable.Value == 25);
//            }))
//            {
//                threadWait.Start();
//                threadWait.Wait(500);//give it time to start waiting
//                lockable.Value = 25; //change the value and allow the thread to continue
//                Assert.IsTrue(threadWait.Wait(1000));//now just wait for the thread to exit
//            }

//            //var thread1 = W.Threading.ThreadSlim.Create(token =>
//            //{
//            //    //because I can't write a test to validate a specific number
//            //    while (lockable.Value < 5000) 
//            //    {
//            //        lockable.Value += 1;
//            //        W.Threading.Thread.Sleep(1);
//            //    }
//            //});
//            //var thread2 = W.Threading.ThreadSlim.Create(token =>
//            //{
//            //    var result = lockable.WaitForChange(1000);
//            //    Assert.IsTrue(result);
//            //});

//            //Assert.IsTrue(lockable.Value == 0);
//            //thread2.Start();//begin waiting
//            //thread1.Start(); //change the value
//            //Assert.IsTrue(thread2.Wait(1000));
//            //Console.WriteLine("lockable.Value = {0}", lockable.Value);
//            //thread2.Dispose();
//            //thread1.Dispose();
//        }

//        [TestMethod]
//        public void _TokenTest()
//        {
//            var cts = new CancellationTokenSource(1000);
//            var mre = new ManualResetEventSlim(false);
//            var thread = W.Threading.ThreadMethod.Create(() => mre.Set());
//            using (StopwatchEx.SendToConsole())
//            {
//                thread.RunSynchronously();
//                mre.Wait(cts.Token);
//                Assert.IsFalse(cts.Token.IsCancellationRequested);
//            }
//        }
//        [TestMethod]
//        public async Task WaitForChange_Success_WithCancellationToken()
//        {
//            var cts = new CancellationTokenSource(500);
//            var lockable = new W.IntravertedLockable<int>(24);
//            var threadChanger = W.Threading.ThreadMethod.Create(() =>
//            {
//                //because I can't write a test to validate a specific number
//                while (!cts.Token.IsCancellationRequested)
//                {
//                    W.Threading.Thread.Sleep(1);
//                    lockable.Value += 1;
//                }
//            });
//            threadChanger.Start();

//            using (StopwatchEx.SendToConsole())
//            {
//                //Assert.IsTrue(lockable.Value == 24, string.Format("{0} != {1}", lockable.Value, 24));
//                await lockable.WaitForChangeAsync(cts.Token).ContinueWith(task => { Console.WriteLine("t={0}", task.Result); });
//                //Assert.IsTrue(lockable.Value == 25);
//            }
//            threadChanger.Dispose();
//        }
//        [TestMethod]
//        public async Task WaitForChangeAsync_Success_WithTimeout()
//        {
//            var lockable = new W.IntravertedLockable<int>();
//            await lockable.WaitForChangeAsync(25).ContinueWith(task =>
//            {
//                Assert.IsTrue(task.IsCompletedSuccessfully);
//                Assert.IsTrue(task.IsCompleted);
//                Assert.IsFalse(task.IsCanceled);
//                Assert.IsFalse(task.IsFaulted);
//                Assert.IsFalse(task.Result == 25);
//            });
//        }
//        [TestMethod]
//        public async Task WaitForChangeAsync_WithCancellationToken()
//        {
//            var cts = new CancellationTokenSource(25);
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 24;
//            Assert.IsTrue(lockable.Value == 24);
//            try
//            {
//                await lockable.WaitForChangeAsync(cts.Token).ContinueWith(task =>
//                {
//                    Assert.IsFalse(task.IsCompletedSuccessfully);
//                    Assert.IsTrue(task.IsCompleted);
//                    Assert.IsTrue(task.IsCanceled);
//                    Assert.IsFalse(task.IsFaulted);
//                });
//            }
//            catch (OperationCanceledException)
//            {
//                Assert.IsTrue(lockable.Value == 24);
//            }
//        }


//        [TestMethod]
//        public void WaitForValue_True()
//        {
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 24;
//            var thread1 = W.Threading.ThreadSlim.Create(async token =>
//            {
//                while (lockable.Value < 5000)
//                {
//                    await Task.Delay(1);
//                    lockable.Value += 1;
//                }
//            });
//            var thread2 = W.Threading.ThreadSlim.Create(token =>
//            {
//                lockable.WaitForValue(5000);
//                Assert.IsTrue(lockable.Value == 5000);
//            });

//            Assert.IsTrue(lockable.Value == 24);
//            thread2.Start();//begin waiting
//            thread1.Start(); //change the value
//            thread2.Wait();
//            Assert.IsTrue(lockable.Value == 25);

//            thread2.Dispose();
//            thread1.Dispose();
//        }
//        [TestMethod]
//        public void WaitForValue_WithTimeout()
//        {
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 24;
//            Assert.IsTrue(lockable.Value == 24);
//            Assert.IsFalse(lockable.WaitForValue(25, 25));
//        }
//        [TestMethod]
//        public void WaitForValue_WithCancellationToken()
//        {
//            var cts = new CancellationTokenSource(25);
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 24;
//            Assert.IsTrue(lockable.Value == 24);
//            try
//            {
//                lockable.WaitForValue(25, cts.Token);
//            }
//            catch (OperationCanceledException)
//            {
//                Assert.IsTrue(lockable.Value == 24);
//            }
//        }
//        [TestMethod]
//        public async Task WaitForValueAsync_WithTimeout()
//        {
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 24;
//            Assert.IsTrue(lockable.Value == 24);
//            await lockable.WaitForValueAsync(25, 25).ContinueWith(task =>
//            {
//                Assert.IsFalse(task.Result);
//                Assert.IsTrue(task.IsCompletedSuccessfully);
//                Assert.IsTrue(task.IsCompleted);
//                Assert.IsFalse(task.IsCanceled);
//                Assert.IsFalse(task.IsFaulted);
//            });
//        }
//        [TestMethod]
//        public async Task WaitForValueAsync_WithCancellationToken()
//        {
//            var cts = new CancellationTokenSource(25);
//            var lockable = new W.IntravertedLockable<int>();
//            lockable.Value = 24;
//            Assert.IsTrue(lockable.Value == 24);
//            try
//            {
//                await lockable.WaitForValueAsync(25, cts.Token).ContinueWith(task =>
//                {
//                    Assert.IsFalse(task.IsCompletedSuccessfully);
//                    Assert.IsTrue(task.IsCompleted);
//                    Assert.IsTrue(task.IsCanceled);
//                    Assert.IsFalse(task.IsFaulted);
//                });
//            }
//            catch (OperationCanceledException)
//            {
//                Assert.IsTrue(lockable.Value == 24);
//            }
//        }

//    }
//}

