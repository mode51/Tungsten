namespace W.Net
{
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