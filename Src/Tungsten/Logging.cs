using System;

namespace W.Logging
{
    /// <summary>
    /// Exposes static methods for logging.  LogTheMessage can be assigned a new value for customized logging.
    /// </summary>
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
        /// The log message type
        /// </summary>
        public enum LogMessageCategory
        {
            /// <summary>
            /// Denotes verbose message
            /// </summary>
            Verbose,
            /// <summary>
            /// Denotes a informational message
            /// </summary>
            Information,
            /// <summary>
            /// Denotes a warning message
            /// </summary>
            Warning,
            /// <summary>
            /// Denotes an error message
            /// </summary>
            Error
        }
        /// <summary>
        /// Log an Exception
        /// </summary>
        /// <param name="e">The exception to log.  This will be boxed with ToString().</param>
        public static void e(Exception e)
        {
            var msg = e.ToString();
            LogTheMessage?.Invoke(LogMessageCategory.Error, msg);
        }
        /// <summary>
        /// Log a formatted exception message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="format">Format of the message</param>
        /// <param name="args">Parameters to be passed during message formatting</param>
        public static void e(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            LogTheMessage?.Invoke(LogMessageCategory.Error, msg);
        }
        /// <summary>
        /// Log a formatted warning message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="format">Format of the message</param>
        /// <param name="args">Parameters to be passed during message formatting</param>
        public static void w(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            LogTheMessage?.Invoke(LogMessageCategory.Warning, msg);
        }
        /// <summary>
        /// Log a formatted informational message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="format">Format of the message</param>
        /// <param name="args">Parameters to be passed during message formatting</param>
        public static void i(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            LogTheMessage?.Invoke(LogMessageCategory.Information, msg);
        }
        /// <summary>
        /// Log a formatted verbose message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="format">Format of the message</param>
        /// <param name="args">Parameters to be passed during message formatting</param>
        public static void v(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            LogTheMessage?.Invoke(LogMessageCategory.Verbose, msg);
        }
    }
}
