using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace W.Tests
{
    [TestFixture]
    internal class PropertyHostMethodsTests
    {
        private static PropertyHostMethodsTests _host = null;
        private Property<PropertyHostMethodsTests, int> SomeInt { get; set; }

        [Test]
        public void InitializePropertiesTest()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>((owner, oldValue, newValue) =>
            {
                _host = owner as PropertyHostMethodsTests;
            });
            PropertyHostMethods.InitializeProperties(this);
            SomeInt.Value++;
            Assert.IsTrue(_host == this);
        }
        [Test]
        public void IsDirty_NotDirty_Test()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>();
            var result = PropertyHostMethods.IsDirty(this);
            Assert.IsFalse(result);
        }
        [Test]
        public void IsDirty_IsDirty_Test()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>();
            SomeInt.Value += 1;
            var result = PropertyHostMethods.IsDirty(this);
            Assert.IsTrue(result);
        }
        [Test]
        public void MarkAsClean_Test()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>();
            SomeInt.Value += 1;
            var result = PropertyHostMethods.IsDirty(this);
            Assert.IsTrue(result);

            PropertyHostMethods.MarkAsClean(this);
            result = PropertyHostMethods.IsDirty(this);
            Assert.IsFalse(result);
        }
    }
}
