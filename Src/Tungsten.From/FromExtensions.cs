using System;

/// <summary>
/// Extensions which convert objects from one type to another
/// </summary>
namespace W
{
    //just using the one function, so we'll internalize it to avoid a reference
    internal static class AsExtensions
    {
        /// <summary>
        /// Converts an encoded byte array to a string
        /// </summary>
        /// <param name="this">The encoded byte array to conver to a string</param>
        /// <returns>The string representation of the encoded byte array</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public static string AsString(this byte[] @this)
        {
            return System.Text.Encoding.UTF8.GetString(@this, 0, @this.Length);
        }
    }
    /// <summary>
    /// Extensions which convert objects of one type to another
    /// </summary>
    public static class FromExtensions
    {
        /// <summary>
        /// Converts a Base64 encoded string back to a normal string
        /// </summary>
        /// <param name="this">The Base64 encoded string to convert</param>
        /// <returns>A non-encoded string</returns>
        //[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[System.Diagnostics.DebuggerStepThrough]
        public static string FromBase64(this string @this)
        {
            return Convert.FromBase64String(@this).AsString();
        }
        /// <summary>
        /// Converts a Base64 encoded byte array back to a normal byte array
        /// </summary>
        /// <param name="this">The Base64 encoded byte array to convert</param>
        /// <returns>A non-encoded string</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public static string FromBase64(this byte[] @this)
        {
            return Convert.FromBase64String(@this.AsString()).AsString();
        }

        /// <summary>
        /// Decompresses the byte array using System.IO.Compression.DeflateStream
        /// </summary>
        /// <param name="bytes">The byte array containing compressed data</param>
        /// <returns>A byte array of the decompressed data</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public static byte[] FromCompressed(this byte[] bytes)
        {
            var input = new System.IO.MemoryStream(bytes);
            //var output = new MemoryStream();
            using (var output = new System.IO.MemoryStream())
            {
                using (var deflater = new System.IO.Compression.DeflateStream(input, System.IO.Compression.CompressionMode.Decompress))
                {
                    deflater.CopyTo(output);
                }
                return output.ToArray();
            }
        }
    }
}