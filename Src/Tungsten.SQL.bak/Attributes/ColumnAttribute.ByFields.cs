using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tungsten.SQL.Attributes
{
    public class ByFields
    {
        public class FieldData
        {
            public FieldInfo FieldInfo { get; internal set; }
            public Attributes Attributes { get; internal set; }
        }
        public static bool IsTableColumn(FieldInfo obj)
        {
            var columnAttributes = obj.GetCustomAttributes(typeof(ColumnAttribute), true);
            return (columnAttributes != null && columnAttributes.Length > 0);
        }
        public static T GetAttribute<T>(FieldInfo obj)
        {
            var columnAttributes = obj.GetCustomAttributes(typeof(T), true);
            if (columnAttributes != null && columnAttributes.Length > 0)
                return ((T)columnAttributes[0]);
            return default(T);
        }
        public static FieldData GetColumn<T>(string columnName)
        {
            var columns = GetColumns<T>();
            var result = from c in columns
                         where c.Attributes.NamedColumn != null && c.Attributes.NamedColumn.Column == columnName
                         select c;
            return result.FirstOrDefault();
        }
        public static List<FieldData> GetColumns<T>()
        {
            var result = new List<FieldData>();

            foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (IsTableColumn(fieldInfo))
                {
                    result.Add(new FieldData
                    {
                        FieldInfo = fieldInfo,
                        Attributes = new Attributes
                        {
                            NamedColumn = GetAttribute<NamedColumnAttribute>(fieldInfo),
                            IsPrimaryKey = GetAttribute<IsPrimaryKeyAttribute>(fieldInfo) != null,
                            IsKeyColumn = GetAttribute<IsKeyColumnAttribute>(fieldInfo) != null,
                            IsNullable = GetAttribute<IsNullableAttribute>(fieldInfo) != null
                        }
                    });
                }
            }
            return result;
        }
        public static FieldData GetPrimaryKey(object obj)
        {
            foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                var columnAttributes = fieldInfo.GetCustomAttributes(typeof(IsPrimaryKeyAttribute), true);
                if (columnAttributes != null && columnAttributes.Length > 0)
                    return new FieldData
                    {
                        FieldInfo = fieldInfo,
                        Attributes = new Attributes
                        {
                            NamedColumn = GetAttribute<NamedColumnAttribute>(fieldInfo),
                            IsPrimaryKey = true,
                            IsKeyColumn = GetAttribute<IsKeyColumnAttribute>(fieldInfo) != null,
                            IsNullable = GetAttribute<IsNullableAttribute>(fieldInfo) != null
                        }
                    };
            }
            return null;
        }
        public static FieldData GetPrimaryKey<T>()
        {
            foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                var columnAttributes = fieldInfo.GetCustomAttributes(typeof(IsPrimaryKeyAttribute), true);
                if (columnAttributes != null && columnAttributes.Length > 0)
                    return new FieldData
                    {
                        FieldInfo = fieldInfo,
                        Attributes = new Attributes
                        {
                            NamedColumn = GetAttribute<NamedColumnAttribute>(fieldInfo),
                            IsPrimaryKey = true,
                            IsKeyColumn = GetAttribute<IsKeyColumnAttribute>(fieldInfo) != null,
                            IsNullable = GetAttribute<IsNullableAttribute>(fieldInfo) != null
                        }
                    };
            }
            return null;
        }
        public static bool SetFieldByColumnName<T>(T obj, string fieldName, object value)
        {
            var field = GetColumn<T>(fieldName);
            if (field != null && field.FieldInfo != null)
            {
                if (value == DBNull.Value)
                {
                    if (field.Attributes.IsNullable)
                        field.FieldInfo.SetValue(obj, null);
                    else
                        throw new ArgumentOutOfRangeException(field.FieldInfo.Name, string.Format("The database column ({0}) contains a NULL value which is invalid for field ({1})", field.Attributes.NamedColumn.Column, field.FieldInfo.Name));
                }
                else
                    field.FieldInfo.SetValue(obj, value);
                return true;
            }
            return false;
        }
    }
}
