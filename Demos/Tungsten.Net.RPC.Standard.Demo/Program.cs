using System;

namespace Tungsten.Net.RPC.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            W.Logging.Log.LogTheMessage = (category, message) =>
            {
                Console.WriteLine("{0} - {1}", category.ToString().ToUpper(), message);
            };

            var exit = false;
            while (!exit)
            {
                Console.WriteLine("Menu");
                Console.WriteLine("1.  Test W.Net.RPC.Server (loading methods and calling them)");
                //Console.WriteLine("2.  Test W.Net.RPC.Client (connecting to the server and calling methods)");
                Console.WriteLine("2.  Test new W.Net.RPC.Client (connecting to the server and calling methods)");

                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        W.Tests.Net_RPC_Standard_Server.Run();
                        break;
                    //case ConsoleKey.D2:
                    //    Console.Clear();
                    //    W.Tests.Net_RPC_Standard_Client.Run().Wait();
                    //    break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        W.Tests.TestRPCClient.Run().Wait();
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