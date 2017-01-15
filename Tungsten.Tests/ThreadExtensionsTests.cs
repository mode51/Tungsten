using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace W.Tests
{
    [TestFixture]
    internal class ThreadExtensionsTests
    {
        [Test]
        public void Simple()
        {
            var age = new Lockable<int>();
            var name = "Jordan";

            var result = name.Thread((n) =>
            {
                for (; age.Value < 47; age.Value++)
                {
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

            var result = this.Thread((tests) =>
            {
                throw new ArgumentNullException("Value");
            }, (b, exception) =>
            {
                e = exception;
            }).Join(1000);

            Assert.IsTrue(result);
            Assert.IsTrue(e != null);
        }
    }
}
