using System;
using System.Collections.Generic;
using System.Text;

namespace W.Net
{
    /// <summary>
    /// A TCP server which hosts StringClient connections
    /// </summary>
    public class StringServer : Server<StringClient> { }
}
