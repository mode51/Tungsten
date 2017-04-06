using System;

namespace W.Tests
{
    [W.Net.RPC.RPCClass()]
    public class Sample_RPC_Class
    {
        [W.Net.RPC.RPCMethod()]
        public static void Test1()
        {
            Console.WriteLine("Test1 Called");
        }
        [W.Net.RPC.RPCMethod()]
        public static void Test2(string message)
        {
            Console.WriteLine("Test2: {0}", message);
        }
        [W.Net.RPC.RPCMethod()]
        public static void Test3(string format, params object[] args)
        {
            Console.WriteLine("Test3: {0}", string.Format(format, args));
        }
        [W.Net.RPC.RPCMethod()]
        public static long TestGetValue1()
        {
            Console.WriteLine("In TestGetValue1");
            return 1;
        }
        [W.Net.RPC.RPCMethod()]
        public static string TestGetValue2()
        {
            Console.WriteLine("In TestGetValue2");
            return "Return Value";
        }
        [W.Net.RPC.RPCMethod()]
        public static object TestGetValue3()
        {
            Console.WriteLine("In TestGetValue3");
            return new object();
        }
        [W.Net.RPC.RPCMethod()]
        public static object TestGetValue4(string key)
        {
            Console.WriteLine("In TestGetValue4");
            return key;
        }
    }
}
