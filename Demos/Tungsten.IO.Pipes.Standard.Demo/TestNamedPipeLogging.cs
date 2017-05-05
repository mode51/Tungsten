using System;
using System.Threading;

namespace W.Demo
{
    public class TestNamedPipeLogging
    {
        public static void Run()
        {
            W.IO.Pipes.PipeClient.AddNamedPipeLogger("ConsoleLogger");
            var r = new Random();
            while (true)
            {
                Console.Write("Send <Return to Exit>:");
                var msg = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(msg))
                    break;

                switch(r.Next(0, 4))
                {
                    case 0:
                        W.Logging.Log.e(msg);
                        break;
                    case 1:
                        W.Logging.Log.w(msg);
                        break;
                    case 2:
                        W.Logging.Log.i(msg);
                        break;
                    case 3:
                        W.Logging.Log.v(msg);
                        break;
                }
            }

            Console.WriteLine("TestNamedPipeLogger Complete");

            //optionally, you can manually Stop the logging Pipe
            W.IO.Pipes.PipeClient.RemoveNamedPipeLogger();
        }
    }
}