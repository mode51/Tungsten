using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using W;
using W.Logging;
using W.Net.Sockets;

namespace W.Net
{
    internal class TestClass
    {
        public class Message
        {
            public Guid Id { get; set; }
            public int Age { get; set; }
            public string Name { get; set; }
        }

        //sample declarations
        private Client _client = new Client();
        private Client<Message> _client2 = new Client<Message>();
        private Client<string> _stringClient = new Client<string>();
        private SecureClient<Message> _secureclient = new SecureClient<Message>();
        private Server<Client<string>> _stringServer = new Server<Client<string>>();

        public TestClass()
        {
            //sample uses
            _client.RawDataReceived += (c, rawBytes) => { Console.WriteLine(rawBytes.AsString()); };
            //_client.WaitForConnected();
            _client.DataReceived += (c, bytes) => { Console.WriteLine(bytes.AsString()); };
            _client.Socket.ConnectAsync("127.0.0.1", 5150).Wait();

            _client2.MessageReceived += (c, message) => { Console.WriteLine(message.Id); };
            //_client2.WaitForConnected();
            _client2.As<IDataSocket>().DataReceived += (c, bytes) => { Console.WriteLine(bytes.AsString()); };
            _client2.Socket.ConnectAsync("127.0.0.1", 5150).Wait();

            _stringClient.MessageReceived += (c, message) => { Console.WriteLine(message); };
            //_stringClient.WaitForConnected();
            _stringClient.Socket.ConnectAsync("127.0.0.1", 5150).Wait();

            _secureclient.MessageReceived += (c, message) => { Console.WriteLine(message.Id); };
            //_secureclient.WaitForConnected();

            //sample in a using
            using (var server = new Server<Client<string>>())
            {
                server.ClientConnected += c =>
                {
                    Console.WriteLine("Server Connected");
                };
                server.ClientDisconnected += (c, ep, ex) =>
                {
                    Console.WriteLine("Server Disconnected");
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var client = new Client<string>())
                {
                    if (client.Socket.ConnectAsync("127.0.0.1", 5150).Wait(5000))
                    {
                        Console.WriteLine("Client Connected");
                    }
                }

            }
        }
    }
}
