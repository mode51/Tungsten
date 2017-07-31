using System;
using System.Net;
using System.Threading;

namespace W.Demo
{
    public class TestCompressedSecureStringClient
    {
        public static void Run()
        {
            var mre = new ManualResetEvent(false);
            using (var server = new W.Net.SecureStringServer())
            {
                server.ClientConnected += client =>
                {
                    client.UseCompression = true;
                    Console.WriteLine("Client Connected To Server");
                    client.MessageReceived += (proxy, message) =>
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            Console.WriteLine("Server Echo: " + message);
                            proxy.As<W.Net.SecureStringClient>().Send(message.ToUpper());
                        }
                    };
                };
                server.ClientDisconnected += (client, exception) =>
                {
                    Console.WriteLine("Client Disconnected");
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var secureClient = new W.Net.SecureStringClient())
                {
                    secureClient.Connected += (s, address) =>
                    {
                        Console.WriteLine("Client connected to " + address.ToString());
                    };
                    secureClient.ConnectionSecured += s =>
                    {
                        mre.Set();
                    };
                    secureClient.Disconnected += (s, exception) =>
                    {
                        if (exception != null)
                            Console.WriteLine("Client Disconnected: " + exception.Message);
                        else
                            Console.WriteLine("Client Disconnected");
                        mre.Set();
                    };
                    secureClient.MessageReceived += (s, message) =>
                    {
                        Console.WriteLine("Client Received: " + message);
                        Console.Write("Send <Return To Exit>: ");
                    };
                    secureClient.MessageSent += s =>
                    {
                        Console.WriteLine("Client Message Sent");
                    };
                    secureClient.UseCompression = true;
                    secureClient.Socket.Connect(IPAddress.Parse("127.0.0.1"), 5150);
                    mre.WaitOne();

                    Console.Write("Send <Return To Exit>: ");
                    while (secureClient.Socket.IsConnected)
                    {
                        var message = Console.ReadLine();
                        if (string.IsNullOrEmpty(message))
                            break;
                        secureClient.Send(message);
                    }
                }
            }
        }
    }
}