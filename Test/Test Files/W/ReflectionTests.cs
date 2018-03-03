using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using W.Reflection;

namespace W.Tests
{
    [TestClass]
    public class ReflectionTests
    {
        public class CustomerAttribute : Attribute
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public CustomerAttribute(string name, int value)
            {
                Name = name;
                Value = value;
            }
        }
        [Customer("Jordan", 47)]
        public class Customer
        {
            public string NameField;
            public int AgeField;

            public string Name { get; set; }
            public int Age { get; set; }

            public Customer()
            {
            }
        }
        [TestMethod]
        public void GetAttributes()
        {
            var customer = new Customer();
            var ca = customer.get
        }
        [TestMethod]
        public void GetFields()
        {
        }
        [TestMethod]
        public void GetProperties()
        {
        }
    }
}
