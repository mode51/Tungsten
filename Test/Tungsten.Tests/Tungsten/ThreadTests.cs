using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using W.Threading;

namespace W.Tests.Tungsten
{
    [TestFixture]
    internal class ThreadTests
    {
        [Test]
        public void Simple()
        {
            var age = new Lockable<int>();
            var name = "Jordan";

            var result = Thread.Create((cts) =>
            {
                for (; age.Value < 47; age.Value++)
                {
                    if (cts.IsCancellationRequested)
                        return;
                    System.Threading.Thread.Sleep(0);
                }
            }, (b, exception) =>
            {
                name = "Jordan Duerksen";
            }).Join(1000);

            Assert.IsTrue(age.Value == 47);
            Assert.IsTrue(result);
            Assert.IsTrue(name == "Jordan Duerksen");
        }
        [Test]
        public void HandleException()
        {
            Exception e = null;

            var result = Thread.Create((cts) =>
            {
                throw new ArgumentNullException("Value");
            }, (b, exception) =>
            {
                e = exception;
            }).Join(1000);

            Assert.IsTrue(result);
            Assert.IsTrue(e != null);
        }
        [Test]
        public void CancelThread()
        {
            Exception e = null;

            var result = Thread.Create((cts) =>
            {
                cts.Cancel();
                throw new ArgumentNullException("Value");
            }, (b, exception) =>
            {
                e = exception;
            }).Join(1000);

            Assert.IsTrue(result);
            Assert.IsTrue(e != null);
        }

        [Test]
        public void SimpleTyped()
        {
            var age = new Lockable<int>(25);
            var name = "Jordan";

            var result = Thread<int>.Create((internalAge, cts) =>
            {
                Assert.IsTrue(age.Value == internalAge);
                age.Value = 47;
            }, (b, exception) =>
            {
                name = "Jordan Duerksen";
            }, null, age.Value).Join(1000);

            Assert.IsTrue(age.Value == 47);
            Assert.IsTrue(result);
            Assert.IsTrue(name == "Jordan Duerksen");
        }
        [Test]
        public void HandleExceptionTyped()
        {
            Exception e = null;

            var result = Thread<int>.Create((value, cts) =>
            {
                throw new ArgumentNullException("Value");
            }, (b, exception) =>
            {
                e = exception;
            },null, 47).Join(1000);

            Assert.IsTrue(result);
            Assert.IsTrue(e != null);
        }
        [Test]
        public void CancelThreadTyped()
        {
            Exception e = null;

            var result = Thread<int>.Create((value, cts) =>
            {
                cts.Cancel();
                throw new ArgumentNullException("Value");
            }, (b, exception) =>
            {
                e = exception;
            }, null, 47).Join(1000);

            Assert.IsTrue(result);
            Assert.IsTrue(e != null);
        }

    }
}
