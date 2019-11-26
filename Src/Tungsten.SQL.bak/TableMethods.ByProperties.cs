using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Tungsten.SQL;
using Tungsten.SQL.Attributes;

namespace Tungsten.SQL
{
    public partial class TableMethods
    {
        public class ByProperties
        {
            private static void Create(IDbConnection connection, params Row[] items)
            {
                try
                {
                    foreach (var item in items)
                    {
                        using (IDbCommand command = Database.ByProperties.Insert(item, connection))
                        {
                            int result = command.ExecuteNonQuery();
                            if (result == 0)
                                throw new Exception(string.Format("Insert for table '{0}' failed.", TableAttribute.GetTable(item)));
                            //Console.WriteLine("Insert Result = " + result.ToString());
                        }
                    }
                }
                catch (DataException ex)
                {
                    Console.WriteLine(ex.ToString());
                    System.Diagnostics.Debug.Assert(false);
                    throw;
                }
            }
            private static void Update(IDbConnection connection, params Row[] items)
            {
                try
                {
                    foreach (var item in items)
                    {
                        using (IDbCommand command = Database.ByProperties.Save(item, connection))
                        {
                            //Console.WriteLine(command.CommandText);
                            int result = command.ExecuteNonQuery();
                            if (result == 0)
                                throw new Exception(string.Format("Save for table '{0}' failed.", TableAttribute.GetTable(item)));
                            //Console.WriteLine("Save Result = " + result.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    System.Diagnostics.Debug.Assert(false);
                    throw;
                }
            }
            private static void Delete(IDbConnection connection, params Row[] items)
            {
                try
                {
                    foreach (var item in items)
                    {
                        using (IDbCommand command = Database.ByProperties.Delete(item, connection))
                        {
                            //Console.WriteLine(command.CommandText);
                            int result = command.ExecuteNonQuery();
                            if (result == 0)
                                throw new Exception(string.Format("Delete for table '{0}' failed", TableAttribute.GetTable(item)));
                            //Console.WriteLine("Delete Result = " + result.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    System.Diagnostics.Debug.Assert(false);
                    throw;
                }
            }

            //public static Exception Save(bool useTransaction, params Row[] items)
            //{
            //    return Save(CreateConnection(GetConnectionString(), true), useTransaction, items);
            //}
            public static Exception Save(IDbConnection connection, bool useTransaction, params Row[] items)
            {
                Exception resultException = null;
                IDbTransaction transaction = null;

                try
                {
                    if (connection == null)
                        throw new ArgumentNullException(nameof(connection));
                        //tempConnection = CreateConnection(GetConnectionString(), true);

                    if (useTransaction)
                        transaction = connection.BeginTransaction();
                    foreach (var item in items)
                    {
                        if (item.IsMarkedForDeletion)
                        {
                            Delete(connection, item);
                        }
                        else if (item.IsNew)
                        {
                            Create(connection, item);
                        }
                        else
                        {
                            Update(connection, item);
                        }
                    }
                    if (useTransaction)
                        transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    resultException = ex;
                    if (transaction != null)
                        transaction.Rollback();
                    System.Diagnostics.Debug.Assert(false);
                }
                //connection?.Close();
                //connection?.Dispose();
                return resultException;
            }

            //public static T SelectByPrimaryKey<T, U>(U primaryKeyValue)
            //{
            //    return SelectByPrimaryKey<T,U>(CreateConnection(GetConnectionString(), true), primaryKeyValue);
            //}
            public static T SelectByPrimaryKey<T, U>(IDbConnection connection, U primaryKeyValue)
            {
                T result = default(T);
                try
                {
                    using (IDbCommand command = Database.ByProperties.SelectByPrimaryKey<T,U>(connection, primaryKeyValue))
                    {
                        //Console.WriteLine(command.CommandText);
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            dynamic newItem = Activator.CreateInstance<T>();
                            newItem.LoadFromReader<T>(reader);
                            result = newItem;
                        }
                        //Console.WriteLine("Delete Result = " + result.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    System.Diagnostics.Debug.Assert(false);
                    throw;
                }
                return result;
            }
            public static List<T> Query<T>(IDbConnection connection, string where, params Database.Parameter[] parameters)
            {
                var result = new List<T>();
                try
                {
                    using (IDbCommand command = Database.ByProperties.Query<T>(connection, where, parameters))
                    {
                        //Console.WriteLine(command.CommandText);
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            dynamic newItem = Activator.CreateInstance<T>();
                            newItem.LoadFromReader<T>(reader);
                            result.Add(newItem);
                        }
                        //Console.WriteLine("Delete Result = " + result.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    System.Diagnostics.Debug.Assert(false);
                    throw;
                }
                return result;
            }
        }
    }
}
