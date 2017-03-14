using System;
using System.Threading;

namespace W.Demo
{
    public class TestPipeClient
    {
        public static void Run()
        {
            var pipeName = "pipe-test";
            var mre = new ManualResetEvent(false);
            var mreExit = new ManualResetEvent(false);

            using (var server = new W.IO.Pipes.PipeServer<W.IO.Pipes.PipeClient>(pipeName))
            {
                server.ClientConnected += (client) =>
                {
                    client.Exception += (o, e) =>
                    {
                        if (e != null)
                            Console.WriteLine("Server Exception: " + e.Message);
                    };
                    client.MessageReceived += (o, m) =>
                    {
                        Console.WriteLine("Server Echo: " + m.AsString());
                        //echo the message
                        client.Write(m);
                    };
                };
                server.Started += (s) =>
                {
                    Console.WriteLine("Server Started");

                    using (var client = new W.IO.Pipes.PipeClient())
                    {
                        client.Disconnected += (o, e) =>
                        {
                            if (e != null)
                                Console.WriteLine("Client Disconnected: " + e.Message);
                            else
                                Console.WriteLine("Client Disconnected");
                        };
                        client.Connected += (o) =>
                        {
                            Console.WriteLine("Client Connected");
                        };
                        client.Exception += (o, e) =>
                        {
                            if (e != null)
                                Console.WriteLine("Client Exception: " + e.Message);
                        };
                        client.MessageReceived += (o, m) =>
                        {
                            Console.WriteLine("Client Received: " + m.AsString());
                            mre.Set();
                        };
                        Console.WriteLine("Press Any Key To Connect To the Server");
                        Console.ReadKey();
                        var connected = client.Connect(pipeName, System.IO.Pipes.PipeDirection.InOut);
                        if (connected)
                        {
                            while (true)
                            {
                                Console.Write("Send <Return to Exit>:");
                                var msg = Console.ReadLine();
                                if (string.IsNullOrEmpty(msg))
                                    break;
                                client.Write(msg.Trim().AsBytes());

                                mre.WaitOne(5000); //wait for the server's response
                                mre.Reset();
                            }
                            mreExit.Set();//release the server's loop
                        }
                        else
                        {
                            Console.WriteLine("Failed to connect to the pipe server");
                            Console.WriteLine("Press Any Key To Exit");
                            Console.ReadKey();
                            mreExit.Set();
                        }
                    }
                };
                server.Start();
                mreExit.WaitOne();
            }
            mre?.Dispose();
            mreExit?.Dispose();
        }
    }
}