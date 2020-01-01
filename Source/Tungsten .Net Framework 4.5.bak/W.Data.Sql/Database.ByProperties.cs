using System;
using System.Data;
using System.Linq;
using W.Data.Sql.Attributes;

namespace W.Data.Sql
{
    public partial class Database
    {
        public class ByProperties
        {
            internal static string GetSelectStatement<T>()
            {
                string select = "Select ";
                string tableName = TableAttribute.GetTable<T>();
                string abbreviation = tableName.Substring(0, 1);
                var properties = Attributes.ByProperties.GetColumns<T>();

                select += string.Join(",", (from f in properties select string.Format("{0}.[{1}]", abbreviation, f.Attributes.NamedColumn.Column)).ToArray());
                select += string.Format(" From [{0}] {1} ", tableName, abbreviation);
                return select;
            }
            public static IDbCommand SelectByPrimaryKey<T, U>(IDbConnection connection, U primaryKeyValue)
            {
                IDbCommand command = connection.CreateCommand();
                string select = GetSelectStatement<T>();
                Attributes.ByProperties.PropertyData primaryKey = Attributes.ByProperties.GetPrimaryKey<T>();
                select += " Where [" + primaryKey.Attributes.NamedColumn.Column + "]=@" + primaryKey.Attributes.NamedColumn.Column;
                AddParameter(command, "@" + primaryKey.Attributes.NamedColumn.Column, primaryKeyValue);

                command.CommandText = select;
                return command;
            }
            public static IDbCommand Query<T>(IDbConnection connection, string where, params Parameter[] parameters)
            {
                IDbCommand command = connection.CreateCommand();
                string select = GetSelectStatement<T>();
                select += " " + where;
                foreach (var parameter in parameters)
                    AddParameter(command, parameter.Name, parameter.Value);

                command.CommandText = select;
                return command;
            }

            //these methods shouldn't have any logic other than to do their expected behavior (that is, no testing to see if Row.IsMarkedForDeletion == true)
            public static IDbCommand Save<T>(T obj, IDbConnection connection)
            {
                IDbCommand command = connection.CreateCommand();
                string update = string.Empty;
                string newValues = string.Empty;
                string where = string.Empty;

                update = "Update [" + TableAttribute.GetTable(obj) + "] Set ";

                var columns = Attributes.ByProperties.GetColumns<T>();
                foreach (var column in columns)
                {
                    var propertyValue = column.PropertyInfo.GetValue(obj, null);
                    if (!column.Attributes.IsPrimaryKey && !(column.Attributes.IsNullable && propertyValue == null))
                    {
                        string columnName = "[" + column.Attributes.NamedColumn.Column + "]";
                        string columnParam = "@" + column.Attributes.NamedColumn.Column;

                        if (column.Attributes.IsKeyColumn)
                        {
                            if (where.Length != 0)
                                where += " and ";
                            where += columnName + " = " + columnParam;
                        }
                        else
                        {
                            if (newValues.Length != 0)
                                newValues += ", ";
                            newValues += columnName + " = " + columnParam;
                        }
                        AddParameter(command, columnParam, propertyValue);
                    }

                }
                command.CommandText = update + newValues + " Where " + where;

                return command;
            }
            public static IDbCommand Insert<T>(T obj, IDbConnection connection)
            {
                IDbCommand command = connection.CreateCommand();
                string insert = string.Empty;
                string columns = string.Empty;
                string values = string.Empty;

                foreach (TableAttribute tableAttribute in obj.GetType().GetCustomAttributes(typeof(TableAttribute), true))
                {
                    insert = string.Format("Insert Into [{0}]", tableAttribute.Table);
                }

                var columnInfo = Attributes.ByProperties.GetColumns<T>();
                foreach (var column in columnInfo)
                {
                    var propertyValue = column.PropertyInfo.GetValue(obj, null);
                    if ((!column.Attributes.IsPrimaryKey) && !((propertyValue == null) && column.Attributes.IsNullable))
                    {
                        if (columns.Length != 0)
                            columns += ", ";
                        columns += "[" + column.Attributes.NamedColumn.Column + "]";

                        if (values.Length != 0)
                            values += ", ";
                        values += "@" + column.Attributes.NamedColumn.Column;

                        AddParameter(command, "@" + column.Attributes.NamedColumn.Column, propertyValue);
                    }
                }
                command.CommandText = insert + " (" + columns + ") VALUES (" + values + ")";

                return command;
            }
            public static IDbCommand Delete(Object obj, IDbConnection connection)
            {
                IDbCommand command = connection.CreateCommand();
                var primaryKey = Attributes.ByProperties.GetPrimaryKey(obj);

                command.CommandText = string.Format("Delete From [{0}] where [{1}]=@{2}", TableAttribute.GetTable(obj), primaryKey.Attributes.NamedColumn.Column, primaryKey.Attributes.NamedColumn.Column);
                AddParameter(command, "@" + primaryKey.Attributes.NamedColumn.Column, primaryKey.PropertyInfo.GetValue(obj, null));

                return command;
            }
        }
    }
}
