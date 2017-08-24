using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace W.Demo
{
    public static class ConsoleExtensions
    {
        public static void WriteFullConsoleLine(this string message)
        {
            int width = Console.BufferWidth;
            int lineCount = (message.Length / width) + 1;
            int index = 0;
            while (index < message.Length)
            {
                var length = Math.Min(width, message.Length - index);
                var line = message.Substring(index, length);
                line = line.PadRight(width);
                Console.Write(line);
                index += length;
            }
        }
    }
    public class TestClientSlim
    {
        public static void Run()
        {
            var useCompression = true;
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            using (var server = new W.Net.TcpServer<W.Net.BlockingSocketClient>(1000))
            {
                server.ClientConnected += (sc) =>
                {
                    //sc.UseCompression = useCompression;
                    sc.MessageReady += (c) =>
                        {
                            while (c.MessageCount > 0)
                            {
                                var msg = c.GetNextMessage();
                                var echo = msg.AsString().ToUpper().AsBytes();
                                c.Send(echo);
                            }
                        };
                };
                W.Threading.Thread.Sleep(10);
                server.Start(localEndPoint);

                var mre = new ManualResetEventSlim(false);
                var proceed = new Lockable<bool>();
                int t = 0;
                while (!Console.KeyAvailable)
                {
                    Console.SetCursorPosition(0, 0);
                    using (var client = new W.Net.BlockingSocketClient())
                    {
                        string request = string.Empty;
                        var sw = System.Diagnostics.Stopwatch.StartNew();
                        //client.UseCompression = useCompression;
                        client.MessageReady += (c) =>
                        {
                            "client.MessageReady".WriteFullConsoleLine();
                            while (c.MessageCount > 0)
                            {
                                var response = client.GetNextMessage();
                                if (response == null)
                                    System.Diagnostics.Debugger.Break();

                                var message = response.AsString();
                                string.Format("Received {0}", response.AsString()).WriteFullConsoleLine();

                                if (message != request.ToUpper())
                                    System.Diagnostics.Debugger.Break();
                            }
                            mre.Set();
                            proceed.Value = true;
                        };
                        //client.Disconnected += (c, ep, e) =>
                        //{
                        //    if (e != null)
                        //        System.Diagnostics.Debug.WriteLine("{0} Disconnected: {1}", ep, e);
                        //    else
                        //        System.Diagnostics.Debug.WriteLine("{0} Disconnected", ep);
                        //};
                        client.ConnectAsync(localEndPoint).Wait();
                        if (client.IsConnected)
                        {
                            request = string.Format("Message: {0}", t);
                            proceed.Value = false;
                            mre.Reset();
                            client.Send(request.AsBytes());
                            while (!mre.Wait(1000))
                                //while (!proceed.Value)
                                W.Threading.Thread.Sleep(1);

                            client.Disconnect();
                            string.Format("Elapsed Connection Time (ms): " + sw.ElapsedMilliseconds).WriteFullConsoleLine();
                        }
                        client.Dispose();
                        string.Format("Total Elapsed Time (ms): " + sw.ElapsedMilliseconds).WriteFullConsoleLine();
                    }

                    t += 1;
                }
            }

            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }


        internal class ClientConnection
        {
            private object _lock = new object();
            private W.Threading.Thread _thread;
            public W.Net.BlockingSocketClient Client { get; private set; }
            public ManualResetEventSlim Mre { get; set; } = new ManualResetEventSlim();
            public Lockable<string> Request { get; } = new Lockable<string>();
            public Lockable<int> Requests { get; } = new Lockable<int>();
            public Lockable<int> Responses { get; } = new Lockable<int>();
            public Lockable<Exception> Exception { get; } = new Lockable<System.Exception>();

            public string Status
            {
                get
                {
                    string result;
                    result = string.Format("{0}: {1}/{2} - {3}{4}", Client?.Name, Requests.Value, Responses.Value, Client.IsConnected ? "Connected" : "Disconnected", Exception.Value != null ? ": " + Exception.Value : "");
                    //if (Client.IsConnected)
                    //    result = string.Format("Client({0}): {1}/{2} - Connected", Client?.Name, Requests.Value, Responses.Value);
                    //else
                    //    result = string.Format("Client({0}): {1}/{2} - Disconnected: {3}", Client?.Name, Requests.Value, Responses.Value, Exception.Value.Message);
                    return result;
                }
            }
            public void Start(IPEndPoint remoteEndPoint)
            {
                if (Client != null)
                    return;
                Client = new Net.BlockingSocketClient();
                Client.Disconnected += (c, ep, e) =>
                {
                    Exception.Value = e;
                };
                Client.MessageReady += (c) =>
                {
                    lock (_lock)
                    {
                        if (c.MessageCount > 0)
                        {
                            var response = c?.GetNextMessage();
                            if (response == null)
                                System.Diagnostics.Debugger.Break();

                            var message = response.AsString();
                            if (message == Request.Value.ToUpper())
                                Responses.Value += 1;
                            else
                                Exception.Value = new System.Exception("Message Lost");
                        }
                        Mre.Set();
                    }
                };
                Client.ConnectAsync(remoteEndPoint).ContinueWith(task =>
                {
                    Mre.Reset();
                    _thread = W.Threading.Thread.Create(cts =>
                    {
                        while (!cts.IsCancellationRequested)
                        {
                            if (Client.IsConnected)
                            {
                                Request.Value = string.Format("Message: {0}", Requests);
                                Requests.Value += 1;
                                Mre.Reset();
                                Client.Send(Request.Value.AsBytes());
                                Mre.Wait();
                            }
                            //W.Threading.Thread.Sleep(1);
                        }
                        if (Client.IsConnected)
                            Client.Disconnect();
                    });
                });
            }
            public void Stop()
            {
                lock (_lock)
                {
                    if (_thread != null)
                    {
                        Mre.Set();
                        _thread.Cancel();
                        _thread.WaitForValue(t => t.IsRunning == false, 1000);
                        _thread.Dispose();
                        _thread = null;
                    }
                    Client?.Disconnect();
                    Client = null;
                }
            }

            ~ClientConnection()
            {
                Stop();
            }
        }
        public static void Run_Test_Concurrency()
        {
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            var maxClients = 10;
            var clients = new ClientConnection[maxClients];

            using (var server = new W.Net.TcpServer<W.Net.BlockingSocketClient>(15))
            {
                server.ClientConnected += (sc) =>
                {
                    //sc.UseCompression = useCompression;
                    sc.MessageReady += (c) =>
                    {
                        while (c.MessageCount > 0)
                        {
                            var msg = c.GetNextMessage();
                            var echo = msg.AsString().ToUpper().AsBytes();
                            c.Send(echo);
                        }
                    };
                };
                //W.Threading.Thread.Sleep(10);
                server.Start(localEndPoint);

                //allocate and configure the clients
                for (int t = 0; t < maxClients; t++)
                {
                    clients[t] = new ClientConnection();
                    clients[t].Start(localEndPoint);
                }

                //display status and wait for exit
                Console.CursorVisible = false;
                while (!Console.KeyAvailable)
                {
                    Console.SetCursorPosition(0, 0);

                    for (int t = 0; t < maxClients; t++)
                    {
                        clients[t].Status.WriteFullConsoleLine();
                    }
                    string.Format("Press Any Key To Return").WriteFullConsoleLine();
                    W.Threading.Thread.Sleep(1);
                }
                Console.ReadKey(true);
                Console.CursorVisible = true;

                //cleanup
                for (int t = 0; t < maxClients; t++)
                {
                    clients[t].Stop();
                }
            }
        }
    }
}
