using System;
using System.Net;
using System.Threading;

namespace W.Demo
{
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