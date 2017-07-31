using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using W.Logging;

namespace W.IO
{
    [Newtonsoft.Json.JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    public partial class LocalStorage
    {
        [System.ComponentModel.Description("Use this class as a base class for your local storage.")]
        public class LocalStorageFile
        {
            private bool _canSave = true;
            private bool _shouldSave = false;

            protected string Filename { get; set; }

            public bool Load()
            {
                if (!string.IsNullOrEmpty(Filename))
                    return Load(Filename);
                return false;
            }
            public bool Load(params Newtonsoft.Json.JsonConverter[] converters)
            {
                if (!string.IsNullOrEmpty(Filename))
                    return Load(Filename, converters);
                return false;
            }
            public bool Load(string filename, params Newtonsoft.Json.JsonConverter[] converters)
            {
                return Load(this, filename, converters);
                //if (FileExists(filename))
                //{
                //    var contents = ReadText(filename);
                //    try
                //    {
                //        Newtonsoft.Json.JsonConvert.PopulateObject(contents, this, new Newtonsoft.Json.JsonSerializerSettings() { Converters = converters });
                //        var lsf = this as LocalStorageFile;
                //        lsf?.OnLoadComplete();
                //        return true;
                //    }
                //    catch(Exception)
                //    {
                //        throw;
                //        //Diagnostics.Log.Output(Diagnostics.Log.ELevel.Error, e, true);
                //    }
                //}
                //return false;
            }
            public bool Save()
            {
                if (_canSave)
                {
                    if (!string.IsNullOrEmpty(Filename))
                        Save(Filename);
                    else
                        return false;
                }
                _shouldSave = false;
                return true;
            }
            public bool Save(params Newtonsoft.Json.JsonConverter[] converters)
            {
                if (_canSave)
                {
                    if (!string.IsNullOrEmpty(Filename))
                        Save(Filename, converters);
                    else
                        return false;
                }
                _shouldSave = false;
                return true;
            }
            public bool Save(string filename, params Newtonsoft.Json.JsonConverter[] converters)
            {
                if (_canSave)
                    return Save(filename, this, converters);
                _shouldSave = true;
                return true;
            }
            public void BeginUpdate()
            {
                _canSave = false;
                _shouldSave = false;
            }
            public void EndUpdate(params Newtonsoft.Json.JsonConverter[] converters)
            {
                _canSave = true;
                if (_shouldSave)
                    Save(converters);
            }
            public void EndUpdate(bool forceSave, params Newtonsoft.Json.JsonConverter[] converters)
            {
                _canSave = true;
                if (_shouldSave || forceSave)
                    Save(converters);
            }

            public static T Load<T>(string filename) //where ControlType : LocalStorageFile
            {
                if (FileExists(filename))
                {
                    var contents = ReadText(filename);
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(contents);
                    var lsf = result as LocalStorageFile;
                    lsf?.OnLoadComplete();
                    return result;
                }
                return default(T);
            }
            public static T Load<T>(string filename, params Newtonsoft.Json.JsonConverter[] converters) //where ControlType : LocalStorageFile
            {
                if (FileExists(filename))
                {
                    var contents = ReadText(filename);
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(contents, converters);
                    var lsf = result as LocalStorageFile;
                    lsf?.OnLoadComplete();
                    return result;
                }
                return default(T);
            }
            public static bool Load<T>(T @this, string filename, params Newtonsoft.Json.JsonConverter[] converters) //where ControlType : LocalStorageFile
            {
                if (FileExists(filename))
                {
                    var contents = ReadText(filename);
                    Newtonsoft.Json.JsonConvert.PopulateObject(contents, @this);
                    var lsf = @this as LocalStorageFile;
                    lsf?.OnLoadComplete();
                    return true;
                }
                return false;
            }
            public static bool Save<T>(string filename, T obj) //where ControlType : LocalStorageFile
            {
                try
                {
                    string contents = string.Empty;
                    //Diagnostics.Log.Output(Diagnostics.Log.ELevel.Information, "", "Saving {0}", filename);
                    contents = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    DeleteFile(filename);
                    WriteText(filename, contents);
                    var lsf = obj as LocalStorageFile;
                    if (lsf != null)
                        lsf.OnSaveComplete();
                    return true;
                }
                catch (Exception)
                {
                    throw;
                    //Diagnostics.Log.Output(Diagnostics.Log.ELevel.Error, e, true);
                    //return false;
                }
            }
            public static bool Save<T>(string filename, T obj, params Newtonsoft.Json.JsonConverter[] converters) //where ControlType : LocalStorageFile
            {
                try
                {
                    string contents = string.Empty;
                    //Diagnostics.Log.Output(Diagnostics.Log.ELevel.Information, "", "Saving {0}", filename);
                    if (converters != null)
                        contents = Newtonsoft.Json.JsonConvert.SerializeObject(obj, converters);
                    else
                        contents = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    DeleteFile(filename);
                    WriteText(filename, contents);
                    var lsf = obj as LocalStorageFile;
                    if (lsf != null)
                        lsf.OnSaveComplete();
                    return true;
                }
                catch (Exception)
                {
                    throw;
                    //Diagnostics.Log.Output(Diagnostics.Log.ELevel.Error, e, true);
                    //return false;
                }
            }

            public LocalStorageFile()
            {
                Filename = GetFilenameByAttribute(this);
            }
            public LocalStorageFile(string filename = "")
            {
                if (string.IsNullOrEmpty(filename))
                    filename = GetFilenameByAttribute(this);
                Filename = filename;
            }
            public LocalStorageFile(System.Environment.SpecialFolder folder, string filename = "")
            {
                if (string.IsNullOrEmpty(filename))
                    filename = GetFilenameByAttribute(this);
                Filename = System.IO.Path.Combine(System.Environment.GetFolderPath(folder), filename);
            }
            //protected string GetFilenameByAttribute()
            //{
            //    var result = this.GetAttributeProperty<LocalStorageFileAttribute, string>("Filename");
            //    if (string.IsNullOrEmpty(result))
            //        result = System.Windows.Forms.Application.ProductName + ".settings";
            //    Filename = result;
            //    return Filename;
            //}

            protected virtual void OnLoadComplete()
            {
                Log.i("Load Complete");
            }
            protected virtual void OnSaveComplete()
            {
                Log.i("Save Complete");
            }
        }

        public static void DeleteFile(string filename)
        {
            if (FileExists(filename))
                System.IO.File.Delete(filename);
        }
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }
        public static TimeSpan FileAge(string filename)
        {
            var createDate = System.IO.File.GetLastWriteTime(filename);
            return DateTime.Now.Subtract(createDate);
        }
        public static string ReadText(string filename)
        {
            return System.IO.File.ReadAllText(filename);
        }
        public static void WriteText(string filename, string contents)
        {
            System.IO.File.WriteAllText(filename, contents);
        }
        public static string GetFilenameByAttribute(object @this)
        {
            var result = @this.GetAttributeProperty<LocalStorageFileAttribute, string>("Filename");
            if (string.IsNullOrEmpty(result))
                result = System.Windows.Forms.Application.ProductName + ".settings";
            return result;
        }
    }
}
