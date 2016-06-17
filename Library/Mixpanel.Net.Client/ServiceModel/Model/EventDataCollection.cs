using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.SDK.ServiceModel
{
    public class EventDataCollection
    {
        [JsonProperty("events")]
        public List<EventData> Collection { get; set; }

        public EventDataCollection()
        {
            Collection = new List<EventData>();
        }
    }
}