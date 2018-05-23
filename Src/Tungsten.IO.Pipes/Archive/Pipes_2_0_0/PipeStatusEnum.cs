namespace W.IO.Pipes
{
    /// <summary>
    /// The current status of the named pipe
    /// </summary>
    public enum PipeStatusEnum
    {
        /// <summary>
        /// The pipe has been disconnected
        /// </summary>
        Disconnected = 0,
        /// <summary>
        /// The pipe is currently connected
        /// </summary>
        Connected
    }
}
