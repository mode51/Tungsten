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
    public class PropertyHostMethodsTests
    {
        //private ITestOutputHelper output;
        //public PropertyHostMethodsTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}

        private static PropertyHostMethodsTests _host = null;
        private Property<PropertyHostMethodsTests, int> SomeInt { get; set; }

        [TestMethod]
        public void PropertyHostMethods_InitializeProperties()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>();// (owner, oldValue, newValue) =>
            //{
            //    _host = owner as PropertyHostMethodsTests;
            //});
            SomeInt.ValueChanged += (owner, oldValue, newValue) =>
            {
                _host = owner as PropertyHostMethodsTests;
            };

            //find all owned properties (SomeInt) and set the Owner to this
            this.InitializeProperties();

            SomeInt.Value++;
            Assert.IsTrue(_host == this);
        }
        [TestMethod]
        public void PropertyHostMethods_Constructor_With_Owner()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>(this);
            Assert.IsTrue(SomeInt.Owner == this);
        }
        [TestMethod]
        public void PropertyHostMethods_IsDirty_NotDirty()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>(this);
            var result = this.IsDirty();
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void PropertyHostMethods_IsDirty_IsDirty()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>(this);
            SomeInt.Value += 1;
            var result = this.IsDirty();
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void PropertyHostMethods_MarkAsClean()
        {
            SomeInt = new Property<PropertyHostMethodsTests, int>(this);
            SomeInt.Value += 1;
            var result = this.IsDirty();
            Assert.IsTrue(result);

            this.MarkAsClean();
            result = this.IsDirty();
            Assert.IsFalse(result);
        }
    }
}
