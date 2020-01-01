using System;
using W;

namespace W.Net.Discovery
{
    public class DiscoveryInformation
    {
        public string ApplicationId { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string AsJson() { return AsJson(this); }
        public static string AsJson<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj).AsBase64();
        }
        //public DiscoveryInformation FromJson(string json)
        //{
        //    json = json.FromBase64();
        //    Newtonsoft.Json.JsonConvert.PopulateObject(json, this);
        //    return this;
        //}
        public static DiscoveryInformation FromJson(string json)
        {
            try
            {
                var result = new DiscoveryInformation();
                json = json.FromBase64();
                Newtonsoft.Json.JsonConvert.PopulateObject(json, result);
                return result;
            }
            catch
            {
                return null;
            }
        }
        public static DiscoveryInformation Create(string applicationId, string data = "")
        {
            var result = new DiscoveryInformation();
            result.ApplicationId = applicationId;
            result.Data = data;
            return result;
        }
    }
}
