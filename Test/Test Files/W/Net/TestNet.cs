using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using W;
using W.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace W.Tests
{
    [TestClass]
    public class TcpTests
    {
        private static IPEndPoint ServerEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2215);
        private static ManualResetEventSlim _mreContinue = new ManualResetEventSlim(false);
        private void LogMessages(int count)
        {
            var r = new Random();
            for (int t = 1; t <= count; t++)
            {
                _mreContinue.Reset();
                var msg = "Test Log Message: " + t.ToString();
                switch (r.Next(0, 4))
                {
                    case 0:
                        W.Logging.Log.e(msg);
                        break;
                    case 1:
                        W.Logging.Log.w(msg);
                        break;
                    case 2:
                        W.Logging.Log.i(msg);
                        break;
                    case 3:
                        W.Logging.Log.v(msg);
                        break;
                }
                Assert.IsTrue(_mreContinue.Wait(1000), $"Failed on {t}");
            }
        }

        private void LogTheMessage_Tcp(string message)
        {
            W.Net.Tcp.TcpLogger.LogTheMessage(ServerEndPoint, message);
        }
        private void ReceiveBytes(Tcp.TcpHost host, Tcp.IClient server, byte[] bytes)
        {
            _mreContinue.Set();
        }
        private void LogTheMessage_SecureTcp(string message)
        {
            W.Net.Tcp.SecureTcpLogger.LogTheMessage(ServerEndPoint, 2048, message);
        }

        [TestMethod]
        public void TcpLogger()
        {
            var numberOfMessagesToSend = 10;

            using (var server = new Tcp.TcpHost())
            {
                Console.WriteLine("Host Created");
                server.BytesReceived += ReceiveBytes;
                server.Listen(ServerEndPoint, 20);
                Console.WriteLine("Host Started");

                W.Logging.Log.LogTheMessage += LogTheMessage_Tcp;
                LogMessages(numberOfMessagesToSend);
                W.Logging.Log.LogTheMessage -= LogTheMessage_Tcp;

                Console.WriteLine("Complete");
            }
            Console.WriteLine("Host Disposed");
        }
        [TestMethod]
        public void SecureTcpLogger()
        {
            var numberOfMessagesToSend = 10;

            using (var server = new Tcp.SecureTcpHost(2048))
            {
                Console.WriteLine("Secure Host Created");
                server.BytesReceived += ReceiveBytes;
                server.Listen(ServerEndPoint, 20);
                Console.WriteLine("Secure Host Started");

                W.Logging.Log.LogTheMessage += LogTheMessage_SecureTcp;
                LogMessages(numberOfMessagesToSend);
                W.Logging.Log.LogTheMessage -= LogTheMessage_SecureTcp;

                Console.WriteLine("Complete");
            }
            Console.WriteLine("Secure Host Disposed");
        }
        [TestMethod]
        public void CreateClient()
        {
            var client = new Tcp.TcpClient();
            client.Connected += c => { };
            client.Disconnected += c => { };
            client.BytesReceived += (c, b) => { };
            client.Dispose();
        }
        [TestMethod]
        public void CreateServer()
        {
            var server = new Tcp.TcpHost();
            server.BytesReceived += (h, s, b) => { };
            server.Dispose();
        }
        [TestMethod]
        public void EchoOnce()
        {
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var hostEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            var latin = System.IO.File.ReadAllText(@"C:\Source\Repos\Tungsten\Test\Test Files\4500_Latin_Words.txt");
            var latinBytes = latin.AsBytes();
            using (var host = new Tcp.TcpHost())
            {
                host.BytesReceived += (h, s, bytes) =>
                {
                    Console.WriteLine("Server Echoing {0} bytes", bytes.Length);
                    //echo
                    s.Write(bytes);
                };

                host.Listen(hostEndPoint, 20);

                using (var client = new Tcp.TcpClient())
                {
                    client.BytesReceived += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received {0} bytes", bytes.Length);
                        Assert.IsTrue(bytes.SequenceEqual(latinBytes));
                        mreContinue.Set();
                    };
                    client.Connect(hostEndPoint);
                    client.Write(latinBytes);
                    Assert.IsTrue(mreContinue.Wait(1000));
                }
            }
        }
        [TestMethod]
        public void EchoLatin()
        {
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var hostEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            var latin = System.IO.File.ReadAllText(@"C:\Source\Repos\Tungsten\Test\Test Files\4500_Latin_Words.txt");
            var latinBytes = latin.AsBytes();
            using (var host = new Tcp.TcpHost())
            {
                //host.ServerConnected += (h, s) =>
                //{
                //    Console.WriteLine("Server: Client Connected = {0}", s.Socket.RemoteEndPoint.ToString());
                //};
                //host.ServerDisconnected += (h, s) =>
                //{
                //    Console.WriteLine("Server: Client Disconnected = {0}", s.Socket.RemoteEndPoint.ToString());
                //};
                host.BytesReceived += (h, s, bytes) =>
                {
                    Console.WriteLine("Server Echoing {0} bytes", bytes.Length);
                    //echo
                    s.Write(bytes);
                };

                host.Listen(hostEndPoint, 20);

                using (var client = new Tcp.TcpClient())
                {
                    client.BytesReceived += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received {0} bytes", bytes.Length);
                        Assert.IsTrue(bytes.SequenceEqual(latinBytes));
                        mreContinue.Set();
                    };
                    client.Connect(hostEndPoint);
                    client.Write(latinBytes);
                    Assert.IsTrue(mreContinue.Wait(1000));
                }
            }
        }
    }
}
