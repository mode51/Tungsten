using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
#if DEBUG
    internal class IpcMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public byte[] Data { get; set; } = new byte[20];

        public override string ToString()
        {
            return $"({Id})-{Name}: {Data.AsString()}";
        }
        public IpcMessage()
        {
            var r = new Random();
            r.NextBytes(Data);
            Name = Data.AsString();
        }
    }

    class Program
    {
        internal class Connection
        {
            public PipeClient Client { get; set; }
            public int Index { get; set; }
            public int MessageCount { get; set; }
        }
        internal class Connection<TMessage> where TMessage : new()
        {
            public PipeClient<TMessage> Client { get; set; }
            public int Index { get; set; }
            public int MessageCount { get; set; }
        }
        private static string _pipeName = "Tungsten.IO.Pipes.TestPipe";

        static async Task TestPipeServer()
        {
            using (var server = new PipeServer())
            {
                $"Server created".WriteFullConsoleLine();
                //server.BytesReceived += async (s, b) =>
                //{
                //    await s.WriteAsync(b); //echo
                //    $"Server echoed: {b.AsString()}".WriteFullConsoleLine();
                //};
                server.WaitForConnection(_pipeName, 1); //waits for connections
                $"Server started".WriteFullConsoleLine();
                using (var client = new PipeClient())
                {
                    $"Client created".WriteFullConsoleLine();
                    client.Connected += c => { $"Client.Connected(evt)".WriteFullConsoleLine(); };
                    client.ConnectionFailed += (c, e) => { $"Client.ConnectionFailed(evt): {e}".WriteFullConsoleLine(); };
                    client.Disconnected += (c, e) => { $"Client.Disconnected(evt): {e}".WriteFullConsoleLine(); };
                    //client.BytesReceived += (c, b) => { $"Client received: {b.AsString()}".WriteFullConsoleLine(); };
                    if (await client.ConnectAsync(".", _pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 10))
                    {
                        $"Client connected to the server".WriteFullConsoleLine();
                        await client.WriteAsync("Please respond".AsBytes());
                        var request = await server.ReadAsync();
                        $"Server received request: {request.AsString()}".WriteFullConsoleLine();
                        await server.WriteAsync(request); //echo
                        var response = await client.ReadAsync();
                        $"Client received response: {response.AsString()}".WriteFullConsoleLine();
                    }
                    else
                        $"Client failed to connect to the server".WriteFullConsoleLine();
                }
                $"Client disposed".WriteFullConsoleLine();
            }
            $"Server disposed".WriteFullConsoleLine();
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        static async Task TestPipeHost()
        {
            Console.WriteLine(nameof(TestPipeHost));
            //var mre = new System.Threading.ManualResetEventSlim(false);
            try
            {
                int runCount = 1000;
                int serverCount = 1;
                for (int i = 0; i < runCount; i++)
                {
                    if (Console.KeyAvailable)
                        break;
                    using (var host = new PipeHost())
                    {
                        Console.WriteLine($"Host Created");
                        host.MessageReceived += (h, s, b) =>
                        {
                            Console.WriteLine($"Server received: {b.AsString()}");
                            s.Write(b); //echo
                        };
                        host.Start("Test Pipe Host", serverCount);
                        Console.WriteLine($"Host started");
                        using (var client = new PipeClient())
                        {
                            client.MessageReceived += (c, b) =>
                            {
                                Console.WriteLine($"Client received: {b.AsString()}");
                            };
                            if (await client.ConnectAsync(".", "Test Pipe Host", System.Security.Principal.TokenImpersonationLevel.Impersonation, 1000))
                            {
                                Console.WriteLine($"Client Connected");
                                await client.WriteAsync($"Hello World {i}/{runCount}".AsBytes());
                                var response = await client.ReadAsync();
                                Console.WriteLine($"Response = {response.AsString()}");
                            }
                            else
                                Console.WriteLine($"Client failed to connect to the server");
                        }
                        Console.WriteLine("Client disposed");
                        //host.Stop(false);
                    }
                    Console.WriteLine("Server disposed");
                    //System.Threading.Thread.Sleep(100);
                }
                if (Console.KeyAvailable)
                    Console.ReadKey(true);
                Console.WriteLine("Press Any Key To Return");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press Any Key To Return");
            }
            Console.ReadKey(true);
        }
        static async Task TestPipeHost2()
        {
            Console.WriteLine(nameof(TestPipeHost2));
            //var mre = new System.Threading.ManualResetEventSlim(false);
            try
            {
                int runCount = 1000;
                int serverCount = 1;
                using (var host = new PipeHost())
                {
                    Console.WriteLine($"Host Created");
                    host.MessageReceived += (h, s, b) =>
                    {
                        Console.WriteLine($"Server received: {b.AsString()}");
                        s.Write(b); //echo
                    };
                    host.Start("Test Pipe Host", serverCount);
                    Console.WriteLine($"Host started");

                    for (int i = 0; i < runCount; i++)
                    {
                        if (Console.KeyAvailable)
                            break;
                        using (var client = new PipeClient())
                        {
                            client.MessageReceived += (c, b) =>
                            {
                                Console.WriteLine($"Client received: {b.AsString()}");
                            };
                            if (await client.ConnectAsync(".", "Test Pipe Host", System.Security.Principal.TokenImpersonationLevel.Impersonation, 2000))
                            {
                                Console.WriteLine($"Client Connected");
                                await client.WriteAsync($"Hello World {i}/{runCount}".AsBytes());
                                var response = await client.ReadAsync();
                                Console.WriteLine($"Response = {response.AsString()}");
                            }
                            else
                                Console.WriteLine($"Client failed to connect to the server");
                        }
                        Console.WriteLine("Client disposed");
                        //W.Threading.Thread.Sleep(100);
                    }
                    if (Console.KeyAvailable)
                        Console.ReadKey(true);
                    host.Stop();
                }
                Console.WriteLine("Host disposed");
                //W.Threading.Thread.Sleep(100);
                Console.WriteLine("Press Any Key To Return");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press Any Key To Return");
            }
            Console.ReadKey(true);
        }
        static async Task TestPipeHost3()
        {
            Console.WriteLine(nameof(TestPipeHost3));
            //var mre = new System.Threading.ManualResetEventSlim(false);
            try
            {
                int runCount = 1000;
                int serverCount = 25;
                using (var host = new PipeHost())
                {
                    host.MessageReceived += (h, s, b) =>
                    {
                        s.Write(b); //echo
                    };
                    host.Start("Test Pipe Host", serverCount);

                    var clients = new List<Connection>();
                    for (int t = 0; t < serverCount; t++)
                    {
                        var connection = new Connection();
                        connection.Client = new PipeClient();
                        connection.Index = t;
                        connection.Client.MessageReceived += (c, b) =>
                        {
                            $"{connection.Index}. Client received: {b.AsString()}".WriteFullConsoleLine(connection.Index);
                        };
                        clients.Add(connection);
                    }
                    foreach (var client in clients)
                    {
                        if (await client.Client.ConnectAsync(".", "Test Pipe Host", System.Security.Principal.TokenImpersonationLevel.Impersonation, 1000))
                            $"{client.Index}. Client Connected".WriteFullConsoleLine(client.Index);
                        else
                            $"{client.Index}. Client failed to connect".WriteFullConsoleLine(client.Index);
                    }
                    for (int i = 0; i < runCount; i++)
                    {
                        if (Console.KeyAvailable)
                            break;
                        foreach (var client in clients)
                        {
                            if (Console.KeyAvailable)
                                break;
                            if (client.Client.Stream.IsConnected)
                            {
                                await client.Client.WriteAsync($"Hello World #{i}/{runCount}".AsBytes());
                                var response = await client.Client.ReadAsync();
                                $"{client.Index}. {i} - Client Received: {response.AsString()}".WriteFullConsoleLine(client.Index);
                            }
                            else
                                $"{client.Index}. {i} - Client disconnected".WriteFullConsoleLine(client.Index);
                            //W.Threading.Thread.Sleep(1);
                        }
                    }
                    if (Console.KeyAvailable)
                        Console.ReadKey(true);

                    for (int t = 0; t < clients.Count; t++)
                    {
                        clients[t].Client.Dispose();
                        $"{clients[t].Index} Client connection closed".WriteFullConsoleLine(clients[t].Index);
                    }
                    host.Stop();
                }
                Console.WriteLine("Host disposed");
                //W.Threading.Thread.Sleep(100);
                Console.WriteLine("Press Any Key To Return");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press Any Key To Return");
            }
            Console.ReadKey(true);
        }
        static async Task TestMaxConnections()
        {
            Console.WriteLine(nameof(TestPipeHost3));
            try
            {
                using (var host = new PipeHost())
                {
                    var count = host.Start("Test Pipe Host", 1000);
                    Console.WriteLine($"Host started with {count} connections");
                    //W.Threading.Thread.Sleep(1000);
                }
                Console.WriteLine("Host disposed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);

        }
        static async Task TestGenericPipeHost()
        {
            Console.WriteLine(nameof(TestGenericPipeHost));
            //var mre = new System.Threading.ManualResetEventSlim(false);
            try
            {
                int runCount = 1000;
                int serverCount = 25;
                using (var host = new PipeHost<IpcMessage>())
                {
                    host.MessageReceived += (h, s, b) =>
                    {
                        s.Write(b); //echo
                    };
                    host.Start("Test Pipe Host", serverCount);

                    var clients = new List<Connection<IpcMessage>>();
                    for (int t = 0; t < serverCount; t++)
                    {
                        var connection = new Connection<IpcMessage>();
                        connection.Client = new PipeClient<IpcMessage>();
                        connection.Index = t;
                        connection.Client.MessageReceived += (c, b) =>
                        {
                            $"{connection.Index}. Client received: {b.ToString()}".WriteFullConsoleLine(connection.Index);
                        };
                        clients.Add(connection);
                    }
                    foreach (var client in clients)
                    {
                        if (await client.Client.ConnectAsync(".", "Test Pipe Host", System.Security.Principal.TokenImpersonationLevel.Impersonation, 1000))
                            $"{client.Index}. Client Connected".WriteFullConsoleLine(client.Index);
                        else
                            $"{client.Index}. Client failed to connect".WriteFullConsoleLine(client.Index);
                    }
                    for (int i = 0; i < runCount; i++)
                    {
                        if (Console.KeyAvailable)
                            break;
                        var message = new IpcMessage();
                        foreach (var client in clients)
                        {
                            message.Name = $"Client {i}";
                            message.Data = BitConverter.GetBytes(i);
                            if (Console.KeyAvailable)
                                break;
                            if (client.Client.Stream.IsConnected)
                            {
                                await client.Client.WriteAsync(message);
                                var response = await client.Client.ReadAsync();
                                $"{client.Index}. {i} - Client Received: {response.ToString()}".WriteFullConsoleLine(client.Index);
                            }
                            else
                                $"{client.Index}. {i} - Client disconnected".WriteFullConsoleLine(client.Index);
                            //W.Threading.Thread.Sleep(1);
                        }
                    }
                    if (Console.KeyAvailable)
                        Console.ReadKey(true);

                    for (int t = 0; t < clients.Count; t++)
                    {
                        clients[t].Client.Dispose();
                        $"{clients[t].Index} Client connection closed".WriteFullConsoleLine(clients[t].Index);
                    }
                    host.Stop();
                }
                Console.WriteLine("Host disposed");
                //W.Threading.Thread.Sleep(100);
                Console.WriteLine("Press Any Key To Return");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press Any Key To Return");
            }
            Console.ReadKey(true);
        }
        static void Main(string[] args)
        {
            var exit = false;
            Console.CursorVisible = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Menu");
                Console.WriteLine("1.  TestPipeServer - 1 server, one client connection, 1 request/response");
                Console.WriteLine("2.  TestPipeHost - 1 host (1 server) one client connection, 100 times");
                Console.WriteLine("3.  TestPipeHost2 - 1 host (1 server), 100 client connections");
                Console.WriteLine("4.  TestPipeHost3 - 1 host (25 servers), 25 client connections, 10,000 request/response");
                Console.WriteLine("5.  TestMaxConnections- MaxConnections == -1");
                Console.WriteLine("6.  TestGenericPipeHost");

                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey(true);
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        TestPipeServer().Wait();
                        break;
                    case ConsoleKey.D2:
                        TestPipeHost().Wait();
                        break;
                    case ConsoleKey.D3:
                        TestPipeHost2().Wait();
                        break;
                    case ConsoleKey.D4:
                        TestPipeHost3().Wait();
                        break;
                    case ConsoleKey.D5:
                        TestMaxConnections().Wait();
                        break;
                    case ConsoleKey.D6:
                        TestGenericPipeHost().Wait();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }
    }

    internal static class ConsoleStringExtensions
    {
        public static void WriteToConsole(this string message, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(message);
        }
        public static void WriteFullConsoleLine(this string message, int verticalOffset = -1, char paddingChar = ' ')
        {
            int width = Console.BufferWidth;
            int lineCount = (message.Length / width) + 1;
            int index = 0;
            while (index < message.Length)
            {
                var length = Math.Min(width, message.Length - index);
                var line = message.Substring(index, length);
                line = line.PadRight(width, paddingChar);
                if (verticalOffset >= 0)
                    Console.SetCursorPosition(0, verticalOffset);
                Console.Write(line);
                index += length;
            }
        }
    }
#endif
}
