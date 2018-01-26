using System.Threading.Tasks;
using System.Net;
using W.AsExtensions;
using W.Logging;

namespace W.Net
{
    public static partial class Tcp
    {
        public class SecureTcpLogger : W.Logging.CustomLogger
        {
            private W.Threading.Lockers.MonitorLocker<SecureTcpClient> _locker = new Threading.Lockers.MonitorLocker<SecureTcpClient>();
            protected override void LogMessage(Log.LogMessageCategory category, string message)
            {
                _locker.InLock(value =>
                {
                    if (value == null) return;
                    message = FormatLogMessage(category, message);
                    value.Write(message.AsBytes());
                });
            }
            protected override void OnDispose()
            {
                _locker.InLock(value =>
                {
                    if (value == null) return;
                    value.Dispose();
                });
                base.OnDispose();
            }
            public SecureTcpLogger(IPEndPoint ep, int keySize, bool addTimestamp) : base("SecureTcpLogger", addTimestamp)
            {
                Task.Run(() =>
                {
                    _locker.InLock(value =>
                    {
                        try
                        {
                            value = new SecureTcpClient(keySize);
                            value.Connect(ep);
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
}