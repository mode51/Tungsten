using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace W.RPC.ServerDemo
{
    class Program
    {
        //The referenced Tungsten.RPC.Server.API dll contains a Sample class which exposes several RPC methods
        static void Main(string[] args)
        {
            W.RPC.API.Demo.Sample.Initialize(); //make sure the dll is loaded

            var server = new W.RPC.Server();
            server.Start(IPAddress.Parse("127.0.0.1"), 5150);
            Console.WriteLine("Press Any Key To Exit");
            Console.ReadKey();
            server.Stop();
        }
    }

    //[RPCClass]
    //public class RPCServer
    //{
    //    [RPCMethod]
    //    public static void WriteLine(string message)
    //    {
    //        Console.WriteLine(message);
    //    }
    //    [RPCMethod]
    //    public static bool CauseTimeout()
    //    {
    //        System.Threading.Thread.Sleep(5000);
    //        return false;
    //    }
    //}
}
