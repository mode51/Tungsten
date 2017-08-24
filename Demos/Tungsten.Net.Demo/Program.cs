using System;

namespace W.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //var logger = new W.Net.StringClientLogger(System.Net.IPAddress.Parse("192.168.2.12"), 2112, true);
            //var logger = new W.IO.Pipes.PipeClientLogger("ConsoleLogger");
            Logging.Log.LogTheMessage += (category, message) => { System.Diagnostics.Debug.WriteLine("{0} - {1}", category, message); };

            var exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Menu");
                Console.WriteLine("1.  Test Client/Server");
                Console.WriteLine("2.  Test SecureStringClient");
                Console.WriteLine("3.  Test SecureStringClient with compression");
                Console.WriteLine("4.  Test StringLogger");
                Console.WriteLine("5.  Test SecureStringLogger");
                Console.WriteLine("6.  Test Client More");
                Console.WriteLine("7.  Test SecureClient More");
                Console.WriteLine("8.  Test SecureClient Connections");
                //Console.WriteLine("9.  Test SecureClient2");
                //Console.WriteLine("A.  Test ClientSlim");

                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey(true);
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        TestClientServer.Run();
                        break;
                    case ConsoleKey.D2:
                        TestSecureStringClient.Run();
                        break;
                    case ConsoleKey.D3:
                        TestCompressedSecureStringClient.Run();
                        break;
                    case ConsoleKey.D4:
                        TestStringLogger.Run();
                        break;
                    case ConsoleKey.D5:
                        TestSecureStringLogger.Run();
                        break;
                    case ConsoleKey.D6:
                        TestClientServer2.Run();
                        break;
                    case ConsoleKey.D7:
                        TestSecureClientServer2.Run();
                        break;
                    case ConsoleKey.D8:
                        TestSecureClientServer3.Run();
                        break;
                    //case ConsoleKey.D9:
                    //    TestSecureClient2.Run();
                    //    break;
                    //case ConsoleKey.A:
                    //    TestClientSlim.Run_Test_Concurrency();
                    //    break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }
    }
}