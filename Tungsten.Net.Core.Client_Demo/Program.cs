using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace W.Demo
{
    public class Program
    {
        public static void Main(string[] args)
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
}
