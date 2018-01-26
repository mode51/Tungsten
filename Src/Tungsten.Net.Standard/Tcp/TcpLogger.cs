using System.Threading.Tasks;
using System.Net;
using W.AsExtensions;
using W.Logging;

namespace W.Net
{
    public static partial class Tcp
    {
        public class TcpLogger : W.Logging.CustomLogger
        {
            private W.Threading.Lockers.MonitorLocker<TcpClient> _locked = new Threading.Lockers.MonitorLocker<TcpClient>();
            protected override void LogMessage(Log.LogMessageCategory category, string message)
            {
                _locked.InLock(value =>
                {
                    if (value == null) return;
                    message = FormatLogMessage(category, message);
                    value.Write(message.AsBytes());
                });
            }
            protected override void OnDispose()
            {
                _locked.InLock(value =>
                {
                    value?.Dispose();
                });
                base.OnDispose();
            }
            public TcpLogger(IPEndPoint ep, bool addTimestamp) : base("TcpLogger", addTimestamp)
            {
                Task.Run(() =>
                {
                    _locked.InLock(value =>
                    {
                        try
                        {
                            value = new TcpClient();
                            value.Connect(ep);
                        }
                        catch
                        {
                            value?.Dispose();
                            value = null;
                        }
                        return value;
                    });
                });
            }
        }
    }
}