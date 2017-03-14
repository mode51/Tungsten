using System;

namespace W.RPC
{
    internal abstract class EncryptedMessageBase
    {
        public Guid Id { get; set; }
        public DateTime ExpireDateTime { get; set; }
    }
}