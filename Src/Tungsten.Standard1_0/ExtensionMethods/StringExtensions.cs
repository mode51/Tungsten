using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Extensions for the string type
/// </summary>
namespace W.StringExtensions
{
    /// <summary>
    /// Extensions for the string type
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Validates the given string conforms to Base64 encoding.  It does not verify the value is a Base64 encoded value.
        /// </summary>
        /// <param name="value">The string to test</param>
        /// <returns>True if the value is valid Base64, otherwise false</returns>
        /// <remarks><para>This solution is based on a StackOverflow <see href="http://stackoverflow.com/questions/8571501/how-to-check-whether-the-string-is-base64-encoded-or-not">answer</see></para></remarks>
        public static bool IsValidBase64(string value)
        {
            var result = false;
            var regex = new System.Text.RegularExpressions.Regex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$");
            result = regex.IsMatch(value);
            return result;
        }
    }
}
