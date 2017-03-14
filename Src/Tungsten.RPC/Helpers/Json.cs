using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.Logging;

namespace W.RPC
{
    internal class Json
    {
        public static string ToJson(object obj)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return string.Empty;
        }
        public static T FromJson<T>(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return default(T);
        }
        public static object Populate(string json, object target)
        {
            try
            {
                Newtonsoft.Json.JsonConvert.PopulateObject(json, target);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return target;
        }
        public static T Populate<T>(string json, T target)
        {
            try
            {
                Newtonsoft.Json.JsonConvert.PopulateObject(json, target);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return target;
        }

        public static T PopulateFrom<T>(object source, T target)
        {
            var json = ToJson(source); //copy values via json serialization (slow, but fast enough? - it'll copy lists fine)
            Populate(json, target);
            return target;
        }
    }
}
