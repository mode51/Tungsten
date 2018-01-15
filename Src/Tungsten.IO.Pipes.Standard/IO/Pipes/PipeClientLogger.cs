using W.AsExtensions;
using W.Logging;
using W.IO.Pipes;

namespace W.IO.Pipes
{
    /// <summary>
    /// A named pipe logger for W.Logging
    /// </summary>
    public class PipeClientLogger : W.Logging.CustomLogger
    {
        /// <summary>
        /// The underlying PipeClient
        /// </summary>
        public W.IO.Pipes.PipeClient Pipe { get; private set; }

        /// <summary>
        /// Log a message to the custom logger
        /// </summary>
        /// <param name="category">The log message category</param>
        /// <param name="message">The log message</param>
        protected override void LogMessage(Log.LogMessageCategory category, string message)
        {
            message = FormatLogMessage(category, message);
            Pipe?.PostAsync(message.AsBytes(), false).Wait();
        }
        /// <summary>
        /// Disposes the CustomLogger, releases resources and supresses the finalizer
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
        }
        /// <summary>
        /// Constructs a new PipeLogger
        /// </summary>
        /// <param name="pipeName">The name of the named pipe</param>
        /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
        public PipeClientLogger(string pipeName, bool addTimestamp = true) : this(".", pipeName, addTimestamp = true) { }
        /// <summary>
        /// Constructs a new PipeLogger
        /// </summary>
        /// <param name="serverName">The name of the server hosting the named pipe</param>
        /// <param name="pipeName">The name of the named pipe</param>
        /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
        public PipeClientLogger(string serverName, string pipeName, bool addTimestamp = true) : base(pipeName, addTimestamp)
        {
            Pipe = PipeClient.Create(serverName, pipeName, 5000)?.Result;
        }
    }
}
