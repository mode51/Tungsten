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
        private static W.Lockable<PipeClient> _pipeClient = new Lockable<PipeClient>();

        /// <summary>
        /// The named pipe client used to send log messages
        /// </summary>
        public static PipeClient PipeClient => _pipeClient.Value;
        private static PipeClient GetInstance(string serverName, string pipeName, bool addTimestamp = true)
        {
            try
            {
                _pipeClient.InLock(Threading.Lockers.LockTypeEnum.Write, client => 
                {
                    if (client != null && (serverName != _serverName || _pipeName != pipeName))
                    {
                        client.Dispose();
                        client = null;
                    }
                    if (client != null && (client.Stream?.IsConnected == false))
                    {
                        client?.Dispose();
                        client = null;
                    }
                    if (client == null)
                    {
                        _serverName = serverName;
                        _pipeName = pipeName;
                        client = new PipeClient();
                        client.Connect(serverName, pipeName, System.Security.Principal.TokenImpersonationLevel.Impersonation, 1000);
                        _pipeClient.SetState(client);
                    }
                });
            }
            catch (Exception e)
            {
                Log.e(e);
                _pipeClient.Value?.Dispose();
                _pipeClient.Value = null;
            }
#if NET45
            AppDomain.CurrentDomain.DomainUnload += (o, e) => { _pipeClient?.Value?.Dispose(); _pipeClient?.Dispose(); _pipeClient = null; };
#elif NETSTANDARD1_4
            //AppDomain.CurrentDomain.DomainUnload += (o, e) => { _pipeClient?.Value?.Dispose(); _pipeClient?.Dispose(); _pipeClient = null; };
#endif
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
                    //instance?.WriteAsync(message.AsBytes()).Wait();
                    instance?.Write(message.AsBytes());
                }
                catch { }
            }
        }
    }
}
