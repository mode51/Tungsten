using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace W.Tests.Tungsten
{
    [TestFixture]
    internal class PropertyHostTests
    {
        internal class Person : PropertyHost
        {
            public Property<Person, string> Last { get; } = new Property<Person, string>();
            public Property<Person, string> First { get; } = new Property<Person, string>();
        }

        [Test]
        public void Constructor_Test()
        {
            var p = new Person();
            Assert.IsTrue(p.Last.Owner == p);
        }
        [Test]
        public void IsDirty_False_Test()
        {
            var p = new Person();
            Assert.IsFalse(p.IsDirty);
        }
        [Test]
        public void IsDirty_True_Test()
        {
            var p = new Person();
            p.Last.Value = "Duerksen";
            p.First.Value = "Jordan";
            Assert.IsTrue(p.IsDirty);
        }
        [Test]
        public void MarkAsClean_Test()
        {
            var p = new Person();
            p.Last.Value = "Duerksen";
            p.First.Value = "Jordan";
            Assert.IsTrue(p.IsDirty);

            p.MarkAsClean();
            Assert.IsFalse(p.IsDirty);
        }
    }
}
