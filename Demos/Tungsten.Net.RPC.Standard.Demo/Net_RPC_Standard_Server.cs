using System;

namespace W.Tests
{
    //3.28.2017 - Note that this test class does NOT start the RPC server; it just tests loading and calling methods
    internal class Net_RPC_Standard_Server : IDisposable
    {
        public static void Run()
        {
            using (var tests = new Net_RPC_Standard_Server())
            {
                tests.Create();
                tests.CallTest1();
                tests.CallTest2();
                tests.CallTest3();
                tests.CallTestGetValue1();
                tests.CallTestGetValue2();
                tests.CallTestGetValue3();
                tests.CallTestGetValue4();
            }
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }
        public void Dispose()
        {
        }
        public void Create()
        {
            using (var server = new W.Net.RPC.Server())
            {
                foreach(var method in server.Methods)
                {
                    Console.WriteLine(method.Key);
                }
            }
        }
        public void CallTest1()
        {
            using (var server = new W.Net.RPC.Server())
            {
                server.Methods.Call("W.Tests.Sample_RPC_Class.Test1");
            }
        }
        public void CallTest2()
        {
            using (var server = new W.Net.RPC.Server())
            {
                server.Methods.Call("W.Tests.Sample_RPC_Class.Test2", "This is a sample message");
            }
        }
        public void CallTest3()
        {
            using (var server = new W.Net.RPC.Server())
            {
                server.Methods.Call("W.Tests.Sample_RPC_Class.Test3", "This is a {0} message", new object[] { "SAMPLE" });
            }
        }
        public void CallTestGetValue1()
        {
            using (var server = new W.Net.RPC.Server())
            {
                var result = server.Methods.Call<long>("W.Tests.Sample_RPC_Class.TestGetValue1");
                Console.WriteLine("TestGetValue1: {0}", result);
            }
        }
        public void CallTestGetValue2()
        {
            using (var server = new W.Net.RPC.Server())
            {
                var result = server.Methods.Call<string>("W.Tests.Sample_RPC_Class.TestGetValue2");
                Console.WriteLine("TestGetValue2: {0}", result);
            }
        }
        public void CallTestGetValue3()
        {
            using (var server = new W.Net.RPC.Server())
            {
                var result = server.Methods.Call<object>("W.Tests.Sample_RPC_Class.TestGetValue3");
                Console.WriteLine("TestGetValue3: {0}", result);
            }
        }
        public void CallTestGetValue4()
        {
            using (var server = new W.Net.RPC.Server())
            {
                var result = server.Methods.Call<string>("W.Tests.Sample_RPC_Class.TestGetValue4", "Echo this!");
                Console.WriteLine("TestGetValue4: {0}", result);
            }
        }
    }
}
