using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace W.Net.Helpers
{
    public static class AsExtensions
    {
        /// <summary>
        /// Serializes an object to a Json string
        /// </summary>
        /// <typeparam name="TType">The type of object to serialize</typeparam>
        /// <param name="this">The object to serialize to Json</param>
        /// <returns>A Json formatted string representation of the specified object</returns>
        public static string AsJson<TType>(this object @this)
        {
            var s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(TType));
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
            var s = new System.Runtime.Serialization.DataContractSerializer(typeof(TType));
            using (var stream = new System.IO.MemoryStream())
            {
                s.WriteObject(stream, @this);
                return stream.ToArray().AsString();
            }
        }
    }
}
