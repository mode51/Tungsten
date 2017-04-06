using System;

namespace W.RPC
{
    public abstract class EncryptedMessageBase
    {
        public Guid Id { get; set; }
        public DateTime ExpireDateTime { get; set; }
    }
}