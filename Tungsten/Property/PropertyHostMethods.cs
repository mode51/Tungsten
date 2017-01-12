using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace W
{
    public static class PropertyHostMethods
    {
        private static IOwnedProperty GetProperty(FieldInfo fieldInfo, object owner)
        {
            if (fieldInfo?.FieldType?.IsClass ?? false)
            {
                var value = fieldInfo.GetValue(owner);
                var property = value as IOwnedProperty;
                return property;
            }
            return null;
        }
        private static IOwnedProperty GetProperty(PropertyInfo propertyInfo, object owner)
        {
            if (propertyInfo?.PropertyType?.IsClass ?? false)
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
        /// <param name="owner"></param>
        public static void InitializeProperties(object owner)// where TOwner : class
        {
            var fieldInfos = owner.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            //foreach (var fieldInfo in fieldInfos)
            //{
            //   SetOwner(fieldInfo, owner);
            //}
            Parallel.For(0, fieldInfos.Length, (i, state) =>
              {
                  var fieldInfo = fieldInfos[i];
                  SetOwner(fieldInfo, owner);
              });
            var propertyInfos = owner.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            //foreach (var propertyInfo in propertyInfos)
            //{
            //    SetOwner(propertyInfo, owner);
            //}
            Parallel.For(0, propertyInfos.Length, (i, state) =>
              {
                  var propertyInfo = propertyInfos[i];
                  SetOwner(propertyInfo, owner);
              });
        }

        /// <summary>
        /// <para>
        /// Scans the IsDirty value of each field and property of type IProperty 
        /// </para>
        /// </summary>
        /// <returns>True if any IProperty member's IsDirty value is true, otherwise false</returns>
        public static bool IsDirty(object owner)// where TOwner : class
        {
            var result = new Lockable<bool>();
            var fieldInfos = owner.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Parallel.For(0, fieldInfos.Length, (i, state) =>
            {
                var fieldInfo = fieldInfos[i];
                var property = GetProperty(fieldInfo, owner) as IProperty;
                if (property?.IsDirty ?? false)
                {
                    result.Value = true;
                    state.Stop();
                }
            });
            if (result.Value)
                return true;
            var propertyInfos = owner.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Parallel.For(0, propertyInfos.Length, (i, state) =>
              {
                  var propertyInfo = propertyInfos[i];//.GetValue(owner) as IProperty;
                  var property = GetProperty(propertyInfo, owner) as IProperty;
                  if (property?.IsDirty ?? false)
                  {
                      result.Value = true;
                      state.Stop();
                  }
              });
            return result.Value;
        }
        /// <summary>
        /// <para>
        /// Scans each field and property of type IProperty and sets it's IsDirty flag to false
        /// </para>
        /// </summary>
        public static void MarkAsClean(object owner)// where TOwner : class
        {
            var fieldInfos = owner.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Parallel.For(0, fieldInfos.Length, (i, state) =>
              {
                  var fieldInfo = fieldInfos[i];//.GetValue(owner) as IProperty;
                  var property = GetProperty(fieldInfo, owner) as IProperty;
                  if (property != null)
                      property.IsDirty = false;
              });
            var propertyInfos = owner.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Parallel.For(0, propertyInfos.Length, (i, state) =>
              {
                  var propertyInfo = propertyInfos[i];//.GetValue(owner) as IProperty;
                  var property = GetProperty(propertyInfo, owner) as IProperty;
                  if (property != null)
                      property.IsDirty = false;
              });
        }

    }
}