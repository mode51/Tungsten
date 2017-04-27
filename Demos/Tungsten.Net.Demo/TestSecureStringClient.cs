using System;
using System.Net;
using System.Threading;

namespace W.Demo
{
    public class TestSecureStringClient
    {
        public static void Run()
        {
            var mre = new ManualResetEvent(false);
            using (var server = new W.Net.SecureStringServer())
            {
                server.ClientConnected += client =>
                {
                    Console.WriteLine("Server Connected To Client: " + client.As<W.Net.SecureStringClient>().Socket.Name);
                    client.MessageReceived += (proxy, message) =>
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            Console.WriteLine("Server Echo: " + message);
                            proxy.As<W.Net.SecureStringClient>().Send(message.ToUpper());
                        }
                    };
                };
                server.ClientDisconnected += (client, remoteEndPoint, exception) =>
                {
                    if (exception != null)
                        Console.WriteLine("Client Disconnected: " + client.As<W.Net.SecureStringClient>().Socket.Name + " - " + exception.Message);
                    else
                        Console.WriteLine("Client Disconnected: " + client.As<W.Net.SecureStringClient>().Socket.Name);

                    Console.WriteLine("Server Disconnected From: " + remoteEndPoint?.ToString());
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var client = new W.Net.SecureStringClient())
                {
                    client.Connected += (ssc, remoteEndPoint) =>
                    {
                        Console.WriteLine("Client Connected: " + ssc.As<W.Net.SecureStringClient>().Socket.Name);
                    };
                    client.ConnectionSecured += s =>
                    {
                        mre.Set();
                    };
                    client.Disconnected += (ssc, remoteEndPoint, exception) =>
                    {
                        if (exception != null)
                            Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString() + " - " + exception.Message);
                        else
                            Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString());
                        mre.Set();
                    };
                    client.MessageReceived += (s, message) =>
                    {
                        Console.WriteLine("Client Received: " + message);
                        Console.Write("Send <Return To Disconnect>: ");
                    };
                    client.MessageSent += s =>
                    {
                        Console.WriteLine("Client Message Sent");
                    };

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