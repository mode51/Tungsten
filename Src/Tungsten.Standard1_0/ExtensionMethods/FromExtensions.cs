using System;
using W.AsExtensions;

/// <summary>
/// Extensions which convert objects from one type to another
/// </summary>
namespace W.FromExtensions
{
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
        ///// <summary>
        ///// Converts a Base64 encoded byte array back to a normal byte array
        ///// </summary>
        ///// <param name="this">The Base64 encoded byte array to convert</param>
        ///// <returns>A non-encoded string</returns>
        //[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
        ////[System.Diagnostics.DebuggerStepThrough]
        //public static byte[] FromBase64(this byte[] @this)
        //{
        //    return Convert.FromBase64String(@this.AsString());
        //}
        /// <summary>
        /// Deserializes a Json string to an object
        /// </summary>
        /// <typeparam name="TType">The type of object to deserialize</typeparam>
        /// <param name="this">The Json formatted string</param>
        /// <returns>A new instance of TType deserialized from the specified Json string</returns>
        public static TType FromJson<TType>(this string @this)
        {
            return FromJson<TType>(@this.AsBytes());
        }
        /// <summary>
        /// Deserializes an encoded byte array of Json to an object
        /// </summary>
        /// <typeparam name="TType">The type of object to deserialize</typeparam>
        /// <param name="this">The byte array containing encoded Json</param>
        /// <returns>A new instance of TType deserialized from the specified Json encoded byte array</returns>
        public static TType FromJson<TType>(this byte[] @this)
        {
            var result = default(TType);
            using (var stream = new System.IO.MemoryStream())
            {
                stream.Write(@this, 0, @this.Length);
                var s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(TType));
                result = (TType)s.ReadObject(stream);
            }
            return result;
        }
        /// <summary>
        /// Deserializes an Xml string to an object
        /// </summary>
        /// <typeparam name="TType">The type of object to deserialize</typeparam>
        /// <param name="this">The Json formatted string</param>
        /// <returns>A new instance of TType deserialized from the specified Xml string</returns>
        public static TType FromXml<TType>(this string @this)
        {
            return FromXml<TType>(@this.AsBytes());
        }
        /// <summary>
        /// Deserializes an Xml string to an object
        /// </summary>
        /// <typeparam name="TType">The type of object to deserialize</typeparam>
        /// <param name="this">The Xml formatted string</param>
        /// <returns>A new instace of TType deserialized from the specified Xml string</returns>
        public static TType FromXml<TType>(this byte[] @this)
        {
            TType result;
            var s = new System.Runtime.Serialization.DataContractSerializer(typeof(TType));
            using (var stream = new System.IO.MemoryStream(@this, 0, @this.Length))
            {
                result = (TType)s.ReadObject(stream);
            }
            return result;
        }

#if NET45
        /// <summary>
        /// Uses binary serialization to deserialize an array of bytes into an object
        /// </summary>
        /// <typeparam name="T">The object Type</typeparam>
        /// <param name="bytes">The bytes containing a serialized object</param>
        /// <returns>The deserialized object</returns>
        public static T FromBytes<T>(this byte[] bytes)
        {
            var stream = new System.IO.MemoryStream(bytes);
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
            return (T)formatter.Deserialize(stream);
        }
#endif

#if NET45 || NETSTANDARD1_3 || WINDOWS_PORTABLE || NETCOREAPP1_0 || WINDOWS_UWP
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
#endif
    }
}