using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using W.LiteDb;

namespace W.Tests.Tungsten.LiteDb
{
    [TestFixture]
    internal class LiteDbTests
    {
        public class Customer : W.LiteDb.LiteDbItem
        {
            public string Name { get; set; }
            public int? Age { get; set; }
            public DateTime? Birthday { get; set; }
        }
        private static string dbName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tungsten.LiteDb.Tests.db");

        [Test]
        public void SaveCustomer()
        {
            var customer = new Customer() { Name = "Joe", Age = 46 };
            var result = W.LiteDb.LiteDbMethods.Save(dbName, customer);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result > 0);
        }
        [Test]
        public void DoesCustomerExist()
        {
            SaveCustomer();
            var result = W.LiteDb.LiteDbMethods.FindOne<Customer>(dbName, "Name", "Joe");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result != null);
            Assert.IsTrue(result.Result.Name == "Joe");
            W.LiteDb.LiteDbMethods.Delete<Customer>(dbName, result.Result._id);
        }
        [Test]
        public void DoesCustomerExist2()
        {
            SaveCustomer();
            var result = W.LiteDb.LiteDbMethods.FindOne<Customer>(dbName, c => c.Name == "Joe");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result != null);
            Assert.IsTrue(result.Result.Name == "Joe");
            W.LiteDb.LiteDbMethods.Delete<Customer>(dbName, result.Result._id);
        }
        [Test]
        public void DeleteCustomer()
        {
            var customer = new Customer() { Name = "Jimmy"};
            var result = W.LiteDb.LiteDbMethods.Save(dbName, customer);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result > 0);

            var result2 = W.LiteDb.LiteDbMethods.Delete<Customer>(dbName, result.Result);
            Assert.IsTrue(result2.Success);
            Assert.IsTrue(result2.Result);
        }
        [Test]
        public void DeleteCustomer2()
        {
            var customer = new Customer() { Name = "Timmy" };
            var result = W.LiteDb.LiteDbMethods.Save(dbName, customer);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result > 0);

            var result2 = W.LiteDb.LiteDbMethods.Delete<Customer>(dbName, c => c.Name == "Timmy");
            Assert.IsTrue(result2.Success);
            Assert.IsTrue(result2.Result > 0);
        }
        [Test]
        public void DeleteCustomer3()
        {
            var customer = new Customer() { Name = "Timmy" };
            var result = W.LiteDb.LiteDbMethods.Save(dbName, customer);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result > 0);

            var result2 = W.LiteDb.LiteDbMethods.Delete<Customer>(dbName, "Name", "Timmy");
            Assert.IsTrue(result2.Success);
            Assert.IsTrue(result2.Result > 0);
        }
        [Test]
        public void DropCustomerCollection()
        {
            var result = W.LiteDb.LiteDbMethods.Drop<Customer>(dbName);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result);
        }
    }
}
