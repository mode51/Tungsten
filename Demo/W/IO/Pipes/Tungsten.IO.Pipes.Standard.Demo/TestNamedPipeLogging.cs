using System;
using System.Threading;
using W.AsExtensions;

namespace W.Demo
{
    public class TestNamedPipeLogging
    {
        public static void Run()
        {
            //Console.WriteLine("Not Implemented");
            //Console.WriteLine("Press Any Key To Return");
            //Console.ReadKey(true);
            var pipeName = "TestPipeLogger";
            var mreSync = new ManualResetEventSlim(true);
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.BytesReceived += (s, bytes) =>
                {
                    Console.WriteLine("Logging: {0}", bytes.AsString());
                    mreSync.Set();
                };
                host.Start(pipeName, 20, false);

                using (var pipeLogger = W.IO.Pipes.PipeClient.Create(".", pipeName, 3000).Result)
                {
                    var r = new Random();
                    while (true)
                    {
                        mreSync.Wait();
                        mreSync.Reset();
                        Console.Write("Send <Return to Exit>:");
                        var msg = Console.ReadLine().Trim();
                        if (string.IsNullOrEmpty(msg))
                            break;

                        switch (r.Next(0, 4))
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
                }
            }
            mreSync.Dispose();
        }
    }
}