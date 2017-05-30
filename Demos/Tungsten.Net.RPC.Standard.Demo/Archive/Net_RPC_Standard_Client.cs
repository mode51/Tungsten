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
    internal class Net_RPC_Standard_secureClient : IDisposable
    {
        private static IPAddress IPADDRESS = IPAddress.Loopback;
        private static int SECUREPORT = 5151;
        private static IPEndPoint SECUREENDPOINT = new IPEndPoint(IPADDRESS, SECUREPORT);

        private W.Net.RPC.Server _secureServer;
        private W.Net.RPC.Client _secureClient = new Net.RPC.Client();

        public async static Task Run()
        {
            try
            {
                //create a new instance to start the server
                using (var callTests = new W.Tests.Net_RPC_Standard_secureClient())
                {
                    callTests.CallRPCMethod1();
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }
        public Net_RPC_Standard_secureClient()
        {
            //_server = new Net.RPC.RPCServer();
            //_server.IsListeningChanged += (isListening) =>
            //{
            //    Console.WriteLine("RPC Server IsListening = {0}", isListening);
            //};
            //_server.Start(IPADDRESS, PORT);
            //if (!_server.WaitForIsListening())
            //    Console.WriteLine("RPC Server failed to start withing the allotted time.");

            _secureServer = new Net.RPC.Server();
            _secureServer.IsListeningChanged += (isListening) =>
            {
                Console.WriteLine("Secure RPC Server IsListening = {0}", isListening);
            };
            _secureServer.Start(IPADDRESS, SECUREPORT);
            if (!_secureServer.WaitForIsListening())
                Console.WriteLine("Secure RPC Server failed to start withing the allotted time.");

            var result = _secureClient.ConnectAsync(new IPEndPoint(IPADDRESS, SECUREPORT)).Result;
            Console.WriteLine("Secure RPC Client Connected = {0}", result);
        }
        ~Net_RPC_Standard_secureClient()
        {
            Dispose();
        }
        public void Dispose()
        {
            _secureClient?.Disconnect();
            _secureClient = null;
            
            _secureServer?.Dispose();
            _secureServer = null;
            //_server?.Dispose();
            //_server = null;
        }

        public void CallRPCMethod1()
        {
            if (_secureClient.ConnectAsync(SECUREENDPOINT).Result)
            {
                var handler = _secureClient.Call("W.Tests.Sample_RPC_Class.Test1");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                    handler.WaitForResponse().Wait(3000);
                _secureClient.Disconnect();
            }
        }
        public void CallRPCMethod2()
        {
            if (_secureClient.ConnectAsync(SECUREENDPOINT).Result)
            {
                var handler = _secureClient.Call("W.Tests.Sample_RPC_Class.Test2", "This is a sample message");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                    handler.WaitForResponse().Wait(3000);
                _secureClient.Disconnect();
            }
        }
        public void CallRPCMethod3()
        {
            if (_secureClient.ConnectAsync(SECUREENDPOINT).Result)
            {
                var handler = _secureClient.Call("W.Tests.Sample_RPC_Class.Test3", "This is a {0} message", new object[] { "SAMPLE" });
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                    handler.WaitForResponse().Wait(3000);
                _secureClient.Disconnect();
            }
        }

        public void CallRPCTestGetvalue1()
        {
            if (_secureClient.ConnectAsync(SECUREENDPOINT).Result)
            {
                var handler = _secureClient.Call("W.Tests.Sample_RPC_Class.TestGetValue1");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = handler.WaitForResponse<long>().Wait(3000);
                    Console.WriteLine("TestGetValue1 = {0}", result);
                }
                _secureClient.Disconnect();
            }
        }
        public void CallRPCTestGetvalue2()
        {
            if (_secureClient.ConnectAsync(SECUREENDPOINT).Result)
            {
                var handler = _secureClient.Call("W.Tests.Sample_RPC_Class.TestGetValue2");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = handler.WaitForResponse<string>().Wait(3000);
                    Console.WriteLine("TestGetValue2 = {0}", result);
                }
                _secureClient.Disconnect();
            }
        }
        public void CallRPCTestGetvalue3()
        {
            if (_secureClient.ConnectAsync(SECUREENDPOINT).Result)
            {
                var handler = _secureClient.Call("W.Tests.Sample_RPC_Class.TestGetValue3");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = handler.WaitForResponse<object>().Wait(3000);
                    Console.WriteLine("TestGetValue3 = {0}", result);
                }
                _secureClient.Disconnect();
            }
        }
        public void CallRPCTestGetvalue4()
        {
            if (_secureClient.ConnectAsync(SECUREENDPOINT).Result)
            {
                var handler = _secureClient.Call("W.Tests.Sample_RPC_Class.TestGetValue4");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = handler.WaitForResponse<string>().Wait(3000);
                    Console.WriteLine("TestGetValue4 = {0}", result);
                }
                _secureClient.Disconnect();
            }
        }

        public async Task CallRPCUnknownMethod()
        {
            if (_secureClient.ConnectAsync(SECUREENDPOINT).Result)
            {
                var handler = _secureClient.Call("W.Tests.Sample_RPC_Class.UnknownMethod");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = handler.WaitForResponse().Wait(3000);
                    Console.WriteLine("UnknownMethod = {0}", result);
                }
                _secureClient.Disconnect();
            }
        }
        public async Task CallRPCUnknownMethod2()
        {
            if (_secureClient.ConnectAsync(SECUREENDPOINT).Result)
            {
                var handler = _secureClient.Call("W.Tests.Sample_RPC_Class.UnknownMethod2", "Sample Echo");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = handler.WaitForResponse<string>().Wait(3000); //not awaited, so the the result is the response
                    Console.WriteLine("UnknownMethod2 = {0}", result);
                }
                _secureClient.Disconnect();
            }
        }
        public async Task CallRPCUnknownMethod3()
        {
            if (_secureClient.ConnectAsync(SECUREENDPOINT).Result)
            {
                var handler = _secureClient.Call("W.Tests.Sample_RPC_Class.UnknownMethod3", 14, 15, 16, "test");
                if (handler.CallException != null)
                    Console.WriteLine(handler.CallException.ToString());
                else
                {
                    var result = await handler.WaitForResponse();
                    Console.WriteLine("UnknownMethod3 = {0}", result.Response);
                }
                _secureClient.Disconnect();
            }
        }

    }
}
