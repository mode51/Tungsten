using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.Domains.Plugin.Interface.Demo
{
    public interface IDomainResult
    {
        bool Success { get; set; }
        object Result { get; set; }
        Exception Exception { get; set; }
    }
    public interface IDomainPlugin
    {
        void LogInformation(string format, params object[] args);
        string GetMessage();
        IDomainResult GetSomeValue(string input);
    }
}
