namespace W.Net
{
    /// <summary>
    /// Used by W.Net.Client.DataSent (might later be used for W.Net.Client.MessageSent)
    /// </summary>
    public struct SocketData
    {
        /// <summary>
        /// Used for tracking messages sent
        /// </summary>
        public ulong Id { get; set; }
        /// <summary>
        /// The data to send
        /// </summary>
        public byte[] Data { get; set; }
    }
}