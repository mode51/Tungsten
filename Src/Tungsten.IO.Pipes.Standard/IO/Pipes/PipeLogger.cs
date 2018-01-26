using System.Threading.Tasks;
using W.AsExtensions;
using W.Logging;
using W.IO.Pipes;

namespace W.IO.Pipes
{
    /// <summary>
    /// A named pipe logger for W.Logging
    /// </summary>
    public class PipeLogger : W.Logging.CustomLogger
    {
        /// <summary>
        /// The underlying PipeClient
        /// </summary>
        private W.Threading.Lockers.SpinLocker<W.IO.Pipes.PipeClient> _locker = new Threading.Lockers.SpinLocker<PipeClient>();

        /// <summary>
        /// Log a message to the custom logger
        /// </summary>
        /// <param name="category">The log message category</param>
        /// <param name="message">The log message</param>
        protected override async void LogMessage(Log.LogMessageCategory category, string message)
        {
            await Task.Run(() =>
            {
                _locker.InLock(async value =>
                {
                    if (value == null) return;
                    message = FormatLogMessage(category, message);
                    await value.PostAsync(message.AsBytes(), false);
                });
            }).ConfigureAwait(false);
        }
        /// <summary>
        /// Disposes the CustomLogger, releases resources and supresses the finalizer
        /// </summary>
        protected override void OnDispose()
        {
            _locker.InLock(value =>
            {
                value?.Dispose();
            });
            base.OnDispose();
        }
        /// <summary>
        /// Constructs a new PipeLogger
        /// </summary>
        /// <param name="pipeName">The name of the named pipe</param>
        /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
        public PipeLogger(string pipeName, bool addTimestamp = true) : this(".", pipeName, addTimestamp = true) { }
        /// <summary>
        /// Constructs a new PipeLogger
        /// </summary>
        /// <param name="serverName">The name of the server hosting the named pipe</param>
        /// <param name="pipeName">The name of the named pipe</param>
        /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
        public PipeLogger(string serverName, string pipeName, bool addTimestamp = true) : base(pipeName, addTimestamp)
        {
            Task.Run(() =>
            {
                _locker.InLock(value =>
                {
                    try
                    {
                        value = PipeClient.Create(serverName, pipeName, 5000)?.Result;
                    }
                    catch
                    {
                        value?.Dispose();
                        value = null;
                    }
                    return value;
                });
            }).ConfigureAwait(false);
        }
    }
}
