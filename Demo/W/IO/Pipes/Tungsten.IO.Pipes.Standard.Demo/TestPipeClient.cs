using System;
using System.Threading;
using System.Threading.Tasks;
using W.AsExtensions;

namespace W.Demo
{
    public class TestPipeClient
    {
        public static void Run()
        {
            var pipeName = "pipe-test";

            using (var host = new W.IO.Pipes.Host())
            {
                host.RequestReceived += (h, s, b) =>
                {
                    Console.WriteLine("Server received: " + b.AsString());
                    //echo the message
                    return b;
                };
                host.Start(pipeName, 20);
                Console.WriteLine("Host started");

                while (true)
                {
                    Console.Write("Send <Return to Exit>:");
                    var msg = Console.ReadLine();
                    if (string.IsNullOrEmpty(msg))
                        break;
                    var response = W.IO.Pipes.SimplePipeClient.Request(pipeName, msg.Trim().AsBytes(), 3000);
                    Console.WriteLine("Server responded with: {0}", response.Result.AsString());
                }
            }
        }
        public static async Task RunAsync()
        {
            var pipeName = "pipe-test";
            var mre = new ManualResetEventSlim(false);
            var mreExit = new ManualResetEventSlim(false);

            using (var host = new W.IO.Pipes.SimplePipeHost())
            {
                host.RequestReceived += (h, s, b) =>
                {
                    Console.WriteLine("Server received: " + b.AsString());
                    //echo the message
                    return b;
                };
                host.Start(pipeName, 20);
                Console.WriteLine("Host started");

                while (true)
                {
                    Console.Write("Send <Return to Exit>:");
                    var msg = Console.ReadLine();
                    if (string.IsNullOrEmpty(msg))
                        break;
                    var cts = new CancellationTokenSource(30000);
                    var response = await W.IO.Pipes.SimplePipeClient.RequestAsync(pipeName, msg.Trim().AsBytes(), 3000);
                    Console.WriteLine("Server responded with: {0}", response.Result.AsString());
                }
            }
            mre?.Dispose();
            mreExit?.Dispose();
        }
    }
}