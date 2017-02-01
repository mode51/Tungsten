using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace W.Tests
{
    /// <summary>
    /// This is a Control test.  It just verifies that Unit testing is working.
    /// </summary>
    [TestFixture]
    internal class _Control //this is the test to show that Testing is working
    {
        [Test]
        public void AssertTrue()
        {
            Assert.IsTrue(true);
        }
    }
    [TestFixture]
    internal class PropertyTests
    {
        public PropertyTests()
        {
            //this really is the first test
            PropertyHostMethods.InitializeProperties(this);
        }

        [Test]
        public void ShowFullName()
        {
            var asm = System.Reflection.Assembly.GetAssembly(typeof(W.IProperty));
            var name = asm.FullName;
            Console.WriteLine(name);
            Assert.IsTrue(true);
        }

        [Test]
        public void Construction_Test()
        {
            var someInt = new Property<int>();
            Assert.IsTrue(someInt.Value == 0);
        }
        [Test]
        public void DefaultValue_Test()
        {
            var someInt = new Property<int>(1);
            Assert.IsTrue(someInt.DefaultValue == 1);
            Assert.IsTrue(someInt.Value == 1);
        }
        [Test]
        public void Assignment_Test()
        {
            var someInt = new Property<int> {Value = 5};
            Assert.IsTrue(someInt.Value == 5);
        }
        [Test]
        public void PropertyChanged_Test()
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
        [Test]
        public void PropertyValueChanged_Test()
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
        [Test]
        public void OnValueChanged_Test()
        {
            var temp = 0;
            var someInt = new Property<int>((o, oldValue, newValue) =>
            {
                temp = 1;
            });
            someInt.Value = 5;
            Assert.IsTrue(temp == 1);
        }

        [Test]
        public void PropertyChangedSender_Test()
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
        [Test]
        public void PropertyValueChangedSender_Test()
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


        [Test]
        public void Owned_Construction_Test()
        {
            var someOwnedInt = new Property<PropertyTests, int>(this);
            Assert.IsTrue(someOwnedInt.Value == 0);
        }
        [Test]
        public void Owned_DefaultValut_Test()
        {
            var someOwnedInt = new Property<PropertyTests, int>(this, 3);
            Assert.IsTrue(someOwnedInt.DefaultValue == 3);
            Assert.IsTrue(someOwnedInt.Value == 3);
        }
        [Test]
        public void Owned_Assignment_Test()
        {
            var someOwnedInt = new Property<PropertyTests, int>(this);
            someOwnedInt.Value = 2;
            Assert.IsTrue(someOwnedInt.Value == 2);
        }
        [Test]
        public void Owned_PropertyChanged_Test()
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
        [Test]
        public void Owned_PropertyValueChanged_Test()
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
        [Test]
        public void Owned_OnValueChanged_Test()
        {
            var temp = 0;
            var someOwnedInt = new Property<PropertyTests, int>(this, (tests, oldValue, newValue) =>
            {
                temp = 1;
            });
            someOwnedInt.Value = 5;
            Assert.IsTrue(temp == 1);
        }
        [Test]
        public void Owned_PropertyChangedSender_Test()
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
        [Test]
        public void Owned_PropertyValueChangedSender_Test()
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
        [Test]
        public void Owned_SetOwner_Test()
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

        [Test]
        public void IsDirty_On_Initialized_With_DefaultValue__Test()
        {
            var someInt = new Property<int>(5);
            Assert.IsFalse(someInt.IsDirty);
        }
        [Test]
        public void IsDirty_On_Object_Initializer__Test()
        {
            var someInt = new Property<int>() { Value = 5 };
            Assert.IsTrue(someInt.IsDirty);
        }
        [Test]
        public void IsDirty_On_LoadValue__Test()
        {
            var someInt = new Property<int>();
            someInt.LoadValue(5);
            Assert.IsFalse(someInt.IsDirty);
        }
        [Test]
        public void IsDirty_On_Assignment__Test()
        {
            var someInt = new Property<int>();
            someInt.Value = 5;
            Assert.IsTrue(someInt.IsDirty);
        }
    }
}
