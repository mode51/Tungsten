using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.Domains;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class Domains
    {
        //private ITestOutputHelper output;
        //public Domains(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}
        [TestMethod]
        public void Domains_Create()
        {
            var domain = new DomainLoader("API", false);
            domain.Load();
            domain.Unload();
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void Domains_CreateWithShadowCopy()
        {
            var domain = new DomainLoader("API", true);
            domain.Load();
            Assert.IsTrue(System.IO.Directory.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "API\\cache")), "API\\cache failed to create");
            domain.Unload();
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void Domains_CallDoCallback()
        {
            var domain = new DomainLoader("API", true);
            domain.Load();
            domain.DoCallback(() =>
            {
                //this method executes in the other domain
                var al = AppDomain.CurrentDomain.GetData("AssemblyLoader");
                Assert.IsTrue(al != null);
            });
            domain.Unload();
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void Domains_CheckDomainName()
        {
            var domain = new DomainLoader("TestDomain", "API", true);
            domain.Load();
            domain.DoCallback(() =>
            {
                //this method executes in the other domain
                Assert.IsTrue(AppDomain.CurrentDomain.FriendlyName == "TestDomain");
            });
            domain.Unload();
            Assert.IsTrue(true);
        }
    }
}
