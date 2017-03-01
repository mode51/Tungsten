using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace W.Net.Helpers
{
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
            TType result;
            var s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(TType));
            //var s = new DataContractJsonSerializer(typeof(TType), new DataContractJsonSerializerSettings() { EmitTypeInformation = EmitTypeInformation.Always, UseSimpleDictionaryFormat = false });
            using (var stream = new System.IO.MemoryStream(@this, 0, @this.Length))
            {
                //stream.Write(@this, 0, @this.Length);
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
