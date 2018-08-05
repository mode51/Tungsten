using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using W;

namespace W.Tests
{
    [TestClass]
    public class PropertyHostTests
    {
        //private ITestOutputHelper output;
        //public PropertyHostTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}

        public class Person : W.PropertyHost
        {
            public Property<Person, string> Last { get; } = new Property<Person, string>();
            public Property<Person, string> First { get; } = new Property<Person, string>();
            public Person() : this("", "") { }
            public Person(string last) : this(last, "") { }
            public Person(string last, string first) : base()
            {
                Last.LoadValue(last);
                First.LoadValue(first);
            }
        }

        [TestMethod]
        public void PropertyHost_Constructor()
        {
            var p = new Person();
            Assert.IsTrue(p.Last.Owner == p);
        }
        [TestMethod]
        public void PropertyHost_IsDirty_False()
        {
            var p = new Person();
            Assert.IsFalse(p.IsDirty);
        }
        [TestMethod]
        public void PropertyHost_IsDirty_True()
        {
            var p = new Person();
            p.Last.Value = "Duerksen";
            p.First.Value = "Jordan";
            Assert.IsTrue(p.IsDirty);
        }
        [TestMethod]
        public void PropertyHost_MarkAsClean()
        {
            var p = new Person();
            p.Last.Value = "Duerksen";
            p.First.Value = "Jordan";
            Assert.IsTrue(p.IsDirty);

            p.MarkAsClean();
            Assert.IsFalse(p.IsDirty);
        }
        [TestMethod]
        public void PropertyHost_IsDirtyFlag()
        {
            var p = new Person("Duerk", "");
            p.Last.Value = "Duerksen";
            Assert.IsTrue(p.IsDirty, "IsDirty should be true");
            Assert.IsTrue(p.IsDirtyFlag.Value, "IsDirtyFlag.Value should be true");
            p.MarkAsClean();
            Assert.IsFalse(p.IsDirtyFlag.Value, "IsDirtyFlag.Value should be false");
        }
        [TestMethod]
        public void PropertyHost_ForcePropertyChanged()
        {
            var p = new Person("Duerk", "");
            p.Last.PropertyChanged += (o, property) =>
            {
                Assert.IsTrue(property.PropertyName == "Value");
            };
            p.Last.LoadValue("Duerksen");
            p.ForcePropertyChanged();
        }
    }
}
