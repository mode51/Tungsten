using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using W.Logging;

namespace W.LiteDb
{
    /// <summary><para>
    /// Static methods, for LiteDb databases, for operations on POCO objects which inherit LiteDbItem.  Class Type names define the LiteDB collection names.
    /// </para></summary>
    public static class LiteDbMethods
    {
        [System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetCollectionName<T>()
        {
            return typeof(T).Name.ToLower();
        }
        [System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static LiteCollection<T> GetCollection<T>(LiteDatabase db) where T : new()
        {
            var collection = GetCollectionName<T>();
            return db.GetCollection<T>(collection);
        }

        /// <summary>
        /// Wraps a database operation in a try/catch block
        /// </summary>
        /// <typeparam name="U">The type of result expected from the function</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="f">The operation to invoke</param>
        /// <returns>A CallResult with the result of the operation</returns>
#pragma warning disable CS1573
        public static CallResult<U> LiteDbAction<U>(string path, Func<LiteDatabase, U> f, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
#pragma warning restore CS1573
        {
            var result = new CallResult<U>();
            try
            {
                if (f == null)
                    throw new ArgumentNullException("LiteDbAction: Parameter 'f' cannot be null");
                using (var db = new LiteDB.LiteDatabase(path))
                {
                    result.Result = f.Invoke(db);
                    result.Success = true;
                }
            }
            catch (LiteException e)
            {
                result.Exception = e;
                Log.e(new Exception(string.Format("LiteException: ErrorCode={0}, Message={1}", e.ErrorCode, e.Message), e));
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(new Exception(string.Format("Caller:{0}, Line:{1}, File:{2}", callerMemberName, callerLineNumber, callerFilePath), e));
            }
            return result;
        }
        // Parameter has no matching param tag in the XML comment (but other parameters do)
        /// <summary>
        /// Wraps a database operation in a try/catch block
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <typeparam name="U">The type of result expected from the operation</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="f">The operation to invoke</param>
        /// <returns>A CallResult with the result of the operation</returns>
#pragma warning disable CS1573
        public static CallResult<U> LiteDbAction<T, U>(string path, Func<LiteDatabase, LiteCollection<T>, U> f, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) where T : new()
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            //return LiteDbAction<T, U>(path, database =>
            //{
            //    return f.Invoke(database, GetCollection<T>(database));
            //}, callerMemberName, callerFilePath, callerLineNumber);
            var result = new CallResult<U>();
            try
            {
                if (f == null)
                    throw new ArgumentNullException("LiteDbAction: Parameter 'f' cannot be null");
                using (var db = new LiteDB.LiteDatabase(path))
                {
                    var collection = GetCollection<T>(db);
                    result.Result = f.Invoke(db, collection);
                    result.Success = true;
                }
            }
            catch (LiteException e)
            {
                result.Exception = e;
                Log.e(new Exception(string.Format("LiteException: ErrorCode={0}, Message={1}", e.ErrorCode, e.Message), e));
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(new Exception(string.Format("Caller:{0}, Line:{1}, File:{2}", callerMemberName, callerLineNumber, callerFilePath), e));
            }
            return result;
        }
        /// <summary>
        /// Wraps a database operation in a try/catch block and transaction
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <typeparam name="U">The type of result expected from the operation</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="f">The operation to invoke</param>
        /// <returns>A CallResult with the result of the oepration</returns>
#pragma warning disable CS1573
        public static CallResult<U> LiteDbAction<T, U>(string path, Func<LiteDatabase, LiteTransaction, LiteCollection<T>, U> f, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) where T : new()
#pragma warning restore CS1573
        {
            //return LiteDbAction<T, U>(path, (database, collection) =>
            //{
            //    using (var transaction = database.BeginTrans())
            //    {
            //        return f.Invoke(database, transaction, collection);
            //    }
            //}, callerMemberName, callerFilePath, callerLineNumber);
            var result = new CallResult<U>();
            try
            {
                if (f == null)
                    throw new ArgumentNullException("LiteDbAction: Parameter 'f' cannot be null");
                using (var db = new LiteDB.LiteDatabase(path))
                {
                    var collection = GetCollection<T>(db);
                    using (var transaction = db.BeginTrans())
                    {
                        result.Result = f.Invoke(db, transaction, collection);
                        result.Success = true;
                    }
                }
            }
            catch (LiteException e)
            {
                result.Exception = e;
                Log.e(new Exception(string.Format("LiteException: ErrorCode={0}, Message={1}", e.ErrorCode, e.Message), e));
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(new Exception(string.Format("Caller:{0}, Line:{1}, File:{2}", callerMemberName, callerLineNumber, callerFilePath), e));
            }
            return result;
        }

        /// <summary>
        /// Creates a new index on the specified field
        /// </summary>
        /// <typeparam name="TItemType">The Type which determines the database collection name to use</typeparam>
        /// <param name="db">The LiteDb database</param>
        /// <param name="fieldName">The field on which to create an index</param>
        /// <returns>True if an index was created, False if an index already exists</returns>
        public static CallResult<bool> EnsureIndex<TItemType>(LiteDatabase db, string fieldName) where TItemType : new()
        {
            var result = new CallResult<bool>();
            result.Result = GetCollection<TItemType>(db)?.EnsureIndex(fieldName, true) ?? false;
            return result;
        }
        /// <summary>
        /// Creates a new index on the specified field
        /// </summary>
        /// <typeparam name="TItemType">The Type which determines the database collection name to use</typeparam>
        /// <param name="collection">The LiteDb collection</param>
        /// <param name="fieldName">The field on which to create an index</param>
        /// <returns>True if an index was created, False if an index already exists</returns>
        public static CallResult<bool> EnsureIndex<TItemType>(LiteCollection<TItemType> collection, string fieldName) where TItemType : new()
        {
            var result = new CallResult<bool>();
            result.Result = collection?.EnsureIndex(fieldName, true) ?? false;
            return result;
        }
        /// <summary>
        /// Creates a new index on the specified field
        /// </summary>
        /// <typeparam name="TItemType">The Type which determines the database collection name to use</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="fieldName">The field on which to create an index</param>
        /// <returns>True if an index was created, False if an index already exists</returns>
        public static CallResult<bool> EnsureIndex<TItemType>(string path, string fieldName) where TItemType : new()
        {
            return LiteDbAction<TItemType, bool>(path, (database, collection) =>
            {
                var result = collection.EnsureIndex(fieldName, true);
                return result;
            });
        }

        /// <summary>
        /// Tests for existing data
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="fieldName">The field to query</param>
        /// <param name="fieldValue">The value to search for</param>
        /// <returns>True if one or more items was found, otherwise False</returns>
        public static CallResult<bool> Exists<T>(string path, string fieldName, object fieldValue) where T : new()
        {
            return LiteDbAction<T, bool>(path, (database, collection) =>
            {
                var query = LiteDB.Query.EQ(fieldName, new LiteDB.BsonValue(fieldValue));
                return collection.Exists(query);
            });
        }
        /// <summary>
        /// Tests for existing data
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="match">A matching function used to find matches</param>
        /// <returns>True if one or more items was found, otherwise False</returns>
        public static CallResult<bool> Exists<T>(string path, System.Linq.Expressions.Expression<Func<T, bool>> match) where T : new()
        {
            return LiteDbAction<T, bool>(path, (database, collection) =>
            {
                return collection.Exists(match);
            });
        }

        /// <summary>
        /// Find and return the first item found matching the criteria
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="fieldName">The name of the field to query</param>
        /// <param name="fieldValue">The value to search for</param>
        /// <returns>The first item found matching the criteria.  Null if no matches were found.</returns>
        public static CallResult<T> FindOne<T>(string path, string fieldName, object fieldValue) where T : new()
        {
            return LiteDbAction<T, T>(path, (database, collection) =>
            {
                var query = LiteDB.Query.EQ(fieldName, new LiteDB.BsonValue(fieldValue));
                return collection.FindOne(query);
            });
        }
        /// <summary>
        /// Find and return the first item found matching the criteria
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="match">A matching function used to find matches</param>
        /// <returns>The first item found matching the criteria.  Null if no matches were found.</returns>
        public static CallResult<T> FindOne<T>(string path, System.Linq.Expressions.Expression<Func<T, bool>> match) where T : new()
        {
            using (var db = new LiteDB.LiteDatabase(path))
            {
                var collection = GetCollection<T>(db);
                var result = collection.Find(match).FirstOrDefault();
                return new CallResult<T>(result != null, result);
            }

            //return LiteDbAction<T, T>(path, (database, collection) =>
            //{
            //    return collection.FindOne(f => match.Invoke(f));
            //});
        }

        /// <summary>
        /// Find all items matching the criteria
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="fieldName">The name of the field to query</param>
        /// <param name="fieldValue">The value to search for</param>
        /// <returns>All items matching the criteria</returns>
        public static CallResult<IEnumerable<T>> Find<T>(string path, string fieldName, object fieldValue) where T : new()
        {
            return LiteDbAction<T, IEnumerable<T>>(path, (database, collection) =>
            {
                var query = LiteDB.Query.EQ(fieldName, new LiteDB.BsonValue(fieldValue));
                return collection.Find(query);
            });
        }
        /// <summary>
        /// Find all items matching the criteria
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="match">A matching function used to find matches</param>
        /// <returns>All items matching the criteria</returns>
        public static CallResult<IEnumerable<T>> Find<T>(string path, System.Linq.Expressions.Expression<Func<T, bool>> match) where T : new()
        {
            return LiteDbAction<T, IEnumerable<T>>(path, (database, collection) =>
            {
                return collection.Find(match);
            });
        }
        /// <summary>
        /// Find all items in a collection
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <returns>All items matching the criteria</returns>
        public static CallResult<IEnumerable<T>> Find<T>(string path) where T : new()
        {
            return LiteDbAction<T, IEnumerable<T>>(path, (database, collection) =>
            {
                return collection.FindAll();
            });
        }

        //1.2.2017 - don't think I need these, when I can aggregate the code in one method (Save)
        //public static CallResult<int> Insert<T>(string path, T item) where T : ILiteDbItem, new() //id will be auto-increment generated
        //{
        //    return LiteDbAction<T, int>(path, (database, transaction, collection) =>
        //    {
        //        var id = collection.Insert(item);
        //        transaction.Commit();
        //        item._id = id;
        //        return id;
        //    });
        //}
        //public static CallResult<int> Update<T>(string path, T item) where T : ILiteDbItem, new()
        //{
        //    return LiteDbAction<T, int>(path, (database, transaction, collection) =>
        //    {
        //        var result = collection.Update(item);
        //        transaction.Commit();
        //        return result ? item._id : 0;
        //    });
        //}

        /// <summary>
        /// Save an item to a databse collection
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="item">The item to save</param>
        /// <returns>The new id of the saved item</returns>
        public static CallResult<int> Save<T>(string path, T item) where T : ILiteDbItem, new()
        {
            //no need for transaction here as a single operation is auto-transacted by LiteDb anyway
            return LiteDbAction<T, int>(path, (database, collection) =>
            {
                int result;
                if (item._id != 0)
                {
                    result = collection.Update(item) ? item._id : 0;
                }
                else
                {
                    result = collection.Insert(item);
                    item._id = result;
                }
                return result;
            });
        }
        /// <summary>
        /// Save items to a databse collection
        /// </summary>
        /// <typeparam name="T">The Type which determines the database collection name</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="items">The items to save</param>
        /// <returns>A list of the saved items. Each items' id field will have a new, non-zero value.</returns>
        public static CallResult<List<T>> Save<T>(string path, List<T> items) where T : class, ILiteDbItem, new()
        {
            return LiteDbAction<T, List<T>>(path, (database, transaction, collection) =>
            {
                foreach (var item in items)
                {
                    if (item._id != 0)
                    {
                        var id = collection.Insert(item);
                        item._id = id;
                    }
                    else
                    {
                        var result = collection.Update(item);
                        if (!result)
                            Log.e("Failed to update item");
                    }
                }
                transaction.Commit();
                return items;
            });
        }
        /// <summary>
        /// Deletes an item from the collection
        /// </summary>
        /// <typeparam name="T">The type of item; this is the name of the collection</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="id">The id of the item to delete</param>
        /// <returns>True if the item with the given Id was found and deleted, otherwise False</returns>
        public static CallResult<bool> Delete<T>(string path, int id) where T : new()
        {
            return LiteDbAction<T, bool>(path, (database, collection) =>
            {
                var result = collection.Delete(id);
                return result;
            });
        }
        /// <summary>
        /// Deletes an item from the collection
        /// </summary>
        /// <typeparam name="T">The type of item; this is the name of the collection</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="fieldName">The field to query</param>
        /// <param name="fieldValue">The value to search for</param>
        /// <returns>The number of items deleted</returns>
        public static CallResult<int> Delete<T>(string path, string fieldName, object fieldValue) where T : new()
        {
            return LiteDbAction<T, int>(path, (database, collection) =>
            {
                var query = LiteDB.Query.EQ(fieldName, new LiteDB.BsonValue(fieldValue));
                var result = collection.Delete(query);
                return result;
            });
        }
        /// <summary>
        /// Deletes an item from the collection
        /// </summary>
        /// <typeparam name="T">The type of item; this is the name of the collection</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <param name="match"></param>
        /// <returns>The number of items deleted</returns>
        public static CallResult<int> Delete<T>(string path, System.Linq.Expressions.Expression<Func<T, bool>> match) where T : new()
        {
            return LiteDbAction<T, int>(path, (database, collection) =>
            {
                var result = collection.Delete(match);
                return result;
            });
        }
        /// <summary>
        /// Removes the specified collection from the database
        /// </summary>
        /// <typeparam name="T">The type of item; this is the collection name</typeparam>
        /// <param name="path">The database path and filename</param>
        /// <returns>True if the collection was removed, otherwise False</returns>
        public static CallResult<bool> Drop<T>(string path) where T : new()
        {
            return LiteDbAction<bool>(path, database =>
            {
                return database.DropCollection(GetCollectionName<T>());
            });
        }
        /// <summary>
        /// The size, in bytes, of the actual database file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>The size, in bytes, of the actual database file</returns>
        public static CallResult<long> DatabaseFileSize(string path)
        {
            var result = new CallResult<long>();
            try
            {
                if (!System.IO.File.Exists(path))
                    return result;
                var fi = new System.IO.FileInfo(path);
                result.Result = fi.Length;
                result.Success = true;
                //using (var db = new LiteDB.LiteDatabase(path))// GetDatabase(path))
                //{
                //    var dbInfo = db.GetDatabaseInfo();
                //    return dbInfo.Get("fileLength").AsInt64;
                //}
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(e);
            }
            return result;
        }

        /// <summary>
        /// Store byte data to the database
        /// </summary>
        /// <param name="path">The database path and filename</param>
        /// <param name="id">The unique id for the data</param>
        /// <param name="data">The byte data to save</param>
        /// <returns>True if the data is saved, otherwise False</returns>
        public static CallResult Upload(this string path, string id, byte[] data)
        {
            var result = new CallResult();
            try
            {
                using (var db = new LiteDB.LiteDatabase(path))
                {
                    using (var ms = new System.IO.MemoryStream(data))
                    {
                        ms.Seek(0, System.IO.SeekOrigin.Begin);
                        db.FileStorage.Upload(id, id, ms);
                    }
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(e);
            }
            return result;
        }
        /// <summary>
        /// Store a file to the database
        /// </summary>
        /// <param name="path">The database path and filename</param>
        /// <param name="id">The unique id for the filename.  Can be the path and filename, or some other unique value.</param>
        /// <param name="filename">The path and name of the file to save</param>
        /// <returns>True if the data is saved, otherwise False</returns>
        public static CallResult Upload(this string path, string id, string filename)
        {
            var result = new CallResult();
            using (var db = new LiteDB.LiteDatabase(path))
            {
                try
                {
                    db.FileStorage.Upload(id, filename);
                    result.Success = true;
                }
                catch (Exception e)
                {
                    result.Exception = e;
                    Log.e(e);
                }
            }
            return result;
        }
        /// <summary>
        /// Retrieve byte data from the database
        /// </summary>
        /// <param name="path">The database path and filename</param>
        /// <param name="id">The unique id used to identify the data</param>
        /// <returns>The data if found, otherwise null</returns>
        public static CallResult<byte[]> Download(this string path, string id)
        {
            var result = new CallResult<byte[]>();
            try
            {
                using (var db = new LiteDB.LiteDatabase(path))
                {
                    var li = db.FileStorage.FindById(id);
                    using (var ms = new System.IO.MemoryStream())
                    {
                        ms.Seek(0, System.IO.SeekOrigin.Begin);
                        li.CopyTo(ms);
                        result.Result = ms.ToArray();
                        result.Success = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(e);
            }
            return result;
        }
        /// <summary>
        /// Retrieves a file from the database
        /// </summary>
        /// <param name="path">The database path and filename</param>
        /// <param name="id">The unique id used to identify the data</param>
        /// <param name="outputFilename">The path and filename to use when saving the file</param>
        /// <param name="overwrite">If true, any existing file will be overwritten</param>
        /// <returns>True if the file was retrieved and written to the file system.  Otherwise False.</returns>
        public static CallResult Download(this string path, string id, string outputFilename, bool overwrite = true)
        {
            var result = new CallResult();
            try
            {
                using (var db = new LiteDB.LiteDatabase(path))
                {
                    var li = db.FileStorage.FindById(id);
                    li.SaveAs(outputFilename, overwrite);
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(e);
            }
            return result;
        }

        //The methods below aren't really necessary becuse Download and Upload work with byte arrays.
#if !NETSTANDARD1_4 && !NETFX_CORE && !WINDOWS_UWP
        #region FileStorage Methods (Bitmap Storage)
        /// <summary>
        /// Save a Bitmap
        /// </summary>
        /// <param name="path">The database path and filename</param>
        /// <param name="url"></param>
        /// <param name="bitmap">The Bitmap to save</param>
        /// <returns></returns>
        public static CallResult<System.Drawing.Bitmap> SaveBitmap(this string path, string url, System.Drawing.Bitmap bitmap)
        {
            var result = new CallResult<System.Drawing.Bitmap>(false, bitmap);

            try
            {
                using (var db = new LiteDB.LiteDatabase(path))// GetDatabase(path))
                {
                    var ms = new System.IO.MemoryStream();
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Seek(0, System.IO.SeekOrigin.Begin);
                    db.FileStorage.Upload(url, url, ms);
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(e);
            }
            return result;
        }
        /// <summary>
        /// Retrieve a bitmap from the database and save it to the file system
        /// </summary>
        /// <param name="path">The databse path and filename</param>
        /// <param name="id">The unique id identifying the bitmap</param>
        public static CallResult DownloadAndSaveBitmap(this string path, string id)
        {
            var result = new CallResult();
            try
            {
                using (var db = new LiteDB.LiteDatabase(path))// GetDatabase(path))
                {
                    System.Net.WebRequest request = System.Net.WebRequest.Create(id);
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream responseStream = response.GetResponseStream();
                    //return new System.Drawing.Bitmap(responseStream);
                    db.FileStorage.Upload(id, id, responseStream);
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(e);
            }
            return result;
        }
        /// <summary>
        /// Retrieves a bitmap from the database and returns it as a Bitmap object
        /// </summary>
        /// <param name="path">The database path and filename</param>
        /// <param name="id">The unique id identifying the bitmap</param>
        /// <returns></returns>
        public static CallResult<System.Drawing.Bitmap> LoadBitmap(this string path, string id)
        {
            var result = new CallResult<System.Drawing.Bitmap>();
            try
            {
                using (var db = new LiteDB.LiteDatabase(path))// GetDatabase(path))
                {
                    System.IO.Stream stream = new System.IO.MemoryStream();
                    db.FileStorage.Download(id, stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    result.Result = new System.Drawing.Bitmap(stream);
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(e);
            }
            return result;
        }
        /// <summary>
        /// Deletes a Bitmap from the database
        /// </summary>
        /// <param name="path">The database path and filename</param>
        /// <param name="id">The unique id identifying the bitmap</param>
        public static CallResult DeleteBitmap(this string path, string id)
        {
            var result = new CallResult();
            try
            {
                using (var db = new LiteDB.LiteDatabase(path))// GetDatabase(path))
                {
                    db.FileStorage.Delete(id);
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                result.Exception = e;
                Log.e(e);
            }
            return result;
        }
        #endregion
#endif
    }
}
