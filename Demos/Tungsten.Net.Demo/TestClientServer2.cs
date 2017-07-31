using System;
using System.Net;
using System.Threading;

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
}