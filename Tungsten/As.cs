using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace W
{
    /// <summary>
    /// Extensions which convert objects of one type to another
    /// </summary>
    public static class AsExtensions
    {
        /// <summary>
        /// Converts a Base64 encoded string back to a normal string
        /// </summary>
        /// <param name="this">The Base64 encoded string to convert</param>
        /// <returns>A non-encoded string</returns>
        public static string FromBase64(this string @this)
        {
            var bytes = Convert.FromBase64String(@this);
            return System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// Converts a Base64 encoded byte array back to a normal byte array
        /// </summary>
        /// <param name="this">The Base64 encoded byte array to convert</param>
        /// <returns>A non-encoded string</returns>
        //[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.DebuggerStepThrough]
        public static byte[] FromBase64(this byte[] @this)
        {
            return Convert.FromBase64String(@this.AsString());
        }
        /// <summary>
        /// Use Generic syntax for the <bold>as</bold> operator.
        /// </summary>
        /// <typeparam name="TType">The type to convert the item reference to.</typeparam>
        /// <param name="this">The item to convert to type TType</param>
        /// <returns>Null if @this cannot be referenced as TType.  Otherwise, the item as TType</returns>
        /// <example><code>expression as type</code> becomes <code>expression&lt;type&gt;()</code></example>
        public static TType As<TType>(this object @this) where TType : class
        {
            return @this as TType;
        }
        /// <summary>
        /// Converts a string to Base64 encoding
        /// </summary>
        /// <param name="this">The string to convert to Base64 encoding</param>
        /// <returns>The Base64 encoded string</returns>
        public static string AsBase64(this string @this)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(@this));
        }
        /// <summary>
        /// Converts a string to an encoded byte array
        /// </summary>
        /// <param name="this">The string to convert to an encoded byte array</param>
        /// <returns>A byte array encoding of the specified string</returns>
        public static byte[] AsBytes(this string @this)
        {
            return System.Text.Encoding.UTF8.GetBytes(@this);
        }
        /// <summary>
        /// Converts an encoded byte array to a string
        /// </summary>
        /// <param name="this">The encoded byte array to conver to a string</param>
        /// <returns>The string representation of the encoded byte array</returns>
        public static string AsString(this byte[] @this)
        {
            return System.Text.Encoding.UTF8.GetString(@this, 0, @this.Length);
        }
        /// <summary>
        /// Creates a MemoryStream object and initializes it with the specified byte array
        /// </summary>
        /// <param name="this">The byte array used in creating the MemoryStream</param>
        /// <returns>A new MemoryStream initialized with the specified byte array</returns>
        public static MemoryStream AsStream(this byte[] @this)
        {
            return new MemoryStream(@this);
        }
        /// <summary>
        /// Creates a MemoryStream object and initializes it with the specified string
        /// </summary>
        /// <param name="this">The string used in creating the MemoryStream</param>
        /// <returns>A new MemoryStream initilized with the specified string</returns>
        public static MemoryStream AsStream(this string @this)
        {
            return new MemoryStream(@this.AsBytes());
        }
        /// <summary>
        /// Serializes an object to a Json string
        /// </summary>
        /// <typeparam name="TType">The type of object to serialize</typeparam>
        /// <param name="this">The object to serialize to Json</param>
        /// <returns>A Json formatted string representation of the specified object</returns>
        public static string AsJson<TType>(this object @this)
        {
            var s = new DataContractJsonSerializer(typeof(TType));
            //var s = new DataContractJsonSerializer(typeof(TType), new DataContractJsonSerializerSettings() { EmitTypeInformation = EmitTypeInformation.Always, UseSimpleDictionaryFormat = false});
            using (var stream = new System.IO.MemoryStream())
            {
                s.WriteObject(stream, @this);
                return stream.ToArray().AsString();
            }
        }
        /// <summary>
        /// Serializes an object to an xml string
        /// </summary>
        /// <typeparam name="TType">The type of object to serialize</typeparam>
        /// <param name="this">The object to serialize</param>
        /// <returns></returns>
        public static string AsXml<TType>(this object @this)
        {
            var s = new DataContractSerializer(typeof(TType));
            using (var stream = new System.IO.MemoryStream())
            {
                s.WriteObject(stream, @this);
                return stream.ToArray().AsString();
            }
        }
    }

    /// <summary>
    /// Extensions which convert objects of one type to another
    /// </summary>
    public static class FromExtensions
    {
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
                var s = new DataContractJsonSerializer(typeof(TType));
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
            var s = new DataContractSerializer(typeof(TType));
            using (var stream = new System.IO.MemoryStream(@this, 0, @this.Length))
            {
                result = (TType)s.ReadObject(stream);
            }
            return result;
        }
    }
}
