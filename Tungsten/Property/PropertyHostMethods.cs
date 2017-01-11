using System;
using System.Reflection;
using System.Threading.Tasks;

namespace W
{
    public static class PropertyHostMethods
    {
        /// <summary>
        /// <para>
        /// Scans the fields and properties of "owner" and sets the member's Owner property to "owner"
        /// This method should be called in the constructor of any class which has IOwnedProperty members
        /// </para>
        /// </summary>
        /// <param name="owner"></param>
        public static void InitializeProperties(object owner)// where TOwner : class
        {
            var fields = owner.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Parallel.For(0, fields.Length, (i, state) =>
              {
                  var value = fields[i].GetValue(owner) as IOwnedProperty;
                  value?.SetOwner(owner);
              });
            var properties = owner.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Parallel.For(0, properties.Length, (i, state) =>
              {
                  var p = properties[i].GetValue(owner) as IOwnedProperty;
                  p?.SetOwner(owner);
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
            var fields = owner.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Parallel.For(0, fields.Length, (i, state) =>
              {
                  var field = fields[i].GetValue(owner) as IProperty;
                  if (field?.IsDirty ?? false)
                  {
                      result.Value = true;
                      state.Stop();
                  }
              });
            if (result.Value)
                return true;
            var properties = owner.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Parallel.For(0, properties.Length, (i, state) =>
              {
                  var p = properties[i].GetValue(owner) as IProperty;
                  if (p?.IsDirty ?? false)
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
            var fields = owner.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Parallel.For(0, fields.Length, (i, state) =>
              {
                  var field = fields[i].GetValue(owner) as IProperty;
                  if (field != null && field.IsDirty)
                      field.IsDirty = false;
              });
            var properties = owner.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Parallel.For(0, properties.Length, (i, state) =>
              {
                  var property = properties[i].GetValue(owner) as IProperty;
                  if (property != null && property.IsDirty)
                      property.IsDirty = false;
              });
        }

    }
}