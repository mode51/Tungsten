using System;
using W.AsExtensions;
using W.IO.Pipes;

namespace LogHost
{
    class Program
    {
        static void ConfigureConsole()
        {
            Console.Clear();
            Console.WriteLine("Press <Escape> to exit");
            //now make the window size the height - 1 (the top line)
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight - 1);
            //and reposition the window to below the first line
            //Console.SetWindowPosition(0, 1);
        }
        static void Main(string[] args)
        {
            ConfigureConsole();
            using (var host = new PipeHost())
            {
                host.BytesReceived += (s, bytes) =>
                {
                    Console.WriteLine(bytes.AsString());
                };
                host.Start("ConsoleLogger", 20, false);
                while (true)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                        break;
                }
            }
        }
    }
}
