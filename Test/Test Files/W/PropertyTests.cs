using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using W;

namespace W.Tests
{
    /// <summary>
    /// This is a Control test.  It just verifies that Unit testing is working.
    /// </summary>
    
    public class _Control //this is the test to show that Testing is working
    {
        [TestMethod]
        public void AssertTrue()
        {
            Assert.IsTrue(true);
        }
    }

    [TestClass]
    public class PropertyTests
    {
        //private ITestOutputHelper output;
        //public PropertyTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //    this.InitializeProperties();
        //}
        public PropertyTests()
        {
            //this really is the first test
            this.InitializeProperties();
        }

        [TestMethod]
        public void Property_ShowFullName()
        {
            //var asm = System.Reflection.Assembly.GetAssembly(typeof(W.IProperty));
            //var name = asm.FullName;
            var name = this.GetType().FullName;
            Console.WriteLine(name);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Property_Construction()
        {
            var someInt = new Property<int>();
            Assert.IsTrue(someInt.Value == 0);
        }
        [TestMethod]
        public void Property_DefaultValue()
        {
            var someInt = new Property<int>(1);
            Assert.IsTrue(someInt.DefaultValue == 1);
            Assert.IsTrue(someInt.Value == 1);
        }
        [TestMethod]
        public void Property_Assignment()
        {
            var someInt = new Property<int> {Value = 5};
            Assert.IsTrue(someInt.Value == 5);
        }
        [TestMethod]
        public void Property_PropertyChanged()
        {
            var temp = 0;
            var someInt = new Property<int>();
            someInt.PropertyChanged += (sender, propertyName) =>
            {
                temp = 1;
            };
            someInt.Value = 5;
            Assert.IsTrue(temp == 1);
        }
        [TestMethod]
        public void Property_PropertyValueChanged()
        {
            var temp = 0;
            var someInt = new Property<int>();
            someInt.ValueChanged += (sender, oldValue, newValue) =>
            {
                temp = 1;
            };
            someInt.Value = 5;
            Assert.IsTrue(temp == 1);
        }
        [TestMethod]
        public void Property_OnValueChanged()
        {
            var temp = 0;
            var someInt = new Property<int>();
            someInt.ValueChanged += (owner, oldValue, newValue) =>
            {
                temp = 1;
            };
            someInt.Value = 5;
            Assert.IsTrue(temp == 1);
        }

        [TestMethod]
        public void Property_PropertyChangedSender()
        {
            Property<int> temp = null;
            var someInt = new Property<int>();
            someInt.PropertyChanged += (sender, propertyName) =>
            {
                Console.WriteLine("temp assigning to sender");
                temp = sender as Property<int>;
                Console.WriteLine("temp assigned to sender");
            };
            someInt.Value = 5;
            Assert.IsTrue(temp == someInt);
        }
        [TestMethod]
        public void Property_PropertyValueChangedSender()
        {
            Property<int> temp = null;
            var someInt = new Property<int>();
            someInt.ValueChanged += (sender, oldValue, newValue) =>
            {
                temp = sender as Property<int>;
            };
            someInt.Value = 5;
            Assert.IsTrue(temp == someInt);
        }


        [TestMethod]
        public void Property_Owned_Construction()
        {
            var someOwnedInt = new Property<PropertyTests, int>(this);
            Assert.IsTrue(someOwnedInt.Value == 0);
        }
        [TestMethod]
        public void Property_Owned_DefaultValut()
        {
            var someOwnedInt = new Property<PropertyTests, int>(this, 3);
            Assert.IsTrue(someOwnedInt.DefaultValue == 3);
            Assert.IsTrue(someOwnedInt.Value == 3);
        }
        [TestMethod]
        public void Owned_Assignment_Test()
        {
            var someOwnedInt = new Property<PropertyTests, int>(this);
            someOwnedInt.Value = 2;
            Assert.IsTrue(someOwnedInt.Value == 2);
        }
        [TestMethod]
        public void Property_Owned_PropertyChanged()
        {
            var temp = 0;
            var someOwnedInt = new Property<PropertyTests, int>(this);
            someOwnedInt.PropertyChanged += (sender, propertyName) =>
            {
                temp = 1;
            };
            someOwnedInt.Value = 5;
            Assert.IsTrue(temp == 1);
        }
        [TestMethod]
        public void Property_Owned_PropertyValueChanged()
        {
            var temp = 0;
            var someOwnedInt = new Property<PropertyTests, int>(this);
            someOwnedInt.ValueChanged += (sender, oldValue, newValue) =>
            {
                temp = 1;
            };
            someOwnedInt.Value = 5;
            Assert.IsTrue(temp == 1);
        }
        [TestMethod]
        public void Property_Owned_OnValueChanged()
        {
            var temp = 0;
            var someOwnedInt = new Property<PropertyTests, int>(this);
            someOwnedInt.ValueChanged += (tests, oldValue, newValue) =>
            {
                temp = 1;
            };
            someOwnedInt.Value = 5;
            Assert.IsTrue(temp == 1);
        }
        [TestMethod]
        public void Property_Owned_PropertyChangedSender()
        {
            PropertyTests temp = null;
            var someOwnedInt = new Property<PropertyTests, int>(this);
            someOwnedInt.PropertyChanged += (sender, propertyName) =>
            {
                //the sender of PropertyChanged should always be the class having the property
                temp = sender as PropertyTests;
            };
            someOwnedInt.Value = 5;
            Assert.IsTrue(temp == this);
        }
        [TestMethod]
        public void Property_Owned_PropertyValueChangedSender()
        {
            PropertyTests temp = null;
            var someOwnedInt = new Property<PropertyTests, int>(this);
            someOwnedInt.ValueChanged += (sender, oldValue, newValue) =>
            {
                //the sender of PropertyValueChanged should always be the class passed in the constructor or in SetOwner
                temp = sender as PropertyTests;
            };
            someOwnedInt.Value = 5;
            Assert.IsTrue(temp == this);
        }
        [TestMethod]
        public void Property_Owned_SetOwner()
        {
            PropertyTests temp = null;
            var someOwnedInt = new Property<PropertyTests, int>();
            ((IOwnedProperty) someOwnedInt).SetOwner(this);
            someOwnedInt.ValueChanged += (sender, oldValue, newValue) =>
            {
                //the sender of PropertyValueChanged should always be the class passed in the constructor or in SetOwner
                temp = sender as PropertyTests;
            };
            someOwnedInt.Value = 5;
            Assert.IsTrue(temp == this);
        }

        [TestMethod]
        public void Property_IsDirty_On_Initialized_With_DefaultValue()
        {
            var someInt = new Property<int>(5);
            Assert.IsFalse(someInt.IsDirty);
        }
        [TestMethod]
        public void Property_IsDirty_On_Object_Initializer()
        {
            var someInt = new Property<int>() { Value = 5 };
            Assert.IsTrue(someInt.IsDirty);
        }
        [TestMethod]
        public void Property_IsDirty_On_LoadValue()
        {
            var someInt = new Property<int>();
            someInt.LoadValue(5);
            Assert.IsFalse(someInt.IsDirty);
        }
        [TestMethod]
        public void Property_IsDirty_On_Assignment()
        {
            var someInt = new Property<int>();
            someInt.Value = 5;
            Assert.IsTrue(someInt.IsDirty);
        }
    }
}
