using System;
using System.Threading.Tasks;
using W.Logging;

namespace W.IO.Pipes
{
    /// <summary>
    /// Sends log messages to a remote server via a named pipe
    /// </summary>
    /// <example>
    ///     Log.LogTheMessage += (category, message) => W.IO.Pipes.PipeLogger.LogTheMessage(".", "PipeLogger", true, category, message);
    /// </example>
    public static class PipeLogger
    {
        private static string _serverName = string.Empty;
        private static string _pipeName = string.Empty;
        /// <summary>
        /// The named pipe client used to send log messages
        /// </summary>
        public static PipeClient PipeClient { get; set; }
        private static PipeClient GetInstance(string serverName, string pipeName, bool addTimestamp = true)
        {
            try
            {
                if (PipeClient != null && (serverName != _serverName || _pipeName != pipeName))
                {
                    PipeClient?.Dispose();
                    PipeClient = null;
                }
                if (PipeClient != null && !PipeClient.Stream.IsConnected)
                {
                    PipeClient.Dispose();
                    PipeClient = null;
                }
                if (PipeClient == null)
                {
                    _serverName = serverName;
                    _pipeName = pipeName;
                    PipeClient = PipeClient.CreateClient(serverName, pipeName, 5000).Result ?? null;
                }
            }
            catch (Exception e)
            {
                Log.e(e);
                PipeClient = null;
            }
            return PipeClient;
        }

        /// <summary>
        /// Log a message to the remote machine.  Note that this message is slower due to parsing the remoteIP with each call
        /// </summary>
        /// <param name="server">The name of the server hosting the named pipe</param>
        /// <param name="pipeName">The name of the named pipe</param>
        /// <param name="message">The log message</param>
        public static void LogTheMessage(string server, string pipeName, string message)
        {
            var instance = GetInstance(server, pipeName);
            if (instance != null)
            {
                try
                {
                    instance?.PostAsync(message.AsBytes(), false).Wait();
                    //PipeClient.PostAsync(server, pipeName, message.AsBytes(), false, 100).Wait();
                }
                catch { }
            }
        }
    }
}
