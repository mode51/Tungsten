using System;
using System.Net;
using System.Threading;
using W.AsExtensions;

namespace W.Demo
{
    public class TestCompressedSecureStringClient
    {
        public static void Run()
        {
            var mre = new ManualResetEvent(false);
            using (var server = new W.Net.Server<W.Net.SecureClient<string>>())
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
                            message = message.ToUpper();
                            proxy.As<W.Net.SecureClient<string>>().Send(ref message);
                        }
                    };
                };
                server.ClientDisconnected += (client, ep, exception) =>
                {
                    Console.WriteLine($"Client({ep}) Disconnected");
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var secureClient = new W.Net.SecureClient<string>())
                {
                    secureClient.Connected += (s, address) =>
                    {
                        Console.WriteLine("Client connected to " + address.ToString());
                    };
                    //secureClient.ConnectionSecured += s =>
                    //{
                    //    mre.Set();
                    //};
                    secureClient.Disconnected += (s, ep, exception) =>
                    {
                        if (exception != null)
                            Console.WriteLine($"Client({ep}) Disconnected: {exception.Message}");
                        else
                            Console.WriteLine($"Client({ep}) Disconnected");
                        //mre.Set();
                    };
                    secureClient.MessageReceived += (s, message) =>
                    {
                        Console.WriteLine("Client Received: " + message);
                        Console.Write("Send <Return To Exit>: ");
                    };
                    //secureClient.MessageSent += s =>
                    //{
                    //    Console.WriteLine("Client Message Sent");
                    //};
                    secureClient.UseCompression = true;
                    secureClient.Socket.Connect(IPAddress.Parse("127.0.0.1"), 5150);
                    //mre.WaitOne();

                    Console.Write("Send <Return To Exit>: ");
                    while (secureClient.Socket.Connected)
                    {
                        var message = Console.ReadLine();
                        if (string.IsNullOrEmpty(message))
                            break;
                        secureClient.Send(ref message);
                    }
                }
            }
        }
    }
}