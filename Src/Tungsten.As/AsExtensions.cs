using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#pragma warning disable CS1587
/// XML comment is not placed on a valid language element
/// <summary>
/// Extensions which convert objects to another
/// </summary>
namespace W
#pragma warning disable CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// Extensions which convert objects of one type to another
    /// </summary>
    public static class AsExtensions
    {
        //[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
        ////[System.Diagnostics.DebuggerStepThrough]

        /// <summary>
        /// Use Generic syntax for the <bold>as</bold> operator.
        /// </summary>
        /// <typeparam name="TType">The type to convert the item reference to.</typeparam>
        /// <param name="this">The item to convert to type TType</param>
        /// <returns>Null if @this cannot be referenced as TType.  Otherwise, the item as TType</returns>
        /// <example><code>expression as type</code> becomes <code>expression&lt;type&gt;()</code></example>
        ////[System.Diagnostics.DebuggerStepThrough]
        public static TType As<TType>(this object @this) where TType : class
        {
            return @this as TType;
        }
        /// <summary>
        /// Converts a string to Base64 encoding
        /// </summary>
        /// <param name="this">The string to convert to Base64 encoding</param>
        /// <returns>The Base64 encoded string</returns>
        ////[System.Diagnostics.DebuggerStepThrough]
        public static string AsBase64(this string @this)
        {
            return Convert.ToBase64String(@this.AsBytes());
        }
        /// <summary>
        /// Converts a byte array to a Base64 encoded string
        /// </summary>
        /// <param name="this">The string to convert to Base64 encoding</param>
        /// <returns>The Base64 encoded string</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public static string AsBase64(this byte[] @this)
        {
            return Convert.ToBase64String(@this);
        }
        /// <summary>
        /// Converts a string to an encoded byte array
        /// </summary>
        /// <param name="this">The string to convert to an encoded byte array</param>
        /// <returns>A byte array encoding of the specified string</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public static byte[] AsBytes(this string @this)
        {
            return System.Text.Encoding.UTF8.GetBytes(@this);
        }
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
        /// <summary>
        /// Converts an encoded byte array to a string
        /// </summary>
        /// <param name="this">The encoded byte array to conver to a string</param>
        /// <param name="index">The starting index</param>
        /// <param name="count">The number of bytes to convert</param>
        /// <returns>The string representation of the encoded byte array</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public static string AsString(this byte[] @this, int index, int count)
        {
            return System.Text.Encoding.UTF8.GetString(@this, index, count);
        }
        /// <summary>
        /// Creates a MemoryStream object and initializes it with the specified byte array
        /// </summary>
        /// <param name="this">The byte array used in creating the MemoryStream</param>
        /// <returns>A new MemoryStream initialized with the specified byte array</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public static MemoryStream AsStream(this byte[] @this)
        {
            return new MemoryStream(@this);
        }
        /// <summary>
        /// Creates a MemoryStream object and initializes it with the specified string
        /// </summary>
        /// <param name="this">The string used in creating the MemoryStream</param>
        /// <returns>A new MemoryStream initilized with the specified string</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public static MemoryStream AsStream(this string @this)
        {
            return new MemoryStream(@this.AsBytes());
        }
        /// <summary>
        /// Compresses the byte array using System.IO.Compression.DeflateStream
        /// </summary>
        /// <param name="bytes">The byte array to compress</param>
        /// <returns>A byte array of compressed data</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public static byte[] AsCompressed(this byte[] bytes)
        {
            //var output = new MemoryStream();
            using (var output = new MemoryStream())
            {
                using (var deflater = new System.IO.Compression.DeflateStream(output, System.IO.Compression.CompressionMode.Compress))
                {
                    deflater.Write(bytes, 0, bytes.Length);
                }
                return output.ToArray();
            }
        }
    }
}
