using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.RPC
{
    internal static class ExtensionMethods
    {
        internal static byte[] AsBytes(this string s)
        {
            return System.Text.Encoding.ASCII.GetBytes(s);
        }
        internal static string AsString(this byte[] bytes)
        {
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

    }
}
