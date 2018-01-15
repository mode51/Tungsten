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
                Console.Clear();
                Console.WriteLine("Menu");
                //Console.WriteLine("1.  Test PipeClient");
                //Console.WriteLine("2.  Test PipeClient Asynchronous");
                //Console.WriteLine("3.  Test PipeClient(sending objects)");
                //Console.WriteLine("4.  Test PipeClient(sending objects) Asynchronous");
                Console.WriteLine("5.  Test Named Pipe Logging");

                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey(true);
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        //TestPipeClient.Run();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        //TestPipeClient.RunAsync().Wait();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        //TestPipeClientObjects.Run();
                        break;
                    case ConsoleKey.D4:
                        Console.Clear();
                        //TestPipeClientObjects.RunAsync().Wait();
                        break;
                    case ConsoleKey.D5:
                        TestNamedPipeLogging.Run();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }
    }
}