using System;

namespace W
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
        [System.Diagnostics.DebuggerStepThrough]
        public static string FromBase64(this string @this)
        {
            return Convert.FromBase64String(@this).AsString();
        }
        /// <summary>
        /// Converts a Base64 encoded byte array back to a normal byte array
        /// </summary>
        /// <param name="this">The Base64 encoded byte array to convert</param>
        /// <returns>A non-encoded string</returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string FromBase64(this byte[] @this)
        {
            return Convert.FromBase64String(@this.AsString()).AsString();
        }
        /// <summary>
        /// Converts a Base64 encoded byte array back to a normal byte array
        /// </summary>
        /// <param name="this">The Base64 encoded byte array to convert</param>
        /// <returns>A non-encoded string</returns>
        //[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[System.Diagnostics.DebuggerStepThrough]
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
    }
}