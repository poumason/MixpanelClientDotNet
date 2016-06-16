using Mixpanel.Net.Client.SDK.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.SDK
{
    public class MixpanelClient
    {
        private string Token { get; set; }

        const string trackUri = "https://api.mixpanel.com/track";

        private HttpService httpService;

        public MixpanelClient(string token)
        {
            Token = token;
            httpService = new HttpService();
        }

        /// <summary>
        /// Track event.
        /// </summary>
        /// <param name="eventitem">EventData</param>
        public void TrackEvent(EventData eventitem)
        {
            eventitem.SetToken(Token);
            var task = httpService.SendRequest(trackUri, $"data={eventitem.ToBase64()}");
        }
    }
}