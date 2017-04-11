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
    internal class Net_RPC_Standard_Client : IDisposable
    {
        private static IPAddress IPADDRESS = IPAddress.Loopback;
        private static int PORT = 5150;

        private W.Net.RPC.Server _server;
        private W.Net.RPC.Client _client = new Net.RPC.Client();

        public static void Run()
        {
            //create a new instance to start the server
            using (var callTests = new W.Tests.Net_RPC_Standard_Client())
            {
                callTests.CallRPCMethod1();
                callTests.CallRPCMethod2();
                callTests.CallRPCMethod3();
                callTests.CallRPCTestGetvalue1();
                callTests.CallRPCTestGetvalue2();
                callTests.CallRPCTestGetvalue3();
                callTests.CallRPCTestGetvalue4();
            }
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }

        public Net_RPC_Standard_Client()
        {
            _server = new Net.RPC.Server();
            _server.IsListeningChanged += (isListening) =>
            {
                Console.WriteLine("Server IsListening = {0}", isListening);
            };
            _server.Start(IPADDRESS, PORT);
            if (!_server.WaitForIsListening())
                Console.WriteLine("Server failed to start withing the allotted time.");
        }
        ~Net_RPC_Standard_Client()
        {
            Dispose();
        }
        public void Dispose()
        {
            _client?.Dispose();
            _client = null;
            _server?.Dispose();
            _server = null;
        }

        public void CallRPCMethod1()
        {
            if (_client.Connect(IPADDRESS, PORT))
            {
                _client.Call("W.Tests.Sample_RPC_Class.Test1");
                _client.Disconnect();
            }
        }
        public void CallRPCMethod2()
        {
            if (_client.Connect(IPADDRESS, PORT))
            {
                _client.Call("W.Tests.Sample_RPC_Class.Test2", "This is a sample message");
                System.Threading.Thread.Sleep(10);
                _client.Disconnect();
            }
        }
        public void CallRPCMethod3()
        {
            if (_client.Connect(IPADDRESS, PORT))
            {
                _client.Call("W.Tests.Sample_RPC_Class.Test3", "This is a {0} message", new object[] { "SAMPLE" });
                System.Threading.Thread.Sleep(10);
                _client.Disconnect();
            }
        }
        public void CallRPCTestGetvalue1()
        {
            if (_client.Connect(IPADDRESS, PORT))
            {
                //optionally, you can wait on the returned ManualResetEvent
                var mre = _client.Call<long>("W.Tests.Sample_RPC_Class.TestGetValue1", (value, isExpired) =>
                {
                    Console.WriteLine("TestGetValue1 = {0}, IsExpired = {1}", value, isExpired);
                });
                mre.WaitOne();
                _client.Disconnect();
            }
        }
        public void CallRPCTestGetvalue2()
        {
            if (_client.Connect(IPADDRESS, PORT))
            {
                _client.Call<string>("W.Tests.Sample_RPC_Class.TestGetValue2", (value, isExpired) =>
                {
                    Console.WriteLine("TestGetValue2 = {0}, IsExpired = {1}", value, isExpired);
                });
                _client.Disconnect();
            }
        }
        public void CallRPCTestGetvalue3()
        {
            if (_client.Connect(IPADDRESS, PORT))
            {
                _client.Call<object>("W.Tests.Sample_RPC_Class.TestGetValue3", (value, isExpired) =>
                {
                    Console.WriteLine("TestGetValue3 = {0}, IsExpired = {1}", value, isExpired);
                });
                _client.Disconnect();
            }
        }
        public void CallRPCTestGetvalue4()
        {
            if (_client.Connect(IPADDRESS, PORT))
            {
                _client.Call<string>("W.Tests.Sample_RPC_Class.TestGetValue4", (value, isExpired) =>
                {
                    Console.WriteLine("TestGetValue4 = {0}, IsExpired = {1}", value, isExpired);
                }, "Sample Echo");
                _client.Disconnect();
            }
        }
    }
}
