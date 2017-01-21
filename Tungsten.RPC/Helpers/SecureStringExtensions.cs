using System.Security;

namespace W.RPC
{
    internal static class SecureStringExtensions
    {
        public static SecureString Secure(this string @this)
        {
            return SecureStringMethods.ConvertToSecureString(@this);
        }
        public static string Unsecure(this SecureString @this)
        {
            return SecureStringMethods.ConvertToUnsecureString(@this);
        }
    }
}