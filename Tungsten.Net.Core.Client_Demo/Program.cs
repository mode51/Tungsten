using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace W.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {
                Console.WriteLine("Menu");
                Console.WriteLine("1.  Test SecureStringClient");
                Console.WriteLine("2.  Test GenericClient/GenericServer");
                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        TestSecureStringClient.Run();
                        break;
                    case ConsoleKey.D2:
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

    public class TestSecureStringClient
    {
        public static void Run()
        {
            Console.WriteLine("Hash = " + W.Encryption.MD5.GetMd5Hash("Jordan Duerksen"));

            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();
            Console.WriteLine("Client Started");

            using (var socketProxy = new W.Net.SecureStringClient())
            {
                socketProxy.Connected += (s, address) =>
                {
                    Console.WriteLine("Connected to " + address.ToString());
                };
                socketProxy.Disconnected += (s, exception) =>
                {
                    if (exception != null)
                        Console.WriteLine("Disconnected: " + exception.Message);
                    else
                        Console.WriteLine("Disconnected");
                };
                //socketProxy.MessageReceived += (s, bytes) =>
                //{
                //    Console.WriteLine("Received: " + bytes.AsString());
                //    Console.Write("Send Message: ");
                //};
                socketProxy.MessageReceived += (s, message) =>
                {
                    Console.WriteLine("Received: " + message);
                    Console.Write("Send Message: ");
                };
                socketProxy.MessageSent += s =>
                {
                    Console.WriteLine("Message Sent");
                };
                socketProxy.Socket.Connect(IPAddress.Parse("127.0.0.1"), 5150);
                Console.Write("Send Message: ");
                while (true)
                {
                    var message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                        break;
                    socketProxy.Send(message);
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

                var client = new W.Net.GenericClient<Customer>();
                client.GenericMessageReceived += (o, customer) =>
                {
                    Console.WriteLine("{0} New Age = {1}", customer.Name, customer.Age);
                };
                client.ConnectionSecured += (o) =>
                {
                    mre.Set();
                };
                client.Socket.Connect(IPAddress.Parse("127.0.0.1"), 5150).Wait();
                mre.WaitOne();

                while (true)
                {
                    Console.WriteLine("Send <Return To Exit>: ");
                    var name = Console.ReadLine();
                    if (string.IsNullOrEmpty(name))
                        break;
                    var customer = new Customer() { Name = name, Age = new Random().Next(1, 75) };
                    Console.WriteLine("{0} Age = {1}", customer.Name, customer.Age);
                    client.Send(customer);
                }
                client.Socket.Disconnect();
            }
        }
    }
}
