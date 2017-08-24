using System;
using System.Net;
using System.Threading;

namespace W.Demo
{
    public class TestSecureStringClient
    {
        public static async void Run()
        {
            var mre = new ManualResetEvent(false);
            using (var server = new W.Net.SecureServer<W.Net.SecureClient<string>>())
            {
                server.ClientConnected += client =>
                {
                    Console.WriteLine("Server Connected To Client: " + client.As<W.Net.SecureClient<string>>().Socket.Name);
                    client.MessageReceived += (proxy, message) =>
                    {
                        if (!string.IsNullOrEmpty(message.As<string>()))
                        {
                            Console.WriteLine("Server Echo: " + message);
                            proxy.As<W.Net.SecureClient<string>>().Send(message.As<string>().ToUpper());
                        }
                    };
                };
                server.ClientDisconnected += (client, remoteEndPoint, exception) =>
                {
                    if (exception != null)
                        Console.WriteLine("Client Disconnected: " + client.As<W.Net.SecureClient<string>>().Socket.Name + " - " + exception.Message);
                    else
                        Console.WriteLine("Client Disconnected: " + client.As<W.Net.SecureClient<string>>().Socket.Name);

                    Console.WriteLine("Server Disconnected From: " + remoteEndPoint?.ToString());
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var client = new W.Net.SecureClient<string>())
                {
                    client.Connected += (ssc, remoteEndPoint) =>
                    {
                        Console.WriteLine("Client Connected: " + ssc.As<W.Net.SecureClient<string>>().Socket.Name);
                    };
                    client.Connected += (ssc, ep) =>
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

                    //we can't decrypt the data because we don't have the private key (so we need a better way to track sent data)
                    //client.MessageSent += (c, message) =>
                    //{
                    //    Console.WriteLine("Client Message Sent");
                    //};

                    for (int t = 0; t < 20; t++)
                    {
                        var connected = client.Socket.ConnectAsync("127.0.0.1", 5150).Result;
                        Console.WriteLine("Connected = {0}", connected);
                        mre.WaitOne();
                        client.Socket.Disconnect();
                        //Console.WriteLine("Disconnected");
                    }

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
            Console.ReadKey(true);
        }
    }
}