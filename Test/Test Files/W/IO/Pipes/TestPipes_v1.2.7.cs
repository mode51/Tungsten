using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W;
using W.AsExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Pipes;
using System.Collections;
#if NET45
using System.Security.Principal;
#endif
using System.Runtime.InteropServices;

namespace W.Tests
{
    [TestClass]
    public class PipeTests
    {
        //private ITestOutputHelper output;
        //public PipeTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}

        private static int Pipe_Count = 0;
        private const string PIPENAME = "test-pipe";
        internal static string GetPipeName()
        {
            return Guid.NewGuid().ToString();
            //Pipe_Count += 1;
            //return PIPENAME + Pipe_Count.ToString();
        }

        internal static string GetLatinText()
        {
            var latin = System.IO.File.ReadAllText(@"C:\Source\Repos\Tungsten\Test\Test Files\4500_Latin_Words.txt");
            return latin;
        }

        #region Standard Pipes
        [TestMethod]
        public void Pipe_CreatePipeServer()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("pipeName = " + pipeName);
            using (var host = new W.IO.Pipes.PipeHost())
            {
                //Assert.IsTrue(server.Start(20), "Pipe Server Failed To Start");
                host.Start(pipeName, 20);
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                host.Stop();
                //server.Started += s => { s.Stop(); };
                //server.Dispose();
            }
        }
        [TestMethod]
        public void Pipe_CreatePipeServer_20Times()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("pipeName = " + pipeName);
            for (int t = 0; t < 20; t++)
            {
                using (var host = new W.IO.Pipes.PipeHost())
                {
                    //Assert.IsTrue(server.Start(20), "Pipe Server Failed To Start");
                    host.Start(pipeName, 20);
                    //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                    //host.Stop();
                    //server.Started += s => { s.Stop(); };
                    //server.Dispose();
                }
                W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
            }
        }
        [TestMethod]
        public void Pipe_CreatePipeClient()
        {
            var client = new W.IO.Pipes.PipeClient();
            client.Dispose();
        }
        [TestMethod]
        public void Pipe_SimplestClientServer()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("pipeName = " + pipeName);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.BytesReceived += (h, s, b) =>
                {
                    Console.WriteLine("Server Echoing: {0}", b.AsString());
                    //echo
                    s.Write(b);
                };
                host.Start(pipeName);
                using (var client = new W.IO.Pipes.PipeClient())
                {
                    client.BytesReceived += (o, bytes) =>
                    {
                        Console.WriteLine("Client Received: {0}", bytes.AsString());
                        mreContinue.Set();
                    };
                    Assert.IsTrue(client.Connect(pipeName));
                    client.Write("Test Message".AsBytes());
                    Assert.IsTrue(mreContinue.Wait(1000));
                }
            }
        }
        [TestMethod]
        public void Pipe_SimplestClientServer_WithCompression()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("PipeName = " + pipeName);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var bigData = new byte[1000000];
            new Random().NextBytes(bigData); //turns out these bytes don't compress
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.BytesReceived += (h, s, bytes) =>
                {
                    Console.WriteLine("Server Received {0} bytes", bytes.Length);
                    Assert.IsTrue(bytes.SequenceEqual(bigData));
                    //echo
                    s.Write(bytes);
                };
                host.UseCompression = true;
                host.Start(pipeName);
                using (var client = new W.IO.Pipes.PipeClient())
                {
                    client.BytesReceived += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received {0} bytes", bytes.Length);
                        Assert.IsTrue(bytes.SequenceEqual(bigData));
                        mreContinue.Set();
                    };
                    client.UseCompression = true;
                    Console.WriteLine("Client sending {0} bytes", bigData.Length);
                    Assert.IsTrue(client.Connect(pipeName));
                    client.Write(bigData);
                    Assert.IsTrue(mreContinue.Wait(30000));
                }
            }
        }

        [TestMethod]
        public void Pipe_CreateClientAndDispose()
        {
            var client = new W.IO.Pipes.PipeClient();
            client.Dispose();
            client = null;
        }
        [TestMethod]
        public void Pipe_CreateHostAndDispose()
        {
            var host = new W.IO.Pipes.PipeHost();
            host.Dispose();
            host = null;
        }
        [TestMethod]
        public void Pipe_PipeServer_ListenAndConnectClient()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("PipeName = " + pipeName);
            int maxConnections = 1;
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.ServerConnected += (h, s) =>
                {
                    Console.WriteLine("Server Connected");
                };
                host.ServerDisconnected += (h, s) =>
                {
                    Console.WriteLine("Server Disconnected");
                };
                host.Start(pipeName, maxConnections);
                using (var client = new W.IO.Pipes.PipeClient())
                {
                    client.Connected += (c) => { Assert.IsTrue(c == client); Console.WriteLine("Client Connected"); };
                    client.Disconnected += (c, e) => { Assert.IsTrue(c == client); Assert.IsTrue(e == null); Console.WriteLine("Client Disconnected"); };
                    Assert.IsTrue(client.Connect(pipeName, 5000), "Unable to connect to the server");
                    W.Threading.Thread.Sleep(1000);
                }
                Console.WriteLine("Client Pipe Disposed");
            }
            Console.WriteLine("Server Pipe Disposed");
        }
        [TestMethod]
        public void Pipe_Test_Uniqueness()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("PipeName = " + pipeName);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            byte[] server1Response = null;
            byte[] server2Response = null;

            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.ServerConnected += (h, s) =>
                {
                    Console.WriteLine("Server Activated");
                };
                host.BytesReceived += (h, c, bytes) =>
                {
                    //echo
                    Console.WriteLine("Server Echo: {0}", bytes.AsString());
                    c.Write(bytes);
                };
                host.Start(pipeName);

                var client1 = new W.IO.Pipes.PipeClient() { Name = "Client 1" };
                var client2 = new W.IO.Pipes.PipeClient() { Name = "Client 2" };

                client1.BytesReceived += (c, bytes) =>
                {
                    Console.WriteLine("Client1: " + bytes.AsString());
                    server1Response = bytes;
                    mreContinue.Set();
                };
                client2.BytesReceived += (c, bytes) =>
                {
                    Console.WriteLine("Client2: " + bytes.AsString());
                    server2Response = bytes;
                    mreContinue.Set();
                };

                Assert.IsTrue(client1.Connect(pipeName), "Client1 failed to connected");
                Assert.IsTrue(client2.Connect(pipeName), "Client2 failed to connected");

                //test to see if client2 receives the echo of client1's request
                mreContinue.Reset();
                client1.Write("Check 1".AsBytes());
                mreContinue.Wait();
                Assert.IsTrue(server1Response.SequenceEqual("Check 1".AsBytes()));

                mreContinue.Reset();
                client2.Write("Check 2".AsBytes());
                mreContinue.Wait();
                Assert.IsTrue(server2Response.SequenceEqual("Check 2".AsBytes()));

                client1.Dispose();
                client2.Dispose();
            }
        }
        [TestMethod]
        public void Pipe_Test_ConcurrentConnections_20()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("PipeName = " + pipeName);
            var clients = new List<W.IO.Pipes.PipeClient>();
            int maxConnections = 20;
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.Start(pipeName, maxConnections + 1);
                //make concurrent client connections
                for (int t = 0; t < maxConnections; t++)
                {
                    var newClient = new W.IO.Pipes.PipeClient();
                    var mreContinue = new System.Threading.ManualResetEventSlim(false);
                    newClient.Connected += c =>
                    {
                        Console.WriteLine("Client Pipe {0} connected", clients.Count);
                        mreContinue.Set();
                    };
                    Assert.IsTrue(newClient.Connect(pipeName, 100), "Unable to connect client #" + t.ToString() + " to the server");
                    Assert.IsTrue(mreContinue.Wait(1000));//, "Client Pipe {0} connection failed", t);
                    clients.Add(newClient);
                    Console.WriteLine("Client Pipe {0} connected", clients.Count);
                }
                //now close/dispose all the clients
                while (clients.Count > 0)
                {
                    clients[0].Dispose();
                    Console.WriteLine("Client Pipe {0} disposed", (maxConnections - clients.Count) + 1);
                    clients.Remove(clients[0]);
                }
            }
            Console.WriteLine("Server Disposed");
        }
        [TestMethod]
        public void Pipe_Test_MaxConnections_MinusOne()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("PipeName = " + pipeName);
            var clients = new List<W.IO.Pipes.PipeClient>();
            int maxConnections = -1;
            int actualConnections = 20;
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.Start(pipeName, maxConnections);
                //make concurrent client connections
                for (int t = 0; t < actualConnections; t++)
                {
                    var newClient = new W.IO.Pipes.PipeClient();
                    var mreContinue = new System.Threading.ManualResetEventSlim(false);
                    newClient.Connected += c =>
                    {
                        Console.WriteLine("Client Pipe {0} connected", clients.Count);
                        mreContinue.Set();
                    };
                    Assert.IsTrue(newClient.Connect(pipeName, 1000), "Unable to connect client #" + t.ToString() + " to the server");
                    Assert.IsTrue(mreContinue.Wait(1000));//, "Client Pipe {0} connection failed", t);
                    clients.Add(newClient);
                    Console.WriteLine("Client Pipe {0} connected", clients.Count);
                }
                //now close/dispose all the clients
                while (clients.Count > 0)
                {
                    clients[0].Dispose();
                    Console.WriteLine("Client Pipe {0} disposed", (actualConnections - clients.Count) + 1);
                    clients.Remove(clients[0]);
                }
                //W.Threading.Thread.Sleep(100);
            }
            Console.WriteLine("Server Disposed");
        }
        #endregion

        #region Generic Pipes
        public class PipeMessage
        {
            public DateTime TimeStamp { get; set; } = DateTime.Now;
            public string Message { get; set; }
            public override string ToString()
            {
                return string.Format("{0}:{1}:{2}.{3} - {4}", TimeStamp.Hour, TimeStamp.Minute, TimeStamp.Second, TimeStamp.Millisecond, Message);
            }
        }
        [TestMethod]
        public void Pipe_SimplestGenericClientServer()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("PipeName = " + pipeName);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            using (var host = new W.IO.Pipes.PipeHost<PipeMessage>())
            {
                host.BytesReceived += (h, s, b) =>
                {
                    Console.WriteLine("Server Received: {0}", b.AsString());
                };
                host.MessageReceived += (h, s, m) =>
                {
                    Console.WriteLine("Server Responding: {0}", m.ToString());
                    //respond
                    s.Write(new PipeMessage() { Message = "Server says: \"Ok to proceed\"" });
                };
                host.Start(pipeName);
                using (var client = new W.IO.Pipes.PipeClient<PipeMessage>())
                {
                    client.BytesReceived += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received: {0}", bytes.AsString());
                    };
                    client.MessageReceived += (c, message) =>
                    {
                        Console.WriteLine("Client Received: {0}", message.ToString());
                        mreContinue.Set();
                    };
                    var testMessage = new PipeMessage() { Message = "This is a test PipeMessage" };

                    Assert.IsTrue(client.Connect(pipeName));
                    client.Write(testMessage);
                    Assert.IsTrue(mreContinue.Wait(60000));
                }
            }
        }
        [TestMethod]
        public void Pipe_Transfer_200k()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("PipeName = " + pipeName);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var cts = new System.Threading.CancellationTokenSource(1000);
            var bigData = new byte[200000]; //.2mb
            //var bigData = new byte[2000000]; //2mb
            //var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.ReadProgress += (h, s, current, total) =>
                {
                    Console.WriteLine("Server Read {0} of {1} bytes", current, total);
                    cts.CancelAfter(1000);
                };
                host.BytesReceived += (h, s, b) =>
                {
                    Console.WriteLine("Server Received {0} Bytes", b.Length);
                    s.Write(b);
                };
                host.Start(pipeName);
                using (var client = new W.IO.Pipes.PipeClient())
                {
                    client.ReadProgress += (c, current, total) =>
                    {
                        Console.WriteLine("Client Read {0} of {1} bytes", current, total);
                        cts.CancelAfter(1000);
                    };
                    client.BytesReceived += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received {0} Bytes", bytes.Length);
                        mreContinue.Set();
                    };

                    Assert.IsTrue(client.Connect(pipeName));
                    client.Write(bigData);
                    mreContinue.Wait(cts.Token);
                    Assert.IsTrue(!cts.IsCancellationRequested);
                    client.WaitForWriteToComplete();
                }
            }
        }
        [TestMethod]
        public void Pipe_Transfer_2_Megabytes()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("PipeName = " + pipeName);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var cts = new System.Threading.CancellationTokenSource(1000);
            //var bigData = new byte[200000]; //.2mb
            var bigData = new byte[2000000]; //2mb
            //var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.ReadProgress += (h, s, current, total) =>
                {
                    Console.WriteLine("Server Read {0} of {1} bytes", current, total);
                    cts.CancelAfter(1000);
                };
                host.BytesReceived += (h, s, b) =>
                {
                    Console.WriteLine("Server Received {0} Bytes", b.Length);
                    s.Write(b);
                };
                host.Start(pipeName);
                using (var client = new W.IO.Pipes.PipeClient())
                {
                    client.ReadProgress += (c, current, total) =>
                    {
                        Console.WriteLine("Client Read {0} of {1} bytes", current, total);
                        cts.CancelAfter(1000);
                    };
                    client.BytesReceived += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received {0} Bytes", bytes.Length);
                        mreContinue.Set();
                    };

                    Assert.IsTrue(client.Connect(pipeName));
                    client.Write(bigData);
                    mreContinue.Wait(cts.Token);
                    Assert.IsTrue(!cts.IsCancellationRequested);
                    client.WaitForWriteToComplete();
                }
            }
        }
        [TestMethod]
        public void Pipe_Transfer_20_Megabytes()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("PipeName = " + pipeName);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var cts = new System.Threading.CancellationTokenSource(1000);
            //var bigData = new byte[200000]; //.2mb
            //var bigData = new byte[2000000]; //2mb
            var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.ReadProgress += (h, s, current, total) =>
                {
                    Console.WriteLine("Server Read {0} of {1} bytes", current, total);
                    cts.CancelAfter(1000);
                };
                host.BytesReceived += (h, s, b) =>
                {
                    Console.WriteLine("Server Received {0} Bytes", b.Length);
                    s.Write(b);
                };
                host.Start(pipeName);
                using (var client = new W.IO.Pipes.PipeClient())
                {
                    client.ReadProgress += (c, current, total) =>
                    {
                        Console.WriteLine("Client Read {0} of {1} bytes", current, total);
                        cts.CancelAfter(1000);
                    };
                    client.BytesReceived += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received {0} Bytes", bytes.Length);
                        mreContinue.Set();
                    };

                    Assert.IsTrue(client.Connect(pipeName));
                    client.Write(bigData);
                    mreContinue.Wait(cts.Token);
                    Assert.IsTrue(!cts.IsCancellationRequested);
                    client.WaitForWriteToComplete();
                }
            }
        }
        [TestMethod]
        public void Pipe_Transfer_4500_Latin_Words()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("pipeName = " + pipeName);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var cts = new System.Threading.CancellationTokenSource(1000);
            var latin = GetLatinText();
            var bigData = latin.AsBytes();
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.ReadProgress += (h, s, current, total) =>
                {
                    Console.WriteLine("Server Read {0} of {1} bytes", current, total);
                    cts.CancelAfter(1000);
                };
                host.BytesReceived += (h, s, bytes) =>
                {
                    Console.WriteLine("Server Received {0} Bytes", bytes.Length);
                    Assert.IsTrue(bytes.AsString() == latin, "The file messages are out of order");
                    s.Write(bytes);
                };
                host.Start(pipeName);
                using (var client = new W.IO.Pipes.PipeClient())
                {
                    client.ReadProgress += (c, current, total) =>
                    {
                        Console.WriteLine("Client Read {0} of {1} bytes", current, total);
                        cts.CancelAfter(1000);
                    };
                    client.BytesReceived += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received {0} Bytes", bytes.Length);
                        Assert.IsTrue(bytes.AsString() == latin, "The file messages are out of out");
                        mreContinue.Set();
                    };

                    Assert.IsTrue(client.Connect(pipeName));
                    client.Write(bigData);
                    mreContinue.Wait(cts.Token);
                    Assert.IsTrue(!cts.IsCancellationRequested);
                    client.WaitForWriteToComplete();
                }
            }
        }
        [TestMethod]
        public void Pipe_Transfer_4500_Latin_Words_Compressed()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("pipeName = " + pipeName);
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var cts = new System.Threading.CancellationTokenSource(1000);
            var latin = GetLatinText();
            var latinBytes = latin.AsBytes();
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.ReadProgress += (h, s, current, total) =>
                {
                    Console.WriteLine("Server Read {0} of {1} bytes", current, total);
                    cts.CancelAfter(1000);
                };
                host.BytesReceived += (h, s, bytes) =>
                {
                    Console.WriteLine("Server Received {0} Bytes", bytes.Length);
                    Assert.IsTrue(bytes.SequenceEqual(latinBytes), "The file messages are out of out");
                    s.Write(bytes);
                };
                host.UseCompression = true;
                host.Start(pipeName);
                using (var client = new W.IO.Pipes.PipeClient())
                {
                    client.ReadProgress += (c, current, total) =>
                    {
                        Console.WriteLine("Client Read {0} of {1} bytes", current, total);
                        cts.CancelAfter(1000);
                    };
                    client.BytesReceived += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received {0} Bytes", bytes.Length);
                        Assert.IsTrue(bytes.SequenceEqual(latinBytes), "The file messages are out of out");
                        mreContinue.Set();
                    };

                    client.UseCompression = true;
                    Console.WriteLine("Client connecting");
                    Assert.IsTrue(client.Connect(pipeName));
                    Console.WriteLine("Client writing bytes");
                    client.Write(latinBytes);
                    mreContinue.Wait(cts.Token);
                    Assert.IsTrue(!cts.IsCancellationRequested);
                    client.WaitForWriteToComplete();
                }
            }
        }
        [TestMethod]
        public void Pipe_Transfer_Aggregate1()
        {
            for (int t = 0; t < 20; t++)
            {
                Pipe_Transfer_4500_Latin_Words();
                Pipe_Transfer_4500_Latin_Words_Compressed();
            }
        }
        #endregion

        #region Logging
        [TestMethod]
        public void Pipe_NamedPipeLoggingWithHost()
        {
            var pipeName = GetPipeName();
            Console.WriteLine("PipeName = " + pipeName);
            Console.WriteLine("If this test fails, try it again.  It just might work.");
            var mreQuit = new System.Threading.ManualResetEventSlim(false);
            var count = 0L;
            using (var host = new W.IO.Pipes.PipeHost())
            {
                host.BytesReceived += (h, s, b) =>
                {
                    Interlocked.Increment(ref count);
                    var thisCount = Interlocked.Read(ref count);
                    Console.WriteLine("{0}, Server received: {1}", thisCount, b.AsString());
                    if (thisCount == 10)
                        mreQuit.Set();
                };
                host.Start(pipeName);
                Console.WriteLine("Started");

                var client = new W.IO.Pipes.PipeClient();
                {
                    Assert.IsTrue(client.Connect(pipeName));
                    for (int t = 0; t < 10; t++)
                    {
                        var message = string.Format("Test Message {0}", t);
                        client.Write(message.AsBytes());
                    }
                    W.Threading.Thread.Sleep(100);
                    Assert.IsTrue(mreQuit.Wait(5000));
                }
                client.Dispose();
                Console.WriteLine("Complete");
            }
        }
        //[TestMethod]
        //public void Pipe_NamedPipeLoggingToConsoleLogger()
        //{
        //    Console.WriteLine("If this test fails, try it again.  It just might work.");
        //    using (var client = new W.IO.Pipes.PipeClient())
        //    {
        //        Assert.IsTrue(client.Connect("ConsoleLogger"));
        //        for (int t = 0; t < 10; t++)
        //            client.Write(string.Format("Test Message {0}", t).AsBytes());
        //        client.WaitForWriteToComplete();
        //    }
        //    Console.WriteLine("Complete");
        //}
        #endregion

        #region PipeSlim
        //[TestMethod]
        //public async Task PipeSlim_CreateHost()
        //{
        //    var pipeName = GetPipeName();
        //    using (var host = W.IO.Pipes.PipeHostSlim.Create(20))
        //    {
        //        host.ServerConnected += (h, s) =>
        //        {
        //            Console.WriteLine("Server Connected");
        //        };
        //        host.ServerDisconnected += (h, s) =>
        //        {
        //            Console.WriteLine("Server Disconnected");
        //        };
        //        host.BytesReceived += (h, s, bytes) =>
        //        {
        //            //echo
        //            s.SendResponseAsync(bytes).ConfigureAwait(false);
        //        };
        //        //await Task.Delay(100);
        //    }
        //}
        //[TestMethod]
        //public async Task PipeSlim_StartStopHost()
        //{
        //    var pipeName = GetPipeName();
        //    using (var host = W.IO.Pipes.PipeHostSlim.Create(20))
        //    {
        //        host.ServerConnected += (h, s) =>
        //        {
        //            Console.WriteLine("Server Connected");
        //        };
        //        host.ServerDisconnected += (h, s) =>
        //        {
        //            Console.WriteLine("Server Disconnected");
        //        };
        //        host.BytesReceived += (h, s, bytes) =>
        //        {
        //            //echo
        //            s.SendResponseAsync(bytes).ConfigureAwait(false);
        //        };

        //        host.Start(pipeName);
        //        await Task.Delay(10);
        //        host.Stop();
        //    }
        //}
        //[TestMethod]
        //public async Task PipeSlim_Echo_Once()
        //{
        //    var pipeName = GetPipeName();
        //    using (var host = W.IO.Pipes.PipeHostSlim.Create(20))
        //    {
        //        host.ServerConnected += (h, s) =>
        //        {
        //            Console.WriteLine("Server Connected");
        //        };
        //        host.ServerDisconnected += (h, s) =>
        //        {
        //            Console.WriteLine("Server Disconnected");
        //        };
        //        host.BytesReceived += (h, s, bytes) =>
        //        {
        //            Console.WriteLine("Server Receieved: " + bytes.AsString());
        //            //echo
        //            s.SendResponseAsync(bytes).Wait();
        //        };

        //        host.Start(pipeName);

        //        using (var client = W.IO.Pipes.PipeClientSlim.CreateAsync(".", pipeName, 1000).Result)
        //        {
        //            client.BytesReceived += (c, bytes) =>
        //            {
        //                Console.WriteLine("Client Received: " + bytes.AsString());
        //            };
        //            var request = "Hello World".AsBytes();
        //            //if (client.IsConnected)
        //            {
        //                var response = await client.RequestAsync(request, CancellationToken.None);
        //                Assert.IsNotNull(response);
        //                Console.WriteLine("Server Response: " + response.AsString());
        //            }
        //        }
        //    }
        //}
        //[TestMethod]
        //public async Task PipeSlim_Echo_Many1()
        //{
        //    var pipeName = GetPipeName();
        //    using (var host = W.IO.Pipes.PipeHostSlim.Create(20))
        //    {
        //        host.ServerConnected += (h, s) =>
        //        {
        //            Console.WriteLine("Server Connected");
        //        };
        //        host.ServerDisconnected += (h, s) =>
        //        {
        //            Console.WriteLine("Server Disconnected");
        //        };
        //        host.BytesReceived += (h, s, bytes) =>
        //        {
        //            Console.WriteLine("Server Receieved: " + bytes.AsString());
        //            //echo
        //            s.SendResponseAsync(bytes).Wait();
        //        };

        //        host.Start(pipeName);

        //        using (var client = W.IO.Pipes.PipeClientSlim.CreateAsync(".", pipeName, 1000).Result)
        //        {
        //            client.BytesReceived += (c, bytes) =>
        //            {
        //                Console.WriteLine("Client Received: " + bytes.AsString());
        //            };
        //            for (int t = 0; t < 100; t++)
        //            {
        //                var request = ("Hello World " + t.ToString()).AsBytes();
        //                //if (client.IsConnected)
        //                {
        //                    var response = await client.RequestAsync(request, CancellationToken.None);
        //                    Assert.IsNotNull(response);
        //                    Console.WriteLine("Server Response: " + response.AsString());
        //                }
        //            }
        //        }
        //    }
        //}
        //[TestMethod]
        //public void PipeSlim_Echo_Many2()
        //{
        //    var mreContinue = new ManualResetEventSlim(false);
        //    var pipeName = GetPipeName();
        //    using (var host = W.IO.Pipes.PipeHostSlim.Create(101))
        //    {
        //        host.ServerConnected += (h, s) =>
        //        {
        //            Console.WriteLine("Server Connected");
        //        };
        //        host.ServerDisconnected += (h, s) =>
        //        {
        //            Console.WriteLine("Server Disconnected");
        //        };
        //        host.BytesReceived += (h, s, bytes) =>
        //        {
        //            Console.WriteLine("Server Receieved: " + bytes.AsString());
        //            //echo
        //            s.SendResponseAsync(bytes).Wait();
        //        };

        //        host.Start(pipeName);

        //        for (int t = 0; t < 10; t++)
        //        {
        //            using (var client = W.IO.Pipes.PipeClientSlim.CreateAsync(".", pipeName, 1000).Result)
        //            {
        //                client.BytesReceived += (c, bytes) =>
        //                {
        //                    Console.WriteLine("Client Received: " + bytes.AsString());
        //                };
        //                client.Disconnected += c =>
        //                {
        //                    //mreContinue.Set();
        //                };
        //                var request = ("Hello World " + t.ToString()).AsBytes();
        //                //if (client.IsConnected)
        //                {
        //                    var response = client.RequestAsync(request, CancellationToken.None).Result;
        //                    Assert.IsNotNull(response);
        //                    Console.WriteLine("Server Response: " + response.AsString());
        //                }
        //                //mreContinue.Wait();
        //            }
        //        }
        //    }
        //    mreContinue.Dispose();
        //}
        #endregion
    }

    public class ConcurrentArray<T>
    {
        private object _lockObject = new object();
        private T[] _value = new T[0];

        public T[] Bytes => _value;
        public int Length => _value.Length;

        public T[] PeekAll()
        {
            lock (_lockObject)
                return ArrayMethods.PeekStart(_value, _value.Length);
        }
        public T[] PeekStart(int length)
        {
            lock (_lockObject)
                return ArrayMethods.PeekStart(_value, length);
        }
        public T[] Peek(int startIndex, int length)
        {
            lock (_lockObject)
                return ArrayMethods.Peek(_value, startIndex, length);
        }
        public T[] PeekEnd(int length)
        {
            lock (_lockObject)
                return ArrayMethods.PeekEnd(_value, length);
        }

        public T[] TakeFromStart(int length)
        {
            lock (_lockObject)
                return ArrayMethods.TakeFromStart(ref _value, length);
        }
        public T[] Take(int startIndex, int length)
        {
            lock (_lockObject)
                return ArrayMethods.Take(ref _value, startIndex, length);
        }
        public T[] TakeFromEnd(int length)
        {
            lock (_lockObject)
                return ArrayMethods.TakeFromEnd(ref _value, length);
        }

        public T[] TrimStart(int length)
        {
            lock (_lockObject)
                return ArrayMethods.TrimStart(ref _value, length);
        }
        public T[] Trim(int startIndex, int length)
        {
            lock (_lockObject)
                return ArrayMethods.Trim(ref _value, startIndex, length);
        }
        public T[] TrimEnd(int length)
        {
            lock (_lockObject)
                return ArrayMethods.TrimEnd(ref _value, length);
        }

        public T[] Append(T[] segment)
        {
            lock (_lockObject)
                return ArrayMethods.Append(ref _value, segment);
        }

        T this[int index]
        {
            get
            {
                return Peek(index, 1)[0];
            }
        }
    }

    [TestClass]
    public partial class Pipe_Tests
    {
        private class MessageList
        {
            private LockableSlim<List<PipeHelpers.PipeMessage>> _items = new LockableSlim<List<PipeHelpers.PipeMessage>>(new List<PipeHelpers.PipeMessage>());

            public int Count
            {
                get
                {
                    int result = 0;
                    _items.InLock(list => result = list.Count);
                    return result;
                }
            }
            public void Add(PipeHelpers.PipeMessage item)
            {
                _items.InLock(list => list.Add(item));
            }
            public void Remove(PipeHelpers.PipeMessage item)
            {
                _items.InLock(list => list.Remove(item));
            }
            public void Add(PipeHelpers.PipeDatagram datagram)
            {
                _items.InLock(list =>
                {
                    var message = list.FirstOrDefault(m => m.Id == datagram.DatagramID);
                    if (message.Id == Guid.Empty)
                    {
                        message = new PipeHelpers.PipeMessage() { Id = datagram.DatagramID, TotalDataLength = datagram.TotalDataSize, Data = new byte[datagram.TotalDataSize] };
                        list.Add(message);
                    }
                    System.Buffer.BlockCopy(datagram.Data, 0, message.Data, message.ReceivedDataLength, datagram.DataLength);
                    message.ReceivedDataLength += datagram.DataLength;
                });
            }
            public PipeHelpers.PipeMessage GetNextCompleteMessage()
            {
                PipeHelpers.PipeMessage result = null;
                _items.InLock(list =>
                {
                    var r = list.FirstOrDefault(m => m.IsComplete);
                    if (r?.Id != Guid.Empty)
                        result = r;
                    if (result != null)
                        list.Remove(r);
                });
                return result;
            }
            //public PipeHelpers.PipeMessage? FirstOrDefault(Func<PipeHelpers.PipeMessage, bool> p)
            //{
            //    PipeHelpers.PipeMessage? result = null;
            //    _items.InReadLock(list => 
            //    {
            //        result = list.FirstOrDefault(p);
            //    });
            //    return result;
            //}
        }
        public class PipeDatagramReader : IDisposable
        {
            private PipeStream _stream;
            private int _datagramSize = 0;
            private W.Threading.ThreadMethod _thread;
            private CancellationTokenSource _cts = new CancellationTokenSource();
            private ManualResetEventSlim _mreReadOk = new ManualResetEventSlim(false);
            private ManualResetEventSlim _mreNotReading = new ManualResetEventSlim(false);
            private byte[] _readBuffer = null;
            private int _bytesToRead = 0;
            private int _bytesRead = 0;

            public event Action<PipeDatagramReader, byte[]> DatagramReceived;
            public int DatagramSize => _datagramSize;

            private async void ReadMessages()
            {
                var readInfo = await PipeHelpers.ReadBytesAsync(_stream, _readBuffer, _bytesToRead, 1);
                if (readInfo.Item1 == PipeHelpers.PipeReadResultEnum.Ok && readInfo.Item3 != null)
                {
                    System.Buffer.BlockCopy(readInfo.Item3, 0, _readBuffer, _bytesRead, readInfo.Item2);
                    _bytesRead += readInfo.Item2;
                    _bytesToRead -= _bytesRead;
                    Console.WriteLine("Read {0} bytes", readInfo.Item3.Length);
                }
            }
            private void AnalyzeData()
            {
                if (_bytesRead == _datagramSize)
                {
                    _bytesToRead = _datagramSize;
                    _bytesRead = 0;
                    DatagramReceived?.Invoke(this, (byte[])_readBuffer.Clone());
                    Array.Clear(_readBuffer, 0, _readBuffer.Length);
                }
            }
            private void ThreadProc()
            {
                while (!_cts.IsCancellationRequested)
                {
                    _mreReadOk.Wait();

                    _mreNotReading.Reset();
                    ReadMessages();
                    _mreNotReading.Set();

                    AnalyzeData();
                }
            }

            public void Start()
            {
                _thread.Start();
                Resume();
            }
            public void Pause()
            {
                _mreReadOk.Reset();
                _mreNotReading.Wait();
            }
            public void Resume()
            {
                _mreReadOk.Set();
            }
            public void Dispose()
            {
                _cts.Cancel();
                Resume(); //allow the threadproc to exit
                _thread.Wait();
                _cts.Dispose();
            }
            public PipeDatagramReader(PipeStream stream, int datagramSize)
            {
                _stream = stream;
                _datagramSize = datagramSize;
                _readBuffer = new byte[_datagramSize];
                _bytesToRead = _datagramSize;
                _thread = Threading.ThreadMethod.Create(ThreadProc);
            }
        }
        public class PipeReadWriteDatagrams : IDisposable
        {
            private MessageList _messages = new MessageList();
            private PipeDatagramReader _reader = null;
            private CancellationTokenSource _disposing { get; } = new CancellationTokenSource();
            private W.Threading.ThreadMethod _threadProc;
            private System.Collections.Concurrent.ConcurrentQueue<PipeHelpers.PipeMessage> _writeQueue = new System.Collections.Concurrent.ConcurrentQueue<PipeHelpers.PipeMessage>();
            private ManualResetEventSlim _writeQueueIsEmpty = new ManualResetEventSlim(false);
            private LockableSlim<bool> _disconnectedRaised = new LockableSlim<bool>(false);

            public event Action<PipeReadWriteDatagrams> Connected;
            public event Action<PipeReadWriteDatagrams> Disconnected;
            public event Action<PipeReadWriteDatagrams, byte[]> MessageReceived;

            private PipeStream _stream;

            public PipeStream Stream
            {
                get
                {
                    return _stream;
                }
                protected set
                {
                    _stream = value;
                    if (_reader != null)
                        _reader.Dispose();
                    if (value == null)
                        return;
                    _reader = new PipeDatagramReader(_stream, PipeHelpers.GetInOutBufferSize(_stream));
                    _reader.DatagramReceived += (reader, bytes) =>
                    {
                        var datagram = PipeHelpers.PipeDatagramManager.AsDatagram(bytes, _reader.DatagramSize);
                        _messages.Add(datagram);
                    };
                }
            }

            private void WriteMessages()
            {
                //write bytes (one message at a time)
                if (_writeQueue.Count > 0)
                {
                    PipeHelpers.PipeMessage message;
                    if (_writeQueue.TryDequeue(out message))
                    {
                        var datagrams = PipeHelpers.PipeDatagramManager.AsDatagrams(message.Id, message.Data, _reader.DatagramSize);
                        _reader.Pause();
                        foreach (var datagram in datagrams)
                            PipeHelpers.WriteCompleteMessageAsync(Stream, datagram.AsBytes()).Wait();
                        _reader.Resume();
                    }
                    if (_writeQueue.Count == 0)
                        _writeQueueIsEmpty.Set();
                }
            }
            private void AnalyzeMessageData()
            {
                var message = _messages.GetNextCompleteMessage();
                if (message == null)
                    return;
                RaiseMessageReceived(message.Data);
            }
            private void ThreadProc()//params object[] args)
            {
                while (!_disposing.IsCancellationRequested)
                {
                    try
                    {
                        //write bytes (one message at a time)
                        WriteMessages();

                        //analyze data (one message at a time)
                        AnalyzeMessageData();
                    }
                    catch (ObjectDisposedException)
                    {
                        RaiseDisconnected();
                    }
                    catch (Exception e) //remove this when I've handled all exceptions
                    {
                        System.Diagnostics.Debugger.Break();
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }
                    finally
                    {
                    }
                    //sleep shouldn't be necessary because of the async Read (but check the cpu use anyway)
                    //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                }
            }

            protected void RaiseConnected()
            {
                Connected?.Invoke(this);
            }
            protected void RaiseDisconnected()
            {
                if (_disconnectedRaised.Value)
                    return;
                _disconnectedRaised.InLock(v2 =>
                {
                    Disconnected?.Invoke(this);
                    return true;
                });
            }
            protected void RaiseMessageReceived(byte[] bytes)
            {
                MessageReceived?.Invoke(this, bytes);
            }
            protected void Start()
            {
                if (_disconnectedRaised.Value)
                    throw new InvalidOperationException("Cannot be restarted");
                if (Stream.As<NamedPipeServerStream>() != null)
                    Console.WriteLine("Server Started");
                else
                    Console.WriteLine("Client Started");

                _reader.Start();
                _threadProc.Start();
            }

            public void Flush()
            {
                _writeQueueIsEmpty.Wait();
            }
            public virtual void Dispose()
            {
                _disposing.Cancel();
                _reader.Dispose();
                _threadProc.Wait();
                _threadProc.Dispose();
            }
            public void Write(byte[] bytes)
            {
                _writeQueueIsEmpty.Reset();
                var message = new PipeHelpers.PipeMessage() { Id = Guid.NewGuid(), Data = bytes };
                _writeQueue.Enqueue(message);
            }
            public PipeReadWriteDatagrams() : this(null) { }
            public PipeReadWriteDatagrams(PipeStream stream)
            {
                Stream = stream;
                _threadProc = W.Threading.ThreadMethod.Create(ThreadProc);
            }
        } //PipeReadWriteDatagrams
        public class PipeReadWrite : IDisposable
        {
            private ConcurrentArray<byte> _bytes = new ConcurrentArray<byte>();
            private CancellationTokenSource _disposing { get; } = new CancellationTokenSource();
            private W.Threading.ThreadMethod _threadProc;
            private System.Collections.Concurrent.ConcurrentQueue<byte[]> _writeQueue = new System.Collections.Concurrent.ConcurrentQueue<byte[]>();
            private ManualResetEventSlim _writeQueueIsEmpty = new ManualResetEventSlim(false);
            private int _nextMessageLength = 0;
            private LockableSlim<bool> _disconnectedRaised = new LockableSlim<bool>(false);
            private byte[] _readBuffer = new byte[PipeHelpers.GetInBufferSize(null)];

            public event Action<PipeReadWrite> Connected;
            public event Action<PipeReadWrite> Disconnected;
            public event Action<PipeReadWrite, byte[]> MessageReceived;
            public PipeStream Stream { get; set; }

            private void WriteMessages()
            {
                //write bytes (one message at a time)
                if (_writeQueue.Count > 0)
                {
                    byte[] bytes;
                    if (_writeQueue.TryDequeue(out bytes))
                    {
                        PipeHelpers.WriteCompleteMessageAsync(Stream, bytes).Wait();
                    }
                    if (_writeQueue.Count == 0)
                        _writeQueueIsEmpty.Set();
                }
            }
            //private ManualResetEventSlim _mreReadOk = new ManualResetEventSlim(true);
            private object _readLock = new object();
            private async void ReadMessages()
            {
                //lock (_readLock)
                {
                    //_mreReadOk.Wait();
                    //_mreReadOk.Reset();
                    //var bytesToRead = PipeHelpers.GetInBufferSize(Stream);
                    var readInfo = await PipeHelpers.ReadBytesAsync(Stream, _readBuffer, _readBuffer.Length, 1);
                    if (readInfo.Item1 == PipeHelpers.PipeReadResultEnum.Ok && readInfo.Item3 != null)
                    {
                        _bytes.Append(readInfo.Item3);
                        Console.WriteLine("Read {0} bytes", readInfo.Item3.Length);
                    }
                }
                //_mreReadOk.Set();
            }
            private void AnalyzeMessageData()
            {
                if (_bytes.Length == 0)
                    return;
                if (_nextMessageLength == 0 && _bytes.Length >= 4)
                    _nextMessageLength = BitConverter.ToInt32(_bytes.PeekStart(4), 0);
                if (_nextMessageLength > 0 && _bytes.Length >= (_nextMessageLength + 4)) //account for the peeked size
                {
                    var bytes = _bytes.Peek(4, _nextMessageLength);
                    _bytes.TrimStart(4 + _nextMessageLength);
                    _nextMessageLength = 0;
                    RaiseMessageReceived(bytes);
                }
            }
            private void ThreadProc()//params object[] args)
            {
                while (!_disposing.IsCancellationRequested)
                {
                    try
                    {
                        //write bytes (one message at a time)
                        WriteMessages();
                        //read bytes for a maximum amount of time
                        lock (_readLock) // why?
                            ReadMessages();
                        //analyze data (one message at a time)
                        AnalyzeMessageData();
                    }
                    catch (ObjectDisposedException)
                    {
                        RaiseDisconnected();
                    }
                    catch (Exception e) //remove this when I've handled all exceptions
                    {
                        System.Diagnostics.Debugger.Break();
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }
                    finally
                    {
                    }
                    //sleep shouldn't be necessary because of the async Read (but check the cpu use anyway)
                    //W.Threading.Thread.Sleep(W.Threading.CPUProfileEnum.Sleep);
                }
            }

            protected void RaiseConnected()
            {
                Connected?.Invoke(this);
            }
            protected void RaiseDisconnected()
            {
                if (_disconnectedRaised.Value)
                    return;
                _disconnectedRaised.InLock(v2 =>
                {
                    Disconnected?.Invoke(this);
                    return true;
                });
            }
            protected void RaiseMessageReceived(byte[] bytes)
            {
                MessageReceived?.Invoke(this, bytes);
            }
            protected void Start()
            {
                if (_disconnectedRaised.Value)
                    throw new InvalidOperationException("Cannot be restarted");
                if (Stream.As<NamedPipeServerStream>() != null)
                    Console.WriteLine("Server Started");
                else
                    Console.WriteLine("Client Started");
                //_threadProc.StartAsync().ConfigureAwait(false);
                _threadProc.Start();
            }

            public void Flush()
            {
                _writeQueueIsEmpty.Wait();
            }
            public virtual void Dispose()
            {
                _disposing.Cancel();
                _threadProc.Wait();
                _threadProc.Dispose();
            }
            public void Write(byte[] bytes)
            {
                _writeQueueIsEmpty.Reset();
                _writeQueue.Enqueue(bytes);
            }
            public PipeReadWrite() : this(null) { }
            public PipeReadWrite(PipeStream stream)
            {
                Stream = stream;
                _threadProc = W.Threading.ThreadMethod.Create(ThreadProc);
            }
        } //PipeReadWrite
        public class ClientPipe : PipeReadWriteDatagrams
        {
            public void Connect()
            {
                Stream.As<NamedPipeClientStream>().Connect();
                if (Stream.IsConnected)
                {
                    Stream.ReadMode = PipeTransmissionMode.Byte;
                    Start();
                    RaiseConnected();
                }
            }
            public void Connect(int msTimeout)
            {
                Stream.As<NamedPipeClientStream>().Connect(msTimeout);
                if (Stream.IsConnected)
                {
                    Stream.ReadMode = PipeTransmissionMode.Byte;
                    Start();
                    RaiseConnected();
                }
            }
            //public async Task ConnectAsync()
            //{
            //    await Stream.As<NamedPipeClientStream>().ConnectAsync().ContinueWith(task =>
            //    {
            //        if (Stream.IsConnected)
            //        {
            //            Stream.ReadMode = PipeTransmissionMode.Byte;
            //            Start();
            //            RaiseConnected();
            //        }
            //    });
            //}
            //public async Task ConnectAsync(int msTimeout)
            //{
            //    await Stream.As<NamedPipeClientStream>().ConnectAsync(msTimeout).ContinueWith(task =>
            //    {
            //        if (Stream.IsConnected)
            //        {
            //            Stream.ReadMode = PipeTransmissionMode.Byte;
            //            Start();
            //            RaiseConnected();
            //        }
            //    });
            //}
            //public async Task ConnectAsync(CancellationToken token)
            //{
            //    await Stream.As<NamedPipeClientStream>().ConnectAsync(token).ContinueWith(task =>
            //    {
            //        if (Stream.IsConnected)
            //        {
            //            Stream.ReadMode = PipeTransmissionMode.Byte;
            //            Start();
            //            RaiseConnected();
            //        }
            //    });
            //}
            //public async Task ConnectAsync(int msTimeout, CancellationToken token)
            //{
            //    await Stream.As<NamedPipeClientStream>().ConnectAsync(msTimeout, token).ContinueWith(task =>
            //    {
            //        if (Stream.IsConnected)
            //        {
            //            Stream.ReadMode = PipeTransmissionMode.Byte;
            //            Start();
            //            RaiseConnected();
            //        }
            //    });
            //}
            public override void Dispose()
            {
                base.Dispose();
                Stream.Dispose();
                RaiseDisconnected();
            }
            public ClientPipe(string pipeName) : this(".", pipeName) { }
            public ClientPipe(string serverName, string pipeName)
            {
                Stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            }
        }
        public class ServerPipe : PipeReadWriteDatagrams
        {
            public override void Dispose()
            {
                base.Dispose();
#if NET45

                if (Stream.IsConnected)
                    Stream.As<NamedPipeServerStream>()?.Disconnect();
                Stream?.Close();
#endif
                Stream.Dispose();
                RaiseDisconnected();
            }
            public ServerPipe(string pipeName, int maxConnections)
            {
                Stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxConnections, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                Stream.As<NamedPipeServerStream>().BeginWaitForConnection(ar =>
                {
                    var server = ar.AsyncState.As<NamedPipeServerStream>();
                    try
                    {
                        server.EndWaitForConnection(ar);
                        Start();
                        RaiseConnected();
                    }
                    catch
                    {
                        RaiseDisconnected();
                    }
                }, Stream);
            }
#if NET45
            public ServerPipe(string pipeName, int maxConnections, int inBufferSize = 4096, int outBufferSize = 4096)
            {
                var pipeSecurity = new PipeSecurity();
                pipeSecurity.AddAccessRule(new PipeAccessRule(WindowsIdentity.GetCurrent().User, PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
                pipeSecurity.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null), PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
                Stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxConnections, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, inBufferSize, outBufferSize, pipeSecurity);
                Stream.As<NamedPipeServerStream>().BeginWaitForConnection(ar =>
                {
                    var server = ar.AsyncState.As<NamedPipeServerStream>();
                    try
                    {
                        server.EndWaitForConnection(ar);
                        Start();
                        RaiseConnected();
                    }
                    catch
                    {
                        RaiseDisconnected();
                    }
                }, Stream);
            }
#endif
        }
        private ServerPipe _server;
        private ClientPipe _client;

        [TestInitialize]
        public void Initialize()
        {
            _server = new ServerPipe("Pipe_Tests", 20);
            _server.Connected += server =>
            {
                Console.WriteLine("Server Connected");
            };
            _server.Disconnected += server =>
            {
                Console.WriteLine("Server Disconnected");
            };

            _client = new ClientPipe("Pipe_Tests");
            _client.Connected += client =>
            {
                Console.WriteLine("Client Connected");
            };
            _client.Disconnected += client =>
            {
                Console.WriteLine("Client Disconnected");
            };
            _client.Connect();
        }
        [TestCleanup]
        public void Cleanup()
        {
            W.Threading.Thread.Sleep(10);
            _client.Dispose();
            Console.WriteLine("Client Disposed");
            _server.Dispose();
            Console.WriteLine("Server Disposed");
        }

        [TestMethod]
        public void IsConnected()
        {
            Assert.IsTrue(_client.Stream.IsConnected);
        }
        [TestMethod]
        public void ClientToServer()
        {
            var mreContinue = new ManualResetEventSlim(false);
            _server.MessageReceived += (server, bytes) =>
            {
                Console.WriteLine("Server received: {0}", bytes.AsString());
                server.Write(bytes); //echo
            };
            _client.MessageReceived += (client, bytes) =>
            {
                Console.WriteLine("Client received: {0}", bytes.AsString());
                mreContinue.Set();
            };
            _client.Write("Test Data".AsBytes());
            mreContinue.Wait();

            Console.WriteLine("Complete");
        }
        [TestMethod]
        public void ClientToServer_10Times_NoReponse()
        {
            var mreContinue = new ManualResetEventSlim(false);
            _server.MessageReceived += (server, bytes) =>
            {
                Console.WriteLine("Server received: {0}", bytes.AsString());
                //server.Write(bytes); //echo
                mreContinue.Set();
            };
            _client.MessageReceived += (client, bytes) =>
            {
                Console.WriteLine("Client received: {0}", bytes.AsString());
            };
            for (int t = 0; t < 10; t++)
            {
                mreContinue.Reset();
                _client.Write(string.Format("Test Data {0}", t).AsBytes());
                mreContinue.Wait();
            }
            Console.WriteLine("Complete");
        }
        [TestMethod]
        public void ClientToServer_Latin()
        {
            var mreContinue = new ManualResetEventSlim(false);
            var latin = PipeHelpers.GetLatinText();
            var latinBytes = latin.AsBytes();
            _server.MessageReceived += (server, bytes) =>
            {
                Console.WriteLine("Server received {0} bytes", bytes.Length);
                for (int t = 0; t < latinBytes.Length; t++)
                {
                    if (bytes[t] != latinBytes[t])
                        System.Diagnostics.Debugger.Break();
                }
                server.Write(bytes);
            };
            _client.MessageReceived += (client, bytes) =>
            {
                Console.WriteLine("Client received {0} bytes", bytes.Length);
                for (int t = 0; t < latinBytes.Length; t++)
                {
                    if (bytes[t] != latinBytes[t])
                        System.Diagnostics.Debugger.Break();
                }
                mreContinue.Set();
            };
            _client.Write(latin.AsBytes());
            mreContinue.Wait();
            Console.WriteLine("Complete");
        }
        [TestMethod]
        public void ClientToServer_1k()
        {
            var mreContinue = new ManualResetEventSlim(false);
            var bigData = new byte[1000]; //.2mb
            //var bigData = new byte[2000000]; //2mb
            //var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);

            _server.MessageReceived += (server, bytes) =>
            {
                Console.WriteLine("Server received {0} bytes", bytes.Length);
                if (bytes.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (bytes[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                server.Write(bytes);
            };
            _client.MessageReceived += (client, bytes) =>
            {
                Console.WriteLine("Client received {0} bytes", bytes.Length);
                if (bytes.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (bytes[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                mreContinue.Set();
            };
            _client.Write(bigData);
            mreContinue.Wait();
            Console.WriteLine("Complete");
        }
        [TestMethod]
        public void ClientToServer_200k()
        {
            var mreContinue = new ManualResetEventSlim(false);
            var bigData = new byte[200000]; //.2mb
            //var bigData = new byte[2000000]; //2mb
            //var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);

            _server.MessageReceived += (server, bytes) =>
            {
                Console.WriteLine("Server received {0} bytes", bytes.Length);
                if (bytes.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (bytes[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                server.Write(bytes);
            };
            _client.MessageReceived += (client, bytes) =>
            {
                Console.WriteLine("Client received {0} bytes", bytes.Length);
                if (bytes.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (bytes[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                mreContinue.Set();
            };
            _client.Write(bigData);
            mreContinue.Wait();
            Console.WriteLine("Complete");
        }
        [TestMethod]
        public void ClientToServer_2MB()
        {
            var mreContinue = new ManualResetEventSlim(false);
            //var bigData = new byte[200000]; //.2mb
            var bigData = new byte[2000000]; //2mb
            //var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);

            _server.MessageReceived += (server, bytes) =>
            {
                Console.WriteLine("Server received {0} bytes", bytes.Length);
                if (bytes.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (bytes[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                server.Write(bytes);
            };
            _client.MessageReceived += (client, bytes) =>
            {
                Console.WriteLine("Client received {0} bytes", bytes.Length);
                if (bytes.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (bytes[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                mreContinue.Set();
            };
            _client.Write(bigData);
            mreContinue.Wait();
            Console.WriteLine("Complete");
        }
        [TestMethod]
        public void ClientToServer_20MB()
        {
            var mreContinue = new ManualResetEventSlim(false);
            //var bigData = new byte[200000]; //.2mb
            //var bigData = new byte[2000000]; //2mb
            var bigData = new byte[20000000]; //20mb
            new Random().NextBytes(bigData);

            _server.MessageReceived += (server, bytes) =>
            {
                Console.WriteLine("Server received {0} bytes", bytes.Length);
                if (bytes.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (bytes[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                server.Write(bytes);
            };
            _client.MessageReceived += (client, bytes) =>
            {
                Console.WriteLine("Client received {0} bytes", bytes.Length);
                if (bytes.Length != bigData.Length)
                    System.Diagnostics.Debugger.Break();
                for (int t = 0; t < bigData.Length; t++)
                    if (bytes[t] != bigData[t]) { System.Diagnostics.Debugger.Break(); }
                mreContinue.Set();
            };
            _client.Write(bigData);
            mreContinue.Wait();
            Console.WriteLine("Complete");
        }
    }
}
