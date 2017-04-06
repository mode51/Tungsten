using System;
using System.Collections.Generic;

namespace W.RPC
{
    //this is the Message class used by the Client and Server
    public class Message : EncryptedMessageBase
    {
        public string Authority { get; set; } = "";
        public string Method { get; set; } = "";
        public List<object> Parameters { get; set; } = new List<object>();
        public object Response { get; set; } = null;
        public Exception Exception { get; set; } = null;
    }
}