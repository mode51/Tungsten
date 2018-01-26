using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using W.AsExtensions;
using W;
using W.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class SecureTcpTests
    {
        public class SecureMessage
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string Message { get; set; }
            public override string ToString() { return $"{Id}: {Message}"; }
        }
        [TestMethod]
        public void CreateSecureTcpClient()
        {
            var client = new Tcp.Generic.SecureTcpClient<SecureMessage>(2048);
            client.Connected.OnRaised += c => { };
            client.Disconnected.OnRaised += c => { };
            client.MessageReceived.OnRaised += (c, m) => { };
            client.Dispose();
        }
        [TestMethod]
        public void CreateGenericSecureTcpHost()
        {
            var server = new Tcp.Generic.SecureTcpHost<SecureMessage>(2048);
            server.MessageReceived.OnRaised += (h, s, m) => { };
            server.Dispose();
        }
        [TestMethod]
        public void SendLatinSecure()
        {
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var hostEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            var latin = System.IO.File.ReadAllText(@"C:\Source\Repos\Tungsten\Test\Test Files\4500_Latin_Words.txt");
            using (var host = new Tcp.Generic.SecureTcpHost<SecureMessage>(2048))
            {
                host.MessageReceived.OnRaised += (h, s, message) =>
                {
                    Console.WriteLine($"Server Echoing {message.Message.Length} bytes");
                    //echo
                    s.Write(message);
                };

                host.Listen(hostEndPoint, 1);

                using (var client = new Tcp.Generic.SecureTcpClient<SecureMessage>(2048))
                {
                    client.MessageReceived.OnRaised += (c, message) =>
                    {
                        Console.WriteLine("Client Received {0} bytes", message.Message.Length);
                        Assert.IsTrue(message.Message == latin);
                        mreContinue.Set();
                    };
                    client.Connect(hostEndPoint);
                    client.Write(new SecureMessage() { Message = latin });
                    Assert.IsTrue(mreContinue.Wait(15000));
                }
            }
        }
    }
}
