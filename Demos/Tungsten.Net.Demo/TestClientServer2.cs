using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace W.Demo
{
    public class TestClientServer2
    {
        public static void Run()
        {
            var mre = new ManualResetEvent(false);
            using (var server = new W.Net.Server<W.Net.Client<string>>())
            {
                server.ClientConnected += client =>
                {
                    Console.WriteLine("Server Connected To Client: " + client.Socket.Name);
                    client.MessageReceived += (c, message) =>
                    {
                        Console.WriteLine("Server Echo: " + message);
                        c.Send(message.As<string>().ToUpper());
                    };
                };
                server.ClientDisconnected += (client, remoteEndPoint, exception) =>
                {
                    if (exception != null)
                        Console.WriteLine("Server Disconnected: " + client.Socket.Name + " - " + exception.Message);
                    else
                        Console.WriteLine("Server Disconnected: " + client.Socket.Name);

                    Console.WriteLine("Server Disconnected From: " + remoteEndPoint?.ToString());
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var client = new W.Net.Client<string>())
                {
                    client.Connected += (c, remoteEndPoint) =>
                    {
                        Console.WriteLine("Client Connected: " + c.Socket.Name);
                    };
                    client.Connected += (c, ep) =>
                    {
                        mre.Set();
                    };
                    client.Disconnected += (c, remoteEndPoint, exception) =>
                    {
                        if (exception != null)
                            Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString() + " - " + exception.Message);
                        else
                            Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString());
                        mre.Set();
                    };
                    client.RawDataReceived += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received Raw: " + bytes?.AsString() ?? "null");
                    };
                    client.MessageReceived += (c, message) =>
                    {
                        Console.WriteLine("Client Received: " + message);
                        Console.Write("Send <Return To Disconnect>: ");
                    };
                    //client.MessageSent += (c, m) =>
                    //{
                    //    Console.WriteLine("Client Message Sent");
                    //};

                    client.Socket.ConnectAsync(IPAddress.Parse("127.0.0.1"), 5150).Wait();
                    mre.WaitOne();

                    Console.Write("Send <Return To Disconnect>: ");
                    while (client.Socket.IsConnected)
                    {
                        var message = Console.ReadLine();
                        if (string.IsNullOrEmpty(message))
                            break;
                        client.Send(message);
                    }
                }
            }
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }
    }
    public class TestSecureClientServer2
    {
        public static void Run()
        {
            var mre = new ManualResetEvent(false);
            using (var server = new W.Net.SecureServer<W.Net.SecureClient<string>>())
            {
                server.ClientConnected += client =>
                {
                    Console.WriteLine("Server Connected To Client: " + client.Socket.Name);
                    client.DataReceived += (s, message) =>
                    {
                        Console.WriteLine("Server Received Data: " + message?.AsString() ?? "null");
                    };
                    client.MessageReceived += (c, message) =>
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            Console.WriteLine("Server Echo: " + message);
                            c.Send(message.As<string>().ToUpper());
                        }
                    };
                };
                server.ClientDisconnected += (client, remoteEndPoint, exception) =>
                {
                    if (exception != null)
                        Console.WriteLine("Server Disconnected: " + client.Socket.Name + " - " + exception.Message);
                    else
                        Console.WriteLine("Server Disconnected: " + client.Socket.Name);

                    Console.WriteLine("Server Disconnected From: " + remoteEndPoint?.ToString());
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var client = new W.Net.SecureClient<string>())
                {
                    client.Connected += (c, remoteEndPoint) =>
                    {
                        Console.WriteLine("Client Connected: " + c.Socket.Name);
                    };
                    client.Connected += (ssc, ep) =>
                    {
                        mre.Set();
                    };
                    client.Disconnected += (c, remoteEndPoint, exception) =>
                    {
                        if (exception != null)
                            Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString() + " - " + exception.Message);
                        else
                            Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString());
                        mre.Set();
                    };
                    client.DataReceived += (s, message) =>
                    {
                        Console.WriteLine("Client Received Data: " + message.AsString());
                        //Console.Write("Send <Return To Disconnect>: ");
                    };
                    client.MessageReceived += (c, message) =>
                    {
                        Console.WriteLine("Client Received Message: " + message);
                        Console.Write("Send <Return To Disconnect>: ");
                    };
                    //client.DataSent += (s, m) =>
                    //{
                    //    Console.WriteLine("Client Message Sent");
                    //};

                    client.Socket.ConnectAsync(IPAddress.Parse("127.0.0.1"), 5150).Wait();
                    mre.WaitOne();

                    Console.Write("Send <Return To Disconnect>: ");
                    while (client.Socket.IsConnected)
                    {
                        var message = Console.ReadLine();
                        if (string.IsNullOrEmpty(message))
                            break;
                        client.Send(message);
                    }
                }
            }
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }
    }
    public class TestSecureClientServer3
    {
        public static void Run()
        {
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);

            var server = new W.Net.SecureEchoServer();
            server.Start(localEndPoint);

            for(int t=0; t<5; t++)
            {
                var mre = new ManualResetEventSlim(false);
                var client = new W.Net.SecureClient<string>();
                client.MessageReceived += (c, message) =>
                {
                    Console.WriteLine("Received {0}", message);
                    mre.Set();
                };
                bool connected = client.Socket.ConnectAsync(localEndPoint.Address, localEndPoint.Port).Result;
                if (connected)
                {
                    client.Send(string.Format("Message: {0}", t));
                    if (!mre.Wait(30000))
                        Console.WriteLine("Waiting for server timed out.");
                    client.Socket.Disconnect();
                }
                client.Dispose();
                mre.Dispose();
            }

            server.Stop();
            server.Dispose();
        }
    }
}