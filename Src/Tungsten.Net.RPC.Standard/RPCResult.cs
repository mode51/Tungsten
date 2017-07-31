namespace W.Net.RPC
{
    /// <summary>
    /// Encapsulates the return value from an RPC call and an exception if one occurred.
    /// </summary>
    /// <typeparam name="TResultType"></typeparam>
    public class RPCResult<TResultType>
    {
        /// <summary>
        /// The return value from the RPC call (will be null if the return type is void)
        /// </summary>
        public TResultType Response { get; internal set; }
        /// <summary>
        /// Exception information if an exception occurred
        /// </summary>
        public ExceptionInformation Exception { get; internal set; }
    }
}
