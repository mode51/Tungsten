using System;
using System.Net;
using System.Threading;

namespace W.Demo
{
    public class TestSecureClient
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
            var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            var mre = new ManualResetEvent(false);

            using (var server = new W.Net.EchoServer<W.Net.SecureClient<Customer>>())
            {
                server.Start(endPoint);

                var client = new W.Net.SecureClient<Customer>();
                {
                    client.MessageReceived += (o, customer) =>
                    {
                        Console.WriteLine("Server Echoed: {0}, Age = {1}", customer.Name, customer.Age);
                        Console.WriteLine("Send <Return To Exit>: ");
                        mre.Set();
                    };
                    if (client.Connect(endPoint))
                    {

                        Console.WriteLine("Send <Return To Exit>: ");
                        while (client.IsConnected)
                        {
                            var name = Console.ReadLine();
                            if (string.IsNullOrEmpty(name))
                                break;
                            mre.Reset();
                            var customer = new Customer() { Name = name, Age = new Random().Next(1, 75) };
                            Console.WriteLine("Sending: {0}, Age = {1}", customer.Name, customer.Age);
                            client.Send(ref customer);
                            mre.WaitOne(5000);
                        }
                        client.Disconnect();
                    }
                }
            }
        }
    }
}