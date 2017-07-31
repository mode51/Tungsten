using System;
using System.IO.Pipes;

namespace W.IO.Pipes
{
    /// <summary>
    /// Defines the required methods for a PipeClient to be used by PipeServer
    /// </summary>
    public interface IPipeClient
    {
        /// <summary>
        /// Can be useful for large data sets.  Set to True to use compression, otherwise False
        /// </summary>
        /// <remarks>Make sure both server and client have the same value</remarks>
        bool UseCompression { get; set; }

        /// <summary>
        /// Initializes the instance with a pre-existing, connected, PipeStream
        /// </summary>
        /// <param name="stream">The previously connected pipe client</param>
        /// <param name="isServerSide">Set to True if your customized server code needs to know the client exists server-side</param>
        /// <remarks>Called by PipeServer when handling a new connection</remarks>
        void Initialize(PipeStream stream, bool isServerSide);
        /// <summary>
        /// Called when the client connects to the server
        /// </summary>
        Action<object> Connected { get; set; }
        /// <summary>
        /// Called when the client disconnects from the server
        /// </summary>
        /// <remarks>Handled by PipeServer to know when to dispose the server-side client</remarks>
        Action<object, Exception> Disconnected { get; set; }

        /// <summary>
        /// Disposes the object and releases resources
        /// </summary>
        void Dispose();
    }
}