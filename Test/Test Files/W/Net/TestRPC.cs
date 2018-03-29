using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using W;
using W.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace W.Tests
{
    [W.Net.RPC.RPCClass()]
    public class RPCAPI
    {
        [W.Net.RPC.RPCMethod()]
        public static void Method1()
        {
            Console.WriteLine("Method1");
        }
        [W.Net.RPC.RPCMethod()]
        public static long Method2()
        {
            Console.WriteLine("Method2");
            return 47;
        }
        [W.Net.RPC.RPCMethod()]
        public static int Method3(int x, int y)
        {
            Console.WriteLine("Method3");
            return x * y;
        }
    }
    [TestClass]
    public class RPCTests
    {
        private static IPEndPoint ServerEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2215);
        private static int KeySize = 2048;
        private static ManualResetEventSlim _mreContinue = new ManualResetEventSlim(false);

        [TestMethod]
        public void RPC_CreateServer()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(ServerEndPoint, this.GetType().Assembly, false);
                Console.WriteLine("Server Started");
            }
            Console.WriteLine("Server Disposed");
        }
        [TestMethod]
        public void RPC_CreateClient()
        {
            using (var client = new W.Net.RPC.Client(ServerEndPoint, KeySize))
            {
                Console.WriteLine("Client Created");
                Assert.IsTrue(client.RemoteEndPoint == ServerEndPoint, "Server IPEndPoint Mismatch");
                Console.WriteLine("Client Verified");
            }
            Console.WriteLine("Client Disposed");
        }
        [TestMethod]
        public void RPC_FindMethods()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(ServerEndPoint, this.GetType().Assembly, false);
                Console.WriteLine("Server Started");
                Assert.IsTrue(server.API.Methods.Count == 3, "Server method count is wrong");
                foreach (var method in server.API.Methods)
                    Console.WriteLine($"Found Method: {method.Key}");
            }
            Console.WriteLine("Server Disposed");
        }
        [TestMethod]
        public void RPC_CallMethod1_Void()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(ServerEndPoint, this.GetType().Assembly, false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(ServerEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Assert.IsTrue(client.RemoteEndPoint == ServerEndPoint, "Server IPEndPoint Mismatch");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Tests.RPCAPI.Method1");
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
        }
        [TestMethod]
        public void RPC_CallMethod2_Function()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(ServerEndPoint, this.GetType().Assembly, false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(ServerEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Tests.RPCAPI.Method2");
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
        }
        [TestMethod]
        public void RPC_CallMethod3_Function_With_Parameters()
        {
            using (var server = new W.Net.RPC.Server(KeySize))
            {
                Console.WriteLine("Server Created");
                server.Start(ServerEndPoint, this.GetType().Assembly, false);
                Console.WriteLine("Server Started");
                using (var client = new W.Net.RPC.Client(ServerEndPoint, KeySize))
                {
                    Console.WriteLine("Client Created");
                    Console.WriteLine("Client Verified");
                    var response = client.Call("W.Tests.RPCAPI.Method3", 3, 19);
                    Console.WriteLine($"Success={response.Success}, Response={response.Response}, Exception={response.Exception}");
                }
                Console.WriteLine("Client Disposed");
            }
            Console.WriteLine("Server Disposed");
        }
    }
}
