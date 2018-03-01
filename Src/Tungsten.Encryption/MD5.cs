using System;
using System.Text;

namespace W.Encryption
{
    /// <summary>
    /// Used to generate MD5 hashes and verify input strings against them
    /// </summary>
    public class MD5
    {
        /// <summary>
        /// Generates an MD5 hash of the input string
        /// </summary>
        /// <param name="input">An MD5 hash of this input will be created</param>
        /// <returns>An MD5 hash of the inputted value</returns>
        public static string GetMd5Hash(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return MD5.GetMd5Hash(input, md5);
            }
        }
        /// <summary>
        /// Generates an MD5 hash of the input string
        /// </summary>
        /// <param name="input">An MD5 hash of this input will be created</param>
        /// <param name="md5">The previously allocated MD5 object to use</param>
        /// <returns>An MD5 hash of the inputted value</returns>
        public static string GetMd5Hash(string input, System.Security.Cryptography.MD5 md5)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Verifies a hash against a string
        /// </summary>
        /// <param name="input">The string to verify</param>
        /// <param name="hash">The MD5 hash used in the verification</param>
        /// <returns>True if the input string is verified, otherwise False</returns>
        public static bool VerifyMd5Hash(string input, string hash)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                // Hash the input.
                string hashOfInput = GetMd5Hash(input, md5);

                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, hash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
