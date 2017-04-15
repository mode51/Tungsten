using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace lib.IO
{
    public static class LocalStorageFileExtensions
    {
        public static bool Load<T>(this T @this, params Newtonsoft.Json.JsonConverter[] converters)
        {
            var filename = LocalStorage.GetFilenameByAttribute(@this);
            if (!string.IsNullOrEmpty(filename))
                return Load<T>(@this, filename, converters);
            return false;
        }
        public static bool Load<T>(this T @this, string filename, params Newtonsoft.Json.JsonConverter[] converters)
        {
            if (LocalStorage.FileExists(filename))
            {
                var contents = LocalStorage.ReadText(filename);
                try
                {
                    Newtonsoft.Json.JsonConvert.PopulateObject(contents, @this, new Newtonsoft.Json.JsonSerializerSettings() { Converters = converters });
                    return true;
                }
                catch (Exception)
                {
                    throw;
                    //Diagnostics.Log.Output(Diagnostics.Log.ELevel.Error, e, true);
                }
            }
            return false;
        }
        public static bool Save<T>(this T @this, params Newtonsoft.Json.JsonConverter[] converters)
        {
            var filename = LocalStorage.GetFilenameByAttribute(@this);
            if (!string.IsNullOrEmpty(filename))
                return Save(@this, filename, null, converters);
            return false;
        }
        public static bool Save<T>(this T @this, string filename, Action onSaveComplete = null, params Newtonsoft.Json.JsonConverter[] converters)
        {
            try
            {
                string contents;
                var settings = new JsonSerializerSettings() {CheckAdditionalContent = false};
                if (converters != null)
                    settings.Converters = converters;
                //if (converters != null)
                    contents = Newtonsoft.Json.JsonConvert.SerializeObject(@this, Formatting.Indented, settings);
                //else
                //    contents = Newtonsoft.Json.JsonConvert.SerializeObject(@this, Formatting.Indented, settings);
                LocalStorage.DeleteFile(filename);
                LocalStorage.WriteText(filename, contents);
                onSaveComplete?.Invoke();
                return true;
            }
            catch (Exception)
            {
                throw;
                //Diagnostics.Log.Output(Diagnostics.Log.ELevel.Error, e, true);
            }
            //return false;
        }
    }
}
