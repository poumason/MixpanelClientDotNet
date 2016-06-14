using Mixpanel.Net.Client.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client
{
    public class MixpanelClient
    {
        public string Token { get; set; }

        const string trackUri = "https://api.mixpanel.com/track";

        private HttpService httpService;

        public MixpanelClient()
        {
            httpService = new HttpService();
        }

        public MixpanelClient(string token)
        {
            Token = token;
            httpService = new HttpService();
        }

        public void TrackEvent(EventData eventitem)
        {
            eventitem.SetToken(Token);
            var task = httpService.SendRequest(trackUri, $"data={eventitem.ToBase64()}");
        }
    }
}