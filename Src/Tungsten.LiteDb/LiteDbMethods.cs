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

        public static CallResult<U> LiteDbAction<T, U>(string path, Func<LiteDatabase, U> f, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) where T : new()
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
        public static CallResult<U> LiteDbAction<T, U>(string path, Func<LiteDatabase, LiteCollection<T>, U> f, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) where T : new()
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
        public static CallResult<U> LiteDbAction<T, U>(string path, Func<LiteDatabase, LiteTransaction, LiteCollection<T>, U> f, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) where T : new()
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

        public static CallResult<bool> EnsureIndex<TItemType>(LiteDatabase db, string fieldName) where TItemType : new()
        {
            var result = new CallResult<bool>();
            result.Result = GetCollection<TItemType>(db)?.EnsureIndex(fieldName, true) ?? false;
            return result;
        }
        public static CallResult<bool> EnsureIndex<TItemType>(LiteCollection<TItemType> collection, string fieldName) where TItemType : new()
        {
            var result = new CallResult<bool>();
            result.Result = collection?.EnsureIndex(fieldName, true) ?? false;
            return result;
        }
        public static CallResult<bool> EnsureIndex<TItemType>(string path, string fieldName) where TItemType : new()
        {
            return LiteDbAction<TItemType, bool>(path, (database, collection) =>
            {
                var result = collection.EnsureIndex(fieldName, true);
                return result;
            });
        }

        public static CallResult<bool> Exists<T>(string path, string fieldName, object fieldValue) where T : new()
        {
            return LiteDbAction<T, bool>(path, (database, collection) =>
            {
                var query = LiteDB.Query.EQ(fieldName, new LiteDB.BsonValue(fieldValue));
                return collection.Exists(query);
            });
        }
        public static CallResult<bool> Exists<T>(string path, Func<T, bool> match) where T : new()
        {
            return LiteDbAction<T, bool>(path, (database, collection) =>
            {
                return collection.Exists(f => match.Invoke(f));
            });
        }

        public static CallResult<T> FindOne<T>(string path, string fieldName, object fieldValue) where T : new()
        {
            return LiteDbAction<T, T>(path, (database, collection) =>
            {
                var query = LiteDB.Query.EQ(fieldName, new LiteDB.BsonValue(fieldValue));
                return collection.FindOne(query);
            });
        }
        public static CallResult<T> FindOne<T>(string path, Func<T, bool> match) where T : new()
        {
            return LiteDbAction<T, T>(path, (database, collection) =>
            {
                return collection.FindOne(f => match.Invoke(f));
            });
        }

        public static CallResult<IEnumerable<T>> Find<T>(string path, string fieldName, object fieldValue) where T : new()
        {
            return LiteDbAction<T, IEnumerable<T>>(path, (database, collection) =>
            {
                var query = LiteDB.Query.EQ(fieldName, new LiteDB.BsonValue(fieldValue));
                return collection.Find(query);
            });
        }
        public static CallResult<IEnumerable<T>> Find<T>(string path, Func<T, bool> match) where T : new()
        {
            return LiteDbAction<T, IEnumerable<T>>(path, (database, collection) =>
            {
                return collection.Find(f => match.Invoke(f));
            });
        }
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

        public static CallResult<bool> Delete<T>(string path, int id) where T : new()
        {
            return LiteDbAction<T, bool>(path, (database, collection) =>
            {
                var result = collection.Delete(id);
                return result;
            });
        }
        public static CallResult<int> Delete<T>(string path, string fieldName, object fieldValue) where T : new()
        {
            return LiteDbAction<T, int>(path, (database, collection) =>
            {
                var query = LiteDB.Query.EQ(fieldName, new LiteDB.BsonValue(fieldValue));
                var result = collection.Delete(query);
                return result;
            });
        }
        public static CallResult<int> Delete<T>(string path, Func<T, bool> match) where T : new()
        {
            return LiteDbAction<T, int>(path, (database, collection) =>
            {
                var result = collection.Delete(f => match.Invoke(f));
                return result;
            });
        }

        public static CallResult<bool> Drop<T>(string path) where T : new()
        {
            return LiteDbAction<T, bool>(path, database =>
            {
                return database.DropCollection(GetCollectionName<T>());
            });
        }
        public static long FileSize(string path)
        {
            try
            {
                if (!System.IO.File.Exists(path))
                    return 0;
                var fi = new System.IO.FileInfo(path);
                return fi.Length;
                //using (var db = new LiteDB.LiteDatabase(path))// GetDatabase(path))
                //{
                //    var dbInfo = db.GetDatabaseInfo();
                //    return dbInfo.Get("fileLength").AsInt64;
                //}
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return 0;
        }

        #region FileStorage Methods (Bitmap Storage)
        public static System.Drawing.Bitmap SaveBitmap(this string path, string url, System.Drawing.Bitmap bitmap)
        {
            using (var db = new LiteDB.LiteDatabase(path))// GetDatabase(path))
            {
                try
                {
                    var ms = new System.IO.MemoryStream();
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Seek(0, System.IO.SeekOrigin.Begin);
#if LOCAL_LITEDB
                    db.FileStorage.Upload(url, url, ms);
#else
                    db.FileStorage.Upload(url, ms);
#endif
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
            }
            return bitmap;
        }
        public static void DownloadAndSaveBitmap(this string path, string url)
        {
            using (var db = new LiteDB.LiteDatabase(path))// GetDatabase(path))
            {
                try
                {
                    System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream responseStream = response.GetResponseStream();
                    //return new System.Drawing.Bitmap(responseStream);
#if LOCAL_LITEDB
                    db.FileStorage.Upload(url, url, responseStream);
#else
                    db.FileStorage.Upload(url, responseStream);
#endif
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
            }
        }
        public static System.Drawing.Bitmap LoadBitmap(this string path, string url)
        {
            using (var db = new LiteDB.LiteDatabase(path))// GetDatabase(path))
            {
                try
                {
                    System.IO.Stream stream = new System.IO.MemoryStream();
                    db.FileStorage.Download(url, stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return new System.Drawing.Bitmap(stream);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
                return null;
            }
        }
        public static void DeleteBitmap(this string path, string url)
        {
            using (var db = new LiteDB.LiteDatabase(path))// GetDatabase(path))
            {
                try
                {
                    db.FileStorage.Delete(url);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
            }
        }
        #endregion
    }
}
