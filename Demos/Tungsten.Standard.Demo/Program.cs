using System;
using W;
using W.Demo;

namespace W.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Menu");
                Console.WriteLine("1.  Test RSA Encryption");
                Console.WriteLine("2.  Test Compression");

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
                        TestCompression.Run();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }
    }
}