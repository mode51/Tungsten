using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.Threading;
using W.Threading.ThreadExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class ThreadExtensionsTests
    {
        //private ITestOutputHelper output;
        //public ThreadExtensionsTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}

        private int Test_Value = 10;

        //[TestMethod]
        //public void ThreadExtensions_Simple()
        //{
        //    Exception e = null;

        //    var thread = Test_Value.CreateThread<int>((token, value) =>
        //    {
        //        System.Threading.Thread.Sleep(0);
        //        Assert.IsTrue(value == 10);
        //    });
        //    thread.Start();
        //    var result = thread.Join(1000);

        //    Assert.IsTrue(result);
        //    Assert.IsTrue(e == null);
        //}
        //[TestMethod]
        //public void ThreadExtensions_ShortLoop()
        //{
        //    var name = "Jordan";

        //    var thread = W.Threading.ThreadExtensions.ThreadExtensions.CreateThread<string>(name, (token, n) =>
        //    {
        //        Console.WriteLine("Name = {0}", n);
        //        for (int t = 0; t < 10; t++)
        //        {
        //            W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
        //            //System.Threading.Thread.Sleep(0);
        //        }
        //        n += " Duerksen";
        //        name += " Duerksen";
        //        Console.WriteLine("Name = " + name);
        //    });
        //    thread.Start();
        //    var result = thread.Wait(15000);
        //    Assert.IsTrue(result);
        //    Console.WriteLine("Name = " + name);
        //    Assert.IsTrue(name == "Jordan Duerksen");
        //}
        //[TestMethod]
        //public void ThreadExtensions_HandleAnException()
        //{
        //    var name = "Jordan";
        //    //var thread = W.Threading.ThreadExtensions.ThreadExtensions.CreateThread<string>(name, (value, token) =>
        //    var thread = W.Threading.ThreadExtensions.ThreadExtensions.CreateThread<string>(name, (token, value) =>
        //    {
        //        Console.WriteLine("Name = {0}", value);
        //        throw new ArgumentNullException("Value");
        //    });
        //    thread.Start();
        //    var result = thread.Wait(1000);

        //    Assert.IsTrue(result);
        //    //Assert.IsTrue(thread.IsFaulted);
        //    //Assert.IsTrue(thread.Exception != null);
        //}
        //[TestMethod]
        //public void ThreadExtensions_ExecuteAndHandleException()
        //{
        //    var thread = Test_Value.CreateThread<int>((token, value) =>
        //    {
        //        Console.WriteLine("Test_Value = {0}", value);
        //        throw new ArgumentNullException("Value");
        //    });
        //    thread.Start(24);
        //    var result = thread.Join(1000);

        //    Assert.IsTrue(result);
        //    //Assert.IsTrue(thread.IsFaulted);
        //    //Assert.IsTrue(thread.Exception != null);
        //}
        //[TestMethod]
        //public void ThreadExtensions_ThreadMethodSlim_Create()
        //{
        //    var action = new Action(() =>
        //    {
        //        Console.WriteLine("The action has been run");
        //    });
        //    var at = action.AsThreadMethodSlim();
        //    Assert.IsTrue(at != null);
        //    at.RunAsync().Wait();
        //    Assert.IsTrue(at.IsComplete);
        //}
        //[TestMethod]
        //public void ThreadExtensions_ThreadMethodSlim_Run()
        //{
        //    var mre = new System.Threading.ManualResetEventSlim(false);
        //    var action = new Action(() =>
        //    {
        //        Console.WriteLine("The action has been run");
        //        mre.Set();
        //    });
        //    var at = action.AsThreadMethodSlim();
        //    Assert.IsTrue(at != null);
        //    Assert.IsFalse(at.IsComplete);
        //    at.Run();
        //    Assert.IsTrue(mre.Wait(5000));

        //    mre.Dispose();
        //}
        //[TestMethod]
        //public void ThreadExtensions_ThreadMethodSlim_Create2()
        //{
        //    var at = ThreadMethodSlim.Create(() =>
        //    {
        //        Console.WriteLine("The action has been run");
        //    });
        //    Assert.IsTrue(at != null);
        //    Assert.IsFalse(at.IsComplete);
        //    at.RunAsync().Wait();
        //    Assert.IsTrue(at.IsComplete);
        //}
        //[TestMethod]
        //public void ThreadExtensions_ThreadMethodSlim_RunAndContinueWith()
        //{
        //    var mre = new System.Threading.ManualResetEventSlim(false);
        //    var at = ThreadMethodSlim.Create(() =>
        //    {
        //        Console.WriteLine("The action has been run");
        //    });
        //    Assert.IsTrue(at != null);
        //    Assert.IsFalse(at.IsComplete);
        //    at.RunAsync().ContinueWith(task =>
        //    {
        //        mre.Set();
        //    });
        //    Assert.IsTrue(mre.Wait(5000));

        //    mre.Dispose();
        //}
        //[TestMethod]
        //public void ThreadExtensions_ThreadMethodSlim_DontRun()
        //{
        //    var mre = new System.Threading.ManualResetEventSlim(false);

        //    var action = new Action(() =>
        //    {
        //        Console.WriteLine("The action has been run");
        //        mre.Set();
        //    });
        //    var at = action.AsThreadMethodSlim();
        //    Assert.IsTrue(at != null);
        //    Assert.IsFalse(mre.Wait(250));
        //    Assert.IsFalse(at.IsComplete);
        //    //at.Dispose();//if I don't call this, the private members don't get released
        //}
        //[TestMethod]
        //public void ThreadExtensions_ThreadMethodSlim_ManyDontRun()
        //{
        //    var mre = new System.Threading.ManualResetEventSlim(false);

        //    var action = new Action(() =>
        //    {
        //        Console.WriteLine("The action has been run");
        //        mre.Set();
        //    });
        //    for (int t = 0; t < 10; t++)
        //    {
        //        var at = action.AsThreadMethodSlim();
        //        //using (var at = action.GetThreadMethodSlim())
        //        {
        //            Console.WriteLine("ThreadMethodSlim {0} Created", t);
        //        }
        //    }
        //}
        [TestMethod]
        public void ThreadMethod_Create()
        {
            var at = ThreadMethod.Create((args) =>
            {
                var argsString = String.Join(", ", args);
                Console.WriteLine("ThreadMethodSlimEx: " + argsString);
            });
            Assert.IsTrue(at != null);
            Assert.IsFalse(at.IsComplete);
            at.StartAsync("Jordan", 47).Wait();
            Assert.IsTrue(at.IsComplete);
        }
        [TestMethod]
        public void ThreadMethod_RunTwice()
        {
            var at = ThreadMethod.Create((args) =>
            {
                var argsString = String.Join(", ", args);
                Console.WriteLine("ThreadMethodSlimEx: " + argsString);
            });
            Assert.IsTrue(at != null);
            Assert.IsFalse(at.IsComplete);
            at.RunSynchronously("Run once");
            at.RunSynchronously("Can't Run", "Twice");
            Assert.IsTrue(at.Wait(1000));
            Assert.IsTrue(at.IsComplete);
        }
        [TestMethod]
        public void ThreadMethod_DontRun()
        {
            var at = ThreadMethod.Create((args) =>
            {
                if (args != null)
                {
                    var argsString = String.Join(", ", args);
                    Console.WriteLine("ThreadMethodSlimEx: " + argsString);
                }
            });
            Assert.IsTrue(at != null);
            at.Wait(100);//allow enough time for the task to start and be waiting on a call to Run/RunAsync
            Assert.IsFalse(at.IsComplete);
        }
        [TestMethod]
        public async Task ThreadMethod_Await_10Times()
        {
            var mre = new System.Threading.ManualResetEventSlim(false);

            var @delegate = new AnyMethodDelegate(args =>
            {
                if (args != null)
                {
                    var argsString = String.Join(", ", args);
                    Console.WriteLine("ThreadMethodSlimEx: " + argsString);
                }
                mre.Set();
            });
            for (int t = 0; t < 10; t++)
            {
                var at = ThreadMethod.Create(@delegate);
                Console.WriteLine("ThreadMethodSlim {0} Created", t);
                await at.StartAsync("Jordan", 47);
            }
        }
        [TestMethod]
        public void ThreadMethod_Run_10times()
        {
            var mre = new System.Threading.ManualResetEventSlim(false);

            var @delegate = new AnyMethodDelegate(args =>
            {
                if (args != null)
                {
                    var argsString = String.Join(", ", args);
                    Console.WriteLine("ThreadMethodSlimEx: " + argsString);
                }
                mre.Set();
            });
            for (int t = 0; t < 10; t++)
            {
                var at = ThreadMethod.Create(@delegate);
                Console.WriteLine("ThreadMethodSlim {0} Created", t);
                at.StartAsync("Jordan", 47).Wait(1000);
            }
        }
    }
}
