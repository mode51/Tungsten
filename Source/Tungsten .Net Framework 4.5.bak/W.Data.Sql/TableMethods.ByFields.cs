﻿using System;
using System.Collections.Generic;
using System.Data;
using W.Data.Sql.Attributes;

namespace W.Data.Sql
{
    public partial class TableMethods
    {
        public class ByFields
        {
            private static void Create(IDbConnection connection, params Row[] items)
            {
                try
                {
                    foreach (var item in items)
                    {
                        using (IDbCommand command = Database.ByFields.Insert(item, connection))
                        {
                            int result = command.ExecuteNonQuery();
                            if (result == 0)
                                throw new Exception(string.Format("Insert for table '{0}' failed.",
                                                                  TableAttribute.GetTable(item)));
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
                        using (IDbCommand command = Database.ByFields.Save(item, connection))
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
                        using (IDbCommand command = Database.ByFields.Delete(item, connection))
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
                //IDbConnection tempConnection = connection;

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
                //connection.Close();
                //connection.Dispose();
                return resultException;
            }

            //public static T SelectByPrimaryKey<T, U>(U primaryKeyValue) where T : Row
            //{
            //    return SelectByPrimaryKey<T, U>(CreateConnection(GetConnectionString(), true), primaryKeyValue);
            //}
            public static T SelectByPrimaryKey<T, U>(IDbConnection connection, U primaryKeyValue) where T : Row
            {
                T result = default(T);
                try
                {
                    using (IDbCommand command = Database.ByFields.SelectByPrimaryKey<T, U>(connection, primaryKeyValue))
                    {
                        //Console.WriteLine(command.CommandText);
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            result = Activator.CreateInstance<T>();
                            result.LoadFromReader<T>(reader);
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
                    using (IDbCommand command = Database.ByFields.Query<T>(connection, where, parameters))
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
