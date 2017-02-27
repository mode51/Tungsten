using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace W.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (false) //test encryption
            {
                //W.Encryption.RSA_Tests.Test1();
                //Console.WriteLine("Press Any Key To Continue");
                //Console.ReadKey();

                var messages = new List<string>();
                for (int iterations = 0; iterations < 20; iterations++)
                {
                    var rsa = new W.Encryption.RSA();
                    //var value = "Jordan_";
                    //for (int t = 0; t < 100000; t++)
                    //    value += "a";
                    //value += "_Duerksen";
                    var value = "Jordan Duerksen is going to the mall to get some shopping done and get in some fitbit steps while at the same time enjoying a day out";

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
                Console.WriteLine("Press Any Key To Continue");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Hash = " + W.Encryption.MD5.GetMd5Hash("Jordan Duerksen"));

                using (var server = new W.Net.SecureStringServer())
                {
                    server.ClientConnected += client =>
                    {
                        Console.WriteLine("Client Connected");
                        client.MessageReceived += (proxy, message) =>
                        {
                            //if (ebc.IsSecure)
                            if (!string.IsNullOrEmpty(message))
                            {
                                Console.WriteLine("Echo: " + message);
                                proxy.As<W.Net.SecureStringClient>().Send(message);
                            }
                        };
                    };
                    server.ClientDisconnected += (client, exception) =>
                    {
                        Console.WriteLine("Client Disconnected");
                    };
                    server.Start(IPAddress.Parse("127.0.0.1"), 5150);

                    Console.WriteLine("Server Started");
                    Console.WriteLine("Press Any Key To Exit");
                    Console.ReadKey();
                }
            }
        }
    }
}
