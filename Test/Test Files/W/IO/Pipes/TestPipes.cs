using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Pipes;
using System.Threading.Tasks;
using W.IO.Pipes;
using W.AsExtensions;
using W.FromExtensions;

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
        private PipeHost _host;
        private bool _useCompression = false;
        private string pipeName = Helpers.GetPipeName();

        [TestInitialize]
        public void Initialize()
        {
            _host = new PipeHost();
            _host.Connected += s =>
            {
                Console.WriteLine("Client connected");
                //s.Post("I see you".AsBytes(), _useCompression);
            };
            _host.Disconnected += s =>
            {
                Console.WriteLine("Client disconnected");
            };
            _host.BytesReceived += (s, bytes) =>
            {
                //do something with the data
                Console.WriteLine($"Server Received {bytes.AsString()}");
                //then respond
                s.PostAsync("ExitNow".AsBytes(), _useCompression).Wait();
            };
            _host.Start(pipeName, 1, _useCompression);
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
            using (var client = PipeClient.Create(".", pipeName, 5000).Result)
            {
                //make a request
                var response = await client.RequestAsync("Retrieve user data".AsBytes(), _useCompression, 5000);
                //do something with the response
                Console.WriteLine($"Client Received: {response?.AsString() ?? "null"}");
            }
        }
        [TestMethod]
        public async Task MultipleRequests()
        {
            using (var client = PipeClient.Create(".", pipeName, 5000).Result)
            {
                for (int t = 0; t < 100; t++)
                {
                    //make a request
                    var response = await client.RequestAsync($"{t}. Retrieve user data".AsBytes(), _useCompression, 5000);
                    //do something with the response
                    Console.WriteLine($"Client Received: {response?.AsString() ?? "null"}");
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
            _host.Connected += s =>
            {
                Console.WriteLine("Client connected");
                //s.Post("I see you".AsBytes(), _useCompression);
            };
            _host.Disconnected += s =>
            {
                Console.WriteLine("Client disconnected");
            };
            _host.BytesReceived += (s, bytes) =>
            {
                //do something with the data
                Console.WriteLine($"Server Received {bytes.AsString()}");
                //then respond
                s.PostAsync("ExitNow".AsBytes(), _useCompression).Wait();
            };
            _host.Start(pipeName, 1, _useCompression);
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
            using (var client = PipeClient.Create(".", pipeName, 5000).Result)
            {
                //make a request
                var response = await client.RequestAsync("Retrieve user data".AsBytes(), _useCompression, 5000);
                //do something with the response
                Console.WriteLine($"{response?.AsString() ?? "null"}");
            }
        }
        [TestMethod]
        public async Task MultipleRequests()
        {
            using (var client = PipeClient.Create(".", pipeName, 5000).Result)
            {
                for (int t = 0; t < 100; t++)
                {
                    //make a request
                    var response = await client.RequestAsync($"{t}. Retrieve user data".AsBytes(), _useCompression, 5000);
                    //do something with the response
                    Console.WriteLine($"{response?.AsString() ?? "null"}");
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
        private PipeHost<Message> _host;
        private bool _useCompression = false;
        private string pipeName = Helpers.GetPipeName();

        [TestInitialize]
        public void Initialize()
        {
            _host = new PipeHost<Message>();
            _host.Connected += s =>
            {
                Console.WriteLine("Client connected");
                //s.PostAsync(new Message() { Information = "I see you" }, _useCompression).Wait();
            };
            _host.Disconnected += s =>
            {
                Console.WriteLine("Client disconnected");
            };
            _host.MessageReceived += (s, message) =>
            {
                //do something with the data
                Console.WriteLine($"Received {message.ToString()}");
                message = new Message() { Information = "ExitNow" };
                //then respond
                s.PostAsync(message, _useCompression).Wait();
            };
            _host.Start(pipeName, 1, _useCompression);
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
            using (var client = PipeClient.Create(".", pipeName, 5000).Result)
            {
                //make a request
                var response = await client.RequestAsync<Message>(new Message() { Information = "Blah blah blah" }, _useCompression, 5000);
                //do something with the response
                Console.WriteLine($"{response?.Timestamp} - {response?.Information ?? "null"}");
            }
        }
        [TestMethod]
        public async Task TypedMultipleRequests()
        {
            //NOTE:  The server must be of type PipeHost<Message>
            using (var client = PipeClient.Create(".", pipeName, 5000).Result)
            {
                for (int t = 0; t < 100; t++)
                {
                    //make a request
                    var response = await client.RequestAsync<Message>(new Message() { Information = $"{t}. Blah blah blah" }, _useCompression, 5000);
                    //do something with the response
                    Console.WriteLine($"{response?.Timestamp} - {response?.Information ?? "null"}");
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
        private PipeHost<Message> _host;
        private bool _useCompression = true;
        private string pipeName = Helpers.GetPipeName();

        [TestInitialize]
        public void Initialize()
        {
            _host = new PipeHost<Message>();
            _host.Connected += s =>
            {
                Console.WriteLine("Client connected");
                //s.PostAsync(new Message() { Information = "I see you" }, _useCompression).Wait();
            };
            _host.Disconnected += s =>
            {
                Console.WriteLine("Client disconnected");
            };
            _host.MessageReceived += (s, message) =>
            {
                //do something with the data
                Console.WriteLine($"Received {message.ToString()}");
                message = new Message() { Information = "ExitNow" };
                //then respond
                s.PostAsync(message, _useCompression).Wait();
            };
            _host.Start(pipeName, 1, _useCompression);
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
            using (var client = PipeClient.Create(".", pipeName, 5000).Result)
            {
                //make a request
                var response = await client.RequestAsync<Message>(new Message() { Information = "Blah blah blah" }, _useCompression, 5000);
                //do something with the response
                Console.WriteLine($"{response?.Timestamp} - {response?.Information ?? "null"}");
            }
        }
        [TestMethod]
        public async Task TypedMultipleRequests()
        {
            //NOTE:  The server must be of type PipeHost<Message>
            using (var client = PipeClient.Create(".", pipeName, 5000).Result)
            {
                for (int t = 0; t < 100; t++)
                {
                    //make a request
                    var response = await client.RequestAsync<Message>(new Message() { Information = $"{t}. Blah blah blah" }, _useCompression, 5000);
                    //do something with the response
                    Console.WriteLine($"{response?.Timestamp} - {response?.Information ?? "null"}");
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
        private PipeHost _host;
        private int _numberOfServers = 10;
        private bool _useCompression = false;
        private string _pipeName = Helpers.GetPipeName();
        private string _latin = Helpers.GetLatinText();

        [TestInitialize]
        public void Initialize()
        {
            _host = new PipeHost();
            _host.Connected += s =>
            {
                Console.WriteLine("Client connected");
            };
            _host.Disconnected += s =>
            {
                Console.WriteLine("Client disconnected");
            };
            _host.BytesReceived += (s, bytes) =>
            {
                //do something with the data
                Console.WriteLine($"Server Received {bytes.AsString()}");
                //send response
                if (bytes.Length == 9 && bytes.AsString() == "get Latin")
                    s.PostAsync(_latin.AsBytes(), _useCompression).Wait();
                else //echo
                {
                    s.PostAsync(bytes, _useCompression).Wait();
                }
            };
            _host.Start(_pipeName, _numberOfServers, _useCompression);
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
            using (var client = PipeClient.Create(".", _pipeName, 5000).Result)
            {
                //make a request
                var response = await client.RequestAsync("get Latin".AsBytes(), _useCompression, 5000);
                //do something with the response
                Console.WriteLine($"Received {response?.Length} bytes");
                Assert.IsNotNull(response);
                Assert.AreEqual(response.Length, _latin.Length);
            }
        }
        [TestMethod]
        public async Task MultipleRequests()
        {
            using (var client = PipeClient.Create(".", _pipeName, 5000).Result)
            {
                for (int t = 1; t <= 100; t++)
                {
                    //make a request
                    var response = await client.RequestAsync($"{t}. Echo this".AsBytes(), _useCompression, 5000);
                    //do something with the response
                    Assert.IsNotNull(response);
                    Console.WriteLine($"{response?.AsString() ?? "null"}");
                }
            }
        }

        [TestMethod]
        public async Task ConcurrentClients()
        {
            var clients = new List<PipeClient>();
            var numClients = _numberOfServers;
            //create the clients
            for (int t = 0; t < numClients; t++)
            {
                var newClient = PipeClient.Create(".", _pipeName, 5000).Result;
                clients.Add(newClient);
            }
            //send the latin text each
            for (int t = 0; t < clients.Count; t++)
            {
                //var response = clients[t].RequestAsync(_latin.AsBytes(), _useCompression, 3000).Result;
                var response = await clients[t].RequestAsync(_latin.AsBytes(), _useCompression, 3000);
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Length == _latin.Length, $"{t}. Length mismatch: {response.Length} vs {_latin.Length}");
            }
            for (int t = 0; t < numClients; t++)
                clients[t].Dispose();
        }

        [TestMethod]
        public async Task SequentialClients()
        {
            var numClients = _numberOfServers * 2;
            for (int t = 0; t < numClients; t++)
            {
                using (var client = PipeClient.Create(".", _pipeName, 1000).Result)
                {
                    var response = await client.RequestAsync(_latin.AsBytes(), _useCompression, 5000);
                    Assert.IsNotNull(response);
                    Assert.IsTrue(response.Length == _latin.Length, $"Length mismatch. {response.Length} vs {_latin.Length}");
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