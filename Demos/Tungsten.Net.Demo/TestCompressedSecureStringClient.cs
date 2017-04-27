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
                server.ClientDisconnected += (client, remoteEndPoint, exception) =>
                {
                    if (exception != null)
                        Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString() + " - " + exception.Message);
                    else
                        Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString());
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var secureClient = new W.Net.SecureStringClient())
                {
                    secureClient.Connected += (s, remoteEndPoint) =>
                    {
                        Console.WriteLine("Client Connected: " + remoteEndPoint?.ToString());
                    };
                    secureClient.ConnectionSecured += s =>
                    {
                        mre.Set();
                    };
                    secureClient.Disconnected += (s, remoteEndPoint, exception) =>
                    {
                        if (exception != null)
                            Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString() + " - " + exception.Message);
                        else
                            Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString());
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
                    secureClient.Socket.ConnectAsync(IPAddress.Parse("127.0.0.1"), 5150).Wait();
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