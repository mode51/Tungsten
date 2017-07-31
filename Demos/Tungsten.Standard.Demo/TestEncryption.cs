using System;
using System.Collections.Generic;

namespace W.Demo
{
    public class TestEncryption
    {
        public static void Run()
        {
            var messages = new List<string>();
            var r = new Random();
            for (int iterations = 0; iterations < 20; iterations++)
            {
                var rsa = new W.Encryption.RSA();
                var value = "Jordan_";
                for (int t = 0; t < 32000; t++) //around 32k
                //for (int t = 0; t < 4080; t++) //for a total of 4k
                    value += Convert.ToChar((byte)r.Next(0,255));
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
}