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
        public static Action<LogMessageCategory, string> LogTheMessage { get; set; } = (level, msg) =>
        {
            var typeName = Enum.GetName(typeof(LogMessageCategory), level);
            try
            {
                //this really only helps for testing purposes.  It's compiled out in a Release build.
                System.Diagnostics.Debug.WriteLine($"{typeName}: {msg}");
                //Console.WriteLine($"{typeName}: {msg}");
            }
            catch
            {
                // ignored
            }
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
#pragma warning disable CS1573
        /// Parameter has no matching param tag in the XML comment (but other parameters do)
        /// <summary>
        /// Log an Exception
        /// </summary>
        /// <param name="e">The exception to log.  This will be boxed with ToString().</param>
        public static void e(Exception e)//, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            //var msg = string.Format("{0}(line {1}): {2}", callerName, callerLineNumber, e.ToString());
            try
            {
                LogTheMessage?.Invoke(LogMessageCategory.Error, e.ToString());// msg);
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
                LogTheMessage?.Invoke(LogMessageCategory.Error, msg);
            }
            catch { } //ignore any exceptions
        }
#pragma warning disable CS1573
        ///Parameter has no matching param tag in the XML comment (but other parameters do)
        /// <summary>
        /// Log a formatted warning message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="message">Format of the message</param>
        /// <param name="callerName">The name of the caller</param>
        /// <param name="callerLineNumber">The line number of the caller</param>
        public static void w(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var msg = string.Format("{0}(line {1}): {2}", callerName, callerLineNumber, message);
            try
            {
                LogTheMessage?.Invoke(LogMessageCategory.Warning, msg);
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
                LogTheMessage?.Invoke(LogMessageCategory.Warning, msg);
            }
            catch { } //ignore any exceptions
        }
#pragma warning disable CS1573
        /// Parameter has no matching param tag in the XML comment (but other parameters do)
        /// <summary>
        /// Log a formatted informational message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="message">Format of the message</param>
        /// <param name="callerName">The name of the caller</param>
        /// <param name="callerLineNumber">The line number of the caller</param>
        public static void i(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var msg = string.Format("{0}(line {1}): {2}", callerName, callerLineNumber, message);
            try
            {
                LogTheMessage?.Invoke(LogMessageCategory.Information, msg);
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
                LogTheMessage?.Invoke(LogMessageCategory.Information, msg);
            }
            catch { } //ignore any exceptions
        }
#pragma warning disable CS1573
        /// Parameter has no matching param tag in the XML comment (but other parameters do)
        /// <summary>
        /// Log a formatted verbose message. This method uses string.Format to format the message.
        /// </summary>
        /// <param name="message">Format of the message</param>
        /// <param name="callerName">The name of the caller</param>
        /// <param name="callerLineNumber">The line number of the caller</param>
        public static void v(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var msg = string.Format("{0}(line {1}): {2}", callerName, callerLineNumber, message);
            try
            {
                LogTheMessage?.Invoke(LogMessageCategory.Verbose, msg);
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
                LogTheMessage?.Invoke(LogMessageCategory.Verbose, msg);
            }
            catch { } //ignore any exceptions
        }

        /// <summary>
        /// A maintained history of log messages
        /// </summary>
        /// <remarks>The default log message limit is 10,000 messages</remarks>
        public static LogMessageHistory MessageHistory { get; } = new LogMessageHistory();
    }
}
