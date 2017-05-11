using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using W;
using W.Logging;

namespace W.IO.Pipes
{
    /// <summary>
    /// A named pipe logger for W.Logging
    /// </summary>
    public class PipeClientLogger : W.Logging.CustomLogger
    {
        private PipeClient _client;

        /// <summary>
        /// Log a message to the custom logger
        /// </summary>
        /// <param name="category">The log message category</param>
        /// <param name="message">The log message</param>
        protected override void LogMessage(Log.LogMessageCategory category, string message)
        {
            message = FormatLogMessage(category, message);
            _client.Write(message.AsBytes());
        }
        /// <summary>
        /// Disposes the CustomLogger, releases resources and supresses the finalizer
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
        /// <summary>
        /// Constructs a new PipeLogger
        /// </summary>
        /// <param name="pipeName">The name of the named pipe</param>
        /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
        public PipeClientLogger(string pipeName, bool addTimestamp = true) : this(".", pipeName, addTimestamp = true)
        {
        }
        /// <summary>
        /// Constructs a new PipeLogger
        /// </summary>
        /// <param name="serverName">The name of the server hosting the named pipe</param>
        /// <param name="pipeName">The name of the named pipe</param>
        /// <param name="addTimestamp">If true, the message will be prefixed with a timestamp</param>
        public PipeClientLogger(string serverName, string pipeName, bool addTimestamp = true) : base(pipeName, addTimestamp)
        {
            _client = new PipeClient();
            _client.Connect(serverName, pipeName, System.IO.Pipes.PipeDirection.InOut);
        }
    }
    //internal class PipeLogger : IDisposable
    //{
    //    private PipeClient _client;

    //    public bool AddTimestamp;
    //    public string PipeName { get; private set; }

    //    private void LogMessage(W.Logging.Log.LogMessageCategory category, string message)
    //    {
    //        if (AddTimestamp)
    //            message = string.Format("{0}: {1} - {2}", DateTime.Now.TimeOfDay.ToString(), category.ToString(), message);
    //        else
    //            message = string.Format("{0} - {1}", category.ToString(), message);

    //        _client.Write(message.AsBytes());
    //    }

    //    public void Dispose()
    //    {
    //        if (_client != null)
    //        {
    //            W.Logging.Log.LogTheMessage -= LogMessage;
    //            _client.Dispose();
    //            _client = null;
    //            GC.SuppressFinalize(this);
    //        }
    //    }

    //    public PipeLogger(string pipeName, bool autoAddTimestamp)
    //    {
    //        PipeName = pipeName;
    //        AddTimestamp = autoAddTimestamp;
    //        _client = new PipeClient();
    //        _client.Connect(pipeName, System.IO.Pipes.PipeDirection.InOut);
    //        W.Logging.Log.LogTheMessage += LogMessage;
    //    }
    //    ~PipeLogger()
    //    {
    //        Dispose();
    //    }
    //}
    ///// <summary>
    ///// Adds support for sending Log messages over a named pipe
    ///// </summary>
    //public static class Logging
    //{
    //    private static System.Collections.Generic.List<PipeLogger> _clients = new List<PipeLogger>();
    //    private static object _lockObj = new object();

    //    /// <summary>
    //    /// Configures W.Logging.Log to send information over a named pipe
    //    /// </summary>
    //    /// <param name="pipeName">The name of the named pipe</param>
    //    /// <param name="autoAddTimestamp">If true, the message will be prefixed with a timestamp</param>
    //    public static void Add(string pipeName, bool autoAddTimestamp = true)
    //    {
    //        var existing = _clients.FirstOrDefault(p => p.PipeName == pipeName);
    //        if (existing == null)
    //        {
    //            var newPipeLogger = new PipeLogger(pipeName, autoAddTimestamp);
    //            _clients.Add(newPipeLogger);
    //        }
    //    }

    //    /// <summary>
    //    /// Disconnects and disposes a specific named pipe logger
    //    /// </summary>
    //    /// <param name="pipeName">The name of the named pipe to remove</param>
    //    public static void Remove(string pipeName)
    //    {
    //        lock (_lockObj)
    //        {
    //            var client = _clients.FirstOrDefault(p => p.PipeName == pipeName);
    //            if (client != null)
    //            {
    //                client.Dispose();
    //                _clients.Remove(client);
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// Disconnects and disposes all named pipe loggers
    //    /// </summary>
    //    public static void RemoveAll()
    //    {
    //        lock (_lockObj)
    //        {
    //            while (_clients.Count > 0)
    //            {
    //                Remove(_clients[0].PipeName);
    //            }
    //        }
    //    }
    //}
}
