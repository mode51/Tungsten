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
            var keySizes = W.Encryption.RSAMethods.LegalKeySizes()[0];
            var keySize = keySizes.MinSize;
            var sw = System.Diagnostics.Stopwatch.StartNew();

            while (keySize <= keySizes.MaxSize)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    break;
                }
                try
                {
                    Console.Write("KeySize={0}", keySize);
                    var keys = W.Encryption.RSAMethods.CreateKeyPair(keySize);
                    var value = "Jordan_";
                    //for (int t = 0; t < 4080; t++) //for a total of 4k
                    for (int t = 0; t < 31000 + r.Next(1000); t++) //around 32k
                        value += Convert.ToChar((byte)r.Next(0, 255));
                    value += "_Duerksen";
                    Console.Write(", Encrypting {0} Bytes", value.Length);
                    sw.Restart();
                    var cipher = W.Encryption.RSAMethods.Encrypt(value, keys.PublicKey);
                    Console.Write(" ({0} seconds, Decrypting {0} Bytes", sw.Elapsed.TotalSeconds, value.Length);
                    sw.Restart();
                    var decipher = W.Encryption.RSAMethods.Decrypt(cipher, keys.PrivateKey);
                    Console.Write(" ({0} seconds", sw.Elapsed.TotalSeconds);
                    if (decipher == value)
                    {
                        Console.WriteLine(", Success");
                    }
                    else
                    {
                        Console.WriteLine(", Failed");
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                keySize += keySizes.SkipSize;
            }

            //for (int iterations = 0; iterations < 20; iterations++)
            //{
            //    var rsa = new W.Encryption.RSA(System.Security.Cryptography.RSAEncryptionPadding.Pkcs1, 2048);
            //    var value = "Jordan_";
            //    for (int t = 0; t < 32000; t++) //around 32k
            //                                    //for (int t = 0; t < 4080; t++) //for a total of 4k
            //        value += Convert.ToChar((byte)r.Next(0, 255));
            //    value += "_Duerksen";
            //    //var value = "Jordan Duerksen is going to the mall to get some shopping done and get in some fitbit steps while at the same time enjoying a day out";

            //    var startTime = DateTime.Now;
            //    var cipher = rsa.Encrypt(value);
            //    var endTime1 = DateTime.Now.Subtract(startTime).TotalMilliseconds;
            //    //Console.WriteLine(cipher);

            //    var text = rsa.Decrypt(cipher);
            //    //Console.WriteLine(text);
            //    if (text != value)
            //    {
            //        Console.WriteLine("decrypt failed");
            //        break;
            //    }
            //    startTime = DateTime.Now;
            //    cipher = rsa.EncryptAsync(value.AsBytes(), rsa.PublicKey).Result;
            //    var endTime2 = DateTime.Now.Subtract(startTime).TotalMilliseconds;
            //    //Console.WriteLine(cipher);

            //    Console.WriteLine("{0} vs {1}", endTime1, endTime2);
            //    text = rsa.Decrypt(cipher);
            //    //Console.WriteLine(text);

            //    if (text != value)
            //    {
            //        Console.WriteLine("decrypt failed");
            //        break;
            //    }
            //}
            foreach (var msg in messages)
                Console.WriteLine(msg);
            Console.WriteLine("Press Any Key To Exit");
            Console.ReadKey();
        }
    }
}