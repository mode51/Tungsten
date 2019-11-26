using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tungsten.SQL.Attributes
{
    public class ByProperties
    {
        public class PropertyData
        {
            public PropertyInfo PropertyInfo { get; internal set; }
            public Attributes Attributes { get; internal set; }
        }
        public static bool IsTableColumn(PropertyInfo obj)
        {
            var columnAttributes = obj.GetCustomAttributes(typeof(ColumnAttribute), true);
            return (columnAttributes != null && columnAttributes.Length > 0);
        }
        public static T GetAttribute<T>(PropertyInfo obj)
        {
            var columnAttributes = obj.GetCustomAttributes(typeof(T), true);
            if (columnAttributes.Length > 0)
                return ((T)columnAttributes[0]);
            return default(T);
        }
        public static PropertyData GetPrimaryKey(object obj)
        {
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                var columnAttributes = propertyInfo.GetCustomAttributes(typeof(IsPrimaryKeyAttribute), true);
                if (columnAttributes.Length > 0)
                    return new PropertyData
                    {
                        PropertyInfo = propertyInfo,
                        Attributes = new Attributes
                        {
                            NamedColumn = GetAttribute<NamedColumnAttribute>(propertyInfo),
                            IsPrimaryKey = true,
                            IsKeyColumn = GetAttribute<IsKeyColumnAttribute>(propertyInfo) != null,
                            IsNullable = GetAttribute<IsNullableAttribute>(propertyInfo) != null
                        }
                    };
            }
            return null;
        }
        public static PropertyData GetPrimaryKey<T>()
        {
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                var columnAttributes = propertyInfo.GetCustomAttributes(typeof(IsPrimaryKeyAttribute), true);
                if (columnAttributes.Length > 0)
                    return new PropertyData
                    {
                        PropertyInfo = propertyInfo,
                        Attributes = new Attributes
                        {
                            NamedColumn = GetAttribute<NamedColumnAttribute>(propertyInfo),
                            IsPrimaryKey = true,
                            IsKeyColumn = GetAttribute<IsKeyColumnAttribute>(propertyInfo) != null,
                            IsNullable = GetAttribute<IsNullableAttribute>(propertyInfo) != null
                        }
                    };
            }
            return null;
        }
        public static PropertyData GetColumn<T>(string columnName)
        {
            var columns = GetColumns<T>();
            var result = from c in columns
                         where c.Attributes.NamedColumn != null && c.Attributes.NamedColumn.Column == columnName
                         select c;
            return result.FirstOrDefault();
        }
        public static List<PropertyData> GetColumns<T>()
        {
            var result = new List<PropertyData>();

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (IsTableColumn(propertyInfo))
                {
                    result.Add(new PropertyData
                    {
                        PropertyInfo = propertyInfo,
                        Attributes = new Attributes
                        {
                            NamedColumn = GetAttribute<NamedColumnAttribute>(propertyInfo),
                            IsPrimaryKey = GetAttribute<IsPrimaryKeyAttribute>(propertyInfo) != null,
                            IsKeyColumn = GetAttribute<IsKeyColumnAttribute>(propertyInfo) != null,
                            IsNullable = GetAttribute<IsNullableAttribute>(propertyInfo) != null
                        }
                    });
                }
            }
            return result;
        }
        public static bool SetPropertyByColumnName<T>(T obj, string fieldName, object value)
        {
            var property = GetColumn<T>(fieldName);
            if (property != null && property.PropertyInfo != null)
            {
                if (value == DBNull.Value)
                {
                    if (property.Attributes.IsNullable)
                        property.PropertyInfo.SetValue(obj, null, null);
                    else
                        throw new ArgumentOutOfRangeException(property.PropertyInfo.Name, string.Format("The database column ({0}) contains a NULL value which is invalid for property ({1})", property.Attributes.NamedColumn.Column, property.PropertyInfo.Name));
                }
                else
                    property.PropertyInfo.SetValue(obj, value, null);
                return true;
            }
            return false;
        }
    }
}
