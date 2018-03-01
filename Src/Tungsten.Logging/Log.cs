using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// Exposes static methods for logging
/// </summary>
namespace W.Logging
{
    /// <summary>
    /// Exposes static methods for logging.  LogTheMessage can be assigned a new value for customized logging.
    /// </summary>
    public static partial class Log
    {
        /// <summary>
        /// Configure this Action to log messages the way you like to
        /// </summary>
        /// <remarks>This method needs to be replaced with something useful to you.</remarks>
        public static event Action<string> LogTheMessage;

        /// <summary>
        /// If True, log messages will be prefixed with a timestamp
        /// </summary>
        public static bool AddTimestamp { get; set; } = true;
        /// <summary>
        /// Provides simple formatting (formats the log message as a string)
        /// </summary>
        /// <param name="category">The log category</param>
        /// <param name="message">The log message</param>
        /// <returns></returns>
        private static string FormatLogMessage(LogMessageCategory category, string message)
        {
            var logMessage = string.Empty;
            if (AddTimestamp)
                logMessage = $"{DateTime.Now.TimeOfDay.ToString()}: {category} - {message}";
            else
                logMessage = $"{category} - {message}";
            return logMessage;
        }

        /// <summary>
        /// The log message type
        /// </summary>
        public enum LogMessageCategory : int
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
        /// Parameter has no matching param tag in the XML comment (but other parameters do)
        /// <summary>
        /// Log an Exception
        /// </summary>
        /// <param name="e">The exception to log.  This will be boxed with ToString().</param>
        public static void e(Exception e)//, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
        {
            //var msg = string.Format("{0}(line {1}): {2}", callerName, callerLineNumber, e.ToString());
            try
            {
                var msg = FormatLogMessage(LogMessageCategory.Error, e.ToString());
                LogTheMessage?.Invoke(msg);// msg);
            }
            catch { } //ignore any exceptions
        }
        /// <summary>
        /// Log a formatted exception message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="format">Format of the message</param>
        /// <param name="args">Parameters to be passed during message formatting</param>
        public static void e(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            try
            {
                msg = FormatLogMessage(LogMessageCategory.Error, msg);
                LogTheMessage?.Invoke(msg);
            }
            catch { } //ignore any exceptions
        }
        ///Parameter has no matching param tag in the XML comment (but other parameters do)
        /// <summary>
        /// Log a formatted warning message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="message">Format of the message</param>
        /// <param name="callerName">The name of the caller</param>
        /// <param name="callerLineNumber">The line number of the caller</param>
        public static void w(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
        {
            var msg = string.Format("{0}(line {1}): {2}", callerName, callerLineNumber, message);
            try
            {
                msg = FormatLogMessage(LogMessageCategory.Warning, msg);
                LogTheMessage?.Invoke(msg);
            }
            catch { } //ignore any exceptions
        }
        /// <summary>
        /// Log a formatted warning message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="format">Format of the message</param>
        /// <param name="args">Parameters to be passed during message formatting</param>
        public static void w(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            try
            {
                msg = FormatLogMessage(LogMessageCategory.Warning, msg);
                LogTheMessage?.Invoke(msg);
            }
            catch { } //ignore any exceptions
        }
        /// Parameter has no matching param tag in the XML comment (but other parameters do)
        /// <summary>
        /// Log a formatted informational message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="message">Format of the message</param>
        /// <param name="callerName">The name of the caller</param>
        /// <param name="callerLineNumber">The line number of the caller</param>
        public static void i(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
        {
            var msg = string.Format("{0}(line {1}): {2}", callerName, callerLineNumber, message);
            try
            {
                msg = FormatLogMessage(LogMessageCategory.Information, msg);
                LogTheMessage?.Invoke(msg);
            }
            catch { } //ignore any exceptions
        }
        /// <summary>
        /// Log a formatted informational message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="format">Format of the message</param>
        /// <param name="args">Parameters to be passed during message formatting</param>
        public static void i(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            try
            {
                msg = FormatLogMessage(LogMessageCategory.Information, msg);
                LogTheMessage?.Invoke(msg);
            }
            catch { } //ignore any exceptions
        }
        /// Parameter has no matching param tag in the XML comment (but other parameters do)
        /// <summary>
        /// Log a formatted verbose message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="message">Format of the message</param>
        /// <param name="callerName">The name of the caller</param>
        /// <param name="callerLineNumber">The line number of the caller</param>
        public static void v(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
        {
            var msg = string.Format("{0}(line {1}): {2}", callerName, callerLineNumber, message);
            try
            {
                msg = FormatLogMessage(LogMessageCategory.Verbose, msg);
                LogTheMessage?.Invoke(msg);
            }
            catch { } //ignore any exceptions
        }
        /// <summary>
        /// Log a formatted verbose message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="format">Format of the message</param>
        /// <param name="args">Parameters to be passed during message formatting</param>
        public static void v(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            try
            {
                msg = FormatLogMessage(LogMessageCategory.Verbose, msg);
                LogTheMessage?.Invoke(msg);
            }
            catch { } //ignore any exceptions
        }

        /// <summary>
        /// A maintained history of log messages
        /// </summary>
        /// <remarks>The default log message limit is 10,000 messages</remarks>
        //public static LogMessageHistory MessageHistory { get; } = new LogMessageHistory();

        static Log()
        {
#if DEBUG
            LogTheMessage += (msg) =>
            {
                try
                {
                    //this really only helps for testing purposes.  It's compiled out in a Release build.
                    System.Diagnostics.Debug.WriteLine(msg);
                }
                catch
                {
                    // ignored
                }
            };
#endif
        }
    }
}
