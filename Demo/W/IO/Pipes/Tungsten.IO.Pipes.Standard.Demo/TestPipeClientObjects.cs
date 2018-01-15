using System;
using System.Threading;
using System.Threading.Tasks;

namespace W.Demo
{
    public class TestPipeClientObjects
    {
        public class PipeMessage
        {
            public DateTime TimeStamp { get; set; } = DateTime.Now;
            public string Message { get; set; }
        }
        public static void Run()
        {
            var pipeName = "pipe-test";

            using (var host = new W.IO.Pipes.SimplePipeHost<PipeMessage>())
            {
                host.RequestReceived += (h, s, m) =>
                {
                    Console.WriteLine(string.Format("Server received: {0}, {1}", m.TimeStamp, m.Message));
                    //echo the message
                    return m;
                };
                host.Start(pipeName, 20);
                Console.WriteLine("Server started");

                while (true)
                {
                    Console.Write("Send <Return to Exit>:");
                    var msg = Console.ReadLine();
                    if (string.IsNullOrEmpty(msg))
                        break;
                    var response = W.IO.Pipes.SimplePipeClient.Request<PipeMessage>(pipeName, new PipeMessage() { Message = msg.Trim() }, 3000);
                    Console.WriteLine(string.Format("Server responded with: {0}, {1}", response.Result.TimeStamp, response.Result.Message));
                }
            }
        }
        public static async Task RunAsync()
        {
            var pipeName = "pipe-test";

            using (var host = new W.IO.Pipes.SimplePipeHost<PipeMessage>())
            {
                host.RequestReceived += (h, s, m) =>
                {
                    Console.WriteLine(string.Format("Server received: {0}, {1}", m.TimeStamp, m.Message));
                    //echo the message
                    return m;
                };
                host.Start(pipeName, 20);
                Console.WriteLine("Server started");

                while (true)
                {
                    Console.Write("Send <Return to Exit>:");
                    var msg = Console.ReadLine();
                    if (string.IsNullOrEmpty(msg))
                        break;
                    var response = await W.IO.Pipes.SimplePipeClient.RequestAsync<PipeMessage>(pipeName, new PipeMessage() { Message = msg.Trim() }, 3000);
                    Console.WriteLine(string.Format("Server responded with: {0}, {1}", response.Result.TimeStamp, response.Result.Message));
                }
            }
        }
    }
}