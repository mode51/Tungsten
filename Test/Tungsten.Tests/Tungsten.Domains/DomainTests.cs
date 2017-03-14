using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using W.Domains;

namespace W.Tests.Tungsten
{
    [TestFixture()]
    internal class Domains
    {
        [Test]
        public void Create()
        {
            var domain = new DomainLoader("API", false);
            domain.Load();
            domain.Unload();
            Assert.IsTrue(true);
        }
        [Test]
        public void CreateWithShadowCopy()
        {
            var domain = new DomainLoader("API", true);
            domain.Load();
            Assert.IsTrue(System.IO.Directory.Exists("API\\cache"));
            domain.Unload();
            Assert.IsTrue(true);
        }
        [Test]
        public void CallDoCallback()
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
        [Test]
        public void CheckDomainName()
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
