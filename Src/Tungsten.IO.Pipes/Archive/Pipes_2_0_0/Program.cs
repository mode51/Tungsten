using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace W.IO.Pipes
{
    //#if DEBUG
    internal class IpcMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public byte[] Data { get; set; } = new byte[20];

        public IpcMessage()
        {
            var r = new Random();
            r.NextBytes(Data);
            Name = Data.AsString();
        }
    }

    internal class Program
    {
        private static string _pipeName = "Tungsten.IO.Pipes.TestPipe";
        private static bool _useCompression = false;

        private static void CreateHost1()
        {
            using (var host = new PipeHost())
            {
                "Host created".WriteFullConsoleLine();
                host.Start(_pipeName, 1, _useCompression);
                "Host started".WriteFullConsoleLine();
                System.Threading.Thread.Sleep(1000);
                host.Stop();
                "Host stopped".WriteFullConsoleLine();
            }
            "Host Disposed".WriteFullConsoleLine();
            "Press Any Key To Return".WriteFullConsoleLine();
            Console.ReadKey(true);
        }
        private static void CreateHost100()
        {
            using (var host = new PipeHost())
            {
                "Host created".WriteFullConsoleLine();
                host.Start(_pipeName, 100, _useCompression);
                "Host started".WriteFullConsoleLine();
                System.Threading.Thread.Sleep(1000);
                host.Stop();
                "Host stopped".WriteFullConsoleLine();
            }
            "Host Disposed".WriteFullConsoleLine();
            "Press Any Key To Return".WriteFullConsoleLine();
            Console.ReadKey(true);
        }
        private static void TestClient()
        {
            "Creating client".WriteFullConsoleLine();
            var result = PipeClient.CreateClient(".", _pipeName, 5000);
            if (result.Success && result.Result != null)
            {
                "Client created".WriteFullConsoleLine();
                using (result.Result)
                {
                    var client = result.Result;
                }
            }
            else
                "Failed to create client".WriteFullConsoleLine();
            "Client disposed".WriteFullConsoleLine();
            "Press Any Key To Return".WriteFullConsoleLine();
            Console.ReadKey(true);
        }
        private static void CreatePipeHostServer()
        {
            "Creating PipeHostServer".WriteFullConsoleLine();
            using (var host = new PipeHostServer())
            {
                host.Start(_pipeName, 5, _useCompression);
                "PipeHostServer created".WriteFullConsoleLine();
                System.Threading.Thread.Sleep(1000);
            }
            "PipeHostServer disposed".WriteFullConsoleLine();
            "Press Any Key To Return".WriteFullConsoleLine();
            Console.ReadKey(true);
        }
        private static void CreatePipeHostServer2()
        {
            "Creating PipeHostServer".WriteFullConsoleLine();
            using (var host = new PipeHostServer())
            {
                host.BytesReceived += (s, b) =>
                {
                    $"Server Received: {b.AsString()}".WriteFullConsoleLine();
                    s.PostAsync(b, false).Wait();
                };
                host.Start(_pipeName, 1, _useCompression);

                "PipeHostServer created".WriteFullConsoleLine();
                var clientResult = PipeClient.CreateClient(".", _pipeName, 1000);
                if (clientResult.Exception != null)
                    $"PipeClient Connection Failed: {clientResult.Exception.Message}".WriteFullConsoleLine();
                else if (clientResult.Result == null)
                    "PipeClient Connection Failed".WriteFullConsoleLine();
                else
                {
                    clientResult.Result.PostAsync("Testing".AsBytes(), false).Wait();
                    var response = clientResult.Result.WaitForMessageAsync(false, 1000).Result;
                    $"Client Received: {response.AsString()}".WriteFullConsoleLine();
                }
                System.Threading.Thread.Sleep(1000);
            }
            "PipeHostServer disposed".WriteFullConsoleLine();
            "Press Any Key To Return".WriteFullConsoleLine();
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
                Console.WriteLine("1.  Create Host (1 server)");
                Console.WriteLine("2.  Create Host (100 servers)");
                Console.WriteLine("3.  Connect Client to Host");
                Console.WriteLine("4.  Test PipeHostServer");
                Console.WriteLine("5.  Test PipeHostServer 2");

                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey(true);
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        CreateHost1();
                        break;
                    case ConsoleKey.D2:
                        CreateHost100();
                        break;
                    case ConsoleKey.D3:
                        TestClient();
                        break;
                    case ConsoleKey.D4:
                        CreatePipeHostServer();
                        break;
                    case ConsoleKey.D5:
                        CreatePipeHostServer2();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        } //main

    } //Program

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
    //#endif
}
