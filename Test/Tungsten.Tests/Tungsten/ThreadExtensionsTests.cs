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
    internal class ThreadExtensionsTests
    {
        [Test]
        public void Simple()
        {
            var age = new Lockable<int>();
            var name = "Jordan";

            var result = name.CreateThread((n, cts) =>
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
            }, null).Join(1000);

            Assert.IsTrue(age.Value == 47);
            Assert.IsTrue(result);
            Assert.IsTrue(name == "Jordan Duerksen");
        }
        [Test]
        public void DoNothingAndSucceed()
        {
            Exception e = null;

            var result = this.CreateThread((tests, cts) =>
            {
                System.Threading.Thread.Sleep(0);
            }, (b, exception) =>
            {
                Assert.IsTrue(b);
                e = exception;
            }, null).Join(1000);

            Assert.IsTrue(result);
            Assert.IsTrue(e == null);
        }
        [Test]
        public void HandleException()
        {
            Exception e = null;

            var result = this.CreateThread((tests, cts) =>
            {
                throw new ArgumentNullException("Value");
            }, (b, exception) =>
            {
                e = exception;
                Assert.IsFalse(b);
            }, null).Join(1000);

            Assert.IsTrue(result);
            Assert.IsTrue(e != null);
        }
        [Test]
        public void CancelThread()
        {
            Exception e = null;

            var result = this.CreateThread((tests, cts) =>
            {
                cts.Cancel();
                throw new ArgumentNullException("Value");
            }, (b, exception) =>
            {
                e = exception;
            }, null).Join(1000);

            Assert.IsTrue(result);
            Assert.IsTrue(e != null);
        }
    }
}
