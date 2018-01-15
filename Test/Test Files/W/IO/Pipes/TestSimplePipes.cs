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
using W.IO.Pipes;

//Todo: 12.8.2017 - This file is a mess.  Clean it up.

namespace W.Tests
{
    public static class Helpers
    {
        public static string GetPipeName([System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
        {
            //return Guid.NewGuid().ToString();
            //Pipe_Count += 1;
            //return PIPENAME + Pipe_Count.ToString();
            //var st = new System.Diagnostics.StackTrace(1, true);
            //return st.GetFrame(0).GetMethod().Name;
            return callerMemberName;
        }
        public static string GetLatinText()
        {
            var latin = System.IO.File.ReadAllText(@"C:\Source\Repos\Tungsten\Test\Test Files\4500_Latin_Words.txt");
            return latin;
        }
    }
    [TestClass]
    public class SimplePipeTests
    {
        #region Async SimplePipes
        [TestMethod]
        public void SimplePipes_CreateHost()
        {
            var pipeName = Helpers.GetPipeName();
            using (var host = new W.IO.SimplePipes.SimplePipeHost())
            {
                host.RequestReceived += (h, bytes) =>
                {
                    //echo
                    return bytes;
                };
            }
        }
        [TestMethod]
        public void SimplePipes_StartStopHost()
        {
            var pipeName = Helpers.GetPipeName();
            using (var host = new W.IO.SimplePipes.SimplePipeHost())
            {
                host.RequestReceived += (h, bytes) =>
                {
                    //echo
                    return bytes;
                };
                host.Start(pipeName, 1, false, "Ok".AsBytes());
                host.Stop();
            }
        }
        [TestMethod]
        public async Task SimplePipes_Timeout()
        {
            var pipeName = Helpers.GetPipeName();
            var request = "Hello Server".AsBytes();
            var response = await W.IO.SimplePipes.SimplePipeClient.RequestAsync(".", pipeName, request, 1000);
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Status == IO.Pipes.PipeStatusEnum.Disconnected);
        }
        [TestMethod]
        public async Task SimplePipes_Echo_Once()
        {
            var pipeName = Helpers.GetPipeName();
            var mreQuit = new ManualResetEventSlim(false);
            using (var host = new W.IO.SimplePipes.SimplePipeHost())
            {
                host.RequestReceived += (h, bytes) =>
                {
                    if (bytes?.Length == 0)
                        System.Diagnostics.Debugger.Break();
                    Console.WriteLine("Server received request: " + bytes.AsString());
                    //echo
                    return "Hello from the server".AsBytes();
                };
                host.Start(pipeName, 1, false, "Ok".AsBytes());
                //await host.StartAsync(pipeName, 1).ContinueWith(async task =>
                {
                    var request = "Hello Server".AsBytes();
                    var response = await W.IO.SimplePipes.SimplePipeClient.RequestAsync(".", pipeName, request, 1000);
                    Assert.IsNotNull(response);
                    Assert.IsTrue(response.Success);
                    Assert.IsNotNull(response.Result);
                    Console.WriteLine("Server Responded: " + response.Result.AsString());
                    mreQuit.Set();
                }//);
                mreQuit.Wait();
            }
        }
        [TestMethod]
        public async Task SimplePipes_Echo_Twice()
        {
            var pipeName = Helpers.GetPipeName();
            var mreQuit = new ManualResetEventSlim(false);
            var requestCount = 0;
            var responseCount = 0;
            var requestsCompletedCount = 0;
            using (var host = new W.IO.SimplePipes.SimplePipeHost())
            {
                host.RequestReceived += (h, bytes) =>
                {
                    if (bytes?.Length == 0)
                        System.Diagnostics.Debugger.Break();
                    Console.WriteLine("Server received request: " + bytes.AsString());
                    Interlocked.Increment(ref responseCount);
                    //echo
                    return "Hello from the server".AsBytes();
                };
                host.Start(pipeName, 2, false, "Ok".AsBytes());
                //.ContinueWith(async task =>
                {
                    //Assert.IsFalse(task.IsCanceled);
                    //Assert.IsFalse(task.IsFaulted);
                    //Assert.IsNull(task.Exception);
                    for (int t = 0; t < 2; t++)
                    {
                        var request = ("Hello World " + t.ToString()).AsBytes();
                        Interlocked.Increment(ref requestCount);
                        var response = await W.IO.SimplePipes.SimplePipeClient.RequestAsync(".", pipeName, request, 1000);
                        Interlocked.Increment(ref requestsCompletedCount);
                        Assert.IsNotNull(response);
                        Assert.IsTrue(response.Success);
                        Assert.IsNotNull(response.Result);
                        Console.WriteLine("Server Responded: " + response.Result.AsString());
                    }
                    mreQuit.Set();
                    Assert.IsTrue(requestCount == 2, "RequestCount = {0}", requestCount);
                    Assert.IsTrue(responseCount == 2, "ResponseCount = {0}", responseCount);
                    Assert.IsTrue(requestsCompletedCount == 2, "RequestsCompletedCount = {0}", requestsCompletedCount);
                }//);
                mreQuit.Wait();
            }
        }
        [TestMethod]
        public async Task SimplePipes_Echo_100Times_Asynchronous()
        {
            var mreQuit = new ManualResetEventSlim(false);
            var pipeName = Helpers.GetPipeName();
            var connectionCount = 100;
            var requestCount = 0;
            var responseCount = 0;
            var requestsCompletedCount = 0;
            using (var host = new W.IO.SimplePipes.SimplePipeHost())
            {
                host.RequestReceived += (h, bytes) =>
                {
                    Console.WriteLine("Server Receieved: " + bytes.AsString());
                    Interlocked.Increment(ref responseCount);
                    //echo
                    return bytes;
                };
                host.Start(pipeName, connectionCount, false, "Ok".AsBytes());
                //await host.StartAsync(pipeName, connectionCount).ContinueWith(async task =>
                {
                    //being async, Parallel doesn't work
                    for (int t = 0; t < connectionCount; t++)
                    {
                        var request = ("Hello World " + t.ToString()).AsBytes();
                        Interlocked.Increment(ref requestCount);
                        var response = await W.IO.SimplePipes.SimplePipeClient.RequestAsync(".", pipeName, request, 1000);
                        Interlocked.Increment(ref requestsCompletedCount);
                        Assert.IsNotNull(response);
                        Assert.IsTrue(response.Success);
                        Assert.IsNotNull(response.Result);
                        Console.WriteLine("Server Response: " + response.Result.AsString());
                    }
                    mreQuit.Set();
                    Assert.IsTrue(requestCount == connectionCount, "RequestCount = {0}", requestCount);
                    Assert.IsTrue(responseCount == connectionCount, "ResponseCount = {0}", responseCount);
                    Assert.IsTrue(requestsCompletedCount == connectionCount, "RequestsCompletedCount = {0}", requestsCompletedCount);
                }//);
                Assert.IsTrue(responseCount == connectionCount);
                mreQuit.Wait();
            }
        }
        #endregion

        #region Sync SimplePipes
        [TestMethod]
        public void SimplePipes_Timeout_Synchronous()
        {
            var pipeName = Helpers.GetPipeName();
            var request = "Hello Server".AsBytes();
            var response = W.IO.SimplePipes.SimplePipeClient.Request(".", pipeName, request, 100);
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Status == IO.Pipes.PipeStatusEnum.Disconnected);
        }
        [TestMethod]
        public void SimplePipes_Echo_Once_Synchronous()
        {
            var pipeName = Helpers.GetPipeName();
            using (var host = new W.IO.SimplePipes.SimplePipeHost())
            {
                host.RequestReceived += (h, bytes) =>
                {
                    if (bytes?.Length == 0)
                        System.Diagnostics.Debugger.Break();
                    Console.WriteLine("Server received request: " + bytes.AsString());
                    //echo
                    return "Hello from the server".AsBytes();
                };
                host.Start(pipeName, 1, false, "Ok".AsBytes());

                var request = "Hello Server".AsBytes();
                var response = W.IO.SimplePipes.SimplePipeClient.Request(".", pipeName, request, 1000);
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                Assert.IsNotNull(response.Result);
                Console.WriteLine("Server Responded: " + response.Result.AsString());
            }
        }
        [TestMethod]
        public void SimplePipes_Echo_100Times_Synchronous()
        {
            var pipeName = Helpers.GetPipeName();
            var maxConnections = 100;
            var requestCount = 0;
            var responseCount = 0;
            var requestsCompletedCount = 0;
            using (var host = new W.IO.SimplePipes.SimplePipeHost())
            {
                host.RequestReceived += (h, bytes) =>
                {
                    if (bytes?.Length == 0)
                        System.Diagnostics.Debugger.Break();
                    Console.WriteLine("Server received request: " + bytes.AsString());
                    Interlocked.Increment(ref responseCount);
                    //echo
                    return "Hello from the server".AsBytes();
                };
                host.Start(pipeName, 1, false, "Ok".AsBytes());

                for (int t = 0; t < maxConnections; t++)
                {
                    var request = ("Hello World " + t.ToString()).AsBytes();
                    Interlocked.Increment(ref requestCount);
                    var response = W.IO.SimplePipes.SimplePipeClient.Request(".", pipeName, request, 1000);
                    Interlocked.Increment(ref requestsCompletedCount);
                    Assert.IsNotNull(response);
                    Assert.IsTrue(response.Success, "Failed on t == {0}", t);
                    Assert.IsNotNull(response.Result);
                    Console.WriteLine("Server Responded: " + response.Result.AsString());
                }
                Assert.IsTrue(requestCount == maxConnections, "RequestCount = {0}", requestCount);
                Assert.IsTrue(responseCount == maxConnections, "ResponseCount = {0}", responseCount);
                Assert.IsTrue(requestsCompletedCount == maxConnections, "RequestsCompletedCount = {0}", requestsCompletedCount);
            }
        }
        [TestMethod]
        public void SimplePipes_Echo_MaxConnections_Synchronous_Parallel()
        {
            var pipeName = Helpers.GetPipeName();
            var maxConnectinos = 254;
            var requestCount = 0;
            var responseCount = 0;
            var requestsCompletedCount = 0;
            using (var host = new W.IO.SimplePipes.SimplePipeHost())
            {
                host.RequestReceived += (h, bytes) =>
                {
                    if (bytes?.Length == 0)
                        System.Diagnostics.Debugger.Break();
                    Console.WriteLine("Server received request: " + bytes.AsString());
                    Interlocked.Increment(ref responseCount);
                    //echo
                    return "Hello from the server".AsBytes();
                };
                host.Start(pipeName, maxConnectinos, false, "Ok".AsBytes());
                Parallel.For(0, maxConnectinos, t =>
                {
                    var request = ("Hello World " + t.ToString()).AsBytes();
                    Interlocked.Increment(ref requestCount);
                    var response = W.IO.SimplePipes.SimplePipeClient.Request(".", pipeName, request, 1000);
                    Interlocked.Increment(ref requestsCompletedCount);
                    Assert.IsNotNull(response);
                    Assert.IsTrue(response.Success);
                    Assert.IsNotNull(response.Result);
                    Console.WriteLine("Server Responded: " + response.Result.AsString());
                });
                Assert.IsTrue(requestCount == maxConnectinos, "RequestCount = {0}", requestCount);
                Assert.IsTrue(responseCount == maxConnectinos, "ResponseCount = {0}", responseCount);
                Assert.IsTrue(requestsCompletedCount == maxConnectinos, "RequestsCompletedCount = {0}", requestsCompletedCount);
            }
        }
        #endregion

        #region SimplePipes with objects
        private static Transaction<Customer> HandleTransaction(Transaction<Customer> transaction)
        {
            switch (transaction.Action)
            {
                case DatabaseActionsEnum.Create:
                    //simulate a customer create
                    transaction.Status = TransactionStatusEnum.Ok;
                    transaction.Data = new Customer() { Name = transaction.Data.As<Customer>()?.Name ?? "New Customer", Id = Guid.NewGuid().ToString() };
                    break;
                case DatabaseActionsEnum.Retrieve:
                    //simulate a customer lookup
                    transaction.Status = TransactionStatusEnum.Ok;
                    transaction.Data = new Customer() { Name = "Jordan", Id = transaction.Data.As<Customer>().Id };
                    break;
                case DatabaseActionsEnum.Update:
                    //simulate a customer update
                    transaction.Status = TransactionStatusEnum.Ok;
                    transaction.Details = "Updated";
                    break;
                case DatabaseActionsEnum.Delete:
                    transaction.Status = TransactionStatusEnum.Exception;
                    transaction.Data = null;
                    transaction.Details = "Customer not found";
                    break;
            }
            return transaction;
        }
        [TestMethod]
        public async Task SimplePipes_Objects_CreateCustomer()
        {
            var pipeName = Helpers.GetPipeName();
            using (var host = new W.IO.SimplePipes.SimplePipeHost<Transaction<Customer>>())
            {
                host.RequestReceived += (h, transaction) =>
                {
                    Console.WriteLine("Host Received: {0}", transaction.ToString());
                    return HandleTransaction(transaction);
                };
                host.Start(pipeName, 20, false, new Transaction<Customer>());
                using (var client = new W.IO.SimplePipes.SimplePipeClient())
                {
                    if (client.Connect(pipeName))
                    {
                        var request = new Transaction<Customer>() { Action = DatabaseActionsEnum.Create, Data = new Customer() { Name = "Jordan" } };
                        var response = await client.RequestAsync(request, 30000);
                        Assert.IsTrue(response != null);
                        Assert.IsTrue(response.Success == true);
                        Assert.IsTrue(response.Exception == null);
                        Assert.IsTrue(response.Result != null);
                        Console.WriteLine("Client Received: {0}", response.Result);
                    }
                }
            }
        }
        [TestMethod]
        public async Task SimplePipes_Objects_RetrieveCustomer()
        {
            var pipeName = Helpers.GetPipeName();
            using (var host = new W.IO.SimplePipes.SimplePipeHost<Transaction<Customer>>())
            {
                host.RequestReceived += (h, transaction) =>
                {
                    Console.WriteLine("Host Received: {0}", transaction);
                    return HandleTransaction(transaction);
                };
                host.Start(pipeName, 20, false, new Transaction<Customer>());
                using (var client = new W.IO.SimplePipes.SimplePipeClient())
                {
                    if (client.Connect(pipeName))
                    {
                        var request = new Transaction<Customer>() { Action = DatabaseActionsEnum.Retrieve, Data = new Customer() { Id = "1" } };
                        var response = await client.RequestAsync(request, 30000);
                        Assert.IsTrue(response != null);
                        Assert.IsTrue(response.Success == true);
                        Assert.IsTrue(response.Exception == null);
                        Assert.IsTrue(response.Result != null);
                        Console.WriteLine("Client Received: {0}", response.Result);
                    }
                }
            }
        }
        [TestMethod]
        public async Task SimplePipes_Objects_UpdateCustomer()
        {
            var pipeName = Helpers.GetPipeName();
            using (var host = new W.IO.SimplePipes.SimplePipeHost<Transaction<Customer>>())
            {
                host.RequestReceived += (h, transaction) =>
                {
                    Console.WriteLine("Host Received: {0}", transaction);
                    return HandleTransaction(transaction);
                };
                host.Start(pipeName, 20, false, new Transaction<Customer>());
                using (var client = new W.IO.SimplePipes.SimplePipeClient())
                {
                    if (client.Connect(pipeName))
                    {
                        var request = new Transaction<Customer>() { Action = DatabaseActionsEnum.Update, Data = new Customer() { Id = "1", Name = "Duerksen" } };
                        var response = await client.RequestAsync(request, 30000);
                        Assert.IsTrue(response != null);
                        Assert.IsTrue(response.Success == true);
                        Assert.IsTrue(response.Exception == null);
                        Assert.IsTrue(response.Result != null);
                        Console.WriteLine("Client Received: {0}", response.Result);
                    }
                }
            }
        }
        [TestMethod]
        public async Task SimplePipes_Objects_DeleteCustomer()
        {
            var pipeName = Helpers.GetPipeName();
            using (var host = new W.IO.SimplePipes.SimplePipeHost<Transaction<Customer>>())
            {
                host.RequestReceived += (h, transaction) =>
                {
                    Console.WriteLine("Host Received: {0}", transaction);
                    return HandleTransaction(transaction);
                };
                host.Start(pipeName, 20, false, new Transaction<Customer>());
                using (var client = new W.IO.SimplePipes.SimplePipeClient())
                {
                    if (client.Connect(pipeName))
                    {
                        var request = new Transaction<Customer>() { Action = DatabaseActionsEnum.Delete, Data = new Customer() { Id = "1" } };
                        var response = await client.RequestAsync(request, 30000);
                        Assert.IsTrue(response != null);
                        Assert.IsTrue(response.Success == true);
                        Assert.IsTrue(response.Exception == null);
                        Assert.IsTrue(response.Result != null);
                        Console.WriteLine("Client Received: {0}", response.Result);
                    }
                }
            }
        }
        #endregion

        #region SimplePipes with compression
        //[TestMethod]
        //public async Task SimplePipes_Objects_WithCompression()
        //{
        //    var pipeName = Helpers.GetPipeName();
        //    using (var host = new W.IO.SimplePipes.SimplePipeHost<Transaction<Customer>>())
        //    {
        //        host.RequestReceived += (h, transaction) =>
        //        {
        //            Console.WriteLine("Host Received: {0}", transaction);
        //            return HandleTransaction(transaction);
        //        };
        //        host.Start(pipeName, 20, true);
        //        using (var client = new W.IO.SimplePipes.SimplePipeClient())
        //        {
        //            if (client.Connect(pipeName, true, 30000))
        //            {
        //                var request = new Transaction<Customer>() { Action = DatabaseActionsEnum.Delete, Data = new Customer() { Id = "1", Name = "Test Customer" } };
        //                var response = await client.RequestAsync(request, 30000);
        //                Assert.IsTrue(response != null);
        //                Assert.IsTrue(response.Success == true);
        //                Assert.IsTrue(response.Exception == null);
        //                Assert.IsTrue(response.Result != null);
        //                Console.WriteLine("Client Received: {0}", response.Result);
        //            }
        //        }
        //    }
        //}
        //[TestMethod]
        //public async Task SimplePipes_Echo_Once_WithCompression()
        //{
        //    var pipeName = Helpers.GetPipeName();
        //    var mreQuit = new ManualResetEventSlim(false);
        //    using (var host = new W.IO.SimplePipes.SimplePipeHost())
        //    {
        //        host.RequestReceived += (h, server, bytes) =>
        //        {
        //            if (bytes?.Length == 0)
        //                System.Diagnostics.Debugger.Break();
        //            Console.WriteLine("Server received request: " + bytes.AsString());
        //            //echo
        //            return "Hello from the server".AsBytes();
        //        };
        //        host.Start(pipeName, 1, true);
        //        {
        //            var request = "Hello Server".AsBytes();
        //            var response = await W.IO.SimplePipes.SimplePipeClient.RequestAsync(".", pipeName, request, true, 1000);
        //            Assert.IsNotNull(response);
        //            Assert.IsTrue(response.Success);
        //            Assert.IsNotNull(response.Result);
        //            Console.WriteLine("Server Responded: " + response.Result.AsString());
        //            mreQuit.Set();
        //        }//);
        //        mreQuit.Wait();
        //    }
        //}
        [TestMethod]
        public void SimplePipes_Message()
        {
            var emptyMsg = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, "");
            var noMsg = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.None, "");
            var okMsg = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, "The details");
            var errMsg = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Exception, "Some exception");
            W.IO.Pipes.Message nullMsg = null;

            //mundane tests
            Assert.IsTrue(emptyMsg.Status == IO.Pipes.MessageStatusEnum.Ok);
            Assert.IsTrue(noMsg.Status == IO.Pipes.MessageStatusEnum.None);
            Assert.IsTrue(okMsg.Status == IO.Pipes.MessageStatusEnum.Ok);
            Assert.IsTrue(errMsg.Status == IO.Pipes.MessageStatusEnum.Exception);

            //test operator ==
            Assert.IsTrue(emptyMsg == W.IO.Pipes.Message.Empty);
            Assert.IsTrue(W.IO.Pipes.Message.Empty == emptyMsg);
            Assert.IsTrue(nullMsg == null);
            //test operator !=
            Assert.IsTrue(emptyMsg != noMsg);
            Assert.IsTrue(emptyMsg != okMsg);
            Assert.IsTrue(emptyMsg != errMsg);
            Assert.IsTrue(emptyMsg != null);
            Assert.IsTrue(null != emptyMsg);
            Assert.IsTrue(okMsg != errMsg);
            Assert.IsTrue(okMsg != nullMsg);

        }
        //[TestMethod]
        //public async Task SimplePipes_Echo_Once_Objects_WithCompression()
        //{
        //    var pipeName = Helpers.GetPipeName();
        //    var mreQuit = new ManualResetEventSlim(false);
        //    using (var host = new W.IO.SimplePipes.SimplePipeHost<W.IO.Pipes.Message>())
        //    {
        //        host.RequestReceived += (h, bytes) =>
        //        {
        //            if (bytes == null)
        //                System.Diagnostics.Debugger.Break();
        //            Console.WriteLine("Server received request = {0}", bytes);
        //            //echo
        //            return W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, "Hello from the server");
        //        };
        //        host.Start(pipeName, 1, true);
        //        {
        //            var request = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, "Hello Server");
        //            var response = await W.IO.SimplePipes.SimplePipeClient.RequestAsync(".", pipeName, request, true, 30000);
        //            Assert.IsNotNull(response);
        //            Assert.IsTrue(response.Success);
        //            Assert.IsNotNull(response.Result);
        //            Console.WriteLine("Server Response = {0}", response.Result);
        //            mreQuit.Set();
        //        }//);
        //        mreQuit.Wait();
        //    }
        //}
        #endregion

        //[TestMethod]
        //public void SimplePipes_MultiHost_with_StaticClientCalls()
        //{
        //    var pipeName = Helpers.GetPipeName();
        //    using (var host = new W.IO.SimplePipes.SimplePipeHost<W.IO.Pipes.Message>())
        //    {
        //        host.RequestReceived += (h, server, message) =>
        //        {
        //            if (message == null)
        //                System.Diagnostics.Debugger.Break();
        //            Console.WriteLine("Server received request = {0}", message);
        //            //respond
        //            return W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, "Hello from the server");
        //        };
        //        host.Start(pipeName, 1, true);
        //        {
        //            //1st request
        //            var request = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, "Hello Server 1");
        //            var response = W.IO.SimplePipes.SimplePipeClient.Request(pipeName, request, true, 15000);
        //            Assert.IsNotNull(response);
        //            Assert.IsTrue(response.Success);
        //            Assert.IsNotNull(response.Result);
        //            Console.WriteLine("Server Response = {0}", response.Result);

        //            //2nd request
        //            request = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, "Hello Server 2");
        //            response = W.IO.SimplePipes.SimplePipeClient.Request(pipeName, request, true, 15000);
        //            Assert.IsNotNull(response);
        //            Assert.IsTrue(response.Success);
        //            Assert.IsNotNull(response.Result);
        //            Console.WriteLine("Server Response = {0}", response.Result);
        //        }
        //    }
        //}
    }

    #region Abstract Base SimplePipe Test Classes
    public abstract class SimplePipe_HostBase
    {
        protected string PipeName = "SimplePipe_Test_" + new Random().Next(int.MaxValue).ToString();// Guid.NewGuid().ToString();
        protected W.IO.SimplePipes.SimplePipeHost Host { get; set; }

        protected byte[] Echo(W.IO.SimplePipes.SimplePipeHost<byte[]> host, NamedPipeServerStream server, byte[] data)
        {
            Assert.IsNotNull(data);
            Console.WriteLine("Server Received: {0}", data.AsString());
            return data; //echo
        }

        [TestInitialize]
        public virtual void Initialize()
        {
            Console.WriteLine("PipeName = {0}", PipeName);
            Host = new W.IO.SimplePipes.SimplePipeHost();
            Assert.IsNotNull(Host);
            Console.WriteLine("Host Created");
            Host.Start(PipeName, 20, false, "Ok".AsBytes());
            Console.WriteLine("Host Started");
        }
        [TestCleanup]
        public virtual void Cleanup()
        {
            Host?.Dispose();
            Host = null;
            Console.WriteLine("Host Disposed");
        }
    }
    public abstract class SimplePipe_HostBase<TData> where TData : new()
    {
        protected string PipeName = "SimplePipe_Test_" + new Random().Next(int.MaxValue).ToString();// Guid.NewGuid().ToString();
        protected W.IO.SimplePipes.SimplePipeHost<TData> Host { get; set; }

        protected TData Echo(W.IO.SimplePipes.SimplePipeHost<TData> host, NamedPipeServerStream server, TData data)
        {
            Assert.IsNotNull(data);
            //Console.WriteLine("Server Received: {0}", data.As<byte[]>()?.AsString() ?? data.ToString());
            Console.WriteLine("Server Received: {0}", data);
            return data; //echo
        }

        [TestInitialize]
        public virtual void Initialize()
        {
            Console.WriteLine("PipeName = {0}", PipeName);
            Host = new W.IO.SimplePipes.SimplePipeHost<TData>();
            Assert.IsNotNull(Host);
            Console.WriteLine("Host Created");
            Host.Start(PipeName, 20, false, new TData());
            Console.WriteLine("Host Started");
        }
        [TestCleanup]
        public virtual void Cleanup()
        {
            Host?.Dispose();
            Host = null;
            Console.WriteLine("Host Disposed");
        }
    }
    public abstract class SimplePipe_Clients : SimplePipe_HostBase
    {
        protected List<W.IO.SimplePipes.SimplePipeClient> Clients = new List<W.IO.SimplePipes.SimplePipeClient>();
        protected int NumberOfClients = 1;
        protected W.IO.SimplePipes.SimplePipeClient Client => Clients[0];

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            for (int t = 0; t < NumberOfClients; t++)
            {
                var newClient = new IO.SimplePipes.SimplePipeClient();
                Assert.IsNotNull(newClient);
                Clients.Add(newClient);
            }
            Console.WriteLine("{0} Clients Created", NumberOfClients);
        }
        [TestCleanup]
        public override void Cleanup()
        {
            for (int t = 0; t < NumberOfClients; t++)
            {
                Clients[t]?.Dispose();
                Clients[t] = null;
                Console.WriteLine("Client {0} Disposed", t);
            }

            base.Cleanup();
        }

        public SimplePipe_Clients() : this(1) { }
        public SimplePipe_Clients(int numberOfClients)
        {
            NumberOfClients = numberOfClients;
        }
    }
    public abstract class SimplePipe_Clients<TData> : SimplePipe_HostBase<TData> where TData : new()
    {
        protected List<W.IO.SimplePipes.SimplePipeClient> Clients = new List<W.IO.SimplePipes.SimplePipeClient>();
        protected int NumberOfClients = 1;
        protected W.IO.SimplePipes.SimplePipeClient Client => Clients[0];

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            for (int t = 0; t < NumberOfClients; t++)
            {
                var newClient = new W.IO.SimplePipes.SimplePipeClient();
                Assert.IsNotNull(newClient);
                Clients.Add(newClient);
            }
            Console.WriteLine("{0} Clients Created", NumberOfClients);
        }
        [TestCleanup]
        public override void Cleanup()
        {
            for (int t = 0; t < NumberOfClients; t++)
            {
                Clients[t]?.Dispose();
                Clients[t] = null;
                Console.WriteLine("Client {0} Disposed", t);
            }

            base.Cleanup();
        }

        public SimplePipe_Clients() : this(1) { }
        public SimplePipe_Clients(int numberOfClients)
        {
            NumberOfClients = numberOfClients;
        }
    }
    public abstract class SimplePipe_Client : SimplePipe_HostBase
    {
        protected W.IO.SimplePipes.SimplePipeClient Client { get; set; }

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            Client = new W.IO.SimplePipes.SimplePipeClient();
            Assert.IsNotNull(Client);
            Console.WriteLine("Client Created");
        }
        [TestCleanup]
        public override void Cleanup()
        {
            Client?.Dispose();
            Client = null;
            Console.WriteLine("Client Disposed");

            base.Cleanup();
        }
    }
    public abstract class SimplePipe_Client<TData> : SimplePipe_HostBase<TData> where TData : new()
    {
        protected W.IO.SimplePipes.SimplePipeClient Client { get; set; }

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            Client = new W.IO.SimplePipes.SimplePipeClient();
            Assert.IsNotNull(Client);
            Console.WriteLine("Client Created");
        }
        [TestCleanup]
        public override void Cleanup()
        {
            Client?.Dispose();
            Client = null;
            Console.WriteLine("Client Disposed");

            base.Cleanup();
        }
    }
    #endregion

    [TestClass]
    public class SimplePipe_Clients_Tests : SimplePipe_Clients<W.IO.Pipes.Message>
    {
        [TestMethod]
        public void SequentialClients_10()
        {
            Host.RequestReceived += (h, bytes) =>
            {
                var number = bytes.Details.Split('.')[0];
                bytes.Status = MessageStatusEnum.Ok;
                bytes.Details = number + ". Hello Client";
                return bytes;
            };

            for (int t = 0; t < 10; t++)
            {
                Assert.IsTrue(Clients[t].Connect(PipeName));
                //send 10 requests
                var request = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, string.Format("{0}. Hello Server", t));
                var response = Clients[t].Request(request, 30000);
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                Assert.IsNotNull(response.Result);
                Console.WriteLine("Server Response = {0}", response.Result);
                Clients[t].Disconnect();
            }
        }
        [TestMethod]
        public async Task SequentialClientsAsync_10()
        {
            Host.RequestReceived += (h, bytes) =>
            {
                var number = bytes.Details.Split('.')[0];
                bytes.Status = MessageStatusEnum.Ok;
                bytes.Details = number + ". Hello Client";
                return bytes;
            };

            for (int t = 0; t < 10; t++)
            {
                Assert.IsTrue(Clients[t].Connect(PipeName));
                //send 10 requests
                var request = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, string.Format("{0}. Hello Server", t));
                var response = await Clients[t].RequestAsync(request, 30000);
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                Assert.IsNotNull(response.Result);
                Console.WriteLine("Server Response = {0}", response.Result);
                Clients[t].Disconnect();
            }
        }

        [TestMethod]
        public void ConcurrentClients_10()
        {
            Host.RequestReceived += (h, bytes) =>
            {
                var number = bytes.Details.Split('.')[0];
                bytes.Status = MessageStatusEnum.Ok;
                bytes.Details = number + ". Hello Client";
                return bytes;
            };

            for (int t = 0; t < 10; t++)
            {
                Assert.IsTrue(Clients[t].Connect(PipeName));
            }
            for (int t = 0; t < 10; t++)
            {
                //send 10 requests
                var request = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, string.Format("{0}. Hello Server", t));
                var response = Clients[t].Request(request, 30000);
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                Assert.IsNotNull(response.Result);
                Console.WriteLine("Server Response = {0}", response.Result);
            }
            for (int t = 0; t < 10; t++)
            {
                Clients[t].Disconnect();
            }
        }
        [TestMethod]
        public async Task ConcurrentClientsAsync_10()
        {
            Host.RequestReceived += (h, bytes) =>
            {
                var number = bytes.Details.Split('.')[0];
                bytes.Status = MessageStatusEnum.Ok;
                bytes.Details = number + ". Hello Client";
                return bytes;
            };

            for (int t = 0; t < 10; t++)
            {
                Assert.IsTrue(Clients[t].Connect(PipeName));
            }
            for (int t = 0; t < 10; t++)
            {
                //send 10 requests
                var request = W.IO.Pipes.Message.Create(IO.Pipes.MessageStatusEnum.Ok, string.Format("{0}. Hello Server", t));
                var response = await Clients[t].RequestAsync(request, 30000);
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                Assert.IsNotNull(response.Result);
                Console.WriteLine("Server Response = {0}", response.Result);
            }
            for (int t = 0; t < 10; t++)
            {
                Clients[t].Disconnect();
            }
        }

        public SimplePipe_Clients_Tests() : base(10) { }
    }

    #region Transaction and Customer Classes
    public enum DatabaseActionsEnum
    {
        None,
        Create,
        Retrieve,
        Update,
        Delete
    }
    public enum TransactionStatusEnum
    {
        Ok,
        Exception
    }
    public interface IDataType { }
    public class Customer : IDataType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return string.Format("Id={0}, Name={1}", Id, Name);
        }
    }
    public class Transaction<TData> where TData : IDataType
    {
        public DatabaseActionsEnum Action { get; set; }
        public TransactionStatusEnum Status { get; set; }
        public string Details { get; set; }
        public TData Data { get; set; }
        public override string ToString()
        {
            return string.Format("Action={0}, Status={1}, Details={2}, Customer={3}", Action, Status, Details, Data);
        }
    }
    #endregion

    [TestClass]
    public class SimplePipe_Client_Transaction_Tests : SimplePipe_Client<Transaction<Customer>>
    {
        private static Transaction<Customer> HandleTransaction(Transaction<Customer> transaction)
        {
            switch (transaction.Action)
            {
                case DatabaseActionsEnum.Create:
                    //simulate a customer create
                    transaction.Status = TransactionStatusEnum.Ok;
                    transaction.Data = new Customer() { Name = transaction.Data.As<Customer>()?.Name ?? "New Customer", Id = Guid.NewGuid().ToString() };
                    break;
                case DatabaseActionsEnum.Retrieve:
                    //simulate a customer lookup
                    transaction.Status = TransactionStatusEnum.Ok;
                    transaction.Data = new Customer() { Name = "Jordan", Id = transaction.Data.As<Customer>().Id };
                    break;
                case DatabaseActionsEnum.Update:
                    //simulate a customer update
                    transaction.Status = TransactionStatusEnum.Ok;
                    transaction.Details = "Updated";
                    break;
                case DatabaseActionsEnum.Delete:
                    transaction.Status = TransactionStatusEnum.Exception;
                    transaction.Data = null;
                    transaction.Details = "Customer not found";
                    break;
            }
            return transaction;
        }
        [TestMethod]
        public async Task SimplePipes_Objects_CreateCustomer()
        {
            Host.RequestReceived += (h, bytes) =>
            {
                Console.WriteLine("Host Received: {0}", bytes.ToString());
                return HandleTransaction(bytes);
            };
            if (Client.Connect(PipeName))
            {
                var request = new Transaction<Customer>() { Action = DatabaseActionsEnum.Create, Data = new Customer() { Name = "Jordan" } };
                var response = await Client.RequestAsync(request, 30000);
                Assert.IsTrue(response != null);
                Assert.IsTrue(response.Success == true);
                Assert.IsTrue(response.Exception == null);
                Assert.IsTrue(response.Result != null);
                Console.WriteLine("Client Received: {0}", response.Result);
            }
        }
        [TestMethod]
        public async Task SimplePipes_Objects_RetrieveCustomer()
        {
            Host.RequestReceived += (h, bytes) =>
            {
                Console.WriteLine("Host Received: {0}", bytes);
                return HandleTransaction(bytes);
            };
            if (Client.Connect(PipeName))
            {
                var request = new Transaction<Customer>() { Action = DatabaseActionsEnum.Retrieve, Data = new Customer() { Id = "1" } };
                var response = await Client.RequestAsync(request, 30000);
                Assert.IsTrue(response != null);
                Assert.IsTrue(response.Success == true);
                Assert.IsTrue(response.Exception == null);
                Assert.IsTrue(response.Result != null);
                Console.WriteLine("Client Received: {0}", response.Result);
            }
        }
        [TestMethod]
        public async Task SimplePipes_Objects_UpdateCustomer()
        {
            Host.RequestReceived += (h, bytes) =>
            {
                Console.WriteLine("Host Received: {0}", bytes);
                return HandleTransaction(bytes);
            };
            if (Client.Connect(PipeName))
            {
                var request = new Transaction<Customer>() { Action = DatabaseActionsEnum.Update, Data = new Customer() { Id = "1", Name = "Duerksen" } };
                var response = await Client.RequestAsync(request, 30000);
                Assert.IsTrue(response != null);
                Assert.IsTrue(response.Success == true);
                Assert.IsTrue(response.Exception == null);
                Assert.IsTrue(response.Result != null);
                Console.WriteLine("Client Received: {0}", response.Result);
            }
        }
        [TestMethod]
        public async Task SimplePipes_Objects_DeleteCustomer()
        {
            Host.RequestReceived += (h, bytes) =>
            {
                Console.WriteLine("Host Received: {0}", bytes);
                return HandleTransaction(bytes);
            };
            if (Client.Connect(PipeName))
            {
                var request = new Transaction<Customer>() { Action = DatabaseActionsEnum.Delete, Data = new Customer() { Id = "1" } };
                var response = await Client.RequestAsync(request, 30000);
                Assert.IsTrue(response != null);
                Assert.IsTrue(response.Success == true);
                Assert.IsTrue(response.Exception == null);
                Assert.IsTrue(response.Result != null);
                Console.WriteLine("Client Received: {0}", response.Result);
            }
        }
    }
}
