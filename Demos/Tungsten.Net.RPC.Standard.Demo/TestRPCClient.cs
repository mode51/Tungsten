using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using W;

namespace W.Tests
{
    internal class TestRPCClient : IDisposable
    {
        private static IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Loopback, 5150);

        private W.Net.RPC.Server _server;
        private W.Net.RPC.Client _caller = new Net.RPC.Client();

        public async static Task Run()
        {
            //create a new instance to start the server
            using (var callTests = new W.Tests.TestRPCClient())
            {
                await callTests.Start();
                try
                {
                    callTests.TestWaitForResponse1().Wait();
                    callTests.TestWaitForResponse2().Wait();
                    callTests.TestEarlyDispose().Wait();
                    callTests.CallTest1().Wait();
                    callTests.CallTest2().Wait();
                    callTests.CallTest2b().Wait();
                    callTests.CallTest3().Wait();
                    callTests.CallTest4().Wait();
                    Console.WriteLine();
                    callTests.CallTestGetValue1().Wait();
                    callTests.CallTestGetValue2().Wait();
                    callTests.CallTestGetValue3().Wait();
                    callTests.CallTestGetValue4().Wait();
                    callTests.CallTestGetValue5().Wait();

                    await callTests.CallRPCMethod1();
                    callTests.CallRPCMethod2();
                    callTests.CallRPCMethod3();

                    callTests.CallRPCTestGetvalue1();
                    callTests.CallRPCTestGetvalue2();
                    callTests.CallRPCTestGetvalue3();
                    callTests.CallRPCTestGetvalue4();

                    await callTests.CallRPCUnknownMethod();
                    await callTests.CallRPCUnknownMethod2();
                    await callTests.CallRPCUnknownMethod3();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            Console.WriteLine(new string('_', 80));
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }
        private async Task Start()
        {
            _server = new Net.RPC.Server();
            Console.WriteLine(new string('_', 80));
            foreach (var method in _server.Methods)
            {
                Console.WriteLine(method.Key);
            }
            Console.WriteLine(new string('_', 80));
            _server.IsListeningChanged += (isListening) =>
            {
                Console.WriteLine("Server IsListening = {0}", isListening);
            };
            _server.Start(RemoteEndPoint.Address, RemoteEndPoint.Port);
            if (!_server.WaitForIsListening())
                Console.WriteLine("Server failed to start withing the allotted time.");
            await _caller.ConnectAsync(RemoteEndPoint);
        }
        public TestRPCClient()
        {
        }
        ~TestRPCClient()
        {
            Dispose();
        }
        public void Dispose()
        {
            _caller.Disconnect();
            _server?.Dispose();
            _server = null;
        }

        public async Task TestWaitForResponse1()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue1"))
            {
                var result = await handler.WaitForResponse();
                Console.WriteLine("TestWaitForResponse1: TestGetValue1 (as object) = {0}", result.Response);
            }
        }
        public async Task TestWaitForResponse2()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue1"))
            {
                var result = await handler.WaitForResponse<long>();
                Console.WriteLine("TestWaitForResponse2: TestGetValue1 (as long) = {0}", result.Response);
            }
        }
        public async Task TestEarlyDispose()
        {
            var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue1");
            handler.Dispose();
            var result = await handler.WaitForResponse<long>();
            Console.WriteLine("TestEarlyDispose: TestGetValue1 (as long, disposed too soon) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
        }
        public async Task CallTest1()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test1"))
            {
                var result = await handler.WaitForResponse();
                Console.WriteLine("CallTest1: Test1 (as object) = {0}", result.Response);
            }
        }
        public async Task CallTest2()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test2"))
            {
                var result = await handler.WaitForResponse();
                Console.WriteLine("CallTest2: Test2 (as object, with no arguments) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            }
        }
        public async Task CallTest2b()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test2", "some string parameter"))
            {
                var result = await handler.WaitForResponse();
                Console.WriteLine("CallTest2b: Test2 (as object, with correct argument) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            }
        }
        public async Task CallTest3()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test3", "{0} != {1}", new object[] { 1, 2 }))
            {
                var result = await handler.WaitForResponse();
                Console.WriteLine("CallTest3: Test3 (as object) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            }
        }
        public async Task CallTest4()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test4"))
            {
                var result = await handler.WaitForResponse();
                Console.WriteLine("CallTest4: Test4 (as object) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            }
        }
        public async Task CallTestGetValue1()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue1"))
            {
                var result = await handler.WaitForResponse<long>();
                Console.WriteLine("CallTestGetValue1: TestGetValue1 (as long) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            }
        }
        public async Task CallTestGetValue2()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue2"))
            {
                var result = await handler.WaitForResponse<string>();
                Console.WriteLine("CallTestGetValue2: TestGetValue2 (as string) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            }
        }
        public async Task CallTestGetValue3()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue3"))
            {
                var result = await handler.WaitForResponse();
                Console.WriteLine("CallTestGetValue3: TestGetValue3 (as object) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            }
        }
        public async Task CallTestGetValue4()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue4", "SomeKey"))
            {
                var result = await handler.WaitForResponse<string>();
                Console.WriteLine("CallTestGetValue4: TestGetValue4 (as string) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            }
        }
        public async Task CallTestGetValue5()
        {
            using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue5"))
            {
                var result = await handler.WaitForResponse<SampleData>();
                Console.WriteLine("CallTestGetValue5: TestGetValue5 (as SampleData) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            }
        }

        #region Previous Tests (some duplicate from above)
        public async Task CallRPCMethod1()
        {
            var connected = await _caller.ConnectAsync(RemoteEndPoint);

            if (connected)
            //if (_caller.ConnectAsync(RemoteEndPoint).Result)
            {
                var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test1");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                    handler.WaitForResponse().Wait(3000);
                _caller.Disconnect();
            }
        }
        public async Task CallRPCMethod2()
        {
            var connected = await _caller.ConnectAsync(RemoteEndPoint);
            if (connected)
            //if (_caller.ConnectAsync(RemoteEndPoint).Result)
            {
                var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test2", "This is a sample message");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                    handler.WaitForResponse().Wait(3000);
                _caller.Disconnect();
            }
        }
        public void CallRPCMethod3()
        {
            var connected = _caller.ConnectAsync(RemoteEndPoint).Result;
            if (connected)
            //if (_caller.ConnectAsync(RemoteEndPoint).Result)
            {
                var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test3", "This is a {0} message", new object[] { "SAMPLE" });
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                    handler.WaitForResponse().Wait(3000);
                _caller.Disconnect();
            }
        }

        public void CallRPCTestGetvalue1()
        {
            if (_caller.ConnectAsync(RemoteEndPoint).Result)
            {
                var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue1");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = handler.WaitForResponse<long>().Wait(3000);
                    Console.WriteLine("TestGetValue1 = {0}", result);
                }
                _caller.Disconnect();
            }
        }
        public void CallRPCTestGetvalue2()
        {
            if (_caller.ConnectAsync(RemoteEndPoint).Result)
            {
                var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue2");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = handler.WaitForResponse<string>().Wait(3000);
                    Console.WriteLine("TestGetValue2 = {0}", result);
                }
                _caller.Disconnect();
            }
        }
        public void CallRPCTestGetvalue3()
        {
            if (_caller.ConnectAsync(RemoteEndPoint).Result)
            {
                var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue3");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = handler.WaitForResponse<object>().Wait(3000);
                    Console.WriteLine("TestGetValue3 = {0}", result);
                }
                _caller.Disconnect();
            }
        }
        public void CallRPCTestGetvalue4()
        {
            if (_caller.ConnectAsync(RemoteEndPoint).Result)
            {
                var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue4");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = handler.WaitForResponse<string>();//.Wait(3000);
                    Console.WriteLine("TestGetValue4 = {0}", result);
                }
                _caller.Disconnect();
            }
        }
        #endregion

        #region Unknown methods
        public async Task CallRPCUnknownMethod()
        {
            if (_caller.ConnectAsync(RemoteEndPoint).Result)
            {
                var handler = _caller.Call("W.Tests.Sample_RPC_Class.UnknownMethod");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = await handler.WaitForResponse();
                    Console.WriteLine("UnknownMethod = {0}", result.Response);
                }
                _caller.Disconnect();
            }
        }
        public async Task CallRPCUnknownMethod2()
        {
            if (_caller.ConnectAsync(RemoteEndPoint).Result)
            {
                var handler = _caller.Call("W.Tests.Sample_RPC_Class.UnknownMethod2", "Sample Echo");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = await handler.WaitForResponse<string>();
                    Console.WriteLine("UnknownMethod2 = {0}", result.Response);
                }
                _caller.Disconnect();
            }
        }
        public async Task CallRPCUnknownMethod3()
        {
            if (_caller.ConnectAsync(RemoteEndPoint).Result)
            {
                var handler = _caller.Call("W.Tests.Sample_RPC_Class.UnknownMethod3", 14, 15, 16, "test");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = await handler.WaitForResponse();
                    Console.WriteLine("UnknownMethod3 = {0}", result.Response);
                }
                _caller.Disconnect();
            }
        }
        #endregion
    }
}
