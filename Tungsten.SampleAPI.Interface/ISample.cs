using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.SampleAPI.Interface
{
    public interface ISample
    {
        void LogInformation(string format, params object[] args);
        string GetMessage();
        ISampleResult GetSomeValue(string input);
    }
}
