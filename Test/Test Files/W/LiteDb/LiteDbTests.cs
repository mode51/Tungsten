using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.LiteDb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class LiteDbTests
    {
        //private ITestOutputHelper output;
        //public LiteDbTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}

        public class Customer : W.LiteDb.LiteDbItem
        {
            public string Name { get; set; }
            public int? Age { get; set; }
            public DateTime? Birthday { get; set; }
        }
        private static string dbName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tungsten.LiteDb.Tests.db");

        [TestMethod]
        public void LiteDb_SaveCustomer()
        {
            var customer = new Customer() { Name = "Joe", Age = 46 };
            var result = W.LiteDb.LiteDbMethods.Save(dbName, customer);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result > 0);
        }
        [TestMethod]
        public void LiteDb_DoesCustomerExist()
        {
            LiteDb_SaveCustomer();
            var result = W.LiteDb.LiteDbMethods.FindOne<Customer>(dbName, "Name", "Joe");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result != null);
            Assert.IsTrue(result.Result.Name == "Joe");
            W.LiteDb.LiteDbMethods.Delete<Customer>(dbName, result.Result._id);
        }
        [TestMethod]
        public void LiteDb_DoesCustomerExist2()
        {
            LiteDb_SaveCustomer();
            var result = W.LiteDb.LiteDbMethods.FindOne<Customer>(dbName, c => c.Name == "Joe");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result != null);
            Assert.IsTrue(result.Result.Name == "Joe");
            W.LiteDb.LiteDbMethods.Delete<Customer>(dbName, result.Result._id);
        }
        [TestMethod]
        public void LiteDb_DeleteCustomer()
        {
            var customer = new Customer() { Name = "Jimmy"};
            var result = W.LiteDb.LiteDbMethods.Save(dbName, customer);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result > 0);

            var result2 = W.LiteDb.LiteDbMethods.Delete<Customer>(dbName, result.Result);
            Assert.IsTrue(result2.Success);
            Assert.IsTrue(result2.Result);
        }
        [TestMethod]
        public void LiteDb_DeleteCustomer2()
        {
            var customer = new Customer() { Name = "Timmy" };
            var result = W.LiteDb.LiteDbMethods.Save(dbName, customer);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result > 0);

            var result2 = W.LiteDb.LiteDbMethods.Delete<Customer>(dbName, c => c.Name == "Timmy");
            Assert.IsTrue(result2.Success);
            Assert.IsTrue(result2.Result > 0);
        }
        [TestMethod]
        public void LiteDb_DeleteCustomer3()
        {
            var customer = new Customer() { Name = "Timmy" };
            var result = W.LiteDb.LiteDbMethods.Save(dbName, customer);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result > 0);

            var result2 = W.LiteDb.LiteDbMethods.Delete<Customer>(dbName, "Name", "Timmy");
            Assert.IsTrue(result2.Success);
            Assert.IsTrue(result2.Result > 0);
        }
        [TestMethod]
        public void LiteDb_DropCustomerCollection()
        {
            var result = W.LiteDb.LiteDbMethods.Drop<Customer>(dbName);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result);
        }
    }
}
