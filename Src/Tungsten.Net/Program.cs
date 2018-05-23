using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using W.Threading;

namespace W.Net
{
#if DEBUG && !NETSTANDARD1_3
    internal class TcpMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public int Age { get; set; }
        public byte[] Data { get; set; } = new byte[20];

        public TcpMessage()
        {
            var r = new Random();
            r.NextBytes(Data);
            Age = r.Next(1, 100);
        }
    }

    [W.Net.RPC.RPCClass()]
    public class RPCAPI
    {
        [W.Net.RPC.RPCMethod()]
        public static void Method1()
        {
            Console.WriteLine("Inside Method1");
        }
        [W.Net.RPC.RPCMethod()]
        public static int Method2()
        {
            Console.WriteLine("Inside Method2");
            return 47;
        }
        [W.Net.RPC.RPCMethod()]
        public static long Method3(long x, long y)
        {
            Console.WriteLine("Inside Method3");
            return x * y;
        }
        [W.Net.RPC.RPCMethod()]
        public static string Method4()
        {
            Console.WriteLine("Inside Method4");
            return "This is the Method4 Response";
        }
        [W.Net.RPC.RPCMethod()]
        public static string Method5(string s1, string s2)
        {
            Console.WriteLine("Inside Method5");
            return s1 + s2;
        }
        [W.Net.RPC.RPCMethod()]
        public static string Method6(string format, params object[] args)
        {
            Console.WriteLine("Inside Method6");
            return string.Format(format, args);
        }
    }

    internal class Program
    {
        private class EchoClient : IDisposable
        {
            private ThreadMethod _thread;
            private ManualResetEventSlim _continue = new ManualResetEventSlim(false);
            private Tcp.TcpClient Client { get; set; }
            private int MessageCount { get; set; }

            private void ThreadProc(CancellationToken token)
            {
                while (!token.IsCancellationRequested)
                {
                    _continue.Reset();
                    Client.Write($"Message {++MessageCount}".AsBytes());
                    _continue.Wait();
                    //ManualResetEvent provides the blocking (thread.sleep)
                    //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                }
            }
            public override string ToString()
            {
                return $"{Client.Socket.LocalEndPoint.ToString()}, MessageCount={MessageCount}";
            }
            public void Dispose()
            {
                _thread.Cancel();
                _thread.Dispose();
                _continue.Dispose();
                Client.Dispose();
            }
            public EchoClient(Tcp.TcpClient client)
            {
                Client = client;
                Client.BytesReceived += (c, b) =>
                {
                    _continue.Set();
                };
                _thread = new ThreadMethod(ThreadProc);
                _thread.Start();
            }
        }
        private class GenericEchoClient : IDisposable
        {
            private ThreadMethod _thread;
            private ManualResetEventSlim _continue = new ManualResetEventSlim(false);
            private Tcp.Generic.TcpClient<TcpMessage> Client { get; set; }
            private int MessageCount { get; set; }

            private void ThreadProc(CancellationToken token)
            {
                while (!token.IsCancellationRequested)
                {
                    _continue.Reset();
                    Client.Write(new TcpMessage() { Name = "Jordan" });
                    _continue.Wait();
                    //ManualResetEvent provides the blocking (thread.sleep)
                    //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                }
            }
            public override string ToString()
            {
                return $"{Client.Socket.LocalEndPoint.ToString()}, MessageCount={MessageCount}";
            }
            public void Dispose()
            {
                _thread.Cancel();
                _thread.Dispose();
                _continue.Dispose();
                Client.Dispose();
            }
            public GenericEchoClient(Tcp.Generic.TcpClient<TcpMessage> client)
            {
                Client = client;
                Client.MessageReceived += (c, m) =>
                {
                    MessageCount += 1;
                    _continue.Set();
                };
                _thread = new ThreadMethod(ThreadProc);
                _thread.Start();
            }
        }
        private class GenericSecureEchoClient : IDisposable
        {
            private ThreadMethod _thread;
            private ManualResetEventSlim _continue = new ManualResetEventSlim(false);
            private Tcp.Generic.SecureTcpClient<TcpMessage> Client { get; set; }
            private int MessageCount { get; set; }

            private void ThreadProc(CancellationToken token)
            {
                while (!token.IsCancellationRequested)
                {
                    _continue.Reset();
                    Client.Write(new TcpMessage() { Name = "Jordan" });
                    _continue.Wait();
                    //ManualResetEvent provides the blocking (thread.sleep)
                    //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                }
            }
            public override string ToString()
            {
                return $"{Client.Socket.LocalEndPoint.ToString()}, MessageCount={MessageCount}";
            }
            public void Dispose()
            {
                _thread.Cancel();
                _thread.Dispose();
                _continue.Dispose();
                Client.Dispose();
            }
            public GenericSecureEchoClient(Tcp.Generic.SecureTcpClient<TcpMessage> client)
            {
                Client = client;
                Client.MessageReceived += (c, m) =>
                {
                    MessageCount += 1;
                    _continue.Set();
                };
                _thread = new ThreadMethod(ThreadProc);
                _thread.Start();
            }
        }
        private static IPEndPoint _hostEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2001);
        private static int KeySize = 2048;

        private static void CreateHost()
        {
            "Creating Host".WriteFullConsoleLine(0);
            using (var host = new W.Net.Tcp.TcpHost())
            {
                "Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Listening".WriteFullConsoleLine(2);
                W.Threading.Thread.Sleep(500);
                "Disposing".WriteFullConsoleLine(3);
            }
            "Disposed".WriteFullConsoleLine(4);
            "Press Any Key To Return".WriteFullConsoleLine(5);
            Console.ReadKey(true);
        }
        private static void CreateClient()
        {
            "Creating Client".WriteFullConsoleLine(0);
            using (var client = new W.Net.Tcp.TcpClient())
            {
                "Created".WriteFullConsoleLine(1);
                W.Threading.Thread.Sleep(500);
                "Disposing".WriteFullConsoleLine(2);
            }
            "Disposed".WriteFullConsoleLine(3);
            "Press Any Key To Return".WriteFullConsoleLine(4);
            Console.ReadKey(true);
        }
        private static void TestConnections()
        {
            var connectionCount = 0;
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.TcpHost())
            {
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                while (!Console.KeyAvailable)
                {
                    using (var client = new W.Net.Tcp.TcpClient())
                    {
                        connectionCount += 1;
                        $"Client Created {connectionCount}".WriteFullConsoleLine(3);
                        //W.Threading.Thread.Sleep(500);
                        $"Connecting Client {connectionCount}".WriteFullConsoleLine(4);
                        client.Connect(_hostEndPoint);
                        $"Client Connected {connectionCount}".WriteFullConsoleLine(5);
                        $"Disposing Client {connectionCount}".WriteFullConsoleLine(6);
                    }
                    $"Client Disposed {connectionCount}".WriteFullConsoleLine(7);
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                }
                "Disposing Host".WriteFullConsoleLine(8);
            }
            "Host Disposed".WriteFullConsoleLine(9);
            "Press Any Key To Return".WriteFullConsoleLine(10);
            Console.ReadKey(true);
        }
        private static void TestEcho()
        {
            var messageCount = 0;
            var mreContinue = new ManualResetEventSlim(false);
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.TcpHost())
            {
                host.BytesReceived += (h, s, bytes) =>
                {
                    s.Write(bytes);//simple echo
                };
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                using (var client = new W.Net.Tcp.TcpClient())
                {
                    "Client Created".WriteFullConsoleLine(3);
                    client.BytesReceived += (c, b) =>
                    {
                        if (b.AsString() != $"Message {messageCount}")
                            System.Diagnostics.Debugger.Break();
                        mreContinue.Set();
                    };
                    "Connecting Client".WriteFullConsoleLine(4);
                    client.Connect(_hostEndPoint);
                    "Client Connected".WriteFullConsoleLine(5);
                    while (!Console.KeyAvailable)
                    {
                        //W.Threading.Thread.Sleep(500);
                        mreContinue.Reset();
                        client.Write($"Message {++messageCount}".AsBytes());
                        mreContinue.Wait();
                        $"Message Looped {messageCount}".WriteFullConsoleLine(6);
                        W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                    }
                    "Disposing Client".WriteFullConsoleLine(7);
                }
                "Client Disposed".WriteFullConsoleLine(8);
                "Disposing Host".WriteFullConsoleLine(9);
            }
            "Host Disposed".WriteFullConsoleLine(10);
            "Press Any Key To Return".WriteFullConsoleLine(11);
            mreContinue.Dispose();
            Console.ReadKey(true);
        }
        private static void TestConcurrentConnections()
        {
            var clients = new EchoClient[20];
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.TcpHost())
            {
                host.BytesReceived += (h, s, b) =>
                {
                    s.Write(b);//simple echo
                };
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                for (int t = 0; t < clients.Length; t++)
                {
                    var newClient = new Tcp.TcpClient();
                    newClient.Connect(_hostEndPoint);
                    clients[t] = new EchoClient(newClient);
                }

                while (!Console.KeyAvailable)
                {
                    for (int t = 0; t < clients.Length; t++)
                    {
                        $"Client {clients[t].ToString()}".WriteFullConsoleLine(t + 3);
                    }
                    //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                }

                for (int t = 0; t < clients.Length; t++)
                {
                    clients[t].Dispose();
                }
                "Disposing Host".WriteFullConsoleLine(14);
            }
            "Host Disposed".WriteFullConsoleLine(15);
            "Press Any Key To Return".WriteFullConsoleLine(16);
            Console.ReadKey(true);
        }

        private static void CreateGenericHost()
        {
            "Creating Host".WriteFullConsoleLine(0);
            using (var host = new W.Net.Tcp.Generic.TcpHost<TcpMessage>())
            {
                "Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Listening".WriteFullConsoleLine(2);
                W.Threading.Thread.Sleep(500);
                "Disposing".WriteFullConsoleLine(3);
            }
            "Disposed".WriteFullConsoleLine(4);
            "Press Any Key To Return".WriteFullConsoleLine(5);
            Console.ReadKey(true);
        }
        private static void CreateGenericClient()
        {
            "Creating Client".WriteFullConsoleLine(0);
            using (var client = new W.Net.Tcp.Generic.TcpClient<TcpMessage>())
            {
                "Created".WriteFullConsoleLine(1);
                W.Threading.Thread.Sleep(500);
                "Disposing".WriteFullConsoleLine(2);
            }
            "Disposed".WriteFullConsoleLine(3);
            "Press Any Key To Return".WriteFullConsoleLine(4);
            Console.ReadKey(true);
        }
        private static void TestGenericConnections()
        {
            var connectionCount = 0;
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.Generic.TcpHost<TcpMessage>())
            {
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                while (!Console.KeyAvailable)
                {
                    using (var client = new W.Net.Tcp.Generic.TcpClient<TcpMessage>())
                    {
                        connectionCount += 1;
                        $"Client Created {connectionCount}".WriteFullConsoleLine(3);
                        //W.Threading.Thread.Sleep(500);
                        $"Connecting Client {connectionCount}".WriteFullConsoleLine(4);
                        client.Connect(_hostEndPoint);
                        $"Client Connected {connectionCount}".WriteFullConsoleLine(5);
                        $"Disposing Client {connectionCount}".WriteFullConsoleLine(6);
                    }
                    $"Client Disposed {connectionCount}".WriteFullConsoleLine(7);
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                }
                "Disposing Host".WriteFullConsoleLine(8);
            }
            "Host Disposed".WriteFullConsoleLine(9);
            "Press Any Key To Return".WriteFullConsoleLine(10);
            Console.ReadKey(true);
        }
        private static void TestGenericEcho()
        {
            var messageCount = 0;
            var mreContinue = new ManualResetEventSlim(false);
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.Generic.TcpHost<TcpMessage>())
            {
                host.MessageReceived += (h, s, message) =>
                {
                    s.Write(message);//simple echo
                };
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                using (var client = new W.Net.Tcp.Generic.TcpClient<TcpMessage>())
                {
                    "Client Created".WriteFullConsoleLine(3);
                    client.MessageReceived += (c, m) =>
                    {
                        //if (m.Data != $"Message {messageCount}")
                        //System.Diagnostics.Debugger.Break();
                        mreContinue.Set();
                    };
                    "Connecting Client".WriteFullConsoleLine(4);
                    client.Connect(_hostEndPoint);
                    "Client Connected".WriteFullConsoleLine(5);
                    while (!Console.KeyAvailable)
                    {
                        //W.Threading.Thread.Sleep(500);
                        mreContinue.Reset();
                        client.Write(new TcpMessage() { Name = "Jordan" });
                        messageCount += 1;
                        mreContinue.Wait();
                        $"Message Looped {messageCount}".WriteFullConsoleLine(6);
                        W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                    }
                    "Disposing Client".WriteFullConsoleLine(7);
                }
                "Client Disposed".WriteFullConsoleLine(8);
                "Disposing Host".WriteFullConsoleLine(9);
            }
            "Host Disposed".WriteFullConsoleLine(10);
            "Press Any Key To Return".WriteFullConsoleLine(11);
            mreContinue.Dispose();
            Console.ReadKey(true);
        }
        private static void TestGenericConcurrentConnections()
        {
            var clients = new GenericEchoClient[20];
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.Generic.TcpHost<TcpMessage>())
            {
                host.MessageReceived += (h, s, message) =>
                {
                    s.Write(message);//simple echo
                };
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                for (int t = 0; t < clients.Length; t++)
                {
                    var newClient = new Tcp.Generic.TcpClient<TcpMessage>();
                    newClient.Connect(_hostEndPoint);
                    clients[t] = new GenericEchoClient(newClient);
                }

                while (!Console.KeyAvailable)
                {
                    for (int t = 0; t < clients.Length; t++)
                    {
                        $"Client {clients[t].ToString()}".WriteFullConsoleLine(t + 3);
                    }
                    //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                }

                for (int t = 0; t < clients.Length; t++)
                {
                    clients[t].Dispose();
                }
                "Disposing Host".WriteFullConsoleLine(14);
            }
            "Host Disposed".WriteFullConsoleLine(15);
            "Press Any Key To Return".WriteFullConsoleLine(16);
            Console.ReadKey(true);
        }

        private static void CreateSecureHost()
        {
            "Creating Host".WriteFullConsoleLine(0);
            using (var host = new W.Net.Tcp.SecureTcpHost(2048))
            {
                "Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Listening".WriteFullConsoleLine(2);
                W.Threading.Thread.Sleep(500);
                "Disposing".WriteFullConsoleLine(3);
            }
            "Disposed".WriteFullConsoleLine(4);
            "Press Any Key To Return".WriteFullConsoleLine(5);
            Console.ReadKey(true);
        }
        private static void CreateSecureClient()
        {
            "Creating Client".WriteFullConsoleLine(0);
            using (var client = new W.Net.Tcp.SecureTcpClient(2048))
            {
                "Created".WriteFullConsoleLine(1);
                W.Threading.Thread.Sleep(500);
                "Disposing".WriteFullConsoleLine(2);
            }
            "Disposed".WriteFullConsoleLine(3);
            "Press Any Key To Return".WriteFullConsoleLine(4);
            Console.ReadKey(true);
        }
        private static void TestSecureConnections()
        {
            var connectionCount = 0;
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.SecureTcpHost(2048))
            {
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                while (!Console.KeyAvailable)
                {
                    using (var client = new W.Net.Tcp.SecureTcpClient(2048))
                    {
                        connectionCount += 1;
                        $"Client Created {connectionCount}".WriteFullConsoleLine(3);
                        //W.Threading.Thread.Sleep(500);
                        $"Connecting Client {connectionCount}".WriteFullConsoleLine(4);
                        client.Connect(_hostEndPoint);
                        $"Client Connected {connectionCount}".WriteFullConsoleLine(5);
                        $"Disposing Client {connectionCount}".WriteFullConsoleLine(6);
                    }
                    $"Client Disposed {connectionCount}".WriteFullConsoleLine(7);
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                }
                "Disposing Host".WriteFullConsoleLine(8);
            }
            "Host Disposed".WriteFullConsoleLine(9);
            "Press Any Key To Return".WriteFullConsoleLine(10);
            Console.ReadKey(true);
        }
        private static void TestSecureEcho()
        {
            var messageCount = 0;
            var mreContinue = new ManualResetEventSlim(false);
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.SecureTcpHost(2048))
            {
                host.BytesReceived += (h, s, bytes) =>
                {
                    s.Write(bytes);//simple echo
                };
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                using (var client = new W.Net.Tcp.SecureTcpClient(2048))
                {
                    "Client Created".WriteFullConsoleLine(3);
                    client.BytesReceived += (c, b) =>
                    {
                        if (b.AsString() != $"Message {messageCount}")
                            System.Diagnostics.Debugger.Break();
                        mreContinue.Set();
                    };
                    "Connecting Client".WriteFullConsoleLine(4);
                    client.Connect(_hostEndPoint);
                    "Client Connected".WriteFullConsoleLine(5);
                    while (!Console.KeyAvailable)
                    {
                        //W.Threading.Thread.Sleep(500);
                        mreContinue.Reset();
                        client.Write($"Message {++messageCount}".AsBytes());
                        mreContinue.Wait();
                        $"Message Looped {messageCount}".WriteFullConsoleLine(6);
                        W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                    }
                    "Disposing Client".WriteFullConsoleLine(7);
                }
                "Client Disposed".WriteFullConsoleLine(8);
                "Disposing Host".WriteFullConsoleLine(9);
            }
            "Host Disposed".WriteFullConsoleLine(10);
            "Press Any Key To Return".WriteFullConsoleLine(11);
            mreContinue.Dispose();
            Console.ReadKey(true);
        }
        private static void TestSecureConcurrentConnections()
        {
            var clients = new EchoClient[20];
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.SecureTcpHost(2048))
            {
                host.BytesReceived += (h, s, b) =>
                {
                    s.Write(b);//simple echo
                };
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                for (int t = 0; t < clients.Length; t++)
                {
                    var newClient = new Tcp.SecureTcpClient(2048);
                    newClient.Connect(_hostEndPoint);
                    clients[t] = new EchoClient(newClient);
                    $"Client {clients[t].ToString()}".WriteFullConsoleLine(t + 3);
                }

                while (!Console.KeyAvailable)
                {
                    for (int t = 0; t < clients.Length; t++)
                    {
                        $"Client {clients[t].ToString()}".WriteFullConsoleLine(t + 3);
                    }
                    //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.SpinWait1);
                }

                for (int t = 0; t < clients.Length; t++)
                {
                    clients[t].Dispose();
                }
                "Disposing Host".WriteFullConsoleLine(14);
            }
            "Host Disposed".WriteFullConsoleLine(15);
            "Press Any Key To Return".WriteFullConsoleLine(16);
            Console.ReadKey(true);
        }

        private static void CreateGenericSecureHost()
        {
            "Creating Host".WriteFullConsoleLine(0);
            using (var host = new W.Net.Tcp.Generic.SecureTcpHost<TcpMessage>(2048))
            {
                "Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Listening".WriteFullConsoleLine(2);
                W.Threading.Thread.Sleep(500);
                "Disposing".WriteFullConsoleLine(3);
            }
            "Disposed".WriteFullConsoleLine(4);
            "Press Any Key To Return".WriteFullConsoleLine(5);
            Console.ReadKey(true);
        }
        private static void CreateGenericSecureClient()
        {
            "Creating Client".WriteFullConsoleLine(0);
            using (var client = new W.Net.Tcp.Generic.SecureTcpClient<TcpMessage>(2048))
            {
                "Created".WriteFullConsoleLine(1);
                W.Threading.Thread.Sleep(500);
                "Disposing".WriteFullConsoleLine(2);
            }
            "Disposed".WriteFullConsoleLine(3);
            "Press Any Key To Return".WriteFullConsoleLine(4);
            Console.ReadKey(true);
        }
        private static void TestGenericSecureConnections()
        {
            var connectionCount = 0;
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.Generic.SecureTcpHost<TcpMessage>(2048))
            {
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                while (!Console.KeyAvailable)
                {
                    using (var client = new W.Net.Tcp.Generic.SecureTcpClient<TcpMessage>(2048))
                    {
                        connectionCount += 1;
                        $"Client Created {connectionCount}".WriteFullConsoleLine(3);
                        //W.Threading.Thread.Sleep(500);
                        $"Connecting Client {connectionCount}".WriteFullConsoleLine(4);
                        client.Connect(_hostEndPoint);
                        $"Client Connected {connectionCount}".WriteFullConsoleLine(5);
                        $"Disposing Client {connectionCount}".WriteFullConsoleLine(6);
                    }
                    $"Client Disposed {connectionCount}".WriteFullConsoleLine(7);
                    W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                }
                "Disposing Host".WriteFullConsoleLine(8);
            }
            "Host Disposed".WriteFullConsoleLine(9);
            "Press Any Key To Return".WriteFullConsoleLine(10);
            Console.ReadKey(true);
        }
        private static void TestGenericSecureEcho()
        {
            var messageCount = 0;
            var mreContinue = new ManualResetEventSlim(false);
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.Generic.SecureTcpHost<TcpMessage>(2048))
            {
                host.MessageReceived += (h, s, message) =>
                {
                    s.Write(message);//simple echo
                };
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                using (var client = new W.Net.Tcp.Generic.SecureTcpClient<TcpMessage>(2048))
                {
                    "Client Created".WriteFullConsoleLine(3);
                    client.MessageReceived += (c, m) =>
                    {
                        //if (m.Data != $"Message {messageCount}")
                        //System.Diagnostics.Debugger.Break();
                        mreContinue.Set();
                    };
                    "Connecting Client".WriteFullConsoleLine(4);
                    client.Connect(_hostEndPoint);
                    "Client Connected".WriteFullConsoleLine(5);
                    while (!Console.KeyAvailable)
                    {
                        //W.Threading.Thread.Sleep(500);
                        mreContinue.Reset();
                        client.Write(new TcpMessage() { Name = "Jordan" });
                        messageCount += 1;
                        mreContinue.Wait();
                        $"Message Looped {messageCount}".WriteFullConsoleLine(6);
                        W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                    }
                    "Disposing Client".WriteFullConsoleLine(7);
                }
                "Client Disposed".WriteFullConsoleLine(8);
                "Disposing Host".WriteFullConsoleLine(9);
            }
            "Host Disposed".WriteFullConsoleLine(10);
            "Press Any Key To Return".WriteFullConsoleLine(11);
            mreContinue.Dispose();
            Console.ReadKey(true);
        }
        private static void TestGenericSecureConcurrentConnections()
        {
            var clients = new GenericSecureEchoClient[20];
            "Creating Host".WriteToConsole(0, 0);
            using (var host = new W.Net.Tcp.Generic.SecureTcpHost<TcpMessage>(2048))
            {
                host.MessageReceived += (h, s, message) =>
                {
                    s.Write(message);//simple echo
                };
                "Host Created".WriteFullConsoleLine(1);
                host.Listen(_hostEndPoint);
                "Host Listening".WriteFullConsoleLine(2);

                for (int t = 0; t < clients.Length; t++)
                {
                    var newClient = new Tcp.Generic.SecureTcpClient<TcpMessage>(2048);
                    newClient.Connect(_hostEndPoint);
                    clients[t] = new GenericSecureEchoClient(newClient);
                }

                while (!Console.KeyAvailable)
                {
                    for (int t = 0; t < clients.Length; t++)
                    {
                        $"Client {clients[t].ToString()}".WriteFullConsoleLine(t + 3);
                    }
                    //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                }

                for (int t = 0; t < clients.Length; t++)
                {
                    clients[t].Dispose();
                }
                "Disposing Host".WriteFullConsoleLine(14);
            }
            "Host Disposed".WriteFullConsoleLine(15);
            "Press Any Key To Return".WriteFullConsoleLine(16);
            Console.ReadKey(true);
        }

#if !NETSTANDARD1_3
        private static void RPC_CreateServer()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(_hostEndPoint, System.Reflection.Assembly.GetEntryAssembly(), false);
                Console.WriteLine("Server Started");
            }
            Console.WriteLine("Server Disposed");
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        private static void RPC_CreateClient()
        {
            using (var client = new W.Net.RPC.Client(_hostEndPoint, KeySize))
            {
                Console.WriteLine("Client Created");
            }
            Console.WriteLine("Client Disposed");
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        private static void RPC_FindMethods()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                server.API.Refresh(System.Reflection.Assembly.GetEntryAssembly(), false);
                foreach (var method in server.API.Methods)
                    Console.WriteLine($"Found Method: {method.Key}");
            }
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        private static void RPC_CallMethod1()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(_hostEndPoint, System.Reflection.Assembly.GetEntryAssembly(), false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(_hostEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Net.RPCAPI.Method1");
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        private static void RPC_CallMethod2()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(_hostEndPoint, System.Reflection.Assembly.GetEntryAssembly(), false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(_hostEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Net.RPCAPI.Method2");
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        private static void RPC_CallMethod3()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(_hostEndPoint, System.Reflection.Assembly.GetEntryAssembly(), false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(_hostEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Net.RPCAPI.Method3", 2, 7);
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        private static void RPC_CallMethod4()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(_hostEndPoint, System.Reflection.Assembly.GetEntryAssembly(), false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(_hostEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Net.RPCAPI.Method4");
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        private static void RPC_CallMethod5()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(_hostEndPoint, System.Reflection.Assembly.GetEntryAssembly(), false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(_hostEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Net.RPCAPI.Method5", "Duerksen", ", Jordan");
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        private static void RPC_CallMethod6a()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(_hostEndPoint, System.Reflection.Assembly.GetEntryAssembly(), false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(_hostEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Net.RPCAPI.Method6", "{0}. {1}", new object[] { 1, " Testing a formatted string" });
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        private static void RPC_CallMethod6b()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(_hostEndPoint, System.Reflection.Assembly.GetEntryAssembly(), false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(_hostEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Net.RPCAPI.Method6", "{0}. {1}", 1, " Testing a formatted string");
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
        private static void RPC_CallMethod6c()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(_hostEndPoint, System.Reflection.Assembly.GetEntryAssembly(), false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(_hostEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Net.RPCAPI.Method6", 1, " Testing wrong number of arguments");
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey(true);
        }
#endif

        private static void InsecureMenu()
        {
            var exit = false;
            Console.CursorVisible = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Insecure Tcp Menu");
                Console.WriteLine("1.  Create TcpHost");
                Console.WriteLine("2.  Create TcpClient");
                Console.WriteLine("3.  Connect");
                Console.WriteLine("4.  Test Echo");
                Console.WriteLine("5.  Test Concurrent Connections");

                Console.WriteLine("Press <Escape> To Return");
                var selection = Console.ReadKey(true);
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        CreateHost();
                        break;
                    case ConsoleKey.D2:
                        CreateClient();
                        break;
                    case ConsoleKey.D3:
                        TestConnections();
                        break;
                    case ConsoleKey.D4:
                        TestEcho();
                        break;
                    case ConsoleKey.D5:
                        TestConcurrentConnections();
                        break;

                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }
        private static void InsecureGenericMenu()
        {
            var exit = false;
            Console.CursorVisible = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Insecure Tcp.Generic Menu");
                Console.WriteLine("1.  Create Generic.TcpHost");
                Console.WriteLine("2.  Create Generic.TcpClient");
                Console.WriteLine("3.  Connect Generic");
                Console.WriteLine("4.  Test Generic Echo");
                Console.WriteLine("5.  Test Concurrent Generic Connections");

                Console.WriteLine("Press <Escape> To Return");
                var selection = Console.ReadKey(true);
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        CreateGenericHost();
                        break;
                    case ConsoleKey.D2:
                        CreateGenericClient();
                        break;
                    case ConsoleKey.D3:
                        TestGenericConnections();
                        break;
                    case ConsoleKey.D4:
                        TestGenericEcho();
                        break;
                    case ConsoleKey.D5:
                        TestGenericConcurrentConnections();
                        break;

                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }

        private static void SecureMenu()
        {
            var exit = false;
            Console.CursorVisible = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Secure Tcp Menu");
                Console.WriteLine("1.  Create SecureTcpHost");
                Console.WriteLine("2.  Create SecureTcpClient");
                Console.WriteLine("3.  Connect Secure");
                Console.WriteLine("4.  Test Secure Echo");
                Console.WriteLine("5.  Test Secure Concurrent Connections");

                Console.WriteLine("Press <Escape> To Return");
                var selection = Console.ReadKey(true);
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        CreateSecureHost();
                        break;
                    case ConsoleKey.D2:
                        CreateSecureClient();
                        break;
                    case ConsoleKey.D3:
                        TestSecureConnections();
                        break;
                    case ConsoleKey.D4:
                        TestSecureEcho();
                        break;
                    case ConsoleKey.D5:
                        TestSecureConcurrentConnections();
                        break;

                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }
        private static void SecureGenericMenu()
        {
            var exit = false;
            Console.CursorVisible = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Secure Tcp.Generic Menu");
                Console.WriteLine("1.  Create Generic.SecureTcpHost");
                Console.WriteLine("2.  Create Generic.SecureTcpClient");
                Console.WriteLine("3.  Connect Generic Secure");
                Console.WriteLine("4.  Test Generic Secure Echo");
                Console.WriteLine("5.  Test Concurrent Generic Secure Connections");

                Console.WriteLine("Press <Escape> To Return");
                var selection = Console.ReadKey(true);
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        CreateGenericSecureHost();
                        break;
                    case ConsoleKey.D2:
                        CreateGenericSecureClient();
                        break;
                    case ConsoleKey.D3:
                        TestGenericSecureConnections();
                        break;
                    case ConsoleKey.D4:
                        TestGenericSecureEcho();
                        break;
                    case ConsoleKey.D5:
                        TestGenericSecureConcurrentConnections();
                        break;

                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }
        private static void RPCMenu()
        {
            var exit = false;
            Console.CursorVisible = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("RPC Menu");
                Console.WriteLine("1.  Create Server");
                Console.WriteLine("2.  Create Client");
                Console.WriteLine("3.  Server Find Methods");
                Console.WriteLine("4.  Call void Method1");
                Console.WriteLine("5.  Call function Method2");
                Console.WriteLine("6.  Call function Method3 with parameters");
                Console.WriteLine("7.  Call function Method4 no parameters");
                Console.WriteLine("8.  Call function Method5 with parameters");
                Console.WriteLine("9.  Call function Method6 with array");
                Console.WriteLine("a.  Call function Method6 with params array");
                Console.WriteLine("b.  Call function Method6 with wrong number of arguments");

                Console.WriteLine("Press <Escape> To Return");
                var selection = Console.ReadKey(true);
                Console.Clear();
                switch (selection.Key)
                {
#if !NETSTANDARD1_3
                    case ConsoleKey.D1:
                        RPC_CreateServer();
                        break;
                    case ConsoleKey.D2:
                        RPC_CreateClient();
                        break;
                    case ConsoleKey.D3:
                        RPC_FindMethods();
                        break;
                    case ConsoleKey.D4:
                        RPC_CallMethod1();
                        break;
                    case ConsoleKey.D5:
                        RPC_CallMethod2();
                        break;
                    case ConsoleKey.D6:
                        RPC_CallMethod3();
                        break;
                    case ConsoleKey.D7:
                        RPC_CallMethod4();
                        break;
                    case ConsoleKey.D8:
                        RPC_CallMethod5();
                        break;
                    case ConsoleKey.D9:
                        RPC_CallMethod6a();
                        break;
                    case ConsoleKey.A:
                        RPC_CallMethod6b();
                        break;
                    case ConsoleKey.B:
                        RPC_CallMethod6c();
                        break;
#endif

                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            var exit = false;
            Console.CursorVisible = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Menu");
                Console.WriteLine("1.  Test Insecure Tcp");
                Console.WriteLine("2.  Test Insecure Tcp.Generic");
                Console.WriteLine("3.  Test Secure Tcp");
                Console.WriteLine("4.  Test Secure Tcp.Generic");
                Console.WriteLine("5.  Test RPC");

                Console.WriteLine("Press <Escape> To Exit");
                var selection = Console.ReadKey(true);
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.D1:
                        InsecureMenu();
                        break;
                    case ConsoleKey.D2:
                        InsecureGenericMenu();
                        break;
                    case ConsoleKey.D3:
                        SecureMenu();
                        break;
                    case ConsoleKey.D4:
                        SecureGenericMenu();
                        break;
                    case ConsoleKey.D5:
                        RPCMenu();
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
#endif
}
