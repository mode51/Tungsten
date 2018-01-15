using System;

namespace W.Net
{
    internal static class Debug
    {
        public static void i(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
        {
            message = string.Format("{0}({1}) - {2}", callerName, callerLineNumber, message);
            System.Diagnostics.Debug.WriteLine(message);
        }
        public static void e(Exception e, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
        {
            var message = string.Format("{0}({1}) - {2}", callerName, callerLineNumber, e.ToString());
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}