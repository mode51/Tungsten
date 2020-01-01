using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Reflection;

namespace Tungsten.Reflection
{
    /// <summary>
    /// Methods for retrieving or enumerating the properties of a CLR object
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// Returns an enumerable array of properties for the given CLR object
        /// </summary>
        /// <param name="obj">The object on which to enumerate properties</param>
        /// <param name="flags">The criteria for finding properties</param>
        /// <returns>An enumerable array of properties</returns>
#if net45
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<PropertyInfo> GetProperties(this object obj, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        {
            return obj.GetType().GetProperties(flags);
        }
        /// <summary>
        /// Returns the value of the property with the specified name
        /// </summary>
        /// <param name="obj">The object on which to find a property value</param>
        /// <param name="propertyName">The name of the property to find</param>
        /// <returns>If found, the value of the specified property, othwise null</returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            object result = null;
            foreach (PropertyInfo propertyInfo in GetProperties(obj))
            {
                if (propertyInfo.Name.Equals(propertyName))
                {
                    result = propertyInfo.GetValue(obj, null);
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// Returns the value of the property with the specified name
        /// </summary>
        /// <typeparam name="T">The Type of the return value</typeparam>
        /// <param name="obj">The object on which to find a property value</param>
        /// <param name="propertyName">The name of the property to find</param>
        /// <returns>If found, the value of the specified property, othwise default(T)</returns>
#if net45
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            var result = GetPropertyValue(obj, propertyName);
            if (result == null)
                return default(T);
            return (T)result;
        }
        /// <summary>
        /// Sets the value of a property
        /// </summary>
        /// <param name="this">The object on which to set the property value</param>
        /// <param name="propertyName">The name of the property to set</param>
        /// <param name="value">The desired property value</param>
        public static void SetPropertyValue(this object @this, string propertyName, object value)
        {
            foreach (PropertyInfo propertyInfo in GetProperties(@this))
            {
                if (propertyInfo.Name.Equals(propertyName))
                {
                    propertyInfo.SetValue(@this, value, null);
                    return;
                }
            }
        }
        /// <summary>
        /// Populates the properties of an object from a SQL DataReader object
        /// </summary>
        /// <remarks>The CLR object's properties should have the same name as the IDataReader columns</remarks>
        /// <param name="this">The object on which to populate properties</param>
        /// <param name="reader">The IDataReader from which to read property values</param>
        /// <returns>The object on which properties were set</returns>
        public static T PopulatePropertyValues<T>(this T @this, IDataReader reader)
        {
            foreach (var property in @this.GetProperties())
            {
                var value = reader[property.Name];
                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                if (converter.CanConvertFrom(value.GetType()))
                {
                    var convertedValue = converter.ConvertFrom(value);
                    property.SetValue(@this, convertedValue, BindingFlags.FlattenHierarchy | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic, null, null, CultureInfo.CurrentCulture);
                }
            }
            return @this;
        }
    }
    /// <summary>
    /// Methods for retrieving or enumerating the fields of a CLR object
    /// </summary>
    public static class FieldExtensions
    {
        /// <summary>
        /// Returns an enumerable array of fields for the given CLR object
        /// </summary>
        /// <param name="obj">The object on which to enumerate fields</param>
        /// <param name="flags">The criteria for finding fields</param>
        /// <returns>An enumerable array of fields</returns>
#if net45
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<FieldInfo> GetFields(this object @this, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        {
            return @this.GetType().GetFields(flags);
        }
        /// <summary>
        /// Returns the value of the field with the specified name
        /// </summary>
        /// <param name="obj">The object on which to find a field value</param>
        /// <param name="propertyName">The name of the field to find</param>
        /// <returns>If found, the value of the specified field, othwise null</returns>
        public static object GetFieldValue(this object @this, string fieldName)
        {
            object result = null;
            foreach (FieldInfo field in GetFields(@this))
            {
                if (field.Name.Equals(fieldName))
                {
                    result = field.GetValue(@this);
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// Returns the value of the field with the specified name
        /// </summary>
        /// <typeparam name="T">The Type of the return value</typeparam>
        /// <param name="obj">The object on which to find a field value</param>
        /// <param name="propertyName">The name of the field to find</param>
        /// <returns>If found, the value of the specified field, othwise default(T)</returns>
#if net45
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        public static T GetFieldValue<T>(this object @this, string fieldName)
        {
            var result = GetFieldValue(@this, fieldName);
            if (result == null)
                return default(T);
            return (T)result;
        }
        /// <summary>
        /// Sets the value of a field
        /// </summary>
        /// <param name="this">The object on which to set the field value</param>
        /// <param name="propertyName">The name of the field to set</param>
        /// <param name="value">The desired field value</param>
        public static void SetFieldValue(this object @this, string fieldName, object value)
        {
            foreach (FieldInfo field in GetFields(@this))
            {
                if (field.Name.Equals(fieldName))
                {
                    field.SetValue(@this, value);
                    break;
                }
            }
        }
        /// <summary>
        /// Populates the field of an object from a SQL DataReader object
        /// </summary>
        /// <remarks>The CLR object's fields should have the same name as the IDataReader columns</remarks>
        /// <param name="this">The object on which to populate fields</param>
        /// <param name="reader">The IDataReader from which to read field values</param>
        /// <returns>The object on which fields were set</returns>
        public static object PopulateFieldValues(this object @this, IDataReader reader)
        {
            foreach (var field in @this.GetFields())
            {
                var value = reader[field.Name];
                var converter = TypeDescriptor.GetConverter(field.FieldType);
                if (converter.CanConvertFrom(value.GetType()))
                {
                    var convertedValue = converter.ConvertFrom(value);
                    field.SetValue(@this, convertedValue, BindingFlags.FlattenHierarchy | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic, null, CultureInfo.CurrentCulture);
                }
            }
            return @this;
        }
    }
    /// <summary>
    /// Various methods for obtaining Attributes of CLR object fields, properties and functions
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        /// Gets the first Attribute of the specified type
        /// </summary>
        /// <typeparam name="AttributeType">The Type of the attribute to find</typeparam>
        /// <param name="this">The Type object on which to find the specified attribute</param>
        /// <returns>The first attribute of the specified type, if found; otherwise default(AttributeType).</returns>
        public static AttributeType GetAttribute<AttributeType>(this System.Type @this)
        {
            var attributes = @this.GetCustomAttributes(typeof(AttributeType), true);
            if (attributes != null && attributes.Length > 0)
                return (AttributeType)attributes[0];
            return default(AttributeType);
        }
        /// <summary>
        /// Gets an array of attributes of the specified type
        /// </summary>
        /// <typeparam name="AttributeType">The Type of the attributes to find</typeparam>
        /// <param name="this">The Type object on which to find the specified attributes</param>
        /// <returns>An array of AttributeType attribute objects, if found; otherwise null</returns>
        public static AttributeType[] GetAttributes<AttributeType>(this System.Type @this)
        {
            var attributes = @this.GetCustomAttributes(typeof(AttributeType), true);
            if (attributes != null && attributes.Length > 0)
                return attributes as AttributeType[];
            return null;
        }
        /// <summary>
        /// Gets the first Attribute of the specified type
        /// </summary>
        /// <typeparam name="AttributeType">The Type of the attribute to find</typeparam>
        /// <param name="this">The MethodInfo object on which to find the specified attribute</param>
        /// <returns>The attribute of the specified type, if found; otherwise default(AttributeType).</returns>
        public static AttributeType GetAttribute<AttributeType>(this MethodInfo @this)
        {
            var columnAttributes = @this.GetCustomAttributes(typeof(AttributeType), true);
            if (columnAttributes != null && columnAttributes.Length > 0)
                return (AttributeType)columnAttributes[0];
            return default(AttributeType);
        }
        /// <summary>
        /// Gets an array of attributes of the specified type
        /// </summary>
        /// <typeparam name="AttributeType">The Type of the attributes to find</typeparam>
        /// <param name="this">The MethodInfo object on which to find the specified attributes</param>
        /// <returns>An array of AttributeType attribute objects, if found; otherwise null</returns>
        public static AttributeType[] GetAttributes<AttributeType>(this MethodInfo @this)
        {
            var columnAttributes = @this.GetCustomAttributes(typeof(AttributeType), true);
            if (columnAttributes != null && columnAttributes.Length > 0)
                return columnAttributes as AttributeType[];
            return null;
        }
        /// <summary>
        /// Gets the first Attribute of the specified object
        /// </summary>
        /// <typeparam name="AttributeType">The Type of the attribute to find</typeparam>
        /// <param name="this">The object on which to find the specified attribute</param>
        /// <returns>The attribute of the specified type, if found; otherwise default(AttributeType).</returns>
        public static AttributeType GetAttribute<AttributeType>(this object @this)
        {
            var columnAttributes = @this.GetType().GetCustomAttributes(typeof(AttributeType), true);
            if (columnAttributes != null && columnAttributes.Length > 0)
                return (AttributeType)columnAttributes[0];
            return default(AttributeType);
        }
        /// <summary>
        /// Gets an array of attributes of the specified object
        /// </summary>
        /// <typeparam name="AttributeType">The Type of the attributes to find</typeparam>
        /// <param name="this">The object on which to find the specified attributes</param>
        /// <returns>An array of AttributeType attribute objects, if found; otherwise null</returns>
        public static AttributeType[] GetAttributes<AttributeType>(this object @this)
        {
            var columnAttributes = @this.GetType().GetCustomAttributes(typeof(AttributeType), true);
            if (columnAttributes != null && columnAttributes.Length > 0)
                return columnAttributes as AttributeType[];
            return null;
        }
        /// <summary>
        /// Gets the first Attribute of the specified FieldInfo
        /// </summary>
        /// <typeparam name="AttributeType">The Type of the attribute to find</typeparam>
        /// <param name="this">The FieldInfo object on which to find the specified attribute</param>
        /// <returns>The attribute of the specified type, if found; otherwise default(AttributeType).</returns>
        public static AttributeType GetAttribute<AttributeType>(this FieldInfo @this)
        {
            var columnAttributes = @this.GetCustomAttributes(typeof(AttributeType), true);
            if (columnAttributes != null && columnAttributes.Length > 0)
                return (AttributeType)columnAttributes[0];
            return default(AttributeType);
        }
        /// <summary>
        /// Gets an array of attributes of the specified FieldInfo
        /// </summary>
        /// <typeparam name="AttributeType">The Type of the attributes to find</typeparam>
        /// <param name="this">The FieldInfo on which to find the specified attributes</param>
        /// <returns>An array of AttributeType attribute objects, if found; otherwise null</returns>
        public static AttributeType[] GetAttributes<AttributeType>(this FieldInfo @this)
        {
            var columnAttributes = @this.GetCustomAttributes(typeof(AttributeType), true);
            if (columnAttributes != null && columnAttributes.Length > 0)
                return columnAttributes as AttributeType[];
            return null;
        }
        /// <summary>
        /// Gets the first Attribute of the specified PropertyInfo
        /// </summary>
        /// <typeparam name="AttributeType">The Type of the attribute to find</typeparam>
        /// <param name="this">The PropertyInfo object on which to find the specified attribute</param>
        /// <returns>The attribute of the specified type, if found; otherwise default(AttributeType).</returns>
        public static AttributeType GetAttribute<AttributeType>(this PropertyInfo @this)
        {
            var columnAttributes = @this.GetCustomAttributes(typeof(AttributeType), true);
            if (columnAttributes != null && columnAttributes.Length > 0)
                return (AttributeType)columnAttributes[0];
            return default(AttributeType);
        }
        /// <summary>
        /// Gets an array of attributes of the specified PropertyInfo
        /// </summary>
        /// <typeparam name="AttributeType">The Type of the attributes to find</typeparam>
        /// <param name="this">The PropertyINfo on which to find the specified attributes</param>
        /// <returns>An array of AttributeType attribute objects, if found; otherwise null</returns>
        public static AttributeType[] GetAttributes<AttributeType>(this PropertyInfo @this)
        {
            var columnAttributes = @this.GetCustomAttributes(typeof(AttributeType), true);
            if (columnAttributes != null && columnAttributes.Length > 0)
                return columnAttributes as AttributeType[];
            return null;
        }

        /// <summary>
        /// Gets the field value of the specified attribute
        /// </summary>
        /// <typeparam name="AttributeType">The attribute Type</typeparam>
        /// <typeparam name="FieldType">The Type of the attribute field</typeparam>
        /// <param name="this">The object on which to find the attribute</param>
        /// <param name="fieldName">The name of the field on the attribute</param>
        /// <returns>The value of the field of the attribute of the object</returns>
        public static FieldType GetAttributeField<AttributeType,FieldType>(this object @this, string fieldName)
        {
            var attr = @this.GetAttribute<AttributeType>();
            if (attr != null)
            {
                var result = attr.GetFieldValue<FieldType>(fieldName);
                return result;
            }
            return default(FieldType);
        }
        /// <summary>
        /// Gets the property value of the specified attribute
        /// </summary>
        /// <typeparam name="AttributeType">The attribute Type</typeparam>
        /// <typeparam name="FieldType">The Type of the attribute property</typeparam>
        /// <param name="this">The object on which to find the attribute</param>
        /// <param name="fieldName">The name of the property on the attribute</param>
        /// <returns>The value of the property of the attribute of the object</returns>
        public static PropertyType GetAttributeProperty<AttributeType, PropertyType>(this object @this, string propertyName)
        {
            var attr = @this.GetAttribute<AttributeType>();
            if (attr != null)
            {
                var result = attr.GetPropertyValue<PropertyType>(propertyName);
                return result;
            }
            return default(PropertyType);
        }

        /// <summary>
        /// Gets the field value of the specified attribute
        /// </summary>
        /// <remarks>Provided for static classes</remarks>
        /// <typeparam name="AttributeType">The attribute Type</typeparam>
        /// <typeparam name="FieldType">The Type of the attribute field</typeparam>
        /// <param name="this">The object on which to find the attribute</param>
        /// <param name="fieldName">The name of the field on the attribute</param>
        /// <returns>The value of the field of the attribute of the object</returns>
        public static PropertyType GetAttributeProperty<AttributeType, PropertyType>(this Type @this, string propertyName)
        {
            var attr = @this.GetAttribute<AttributeType>();
            if (attr != null)
            {
                var result = attr.GetPropertyValue<PropertyType>(propertyName);
                return result;
            }
            return default(PropertyType);
        }
    }
    //public static class TypeExtensions
    //{
    //    [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    //    public static string GetTypeName(this object @this)
    //    {
    //        return @this.GetType().Name;
    //    }
    //}
    //public static class Resources
    //{
    //    public static string[] GetAssemblyResourceNames(Assembly asm)
    //    {
    //        return asm.GetManifestResourceNames();
    //    }
    //    public static string[] GetEntryAssemblyResourceNames()
    //    {
    //        return System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceNames();
    //    }
    //}
}
