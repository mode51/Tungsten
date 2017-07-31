using System;

namespace W.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {
                Console.WriteLine("Menu");
                Console.WriteLine("1.  Test Client/Server");
                Console.WriteLine("2.  Test SecureStringClient");
                Console.WriteLine("3.  Test SecureStringClient with compression");
                Console.WriteLine("4.  Test StringLogger");
                Console.WriteLine("5.  Test SecureStringLogger");
                Console.WriteLine("6.  Test Client More");
                Console.WriteLine("7.  Test SecureClient More");

                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        TestClientServer.Run();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        TestSecureStringClient.Run();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        TestCompressedSecureStringClient.Run();
                        break;
                    case ConsoleKey.D4:
                        Console.Clear();
                        TestStringLogger.Run();
                        break;
                    case ConsoleKey.D5:
                        Console.Clear();
                        TestSecureStringLogger.Run();
                        break;
                    case ConsoleKey.D6:
                        Console.Clear();
                        TestClientServer2.Run();
                        break;
                    case ConsoleKey.D7:
                        Console.Clear();
                        TestSecureClientServer2.Run();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
                Console.Clear();
            }
        }
    }
}