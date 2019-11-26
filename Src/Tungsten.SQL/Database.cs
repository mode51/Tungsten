using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using Tungsten.SQL;
using Tungsten.SQL.Attributes;

namespace Tungsten.SQL
{
    public partial class Database
    {
        private static void AddParameter(IDbCommand command, string parameterName, object value)
        {
            IDataParameter param = command.CreateParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            command.Parameters.Add(param);
        }

        public class Parameter
        {
            public Parameter()
            {
            }
            public Parameter(string name, object value)
            {
                this.Name = name;
                this.Value = value;
            }
            public string Name { get; set; }
            public object Value { get; set; }
        }
        public static int ExecuteNonQuery(IDbConnection connection, string sql, params Parameter[] parameters)
        {
            int result = 0;
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        foreach (Parameter parameter in parameters)
                        {
                            Database.AddParameter(command, parameter.Name, parameter.Value);
                        }
                        result = command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            catch (System.Data.DataException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return result;
        }
        public static IDataReader ExecuteReader(IDbConnection connection, string sql, params Parameter[] parameters)
        {
            IDataReader result = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    foreach (Parameter parameter in parameters)
                    {
                        Database.AddParameter(command, parameter.Name, parameter.Value);
                    }
                    result = command.ExecuteReader();
                }
            }
            catch (System.Data.DataException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return result;
        }
        public static object ExecuteScalar(IDbConnection connection, string sql, params Parameter[] parameters)
        {
            object result = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    foreach (Parameter parameter in parameters)
                    {
                        Database.AddParameter(command, parameter.Name, parameter.Value);
                    }
                    result = command.ExecuteScalar();
                }
            }
            catch (System.Data.DataException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return result;
        }

        public static bool CreateDatabase(IDbConnection connection, string database)
        {
            string sql = string.Format("If Object_Id('{0}') IS NULL Create Database [{0}]", database);
            return false;
        }
        public static bool DropDatabase(IDbConnection connection, string database)
        {
            string sql = string.Format("DROP DATABASE {0}", database);
            return false;
        }
        public static bool CreateTable(IDbConnection connection, string database, string table)
        {
            return false;
        }
        public static bool DropTable(IDbConnection connection, string database, string table)
        {
            string sql = string.Format("USE {0}; DROP TABLE {1}", database, table);
            return false;
        }

        public static Dictionary<string, List<string>> GetTablesAndColumns(SqlConnection connection)
        {
            var result = new Dictionary<string, List<string>>();
            try
            {
                var tableNames = GetTables(connection);
                foreach (var tableName in tableNames)
                {
                    result.Add(tableName, new List<string>());
                    var columnNames = GetColumns(connection, tableName);
                    result[tableName].AddRange(columnNames);
                }
            }
            catch (System.Data.DataException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return result;

        }
        public static List<string> GetTables(SqlConnection connection)
        {
            var result = new List<string>();
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                DataTable schema = connection.GetSchema("Tables");
                foreach (DataRow row in schema.Rows)
                {
                    result.Add(row[2].ToString());
                }
            }
            catch (System.Data.DataException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return result;
        }
        public static List<string> GetColumns(SqlConnection connection, string tableName)
        {
            string[] restrictions = new string[4] { null, null, tableName, null };
            if (connection.State != ConnectionState.Open)
                connection.Open();
            var columnList = connection.GetSchema("Columns", restrictions).AsEnumerable().Select(s => s.Field<String>("Column_Name")).ToList();
            return columnList;
        } 
    }
}
