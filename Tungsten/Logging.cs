using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.Logging
{
    public static class Log
    {
        /// <summary>
        /// Configure this Action to log messages the way you like to
        /// </summary>
        public static Action<LogMessageCategory, string> LogTheMessage { get; set; } = (level, msg) =>
        {
#if !WINDOWS_PORTABLE && !WINDOWS_UWP
            var typeName = Enum.GetName(typeof(LogMessageCategory), level);
            System.Diagnostics.Trace.WriteLine($"{typeName}: {msg}");
#endif
        };

        /// <summary>
        /// A message type
        /// </summary>
        public enum LogMessageCategory
        {
            Verbose,
            Information,
            Warning,
            Error
        }
        public static void e(Exception e)
        {
            var msg = e.ToString();
            LogTheMessage?.Invoke(LogMessageCategory.Error, msg);
        }
        public static void e(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            LogTheMessage?.Invoke(LogMessageCategory.Error, msg);
        }
        public static void w(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            LogTheMessage?.Invoke(LogMessageCategory.Warning, msg);
        }
        public static void i(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            LogTheMessage?.Invoke(LogMessageCategory.Information, msg);
        }
        public static void v(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            LogTheMessage?.Invoke(LogMessageCategory.Verbose, msg);
        }
    }
}
