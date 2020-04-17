using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
//using System.Threading.Tasks;

namespace W.Reflection
{
    /// <summary>
    /// Provides a binary clone function
    /// </summary>
    public static class BinaryClone
    {
        /// <summary>
        /// Clones a CLR object using the BinaryFormatter
        /// </summary>
        /// <typeparam name="T">The Type of the clone</typeparam>
        /// <param name="source">The source of the clone operation</param>
        /// <returns>A clone of the source CLR object</returns>
        public static T Clone<T>(T source)
        {
            T result = default(T);
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            bf.Serialize(ms, source);
            ms.Seek(0, SeekOrigin.Begin);
            result = (T)bf.Deserialize(ms);
            ms.Close();
            return result;
        }
    }
    /// <summary>
    /// Provides a binary clone extension method
    /// </summary>
    public static class BinaryCloneExtensions
    {
        /// <summary>
        /// Clones a CLR object using the BinaryFormatter
        /// </summary>
        /// <typeparam name="T">The Type of the clone</typeparam>
        /// <param name="source">The source of the clone operation</param>
        /// <returns>A clone of the source CLR object</returns>
        public static T Clone<T>(this T source)
        {
            T result = default(T);
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            bf.Serialize(ms, source);
            ms.Seek(0, SeekOrigin.Begin);
            result = (T)bf.Deserialize(ms);
            ms.Close();
            return result;
        }
    }
}
