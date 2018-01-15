using System.IO.Pipes;

namespace W.IO.Pipes
{
    /// <summary>
    /// Specified the direction of data for the Pipe
    /// </summary>
    public enum EPipeDirection
    {
        /// <summary>
        /// Receive data only
        /// </summary>
        In = PipeDirection.In,
        /// <summary>
        /// Send data only
        /// </summary>
        Out = PipeDirection.Out,
        /// <summary>
        /// Send and Receive data
        /// </summary>
        InOut = PipeDirection.InOut
    }
}