﻿using System.Reflection;

/// <summary>
/// Extension methods related to W.PropertyHost or any class which has one or more W.Property members
/// </summary>
namespace W
{
    /// <summary>
    /// Extension methods related to W.PropertyHost or any class which has multiple W.Property members
    /// </summary>
    public static class PropertyHostExtensions
    {
        //1.20.2017 - removed Parallel implementations because this causes cross-thread exceptions on UI objects
        private static TInterface GetProperty<TInterface>(FieldInfo fieldInfo, object owner)

        {
            if (fieldInfo?.FieldType.GetTypeInfo().IsClass ?? false)
            {
                var value = fieldInfo.GetValue(owner);
                if (value is TInterface)
                    return (TInterface)value;
            }
            return default(TInterface);
        }
        private static TInterface GetProperty<TInterface>(PropertyInfo propertyInfo, object owner)
        {
            if (propertyInfo?.PropertyType.GetTypeInfo().IsClass ?? false)
            {
                var value = propertyInfo.GetValue(owner);
                if (value is TInterface)
                    return (TInterface)value;
            }
            return default(TInterface);
        }
        private static void SetOwner(FieldInfo fi, object owner)
        {
            var property = GetProperty<IOwnedProperty>(fi, owner);
            property?.SetOwner(owner);
        }
        private static void SetOwner(PropertyInfo pi, object owner)
        {
            var property = GetProperty<IOwnedProperty>(pi, owner);
            property?.SetOwner(owner);
        }
        /// <summary>
        /// <para>
        /// Scans the fields and properties of "owner" and sets the member's Owner property to "owner"
        /// This method should be called in the constructor of any class which has IOwnedProperty members
        /// </para>
        /// </summary>
        /// <param name="this">The object on which to find and initialize properties</param>
        public static void InitializeProperties(this object @this)// where TOwner : class
        {
            var fieldInfos = @this.GetType().GetTypeInfo().DeclaredFields;//.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var fieldInfo in fieldInfos)
            {
                SetOwner(fieldInfo, @this);
            }

            var propertyInfos = @this.GetType().GetTypeInfo().DeclaredProperties;//.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                SetOwner(propertyInfo, @this);
            }
        }

        /// <summary>
        /// <para>
        /// Scans the IsDirty value of each field and property of type IProperty 
        /// </para>
        /// </summary>
        /// <param name="this">The object on which to inspect for dirty properties</param>
        /// <returns>True if any IProperty member's IsDirty value is true, otherwise false</returns>
        public static bool IsDirty(this object @this)// where TOwner : class
        {
            using (var result = new LockableSlim<bool>())
            {
                var fieldInfos = @this.GetType().GetTypeInfo().DeclaredFields;//.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
                foreach (var fieldInfo in fieldInfos)
                {
                    var property = GetProperty<IProperty>(fieldInfo, @this);
                    if (property?.IsDirty ?? false)
                    {
                        result.Value = true;
                        break;
                    }
                }
                if (result.Value)
                    return true;

                var propertyInfos = @this.GetType().GetTypeInfo().DeclaredProperties;//.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
                foreach (var propertyInfo in propertyInfos)
                {
                    var property = GetProperty<IProperty>(propertyInfo, @this);
                    if (property?.IsDirty ?? false)
                    {
                        result.Value = true;
                        break;
                    }
                }
                return result.Value;
            }
        }
        /// <summary>
        /// <para>
        /// Scans each field and property of type IProperty and sets it's IsDirty flag to false
        /// </para>
        /// </summary>
        /// <param name="this">The object on which to mark all properties as clean</param>
        public static void MarkAsClean(this object @this)// where TOwner : class
        {
            var fieldInfos = @this.GetType().GetTypeInfo().DeclaredFields;//.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var fieldInfo in fieldInfos)
            {
                var property = GetProperty<IProperty>(fieldInfo, @this);
                if (property != null)
                    property.IsDirty = false;
            }
            var propertyInfos = @this.GetType().GetTypeInfo().DeclaredProperties;//.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                var property = GetProperty<IProperty>(propertyInfo, @this);
                if (property != null)
                    property.IsDirty = false;
            }
        }
        /// <summary>
        /// Raises the PropertyChanged event regardless of whether the value has changed or not
        /// </summary>
        public static void ForcePropertyChanged(this object @this)
        {
            var fieldInfos = @this.GetType().GetTypeInfo().DeclaredFields;//.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var fieldInfo in fieldInfos)
            {
                var property = GetProperty<IProperty>(fieldInfo, @this);
                if (property != null)
                    property.ForcePropertyChanged("Value");
            }
            var propertyInfos = @this.GetType().GetTypeInfo().DeclaredProperties;//.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                var property = GetProperty<IProperty>(propertyInfo, @this);
                if (property != null)
                    property.ForcePropertyChanged("Value");
            }
        }
    }
}
