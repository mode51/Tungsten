using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.SampleAPI.Interface;
using W.RPC;

namespace W.SampleAPI
{
    [RPCClass]
    public class Sample : ISample
    {
        public void LogInformation(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public string GetMessage()
        {
            return "This is a string return value from GetMessage()";
        }

        public static int Multiply(int x, int y)
        {
            return x * y;
        }

        public ISampleResult GetSomeValue(string input)
        {
            var result = new SampleResult
            {
                Success = true,
                Result = "The Result Value",
                Exception = new Exception("Sample Exception")
            };
            return result;
        }

        //static methods

        public static void Initialize()
        {
            //call this method to ensure the dll is loaded into the current AppDomain
        }
        [RPCMethod]
        public static void LogInformationEx(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        [RPCMethod]
        public static string GetMessageEx()
        {
            return "GetMessage Result";
        }

        [RPCMethod]
        public static ISampleResult Echo(string input)
        {
            var result = new SampleResult
            {
                Success = true,
                Result = input,
                Exception = new Exception("Sample Exception")
            };
            return result;
        }
    }
}
