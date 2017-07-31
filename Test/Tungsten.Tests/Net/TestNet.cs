using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using W;

namespace W.Tests.Net
{
    [TestFixture]
    public class TestNet
    {
        //[Test]
        //public void TestClientServer()
        //{
        //    var serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
        //    using (var server = new W.Net.Server<W.Net.Client<string>>())
        //    {
        //        server.ClientConnected += c =>
        //        {
        //            Console.WriteLine("Server: Client Connected = {0}", c.Socket.RemoteEndPoint.ToString());
        //        };
        //        server.ClientDisconnected += (c, ep, e) =>
        //        {
        //            Console.WriteLine("Server: Client Disconnected = {0}, Exception = {1}", c.Socket.RemoteEndPoint.ToString(), e?.ToString() ?? "null");
        //        };
        //        server.IsListeningChanged += isListening =>
        //        {
        //            Console.WriteLine("Server: IsListening = {0}", isListening);
        //        };
        //        server.Start(serverEndPoint);

        //        using (var client = new W.Net.Client<string>())
        //        {
        //        }

        //    }
        //}
        [Test]
        public void TestSecureStringClientLogger()
        {
            var received = 0;
            var numberOfMessagesToSend = 10;
            var mreQuit = new System.Threading.ManualResetEvent(false);


            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.2.12"), 2213);
            using (var server = new W.Net.SecureServer<W.Net.SecureClient<string>>())
            {
                server.ClientConnected += client =>
                {
                    client.MessageReceived += (c, m) =>
                    {
                        received += 1;
                        Console.WriteLine("Received Message " + received.ToString() + ": " + m);
                        if (received == numberOfMessagesToSend)
                            mreQuit.Set();
                    };
                    //client.ConnectionSecured += client2 =>
                    //{
                    //    client.As<W.Net.SecureStringClient>().MessageReceived += (o, message) =>
                    //    {
                    //        received += 1;
                    //        Console.WriteLine("Received Message " + received.ToString() + ": " + message);
                    //        if (received == numberOfMessagesToSend)
                    //            mreQuit.Set();
                    //    };
                    //};
                };
                server.Start(ipEndPoint.Address, ipEndPoint.Port);
                Console.WriteLine("Server Started");

                //To verify this method, an external server must be listening
                using (var logger = new W.Net.SecureStringClientLogger(ipEndPoint))
                {
                    var r = new Random();
                    for(int t=1; t<=numberOfMessagesToSend; t++)
                    {
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
                    }
                    if (!mreQuit.WaitOne(10000))
                        Console.WriteLine("Failed To Receive All Messages");
                    Console.WriteLine("Completed Logging");
                }
                Console.WriteLine("Complete");
            }
        }

        [Test]
        public void TestStringClientLogger()
        {
            var received = 0;
            var numberOfMessagesToSend = 10;
            var mreQuit = new System.Threading.ManualResetEvent(false);


            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.2.12"), 2213);
            using (var server = new W.Net.Server<W.Net.Client<string>>())
            {
                server.ClientConnected += client =>
                {
                    client.MessageReceived += (c, m) =>
                    {
                        received += 1;
                        Console.WriteLine("Received Message " + received.ToString() + ": " + m);
                        if (received == numberOfMessagesToSend)
                            mreQuit.Set();
                    };
                };
                server.Start(ipEndPoint.Address, ipEndPoint.Port);
                Console.WriteLine("Server Started");

                //To verify this method, an external server must be listening
                using (var logger = new W.Net.StringClientLogger(ipEndPoint))
                {
                    var r = new Random();
                    for (int t = 1; t <= numberOfMessagesToSend; t++)
                    {
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
                    }
                    if (!mreQuit.WaitOne(10000))
                        Console.WriteLine("Failed To Receive All Messages");
                    Console.WriteLine("Completed Logging");
                }
                Console.WriteLine("Complete");
            }
        }
    }
}
