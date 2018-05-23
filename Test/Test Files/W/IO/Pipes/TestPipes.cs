using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Pipes;
using System.Threading.Tasks;
using W.IO.Pipes;

namespace W.Tests
{
    internal class Message
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Information { get; set; }
        public override string ToString()
        {
            return $"{Timestamp.ToString()}: {Information}";
        }
    }

    internal class Helpers
    {
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
    }
    [TestClass]
    public class TestPipes
    {
        private static object _consoleLock = new object();
        private string pipeName = Helpers.GetPipeName();

        private static void WriteLine(string format, params object[] args)
        {
            lock (_consoleLock)
                Console.WriteLine(format, args);
        }
        //private void RunInServer(string pipeName, Action action)
        //{
        //    using (var server = new PipeServer())
        //    {
        //        server.Disconnected += (s, e) =>
        //        {
        //            WriteLine("Client disconnected from server");
        //        };
        //        server.MessageReceived += (s, bytes) =>
        //        {
        //            //do something with the data
        //            WriteLine($"Server Received {bytes.AsString()}");
        //            //then respond
        //            s.WriteAsync("ExitNow".AsBytes()).Wait();
        //        };
        //        if (server.WaitForConnection(pipeName, 1))
        //        {
        //            server.Listen();

        //            try
        //            {
        //                action?.Invoke();
        //            }
        //            catch (Exception e)
        //            {
        //                System.Diagnostics.Debugger.Break();
        //            }
        //        }
        //    }
        //}
        //private void RunInHost(string pipeName, Action action)
        //{
        //    using (var _host = new PipeHost())
        //    {
        //        _host.MessageReceived += (h, s, bytes) =>
        //        {
        //            //do something with the data
        //            WriteLine($"Server Received {bytes.AsString()}");
        //            //then respond
        //            s.WriteAsync("ExitNow".AsBytes()).Wait();
        //        };
        //        _host.Start(pipeName, 1);

        //        try
        //        {
        //            action?.Invoke();
        //        }
        //        catch (Exception e)
        //        {
        //            System.Diagnostics.Debugger.Break();
        //        }
        //    }
        //}

        [TestMethod]
        public void SingleRequest()
        {
            var pipe = pipeName + "1";
            using (var _host = new PipeHost())
            {
                _host.MessageReceived += (h, s, bytes) =>
                {
                    //do something with the data
                    WriteLine($"Server Received {bytes.AsString()}");
                    //then respond
                    s.WriteAsync("ExitNow".AsBytes()).Wait();
                };
                _host.Start(pipe, 1);

                try
                {
                    using (var client = new PipeClient())
                    {
                        if (client.Connect(".", pipe, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                        {
                            //make a request
                            //var response = await client.RequestAsync("Retrieve user data".AsBytes(), _useCompression, 5000);
                            client.WriteAsync("Retrieve user data".AsBytes()).Wait();
                            var response = client.ReadAsync().Result;
                            //do something with the response
                            WriteLine($"Client Received: {response?.AsString() ?? "null"}");
                        }
                        else
                            WriteLine("Unable to connect to the pipe host");
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }
        [TestMethod]
        public async Task SingleRequestAsync()
        {
            var pipe = pipeName + "1";
            using (var _host = new PipeHost())
            {
                _host.MessageReceived += (h, s, bytes) =>
                {
                    //do something with the data
                    WriteLine($"Server Received {bytes.AsString()}");
                    //then respond
                    s.WriteAsync("ExitNow".AsBytes()).Wait();
                };
                _host.Start(pipe, 1);

                try
                {
                    using (var client = new PipeClient())
                    {
                        if (!client.Connect(".", pipe, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                            WriteLine("Unable to connect to the pipe host");
                        else
                        {
                            //make a request
                            await client.WriteAsync("Retrieve user data".AsBytes());
                            var response = await client.ReadAsync();
                            //do something with the response
                            WriteLine($"Client Received: {response?.AsString() ?? "null"}");
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }
        [TestMethod]
        public void MultipleRequests()
        {
            var pipe = pipeName + "2";
            using (var _host = new PipeHost())
            {
                _host.MessageReceived += (h, s, bytes) =>
                {
                    //do something with the data
                    WriteLine($"Server Received {bytes.AsString()}");
                    //then respond
                    s.WriteAsync("ExitNow".AsBytes()).Wait();
                };
                _host.Start(pipe, 1);
                try
                {
                    using (var client = new PipeClient())
                    {
                        if (!client.Connect(".", pipe, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                            WriteLine("Unable to connect to the pipe host");
                        else
                        {
                            for (int t = 0; t < 100; t++)
                            {
                                //make a request
                                client.WriteAsync($"{t}. Retrieve user data".AsBytes()).Wait();
                                var response = client.ReadAsync().Result;
                                //do something with the response
                                WriteLine($"Client Received: {response?.AsString() ?? "null"}");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }
        [TestMethod]
        public void MultipleRequestsAsync()
        {
            var pipe = pipeName + "2";
            using (var _host = new PipeHost())
            {
                _host.MessageReceived += (h, s, bytes) =>
                {
                    //do something with the data
                    WriteLine($"Server Received {bytes.AsString()}");
                    //then respond
                    s.WriteAsync("ExitNow".AsBytes()).Wait();
                };
                _host.Start(pipe, 1);

                try
                {
                    using (var client = new PipeClient())
                    {
                        if (!client.Connect(".", pipe, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                            WriteLine("Unable to connect to the pipe host");
                        else
                        {
                            for (int t = 0; t < 100; t++)
                            {
                                //make a request
                                client.WriteAsync($"{t}. Retrieve user data".AsBytes()).Wait();
                                var response = client.ReadAsync().Result;
                                //do something with the response
                                WriteLine($"Client Received: {response?.AsString() ?? "null"}");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        //[TestMethod]
        //public void CreateListeningClient()
        //{
        //    var client = Pipe.Create(".", "PipeName", 5000);
        //    if (client != null)
        //    {
        //        while (true)
        //        {
        //            var serverSays = client.WaitForMessage(false, 5000);
        //            if (serverSays == null)
        //                continue;
        //            //do something with the servers post
        //            Console.WriteLine($"Server Says: {serverSays.AsString()}");
        //            if (serverSays.AsString() == "ExitNow")
        //                break;
        //        }
        //    }
        //}
    }
    [TestClass]
    public class TestPipes_Compressed
    {
        private PipeHost _host;
        private bool _useCompression = true;
        private string pipeName = Helpers.GetPipeName();

        [TestInitialize]
        public void Initialize()
        {
            _host = new PipeHost();
            _host.MessageReceived += (h, s, bytes) =>
            {
                //do something with the data
                Console.WriteLine($"Server Received {bytes.AsString()}");
                //then respond
                s.WriteAsync("ExitNow".AsBytes()).Wait();
            };
            _host.Start(pipeName, 1);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _host.Dispose();
            Console.WriteLine("Host terminated");
        }

        [TestMethod]
        public async Task SingleRequest()
        {
            using (var client = new PipeClient())
            {
                if (!client.Connect(".", pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                    Console.WriteLine("Failed to connect to the server");
                else
                {
                    //make a request
                    await client.WriteAsync("Retrieve user data".AsBytes());
                    var response = await client.ReadAsync();
                    //do something with the response
                    Console.WriteLine($"{response?.AsString() ?? "null"}");
                }
            }
        }
        [TestMethod]
        public async Task MultipleRequests()
        {
            using (var client = new PipeClient())
            {
                if (!client.Connect(".", pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                    Console.WriteLine("Failed to connect to the server");
                else
                {
                    for (int t = 0; t < 100; t++)
                    {
                        //make a request
                        await client.WriteAsync($"{t}. Retrieve user data".AsBytes());
                        var response = await client.ReadAsync();
                        //do something with the response
                        Console.WriteLine($"{response?.AsString() ?? "null"}");
                    }
                }
            }
        }
        //[TestMethod]
        //public void CreateListeningClient()
        //{
        //    var client = Pipe.Create(".", "PipeName", 5000);
        //    if (client != null)
        //    {
        //        while (true)
        //        {
        //            var serverSays = client.WaitForMessage(false, 5000);
        //            if (serverSays == null)
        //                continue;
        //            //do something with the servers post
        //            Console.WriteLine($"Server Says: {serverSays.AsString()}");
        //            if (serverSays.AsString() == "ExitNow")
        //                break;
        //        }
        //    }
        //}
    }
    [TestClass]
    public class TestPipesTyped
    {
        private W.IO.Pipes.PipeHost<Message> _host;
        private bool _useCompression = false;
        private string pipeName = Helpers.GetPipeName();

        [TestInitialize]
        public void Initialize()
        {
            _host = new PipeHost<Message>();
            _host.MessageReceived += (h, s, message) =>
            {
                //do something with the data
                Console.WriteLine($"Received {message.ToString()}");
                message = new Message() { Information = "ExitNow" };
                //then respond
                s.WriteAsync(message).Wait();
            };
            _host.Start(pipeName, 1);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _host.Dispose();
            Console.WriteLine("Host terminated");
        }

        [TestMethod]
        public async Task TypedSingleRequest()
        {
            //NOTE:  The server must be of type PipeHost<Message>
            using (var client = new PipeClient<Message>())
            {
                if (!client.Connect(".", pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                    Console.WriteLine("Failed to connect to the server");
                else
                {
                    //make a request
                    await client.WriteAsync(new Message() { Information = "Blah blah blah" });
                    var response = await client.ReadAsync();
                    //do something with the response
                    Console.WriteLine($"{response?.Timestamp} - {response?.Information ?? "null"}");
                }
            }
        }
        [TestMethod]
        public async Task TypedMultipleRequests()
        {
            //NOTE:  The server must be of type PipeHost<Message>
            using (var client = new PipeClient<Message>())
            {
                if (!client.Connect(".", pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                    Console.WriteLine("Failed to connect to the server");
                else
                {
                    for (int t = 0; t < 100; t++)
                    {
                        //make a request
                        await client.WriteAsync<Message>(new Message() { Information = $"{t}. Blah blah blah" });
                        var response = await client.ReadAsync<Message>();
                        //do something with the response
                        Console.WriteLine($"{response?.Timestamp} - {response?.Information ?? "null"}");
                    }
                }
            }
        }
        //    [TestMethod]
        //    public void CreateListeningClient()
        //    {
        //        var client = Pipe.Create(".", "PipeName", 5000);
        //        if (client != null)
        //        {
        //            while (true)
        //            {
        //                var serverSays = client.WaitForMessage<Message>(false, 5000);
        //                if (serverSays == null)
        //                    continue;
        //                //do something with the servers post
        //                Console.WriteLine($"Server Says: {serverSays.ToString()}");
        //                if (serverSays.Information == "ExitNow")
        //                    break;
        //            }
        //        }
        //    }
    }
    [TestClass]
    public class TestPipesTyped_Compressed
    {
        //private PipeHost<Message> _host;
        private string pipeName = Helpers.GetPipeName();

        //[TestInitialize]
        //public void Initialize()
        //{
        //    _host = new PipeHost<Message>();
        //    _host.MessageReceived += (h, s, message) =>
        //    {
        //        //do something with the data
        //        Console.WriteLine($"Received {message.ToString()}");
        //        message = new Message() { Information = "ExitNow" };
        //        //then respond
        //        s.WriteAsync(message).Wait();
        //    };
        //    _host.Start(pipeName, 1);
        //}
        //[TestCleanup]
        //public void Cleanup()
        //{
        //    _host.Dispose();
        //    Console.WriteLine("Host terminated");
        //}

        [TestMethod]
        public async Task TypedSingleRequest()
        {
            using (var host = new W.IO.Pipes.PipeHost<Message>())
            {
                host.MessageReceived += (h, s, message) =>
                {
                    //do something with the data
                    Console.WriteLine($"Received {message.ToString()}");
                    message = new Message() { Information = "ExitNow" };
                    //then respond
                    s.Write(message);
                };
                host.Start(pipeName, 1);

                //NOTE:  The server must be of type PipeHost<Message>
                using (var client = new W.IO.Pipes.PipeClient<Message>())
                {
                    if (!client.Connect(".", pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                        Console.WriteLine("Failed to connect to the server");
                    else
                    {
                        //make a request
                        client.Write(new Message() { Information = "Blah blah blah" });
                        var response = client.Read<Message>();
                        //do something with the response
                        Console.WriteLine($"{response?.Timestamp} - {response?.Information ?? "null"}");
                    }
                }
            }
        }
        [TestMethod]
        public async Task TypedMultipleRequests()
        {
            using (var host = new W.IO.Pipes.PipeHost<Message>())
            {
                host.MessageReceived += (h, s, message) =>
                {
                    //do something with the data
                    Console.WriteLine($"Received {message.ToString()}");
                    message = new Message() { Information = "ExitNow" };
                    //then respond
                    s.WriteAsync(message).Wait();
                };
                host.Start(pipeName, 1);

                //NOTE:  The server must be of type PipeHost<Message>
                using (var client = new W.IO.Pipes.PipeClient<Message>())
                {
                    if (!client.Connect(".", pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                        Console.WriteLine("Failed to connect to the server");
                    else
                    {
                        for (int t = 0; t < 100; t++)
                        {
                            //make a request
                            await client.WriteAsync<Message>(new Message() { Information = $"{t}. Blah blah blah" });
                            var response = await client.ReadAsync<Message>();
                            //do something with the response
                            Console.WriteLine($"{response?.Timestamp} - {response?.Information ?? "null"}");
                        }
                    }
                }
            }
        }
        //    [TestMethod]
        //    public void CreateListeningClient()
        //    {
        //        var client = Pipe.Create(".", "PipeName", 5000);
        //        if (client != null)
        //        {
        //            while (true)
        //            {
        //                var serverSays = client.WaitForMessage<Message>(false, 5000);
        //                if (serverSays == null)
        //                    continue;
        //                //do something with the servers post
        //                Console.WriteLine($"Server Says: {serverSays.ToString()}");
        //                if (serverSays.Information == "ExitNow")
        //                    break;
        //            }
        //        }
        //    }
    }
    [TestClass]
    public class TestPipes_Concurrently
    {
        //private PipeHost _host;
        private int _numberOfServers = 10;
        private string _pipeName = Helpers.GetPipeName();
        private string _latin = Helpers.GetLatinText();

        //[TestInitialize]
        //public void Initialize()
        //{
        //    _host = new PipeHost();
        //    _host.MessageReceived += (h, s, bytes) =>
        //    {
        //        //do something with the data
        //        Console.WriteLine($"Server Received {bytes.AsString()}");
        //        //send response
        //        if (bytes.Length == 9 && bytes.AsString() == "get Latin")
        //            s.WriteAsync(_latin.AsBytes()).Wait();
        //        else //echo
        //        {
        //            s.WriteAsync(bytes).Wait();
        //        }
        //    };
        //    _host.Start(_pipeName, _numberOfServers);
        //}
        //[TestCleanup]
        //public void Cleanup()
        //{
        //    _host.Dispose();
        //    Console.WriteLine("Host terminated");
        //}

        [TestMethod]
        public async Task SingleRequest()
        {
            using (var host = new PipeHost())
            {
                host.MessageReceived += (h, s, bytes) =>
                {
                    //do something with the data
                    Console.WriteLine($"Server Received {bytes.AsString()}");
                    //send response
                    if (bytes.Length == 9 && bytes.AsString() == "get Latin")
                        s.WriteAsync(_latin.AsBytes()).Wait();
                    else //echo
                    {
                        s.WriteAsync(bytes).Wait();
                    }
                };
                host.Start(_pipeName, _numberOfServers);
                using (var client = new PipeClient())
                {
                    if (!client.Connect(".", _pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                        Console.WriteLine("Failed to connect to the server");
                    else
                    {
                        //make a request
                        await client.WriteAsync("get Latin".AsBytes());
                        var response = await client.ReadAsync();
                        //do something with the response
                        Console.WriteLine($"Received {response?.Length} bytes");
                        Assert.IsNotNull(response);
                        Assert.AreEqual(response.Length, _latin.Length);
                    }
                }
            }
        }
        [TestMethod]
        public async Task MultipleRequests()
        {
            using (var host = new PipeHost())
            {
                host.MessageReceived += (h, s, bytes) =>
                {
                    //do something with the data
                    Console.WriteLine($"Server Received {bytes.AsString()}");
                    //send response
                    if (bytes.Length == 9 && bytes.AsString() == "get Latin")
                        s.WriteAsync(_latin.AsBytes()).Wait();
                    else //echo
                    {
                        s.WriteAsync(bytes).Wait();
                    }
                };
                host.Start(_pipeName, _numberOfServers);
                using (var client = new PipeClient())
                {
                    if (!client.Connect(".", _pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                        Console.WriteLine("Failed to connect to the server");
                    else
                    {
                        for (int t = 1; t <= 100; t++)
                        {
                            //make a request
                            await client.WriteAsync($"{t}. Echo this".AsBytes());
                            var response = await client.ReadAsync();
                            //do something with the response
                            Assert.IsNotNull(response);
                            Console.WriteLine($"{response?.AsString() ?? "null"}");
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void ConcurrentClients()
        {
            using (var host = new PipeHost())
            {
                host.MessageReceived += (h, s, bytes) =>
                {
                    //do something with the data
                    Console.WriteLine($"Server Received {bytes.AsString()}");
                    //send response
                    if (bytes.Length == 9 && bytes.AsString() == "get Latin")
                        s.WriteAsync(_latin.AsBytes()).Wait();
                    else //echo
                    {
                        s.WriteAsync(bytes).Wait();
                    }
                };
                host.Start(_pipeName, _numberOfServers);

                var clients = new List<PipeClient>();
                var numClients = _numberOfServers;
                //create the clients
                for (int t = 0; t < numClients; t++)
                {
                    var newClient = new PipeClient();
                    if (newClient.Connect(".", _pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 5000))
                        clients.Add(newClient);
                }
                Console.WriteLine($"Latin.Length = {_latin.AsBytes().Length}");
                //send the latin text each
                for (int t = 0; t < clients.Count; t++)
                {
                    //var response = clients[t].RequestAsync(_latin.AsBytes(), _useCompression, 3000).Result;
                    //clients[t].WriteAsync(_latin.AsBytes()).Wait();
                    clients[t].WriteAsync(_latin.AsBytes()).Wait();
                    var response = clients[t].ReadAsync().Result;
                    Assert.IsNotNull(response);
                    Assert.IsTrue(response.Length == _latin.Length, $"{t}. Length mismatch: {response.Length} vs {_latin.Length}");
                }
                for (int t = 0; t < numClients; t++)
                    clients[t].Dispose();
            }
        }

        [TestMethod]
        public async Task SequentialClients()
        {
            using (var host = new PipeHost())
            {
                host.MessageReceived += (h, s, bytes) =>
                {
                    //do something with the data
                    Console.WriteLine($"Server Received {bytes.AsString()}");
                    //send response
                    if (bytes.Length == 9 && bytes.AsString() == "get Latin")
                        s.WriteAsync(_latin.AsBytes()).Wait();
                    else //echo
                    {
                        s.WriteAsync(bytes).Wait();
                    }
                };
                host.Start(_pipeName, _numberOfServers);
                var numClients = _numberOfServers * 2;
                for (int t = 0; t < numClients; t++)
                {
                    using (var client = new PipeClient())
                    {
                        if (!client.Connect(".", _pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 1000))
                            Console.WriteLine("Failed to connect to the server");
                        else
                        {
                            await client.WriteAsync(_latin.AsBytes());
                            var response = await client.ReadAsync();
                            Assert.IsNotNull(response);
                            Assert.IsTrue(response.Length == _latin.Length, $"Length mismatch. {response.Length} vs {_latin.Length}");
                        }
                    }
                }
            }
        }

        //[TestMethod]
        //public void CreateListeningClient()
        //{
        //    var client = Pipe.Create(".", "PipeName", 5000);
        //    if (client != null)
        //    {
        //        while (true)
        //        {
        //            var serverSays = client.WaitForMessage(false, 5000);
        //            if (serverSays == null)
        //                continue;
        //            //do something with the servers post
        //            Console.WriteLine($"Server Says: {serverSays.AsString()}");
        //            if (serverSays.AsString() == "ExitNow")
        //                break;
        //        }
        //    }
        //}
    }
}