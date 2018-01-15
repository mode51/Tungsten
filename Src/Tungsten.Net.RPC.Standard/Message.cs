using System;
using System.Collections.Generic;

namespace W.Net.RPC
{
    //internal abstract class EncryptedMessageBase
    //{
    //    public Guid Id { get; set; }
    //    public DateTime ExpireDateTime { get; set; }
    //}

    //this is the Message class used by the Client and Server
    internal class Message //: EncryptedMessageBase
    {
        public Guid Id { get; set; }
        public DateTime ExpireDateTime { get; set; }
        public string Authority { get; set; } = "";
        public string Method { get; set; } = "";
        public List<object> Parameters { get; set; } = new List<object>();
        public object Response { get; set; } = null;
        //public ExceptionInformation Exception { get; set; } = null;
        public string Exception { get; set; } = "";
    }
}