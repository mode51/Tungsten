using System;
using System.Data;

namespace Tungsten.SQL.Attributes
{
    public class ColumnAttribute : Attribute { }
    public class IsPrimaryKeyAttribute : ColumnAttribute { }
    public class IsKeyColumnAttribute : ColumnAttribute { }
    public class IsNullableAttribute : ColumnAttribute { }

    //The idea is that any field/property which has one of the above attributes, WILL have a NamedColumn attribute
    public class NamedColumnAttribute : ColumnAttribute
    {
        public string Column { get; set; }
        public SqlDbType Type { get; set; }

        public NamedColumnAttribute() { }
        public NamedColumnAttribute(string column, SqlDbType type)
        {
            Column = column;
            Type = type;
        }
    }
}