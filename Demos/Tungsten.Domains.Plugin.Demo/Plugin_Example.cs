using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.Domains.Plugin.Interface.Demo;

namespace W.Domains.Plugin.Demo
{
    public class DomainResult : MarshalByRefObject, IDomainResult
    {
        public bool Success { get; set; }
        public object Result { get; set; }
        public Exception Exception { get; set; }
    }
    public class Plugin_Example : MarshalByRefObject, IDomainPlugin
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
            return x*y;
        }

        public IDomainResult GetSomeValue(string input)
        {
            var result = new DomainResult();
            result.Success = true;
            result.Result = "The Result Value";
            result.Exception = new Exception("Sample Exception");
            return result;
        }
        public static IDomainResult MultiplyEx(int x, int y)
        {
            var result = new DomainResult();
            result.Success = true;
            result.Result = x * y;
            result.Exception = new Exception("Sample Exception");
            return result;
        }
    }
}
