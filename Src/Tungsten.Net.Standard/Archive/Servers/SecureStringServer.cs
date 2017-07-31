using System;
using System.Collections.Generic;
using System.Text;

namespace W.Net
{
    /// <summary>
    /// A secure TCP server which hosts SecureStringClient connections.  Assymetric encryption is used to secure the transmitted data.
    /// </summary>
    public class SecureStringServer : SecureServer<SecureStringClient> { }
}
