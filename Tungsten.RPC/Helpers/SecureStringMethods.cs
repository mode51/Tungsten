using System;
using System.Runtime.InteropServices;
using System.Security;

namespace W.RPC
{
    internal class SecureStringMethods
    {
        public static string ConvertToUnsecureString(SecureString secureValue)
        {
            if (secureValue == null)
                throw new ArgumentNullException("secureValue");

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureValue);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        public static SecureString ConvertToSecureString(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            //using unsafe is 10x faster, if you can compile with "Allow unsafe code" checked
            //unsafe
            //{
            //    fixed (char* valueChars = value)
            //    {
            //        var secureValue = new SecureString(valueChars, value.Length);
            //        secureValue.MakeReadOnly();
            //    }
            //}
            var secureValue = new SecureString();
            foreach (var c in value)
                secureValue.AppendChar(c);

            secureValue.MakeReadOnly();
            return secureValue;
        }
    }
}