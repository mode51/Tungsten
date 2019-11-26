using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Tungsten.Reflection;

namespace Tungsten.Reflection
{
    /// <summary>
    /// Provides methods to copy fields and properties from one CLR object to another
    /// </summary>
    public static class ShallowCopy
    {
        /// <summary>
        /// Copies all the common fields from the source object to the destination object
        /// </summary>
        /// <param name="destination">The object on which to set fields</param>
        /// <param name="source">The object from which to obtain field values</param>
        /// <param name="flags">The criteria for finding fields</param>
        public static void CopyFields(object destination, object source, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        {
            foreach (var field in source.GetFields(flags))
            {
                destination.SetFieldValue(field.Name, field.GetValue(source));
            }
        }
        /// <summary>
        /// Copies all the common properties from the source object to the destination object
        /// </summary>
        /// <param name="destination">The object on which to set properties</param>
        /// <param name="source">The object from which to obtain property values</param>
        /// <param name="flags">The criteria for finding properties</param>
        public static void CopyProperties(object destination, object source, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        {
            foreach (var property in source.GetProperties(flags))
            {
                destination.SetPropertyValue(property.Name, property.GetValue(source));
            }
        }
    }
    /// <summary>
    /// Provides extension methods to copy fields and properties from one CLR object to another
    /// </summary>
    public static class ShallowCopyExtensions
    {
        /// <summary>
        /// Copies all the common fields from the source object to the destination object
        /// </summary>
        /// <typeparam name="T">The Type of the CLR object</typeparam>
        /// <param name="this">The object on which to set fields</param>
        /// <param name="source">The object from which to obtain field values</param>
        /// <param name="flags">The criteria for finding fields</param>
        /// <returns></returns>
        public static T CopyFields<T>(this T @this, T source, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        {
            ShallowCopy.CopyFields(@this, source, flags);
            return @this;
        }
        /// <summary>
        /// Copies all the common properties from the source object to the destination object
        /// </summary>
        /// <typeparam name="T">The Type of the CLR object</typeparam>
        /// <param name="this">The object on which to set properties</param>
        /// <param name="source">The object from which to obtain property values</param>
        /// <param name="flags">The criteria for finding properties</param>
        public static T CopyProperties<T>(this T @this, T source, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        {
            ShallowCopy.CopyProperties(@this, source, flags);
            return @this;
        }
    }
    /// <summary>
    /// Provides extension methods to copy fields and properties from one CLR object to another
    /// </summary>
    public static class ShallowCloneExtensions
    {
        /// <summary>
        /// Creates a new instance of the specified CLR Type and copies fields from the source object
        /// </summary>
        /// <typeparam name="T">The Type of the CLR object</typeparam>
        /// <param name="source">The object from which to copy fields</param>
        /// <param name="flags">The criteria for finding fields</param>
        /// <returns>The newly created object</returns>
        public static T CloneFields<T>(this T source, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        {
            var result = Activator.CreateInstance<T>();
            result.CopyFields(source, flags);
            return result;
        }
        /// <summary>
        /// Creates a new instance of the specified CLR Type and copies properties from the source object
        /// </summary>
        /// <typeparam name="T">The Type of the CLR object</typeparam>
        /// <param name="source">The object from which to copy properties</param>
        /// <param name="flags">The criteria for finding properties</param>
        /// <returns>The newly created object</returns>
        public static T CloneProperties<T>(this T source, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        {
            var result = Activator.CreateInstance<T>();
            result.CopyProperties(source, flags);
            return result;
        }
    }
}
