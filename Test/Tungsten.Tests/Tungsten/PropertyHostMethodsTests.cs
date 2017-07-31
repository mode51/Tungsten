using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace W.Tests.Tungsten
{
    [TestFixture]
    internal class PropertyHostMethodsTests
    {
        private static PropertyHostMethodsTests _host = null;
        private Property<PropertyHostMethodsTests, int> SomeInt { get; set; }

        [Test]
        public void InitializeProperties_Test()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>((owner, oldValue, newValue) =>
            {
                _host = owner as PropertyHostMethodsTests;
            });

            //find all owned properties (SomeInt) and set the Owner to this
            PropertyHostMethods.InitializeProperties(this);

            SomeInt.Value++;
            Assert.IsTrue(_host == this);
        }
        [Test]
        public void Constructor_With_Owner_Test()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>(this);
            Assert.IsTrue(SomeInt.Owner == this);
        }
        [Test]
        public void IsDirty_NotDirty_Test()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>(this);
            var result = PropertyHostMethods.IsDirty(this);
            Assert.IsFalse(result);
        }
        [Test]
        public void IsDirty_IsDirty_Test()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>(this);
            SomeInt.Value += 1;
            var result = PropertyHostMethods.IsDirty(this);
            Assert.IsTrue(result);
        }
        [Test]
        public void MarkAsClean_Test()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>(this);
            SomeInt.Value += 1;
            var result = PropertyHostMethods.IsDirty(this);
            Assert.IsTrue(result);

            PropertyHostMethods.MarkAsClean(this);
            result = PropertyHostMethods.IsDirty(this);
            Assert.IsFalse(result);
        }
    }
}
