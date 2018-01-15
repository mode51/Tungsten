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
        private W.Net.RPC.Client _caller = new Net.RPC.Client(RemoteEndPoint);

        public async static Task Run()
        {
            //create a new instance to start the server
            using (var callTests = new W.Tests.TestRPCClient())
            {
                callTests.Start();
                try
                {
                    callTests.TestWaitForResponse1().Wait();
                    callTests.TestWaitForResponse2().Wait();
                    callTests.CallTest1().Wait();
                    callTests.CallTest2_WrongNumberOfArguments().Wait();
                    callTests.CallTest2_WrongNumberOfArguments().Wait();
                    callTests.CallTest3().Wait();
                    callTests.CallTest4().Wait();
                    Console.WriteLine();
                    callTests.CallTestGetValue1().Wait();
                    callTests.CallTestGetValue2().Wait();
                    callTests.CallTestGetValue3().Wait();
                    callTests.CallTestGetValue4().Wait();
                    callTests.CallTestGetValue5().Wait();

                    await callTests.CallRPCMethod1();
                    await callTests.CallRPCMethod2();
                    callTests.CallRPCMethod3();

                    callTests.CallRPCTestGetvalue1();
                    callTests.CallRPCTestGetvalue2();
                    callTests.CallRPCTestGetvalue3();
                    callTests.CallRPCTestGetvalue4_WrongNumberOfArguments();

                    callTests.CallRPCUnknownMethod();
                    callTests.CallRPCUnknownMethod2();
                    callTests.CallRPCUnknownMethod3();
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
        private void Start()
        {
            _server = new Net.RPC.Server();
            Console.WriteLine(new string('_', 80));
            foreach (var method in _server.Methods)
            {
                Console.WriteLine(method.Key);
            }
            Console.WriteLine(new string('_', 80));
            //_server.IsListeningChanged += (isListening) =>
            //{
            //    Console.WriteLine("Server IsListening = {0}", isListening);
            //};
            _server.Start(RemoteEndPoint);
            //if (!_server.WaitForIsListening())
            //    Console.WriteLine("Server failed to start withing the allotted time.");
            //await _caller.ConnectAsync(RemoteEndPoint);
        }
        private void ShowResponse(W.Net.RPC.RPCResponse result, string description = "", [System.Runtime.CompilerServices.CallerMemberName] string callerName = "")
        {
            Console.WriteLine(new string('_', Console.BufferWidth-2));
            if (string.IsNullOrEmpty(description))
                Console.WriteLine("{0}\n{1}", callerName, result.ToString());
            else
                Console.WriteLine("{0}: {1}\n{2}", callerName, description, result.ToString());
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
            //_caller.Disconnect();
            _caller.Dispose();
            _server?.Dispose();
            _server = null;
        }

        public async Task TestWaitForResponse1()
        {
            var result = await _caller.CallAsync<object>("W.Tests.Sample_RPC_Class.TestGetValue1");
            //Console.WriteLine("TestWaitForResponse1: TestGetValue1 (as object)\n{0}", result);
            ShowResponse(result, "<object>");
            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue1"))
            //{
            //    var result = await handler.WaitForResponse();
            //    Console.WriteLine("TestWaitForResponse1: TestGetValue1 (as object) = {0}", result.Response);
            //}
        }
        public async Task TestWaitForResponse2()
        {
            var result = await _caller.CallAsync<long>("W.Tests.Sample_RPC_Class.TestGetValue1");
            //Console.WriteLine("TestWaitForResponse2: TestGetValue1 (as long)\n{0}", result);
            ShowResponse(result, "<long>");
            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue1"))
            //{
            //    var result = await handler.WaitForResponse<long>();
            //    Console.WriteLine("TestWaitForResponse2: TestGetValue1 (as long) = {0}", result.Response);
            //}
        }
        public async Task CallTest1()
        {
            var result = await _caller.CallAsync<object>("W.Tests.Sample_RPC_Class.Test1");
            //Console.WriteLine("CallTest1: Test1 (as object)\n{0}", result);
            ShowResponse(result);

            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test1"))
            //{
            //    var result = await handler.WaitForResponse();
            //    Console.WriteLine("CallTest1: Test1 (as object) = {0}", result.Response);
            //}
        }
        public async Task CallTest3()
        {
            var result = await _caller.CallAsync<object>("W.Tests.Sample_RPC_Class.Test3", "{0} != {1}", new object[] { 1, 2 });
            //Console.WriteLine("CallTest3: Test3 (as object)\n{0}", result);
            ShowResponse(result, "<object>");
            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test3", "{0} != {1}", new object[] { 1, 2 }))
            //{
            //    var result = await handler.WaitForResponse();
            //    Console.WriteLine("CallTest3: Test3 (as object) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            //}
        }
        public async Task CallTest4()
        {
            var result = await _caller.CallAsync<object>("W.Tests.Sample_RPC_Class.Test4");
            //Console.WriteLine("CallTest4: Test4 (as object)\n{0}", result);
            ShowResponse(result, "<object>");
            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test4"))
            //{
            //    var result = await handler.WaitForResponse();
            //    Console.WriteLine("CallTest4: Test4 (as object) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            //}
        }
        public async Task CallTestGetValue1()
        {
            var result = await _caller.CallAsync<long>("W.Tests.Sample_RPC_Class.TestGetValue1");
            //Console.WriteLine("CallTestGetValue1: TestGetValue1 (as long)\n{0}", result);
            ShowResponse(result, "<long>");
            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue1"))
            //{
            //    var result = await handler.WaitForResponse<long>();
            //    Console.WriteLine("CallTestGetValue1: TestGetValue1 (as long) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            //}
        }
        public async Task CallTestGetValue2()
        {
            var result = await _caller.CallAsync<string>("W.Tests.Sample_RPC_Class.TestGetValue2");
            //Console.WriteLine("CallTestGetValue2: TestGetValue2 (as string)\n{0}", result);
            ShowResponse(result, "<string>");
            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue2"))
            //{
            //    var result = await handler.WaitForResponse<string>();
            //    Console.WriteLine("CallTestGetValue2: TestGetValue2 (as string) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            //}
        }
        public async Task CallTestGetValue3()
        {
            var result = await _caller.CallAsync<object>("W.Tests.Sample_RPC_Class.TestGetValue3");
            //Console.WriteLine("CallTestGetValue3: TestGetValue3 (as object)\n{0}", result);
            ShowResponse(result, "<object>");
            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue3"))
            //{
            //    var result = await handler.WaitForResponse();
            //    Console.WriteLine("CallTestGetValue3: TestGetValue3 (as object) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            //}
        }
        public async Task CallTestGetValue4()
        {
            var result = await _caller.CallAsync<string>("W.Tests.Sample_RPC_Class.TestGetValue4", "SomeKey");
            //Console.WriteLine("CallTestGetValue4: TestGetValue4 (as string)\n{0}", result);
            ShowResponse(result, "<string>");
            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.TestGetValue4", "SomeKey"))
            //{
            //    var result = await handler.WaitForResponse<string>();
            //    Console.WriteLine("CallTestGetValue4: TestGetValue4 (as string) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            //}
        }
        public async Task CallTestGetValue5()
        {
            var result = await _caller.CallAsync<SampleData>("W.Tests.Sample_RPC_Class.TestGetValue5");
            //Console.WriteLine("CallTestGetValue5: TestGetValue5 (as SampleData) = {0}/{1}, Exception: {2}", result.Response.Name, result.Response.Age, result.Exception);
            ShowResponse(result, "<SampleData>");
        }

        #region Previous Tests (some duplicate from above)
        public async Task CallRPCMethod1()
        {
            var result = await _caller.CallAsync("W.Tests.Sample_RPC_Class.Test1");
            ShowResponse(result, "void");
            //if (!string.IsNullOrEmpty(result.Exception))
            //    Console.WriteLine(result.Exception);
        }
        public async Task CallRPCMethod2()
        {
            var result = await _caller.CallAsync<object>("W.Tests.Sample_RPC_Class.Test2", "This is a sample message");
            ShowResponse(result, "<object>");
            //if (!string.IsNullOrEmpty(result.Exception))
            //    Console.WriteLine(result.Exception);
        }
        public void CallRPCMethod3()
        {
            var result = _caller.Call<object>("W.Tests.Sample_RPC_Class.Test3", "This is a {0} message", new object[] { "SAMPLE" });
            ShowResponse(result, "<object>");
            //if (!string.IsNullOrEmpty(result.Exception))
            //    Console.WriteLine(result.Exception);
        }

        public void CallRPCTestGetvalue1()
        {
            var result = _caller.Call<long>("W.Tests.Sample_RPC_Class.TestGetValue1");
            ShowResponse(result, "<long>");
            //if (!string.IsNullOrEmpty(result.Exception))
            //    Console.WriteLine(result.Exception);
            //else
            //    Console.WriteLine("TestGetValue1 = {0}", result.Response);
        }
        public void CallRPCTestGetvalue2()
        {
            var result = _caller.Call<string>("W.Tests.Sample_RPC_Class.TestGetValue2");
            ShowResponse(result, "<string>");
            //if (!string.IsNullOrEmpty(result.Exception))
            //    Console.WriteLine(result.Exception);
            //else
            //    Console.WriteLine("TestGetValue2 = {0}", result.Response);
        }
        public void CallRPCTestGetvalue3()
        {
            var result = _caller.Call<object>("W.Tests.Sample_RPC_Class.TestGetValue3");
            ShowResponse(result, "<object>");
            //if (!string.IsNullOrEmpty(result.Exception))
            //    Console.WriteLine(result.Exception);
            //else
            //    Console.WriteLine("TestGetValue3\n{0}", result);
        }
        #endregion

        #region Wrong number of arguments
        public async Task CallTest2_WrongNumberOfArguments_No_Arguments()
        {
            var result = await _caller.CallAsync<object>("W.Tests.Sample_RPC_Class.Test2");
            //Console.WriteLine("CallTest2: Test2 (as object, with no arguments)\n{0}", result);
            ShowResponse(result, "<object>, wrong number of no arguments");
            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test2"))
            //{
            //    var result = await handler.WaitForResponse();
            //    Console.WriteLine("CallTest2: Test2 (as object, with no arguments) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            //}
        }
        public async Task CallTest2_WrongNumberOfArguments()
        {
            var result = await _caller.CallAsync<object>("W.Tests.Sample_RPC_Class.Test2", "some string parameter");
            //Console.WriteLine("CallTest2b: Test2 (as object, with correct argument)\n{0}", result);
            ShowResponse(result, "<object> correct number of arguments");
            //using (var handler = _caller.Call("W.Tests.Sample_RPC_Class.Test2", "some string parameter"))
            //{
            //    var result = await handler.WaitForResponse();
            //    Console.WriteLine("CallTest2b: Test2 (as object, with correct argument) = {0}, Exception: {1}", result.Response, result.Exception?.Message);
            //}
        }
        public void CallRPCTestGetvalue4_WrongNumberOfArguments()
        {
            var result = _caller.Call<string>("W.Tests.Sample_RPC_Class.TestGetValue4");
            ShowResponse(result, "<string>");
            //if (!string.IsNullOrEmpty(result.Exception))
            //    Console.WriteLine(result.Exception);
            //else
            //    Console.WriteLine("TestGetValue4 (wrong number of arguments)\n{0}", result);
        }
        #endregion

        #region Unknown methods
        public void CallRPCUnknownMethod()
        {
            var result = _caller.Call<object>("W.Tests.Sample_RPC_Class.UnknownMethod");
            //Console.WriteLine("UnknownMethod\n{0}", result);
            ShowResponse(result, "<object>");
        }
        public void CallRPCUnknownMethod2()
        {
            var result = _caller.Call<int>("W.Tests.Sample_RPC_Class.UnknownMethod2", "Sample Echo");
            //Console.WriteLine("UnknownMethod2\n{0}", result);
            ShowResponse(result, "<int>");
        }
        public void CallRPCUnknownMethod3()
        {
            var result = _caller.Call<object>("W.Tests.Sample_RPC_Class.UnknownMethod3", 14, 15, 16, "Test");
            //Console.WriteLine("UnknownMethod3\n{0}", result);
            ShowResponse(result, "<object>");
        }
        #endregion
    }
}
