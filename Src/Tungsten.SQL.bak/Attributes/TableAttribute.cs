using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tungsten.SQL.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TableAttribute : System.Attribute
	{
	    public TableAttribute(string table)
		{
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException(nameof(table));
			Table = table;
		}

		public string Table { get; set; }

	    public static object GetTableAttribute(object obj)
        {
            var tableAttributes = obj.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (tableAttributes.Length > 0)
                return ((TableAttribute)tableAttributes[0]);
            return null;
        }
        public static string GetTable(object obj)
        {
            var tableAttributes = obj.GetType().GetCustomAttributes(typeof (TableAttribute), true);
            if (tableAttributes.Length > 0)
                return ((TableAttribute)tableAttributes[0]).Table;
            return string.Empty;
        }
        public static string GetTable<T>()
        {
            var tableAttributes = typeof(T).GetCustomAttributes(typeof(TableAttribute), true);
            if (tableAttributes.Length > 0)
                return ((TableAttribute)tableAttributes[0]).Table;
            return string.Empty;
        }
    }
}
