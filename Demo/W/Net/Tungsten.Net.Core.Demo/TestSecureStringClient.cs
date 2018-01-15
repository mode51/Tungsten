using System;
using System.Net;
using System.Threading;
using W.AsExtensions;

namespace W.Demo
{
    public class TestSecureStringClient
    {
        public static void Run()
        {
            var mre = new ManualResetEvent(false);
            using (var server = new W.Net.Server<W.Net.SecureClient<string>>())
            {
                server.ClientConnected += client =>
                {
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
                    Console.WriteLine("Client Disconnected");
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var socketProxy = new W.Net.SecureClient<string>())
                {
                    socketProxy.Connected += (s, address) =>
                    {
                        Console.WriteLine("Client connected to " + address.ToString());
                    };
                    //socketProxy.ConnectionSecured += s =>
                    //{
                    //    mre.Set();
                    //};
                    socketProxy.Disconnected += (s, ep, exception) =>
                    {
                        if (exception != null)
                            Console.WriteLine("Client Disconnected: " + exception.Message);
                        else
                            Console.WriteLine("Client Disconnected");
                        mre.Set();
                    };
                    socketProxy.MessageReceived += (s, message) =>
                    {
                        Console.WriteLine("Client Received: " + message);
                        Console.Write("Send <Return To Exit>: ");
                    };
                    //socketProxy.MessageSent += s =>
                    //{
                    //    Console.WriteLine("Client Message Sent");
                    //};

                    socketProxy.Socket.Connect(IPAddress.Parse("127.0.0.1"), 5150);
                    mre.WaitOne();

                    Console.Write("Send <Return To Exit>: ");
                    while (socketProxy.Socket.Connected)
                    {
                        var message = Console.ReadLine();
                        if (string.IsNullOrEmpty(message))
                            break;
                        socketProxy.Send(ref message);
                    }
                }
            }
        }
    }
}