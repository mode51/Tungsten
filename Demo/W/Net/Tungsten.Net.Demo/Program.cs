using System;

namespace W.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //var logger = new W.Net.StringClientLogger(System.Net.IPAddress.Parse("192.168.2.12"), 2112, true);
            //var logger = new W.IO.Pipes.PipeClientLogger("ConsoleLogger");
            Logging.Log.LogTheMessage += (category, message) => 
            {
                System.Diagnostics.Debug.WriteLine("{0} - {1}", category, message);
            };

            var exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Menu");
                Console.WriteLine("1.  Test Client");
                Console.WriteLine("2.  Test SecureClient");
                Console.WriteLine("3.  Test Client With Compression");
                Console.WriteLine("4.  Test Client Sequential Connections");
                Console.WriteLine("5.  Test Client Concurrent Connections");
                Console.WriteLine("6.  Test StringLogger");
                Console.WriteLine("7.  Test SecureStringLogger");
                Console.WriteLine("8.  Test Alpha Client/Server");
                Console.WriteLine("a.  Test Alpha Client Sequential Connections");
                Console.WriteLine("b.  Test Alpha Client Concurrent Connections");

                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey(true);
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        TestClient.Run();
                        break;
                    case ConsoleKey.D2:
                        TestSecureClient.Run();
                        break;
                    case ConsoleKey.D3:
                        TestClientWithCompression.Run();
                        break;
                    case ConsoleKey.D4:
                        TestClientConcurrency.Run_Sequentially();
                        break;
                    case ConsoleKey.D5:
                        TestClientConcurrency.Run_Concurrently();
                        break;
                    case ConsoleKey.D6:
                        TestStringLogger.Run();
                        break;
                    case ConsoleKey.D7:
                        TestSecureStringLogger.Run();
                        break;
                    case ConsoleKey.D8:
                        TestAlphaClientServer.Run();
                        break;
                    case ConsoleKey.A:
                        TestAlphaClientServer.Run_Sequentially();
                        break;
                    case ConsoleKey.B:
                        TestAlphaClientServer.Run_Concurrently();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }
    }
}