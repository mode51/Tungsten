using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using W;

namespace W.Net.Core.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {
                Console.WriteLine("Menu");
                Console.WriteLine("1.  Test Encryption");
                Console.WriteLine("2.  Test SecureStringClient");
                Console.WriteLine("3.  Test GenericClient/GenericServer");
                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        TestEncryption.Run();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        TestSecureStringClient.Run();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        TestGenericClientServer.Run();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
                Console.Clear();
            }
        }
    }

    public class TestEncryption
    {
        public static void Run()
        {
            var messages = new List<string>();
            for (int iterations = 0; iterations < 20; iterations++)
            {
                var rsa = new W.Encryption.RSA();
                var value = "Jordan_";
                for (int t = 0; t < 4080; t++) //for a total of 4k
                    value += "a";
                value += "_Duerksen";
                //var value = "Jordan Duerksen is going to the mall to get some shopping done and get in some fitbit steps while at the same time enjoying a day out";

                var startTime = DateTime.Now;
                var cipher = rsa.Encrypt(value);
                var endTime1 = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                //Console.WriteLine(cipher);

                var text = rsa.Decrypt(cipher);
                //Console.WriteLine(text);
                if (text != value)
                {
                    Console.WriteLine("decrypt failed");
                    break;
                }
                startTime = DateTime.Now;
                cipher = rsa.EncryptAsync(value.AsBytes(), rsa.PublicKey).Result;
                var endTime2 = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                //Console.WriteLine(cipher);

                Console.WriteLine("{0} vs {1}", endTime1, endTime2);
                text = rsa.Decrypt(cipher);
                //Console.WriteLine(text);

                if (text != value)
                {
                    Console.WriteLine("decrypt failed");
                    break;
                }
            }
            foreach (var msg in messages)
                Console.WriteLine(msg);
            Console.WriteLine("Press Any Key To Exit");
            Console.ReadKey();
        }
    }
    public class TestSecureStringClient
    {
        public static void Run()
        {
            var mre = new ManualResetEvent(false);
            using (var server = new W.Net.SecureStringServer())
            {
                server.ClientConnected += client =>
                {
                    Console.WriteLine("Client Connected To Server");
                    client.MessageReceived += (proxy, message) =>
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            Console.WriteLine("Server Echo: " + message);
                            proxy.As<W.Net.SecureStringClient>().Send(message);
                        }
                    };
                };
                server.ClientDisconnected += (client, exception) =>
                {
                    Console.WriteLine("Client Disconnected");
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var socketProxy = new W.Net.SecureStringClient())
                {
                    socketProxy.Connected += (s, address) =>
                    {
                        Console.WriteLine("Client connected to " + address.ToString());
                    };
                    socketProxy.ConnectionSecured += s =>
                    {
                        mre.Set();
                    };
                    socketProxy.Disconnected += (s, exception) =>
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
                    socketProxy.MessageSent += s =>
                    {
                        Console.WriteLine("Client Message Sent");
                    };

                    socketProxy.Socket.Connect(IPAddress.Parse("127.0.0.1"), 5150);
                    mre.WaitOne();

                    Console.Write("Send <Return To Exit>: ");
                    while (socketProxy.Socket.IsConnected)
                    {
                        var message = Console.ReadLine();
                        if (string.IsNullOrEmpty(message))
                            break;
                        socketProxy.Send(message);
                    }
                }
            }
        }
    }
    public class TestGenericClientServer
    {
        public class Customer
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime Created { get; set; }
            public bool IsDeleted { get; set; }
        }
        public static void Run()
        {
            var mre = new ManualResetEvent(false);

            using (var server = new W.Net.GenericServer<Customer>())
            {
                server.ClientConnected += genericClient =>
                {
                    genericClient.GenericMessageReceived += (o, customer) =>
                    {
                        //increase the age and echo the customer
                        customer.Age += 1;
                        customer.Name = customer.Name.ToUpper();
                        o.As<W.Net.GenericClient<Customer>>().Send(customer);
                    };
                };
                server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                using (var client = new W.Net.GenericClient<Customer>())
                {
                    client.GenericMessageReceived += (o, customer) =>
                    {
                        Console.WriteLine("{0} New Age = {1}", customer.Name, customer.Age);
                        Console.WriteLine("Send <Return To Exit>: ");
                    };
                    client.ConnectionSecured += (o) =>
                    {
                        mre.Set();
                    };
                    client.Disconnected += (s, exception) =>
                    {
                        mre.Set();
                    };
                    client.Socket.Connect(IPAddress.Parse("127.0.0.1"), 5150).Wait();
                    mre.WaitOne();

                    Console.WriteLine("Send <Return To Exit>: ");
                    while (client.Socket.IsConnected)
                    {
                        var name = Console.ReadLine();
                        if (string.IsNullOrEmpty(name))
                            break;
                        var customer = new Customer() { Name = name, Age = new Random().Next(1, 75) };
                        Console.WriteLine("{0} Age = {1}", customer.Name, customer.Age);
                        client.Send(customer);
                    }
                }
            }
        }
    }
}
