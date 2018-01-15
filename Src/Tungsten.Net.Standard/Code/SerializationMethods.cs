using W.AsExtensions;

namespace W.Net
{
    /// <summary>
    /// Serialization methods
    /// </summary>
    public static class SerializationMethods
    {
        /// <summary>
        /// Serializes an object to a json formatted string
        /// </summary>
        /// <param name="item">The item to serialize</param>
        /// <returns>If the object is serializable, a json string containing the serialized object, otherwise null.</returns>
        public static string Serialize(object item)
        {
            try
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                return json;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Deserializes a json formatted string to an object of the specified type
        /// </summary>
        /// <returns>The deserialized object if successful, otherwise the default value for TItemType(usually null)</returns>
        public static TItemType Deserialize<TItemType>(string json)
        {
            try
            {
                var item = Newtonsoft.Json.JsonConvert.DeserializeObject<TItemType>(json);
                return item;
            }
            catch (System.Exception)
            {
                return default(TItemType);
            }
        }
        /// <summary>
        /// Deserializes a byte array containing a json formatted string to an object of the specified type
        /// </summary>
        /// <returns>The deserialized object if successful, otherwise the default value for TItemType(usually null)</returns>
        public static TItemType Deserialize<TItemType>(ref byte[] bytes)
        {
            try
            {
                return Deserialize<TItemType>(bytes.AsString());
            }
            catch (System.Exception)
            {
                return default(TItemType);
            }
        }
    }
}