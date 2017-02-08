using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using W.Domains;
using W.SampleAPI;

namespace W.Tests.Tungsten
{
    [TestFixture]
    internal class RPC
    {
        public RPC()
        {
            //ensure the SampleAPI dll is loaded into the AppDomain
            W.SampleAPI.Sample.Initialize();
        }
        [Test]
        public void Create()
        {
            System.Threading.Thread.Sleep(1000); //just allow other Tests to cleanup
            var server = new W.RPC.Server();
            Assert.IsTrue(server != null);
        }
        [Test]
        public void StartAndStop()
        {
            System.Threading.Thread.Sleep(1000); //just allow other Tests to cleanup
            var server = new W.RPC.Server();
            server.Start(IPAddress.Parse("127.0.0.1"), 5150);
            server.Stop();
            Assert.IsTrue(true);
        }
        [Test]
        public void Dispose()
        {
            System.Threading.Thread.Sleep(1000); //just allow other Tests to cleanup
            using (var server = new W.RPC.Server(IPAddress.Parse("127.0.0.1"), 5150))
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void ClientConnection()
        {
            System.Threading.Thread.Sleep(1000); //just allow other Tests to cleanup
            using (var server = new W.RPC.Server(IPAddress.Parse("127.0.0.1"), 5150))
            {
                using (var client = new W.RPC.Client("127.0.0.1", 5150))
                {
                    client.Connected += (client1, address) =>
                    {
                        Assert.IsTrue(client.IsConnected);
                    };
                    client.Disconnected += (client1, exception) =>
                    {
                        Assert.IsTrue(!client.IsConnected);
                        Assert.IsNull(exception);
                    };
                }
            }
        }

        private bool ClientCode1(ManualResetEvent isCompleteMre)
        {
            var result = new Lockable<bool>();
            var client = new W.RPC.Client();
            client.Connected += (c, address) =>
            {
                Assert.IsTrue(c.IsConnected);
                var mre = c.MakeRPCCall<string>("W.SampleAPI.Sample.GetMessageEx", r =>
                {
                    Console.WriteLine("GetMessageEx = " + r);
                    result.Value = !string.IsNullOrEmpty(r);
                    c.Disconnect();
                });
                if (!mre.WaitOne(5000))
                    Console.WriteLine("The call to MakeRPCCall timed out");
            };
            client.Disconnected += (c, exception) =>
            {
                isCompleteMre.Set();
            };
            client.Connect("127.0.0.1", 5150);

            if (!isCompleteMre.WaitOne(30000))
                Console.WriteLine("Timed out!");
            client.Dispose();

            return result.Value;
        }

        private bool ClientCode2(ManualResetEvent isCompleteMre)
        {
            var result = new Lockable<bool>();
            Console.WriteLine("Entering client code");
            using (var client = new W.RPC.Client("127.0.0.1", 5151))
            {
                Console.WriteLine("In client code");
                //if we're here, we're connected
                var mre = client.MakeRPCCall<string>("W.SampleAPI.Sample.GetMessageEx", r =>
                {
                    Console.WriteLine("GetMessageEx = " + r);
                    result.Value = !string.IsNullOrEmpty(r);
                });
                if (!mre.WaitOne(10000))
                    Console.WriteLine("The call to the server timed out");
                Console.WriteLine("Leaving client code");
            }
            Console.WriteLine("Setting Event");
            isCompleteMre.Set();
            return result.Value;
        }
        [Test]
        public void MakeRPCCall_GetMessageEx()
        {
            //System.Threading.Thread.Sleep(5000); // allow other Tests to cleanup
            var result = new Lockable<string>();
            var isCompleteMre = new ManualResetEvent(false);
            var server = new W.RPC.Server(IPAddress.Parse("127.0.0.1"), 5150);
            var client = new W.RPC.Client();
            client.Connected += (c, address) =>
            {
                Assert.IsTrue(c.IsConnected);
                var mre = c.MakeRPCCall<string>("W.SampleAPI.Sample.GetMessageEx", r =>
                {
                    Console.WriteLine("GetMessageEx = " + r);
                    result.Value = r;
                    c.Disconnect();
                });
                //if (!mre.WaitOne(5000))
                //    Assert.Fail("The call to the server timed out");
            };
            client.Disconnected += (c, exception) =>
            {
                isCompleteMre.Set();
            };
            client.Connect("127.0.0.1", 5150);

            if (!isCompleteMre.WaitOne(30000))
                Console.WriteLine("Timed out!");
            client.Dispose();
            server.Dispose();

            Assert.IsTrue(!string.IsNullOrEmpty(result.Value));
        }
        [Test]
        public void MakeRPCCall_GetMessageEx2()
        {
            //System.Threading.Thread.Sleep(5000); // allow other Tests to cleanup
            var result = new Lockable<bool>();
            var isCompleteMre = new ManualResetEvent(false);
            using (var server = new W.RPC.Server(IPAddress.Parse("127.0.0.1"), 5151))
            {
                try
                {
                    //Task.Run(() =>
                    //{
                        result.Value = ClientCode2(isCompleteMre);
                    //});
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                    server.Dispose();
                }
                if (!isCompleteMre.WaitOne(30000))
                    Console.WriteLine("Client code timed out");
            }
            Assert.IsTrue(result.Value);
        }
        [Test]
        public void MakeRPCCall_Echo()
        {
            var result = new Lockable<bool>();
            using (var server = new W.RPC.Server(IPAddress.Parse("127.0.0.1"), 5152))
            {
                Console.WriteLine("Entering client code");
                using (var client = new W.RPC.Client("127.0.0.1", 5152))
                {
                    Console.WriteLine("In client code");
                    var mre = client.MakeRPCCall<SampleResult>("W.SampleAPI.Sample.Echo", r =>
                    {
                        //System.Threading.Thread.Sleep(5000);
                        if (r != null)
                        {
                            Console.WriteLine("Success={0}, Result={1}, Exception={2}", r.Success, r.Result, r.Exception?.Message);
                            result.Value = !string.IsNullOrEmpty((string)r.Result);
                        }
                        else
                            Console.WriteLine("Result from MakeRPCall is null");
                    }, "some input");
                    if (!mre.WaitOne(3000))
                        Console.WriteLine("The call to the server timed out");
                    Console.WriteLine("Leaving client code");
                };
                Console.WriteLine("After client code");
            }
            Assert.IsTrue(result.Value);
        }
        [Test]
        public void MakeRPCCall_Echo_AsFail()
        {
            var result = new Lockable<bool>(true);
            using (var server = new W.RPC.Server(IPAddress.Parse("127.0.0.1"), 5152))
            {
                Console.WriteLine("Entering client code");
                using (var client = new W.RPC.Client("127.0.0.1", 5152))
                {
                    Console.WriteLine("In client code");
                    var mre = client.MakeRPCCall<SampleResult>("W.SampleAPI.Sample.Echo", r =>
                    {
                        System.Threading.Thread.Sleep(5000);
                        if (r != null)
                        {
                            Console.WriteLine("Success={0}, Result={1}, Exception={2}", r.Success, r.Result, r.Exception?.Message);
                            result.Value = !string.IsNullOrEmpty((string)r.Result);
                        }
                        else
                            Console.WriteLine("Result from MakeRPCall is null");
                    }, "some input");
                    if (!mre.WaitOne(3000))
                    {
                        Console.WriteLine("The call to the server timed out");
                        result.Value = false;
                    }
                    Console.WriteLine("Leaving client code");
                };
                Console.WriteLine("After client code");
            }
            Assert.IsFalse(result.Value);
        }
    }
}
