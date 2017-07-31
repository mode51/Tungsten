using System.Linq;
using System.Reflection;

namespace W
{

    /// <summary>
    /// Exposes static PropertyHost extension methods
    /// </summary>
    public static class PropertyHostMethods
    {
        //1.20.2017 - removed Parallel implementations because this causes cross-thread exceptions on UI objects
        private static IOwnedProperty GetProperty(FieldInfo fieldInfo, object owner)

        {
#if WINDOWS_UWP || WINDOWS_PORTABLE || NETCOREAPP1_0 || NETCOREAPP1_1
            if (fieldInfo?.FieldType.GetTypeInfo().IsClass ?? false)
#else
            if (fieldInfo?.FieldType?.IsClass ?? false)
#endif
            {
                var value = fieldInfo.GetValue(owner);
                var property = value as IOwnedProperty;
                return property;
            }
            return null;
        }
        private static IOwnedProperty GetProperty(PropertyInfo propertyInfo, object owner)
        {
#if WINDOWS_UWP || WINDOWS_PORTABLE || NETCOREAPP1_0 || NETCOREAPP1_1
            if (propertyInfo?.PropertyType.GetTypeInfo().IsClass ?? false)
#else
            if (propertyInfo?.PropertyType?.IsClass ?? false)
#endif
            {
                var value = propertyInfo.GetValue(owner);
                var property = value as IOwnedProperty;
                return property;
            }
            return null;
        }
        private static void SetOwner(FieldInfo fi, object owner)
        {
            var property = GetProperty(fi, owner);
            property?.SetOwner(owner);
        }
        private static void SetOwner(PropertyInfo pi, object owner)
        {
            var property = GetProperty(pi, owner);
            property?.SetOwner(owner);
        }

        /// <summary>
        /// <para>
        /// Scans the fields and properties of "owner" and sets the member's Owner property to "owner"
        /// This method should be called in the constructor of any class which has IOwnedProperty members
        /// </para>
        /// </summary>
        /// <param name="this"></param>
        public static void InitializeProperties(this object @this)// where TOwner : class
        {
#if WINDOWS_PORTABLE
            var fieldInfos = @this.GetType().GetTypeInfo().DeclaredFields.ToList();
            foreach (var fieldInfo in fieldInfos)
            {
               SetOwner(fieldInfo, @this);
            }
#else
            var fieldInfos = @this.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var fieldInfo in fieldInfos)
            {
                SetOwner(fieldInfo, @this);
            }
            //Parallel.For(0, fieldInfos.Count, (i, state) =>
            //  {
            //      var fieldInfo = fieldInfos[i];
            //      SetOwner(fieldInfo, @this);
            //  });
#endif
#if WINDOWS_PORTABLE
            var propertyInfos = @this.GetType().GetTypeInfo().DeclaredProperties.ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                SetOwner(propertyInfo, @this);
            }
#else
            var propertyInfos = @this.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                SetOwner(propertyInfo, @this);
            }
            //Parallel.For(0, propertyInfos.Count, (i, state) =>
            //  {
            //      var propertyInfo = propertyInfos[i];
            //      SetOwner(propertyInfo, @this);
            //  });
#endif
        }

        /// <summary>
        /// <para>
        /// Scans the IsDirty value of each field and property of type IProperty 
        /// </para>
        /// </summary>
        /// <returns>True if any IProperty member's IsDirty value is true, otherwise false</returns>
        public static bool IsDirty(this object @this)// where TOwner : class
        {
            var result = new Lockable<bool>();
#if WINDOWS_PORTABLE
            var fieldInfos = @this.GetType().GetTypeInfo().DeclaredFields.ToList();
            foreach (var fieldInfo in fieldInfos)
            {
                var property = GetProperty(fieldInfo, @this) as IProperty;
                if (property?.IsDirty ?? false)
                {
                    result.Value = true;
                    break;
                }
            }
#else
            var fieldInfos = @this.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var fieldInfo in fieldInfos)
            {
                var property = GetProperty(fieldInfo, @this) as IProperty;
                if (property?.IsDirty ?? false)
                {
                    result.Value = true;
                    break;
                }
            }
            //Parallel.For(0, fieldInfos.Count, (i, state) =>
            //{
            //    var fieldInfo = fieldInfos[i];
            //    var property = GetProperty(fieldInfo, @this) as IProperty;
            //    if (property?.IsDirty ?? false)
            //    {
            //        result.Value = true;
            //        state.Stop();
            //    }
            //});
#endif
            if (result.Value)
                return true;
#if WINDOWS_PORTABLE
            var propertyInfos = @this.GetType().GetTypeInfo().DeclaredProperties.ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                var property = GetProperty(propertyInfo, @this) as IProperty;
                if (property?.IsDirty ?? false)
                {
                    result.Value = true;
                    break;
                }
            }
#else
            var propertyInfos = @this.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                var property = GetProperty(propertyInfo, @this) as IProperty;
                if (property?.IsDirty ?? false)
                {
                    result.Value = true;
                    break;
                }
            }
            //Parallel.For(0, propertyInfos.Count, (i, state) =>
            //  {
            //      var propertyInfo = propertyInfos[i];//.GetValue(@this) as IProperty;
            //      var property = GetProperty(propertyInfo, @this) as IProperty;
            //      if (property?.IsDirty ?? false)
            //      {
            //          result.Value = true;
            //          state.Stop();
            //      }
            //  });
#endif
            return result.Value;
        }
        /// <summary>
        /// <para>
        /// Scans each field and property of type IProperty and sets it's IsDirty flag to false
        /// </para>
        /// </summary>
        public static void MarkAsClean(this object @this)// where TOwner : class
        {
#if WINDOWS_PORTABLE
            var fieldInfos = @this.GetType().GetTypeInfo().DeclaredFields.ToList();
            foreach (var fieldInfo in fieldInfos)
            {
                var property = GetProperty(fieldInfo, @this) as IProperty;
                if (property != null)
                    property.IsDirty = false;
            }
#else
            var fieldInfos = @this.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var fieldInfo in fieldInfos)
            {
                var property = GetProperty(fieldInfo, @this) as IProperty;
                if (property != null)
                    property.IsDirty = false;
            }
            //Parallel.For(0, fieldInfos.Count, (i, state) =>
            //  {
            //      var fieldInfo = fieldInfos[i];//.GetValue(@this) as IProperty;
            //      var property = GetProperty(fieldInfo, @this) as IProperty;
            //      if (property != null)
            //          property.IsDirty = false;
            //  });
#endif
#if WINDOWS_PORTABLE
            var propertyInfos = @this.GetType().GetTypeInfo().DeclaredProperties.ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                var property = GetProperty(propertyInfo, @this) as IProperty;
                if (property != null)
                    property.IsDirty = false;
            }
#else
            var propertyInfos = @this.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                var property = GetProperty(propertyInfo, @this) as IProperty;
                if (property != null)
                    property.IsDirty = false;
            }
            //Parallel.For(0, propertyInfos.Count, (i, state) =>
            //  {
            //      var propertyInfo = propertyInfos[i];//.GetValue(@this) as IProperty;
            //      var property = GetProperty(propertyInfo, @this) as IProperty;
            //      if (property != null)
            //          property.IsDirty = false;
            //  });
#endif
        }

    }
}