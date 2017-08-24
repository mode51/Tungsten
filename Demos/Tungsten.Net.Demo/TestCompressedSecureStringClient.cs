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
            using (var server = new W.Net.SecureServer<W.Net.SecureClient<string>>())
            {
                server.ClientConnected += client =>
                {
                    client.Socket.UseCompression = true;
                    Console.WriteLine("Client Connected To Server");
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
                        Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString() + " - " + exception.Message);
                    else
                        Console.WriteLine("Client Disconnected: " + remoteEndPoint?.ToString());
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var secureClient = new W.Net.SecureClient<string>())
                {
                    secureClient.Connected += (ssc, remoteEndPoint) =>
                    {
                        Console.WriteLine("Client Connected: " + remoteEndPoint?.ToString());
                    };
                    secureClient.Connected += (scc, ep) =>
                    {
                        mre.Set();
                    };
                    secureClient.Disconnected += (scc, remoteEndPoint, exception) =>
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
                    //secureClient.MessageSent += (c, message) =>
                    //{
                    //    Console.WriteLine("Client Message Sent");
                    //};
                    secureClient.Socket.UseCompression = true;
                    secureClient.Socket.ConnectAsync("127.0.0.1", 5150).Wait();
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