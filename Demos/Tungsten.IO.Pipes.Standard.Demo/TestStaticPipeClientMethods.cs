using System;
using System.Threading;

namespace W.Demo
{
    public class TestStaticPipeClientMethods
    {
        public static void Run()
        {
            var pipeName = "pipe-test";
            var mreStarted = new ManualResetEvent(false);
            var mreReceived = new ManualResetEvent(false);

            using (var server = new W.IO.Pipes.PipeServer<W.IO.Pipes.PipeClient>(pipeName))
            {
                server.ClientConnected += (client) =>
                {
                    client.As<W.IO.Pipes.PipeClient>().MessageReceived += (o, m) =>
                    {
                        Console.WriteLine("Server Received: " + m.AsString());
                        //o.As<W.IO.Pipes.PipeClient>().Write("".AsBytes());
                        mreReceived.Set();
                    };
                    client.As<W.IO.Pipes.PipeClient>().Disconnected += (o, e) =>
                    {
                        //System.Diagnostics.Debugger.Break();
                    };
                };
                server.Started += (s) =>
                {
                    Console.WriteLine("Server Started");
                    mreStarted.Set();
                };
                server.Start();
                if (!mreStarted.WaitOne(10000))
                {
                    Console.WriteLine("Server failed to start");
                    Console.Write("Press any key to return");
                    Console.ReadKey();
                }
                else
                {
                    while (true)
                    {
                        Console.Write("Send <Return to Exit>:");
                        var msg = Console.ReadLine().Trim();
                        if (string.IsNullOrEmpty(msg))
                            break;

                        W.IO.Pipes.PipeClient.Write(pipeName, msg.AsBytes());
                        mreReceived.WaitOne(5000);
                        mreReceived.Reset();
                    }
                }
            }
        }
    }
}