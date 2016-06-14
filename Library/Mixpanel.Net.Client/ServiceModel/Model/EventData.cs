using Mixpanel.Net.Client.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.ServiceModel
{
    public class EventData
    {
        [JsonProperty("event")]
        public string Name { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, object> Properties { get; private set; }

        public EventData()
        {
            Properties = new Dictionary<string, object>();
            Properties.Add("time", DateTime.UtcNow.Ticks);
        }

        public object GetProperty(string key)
        {
            if (Properties.ContainsKey(key))
            {
                return Properties[key];
            }
            else
            {
                return null;
            }
        }

        public void SetProperty(string key, object value)
        {
            if (Properties.ContainsKey(key))
            {
                Properties[key] = value;
            }
            else
            {
                Properties.Add(key, value);
            }
        }

        public void SetToken(string token)
        {
            SetProperty("token", token);
        }

        public void SetDistinctId(string distictId)
        {
            SetProperty("distinct_id", distictId);
        }

        public void SetIp()
        {
            SetProperty("ip", NetworkHelper.GetIpAddress());
        }

        public void ClearProperties()
        {
            Properties.Clear();
        }

        public string ToBase64()
        {
            string json = JsonConvert.SerializeObject(this);
            byte[] data = UTF8Encoding.UTF8.GetBytes(json);
            string base64 = Convert.ToBase64String(data);
            return base64;
        }
    }
}