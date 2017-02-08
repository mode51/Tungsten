using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.RPC
{
    public class Constants
    {
        /// <summary>
        /// The default timeout for Connect
        /// </summary>
        public const int DefaultConnectTimeout = 10000;
        /// <summary>
        /// The default timeout for a call to MakeRPCCall
        /// </summary>
        public const int DefaultMakeRPCCallTimeout = 30000;
    }
}
