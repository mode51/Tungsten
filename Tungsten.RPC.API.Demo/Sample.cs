using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.RPC;

namespace W.RPC.API.Demo
{
    [RPCClass]
    public class Sample
    {
        [RPCMethod]
        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
        [RPCMethod]
        public static bool CauseTimeout()
        {
            System.Threading.Thread.Sleep(5000);
            return false;
        }

        public static void Initialize()
        {
            //used by the caller to ensure the dll is loaded
        }
    }
}
