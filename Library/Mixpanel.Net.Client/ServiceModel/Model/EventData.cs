using Mixpanel.Net.Client.SDK.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.SDK.ServiceModel
{
    /// <summary>
    /// <para>Tracking via HTTP</para>
    /// <para>https://mixpanel.com/help/reference/http</para>
    /// </summary>
    public class EventData
    {
        /// <summary>
        /// Event Name.
        /// </summary>
        [JsonProperty("event")]
        public string Name { get; set; }

        /// <summary>
        /// Event Properties.
        /// </summary>
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

        /// <summary>
        /// <para>token, is required field.</para>
        /// <para>The Mixpanel token associated with your project.</para>
        /// <para>You can find your Mixpanel token in the project settings dialog in the Mixpanel app. Events without a valid token will be ignored.</para>
        /// </summary>
        /// <param name="token"></param>
        internal void SetToken(string token)
        {
            SetProperty("token", token);
        }

        /// <summary>
        /// <para>distinct_id, is option field.</para>
        /// <para>The value of distinct_id will be treated as a string, and used to uniquely identify a user associated with your event.</para>
        /// <para>If you provide a distinct_id property with your events, you can track a given user through funnels and distinguish unique users for retention analyses.</para>
        /// <para>You should always send the same distinct_id when an event is triggered by the same user. </para>
        /// </summary>
        /// <param name="distictId"></param>
        public void SetDistinctId(string distictId)
        {
            SetProperty("distinct_id", distictId);
        }

        /// <summary>
        /// <para>ip, is option field.</para>
        /// <para>An IP address string (e.g. "127.0.0.1") associated with the event.</para>
        /// <para>This is used for adding geolocation data to events, and should only be required if you are making requests from your backend.</para>
        /// <para> If "ip" is absent (and ip=1 is not provided as a URL parameter), Mixpanel will ignore the IP address of the request. </para>
        /// </summary>
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