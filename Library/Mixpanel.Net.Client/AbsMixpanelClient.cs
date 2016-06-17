using Mixpanel.Net.Client.SDK.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.SDK
{
    public abstract class AbsMixpanelClient : IMixpanelClient
    {
        protected string apiKey;

        protected List<EventData> TempEventCollection { get; set; }

        protected Timer looptimer;

        protected string Token { get; set; }

        private HttpService httpService;

        public AbsMixpanelClient(string token)
        {
            apiKey = token;
            httpService = new HttpService();
            TempEventCollection = new List<EventData>();
        }

        #region Timer
        protected void StartTimer()
        {
            if (looptimer == null)
            {
                looptimer = new Timer(TimerInterval_Callback, null, Timeout.Infinite, CommonDefine.POSITION_TIMER_INTERVAL);
            }
            looptimer.Change(0, CommonDefine.POSITION_TIMER_INTERVAL);
        }

        protected void StopTimer()
        {
            if (looptimer != null)
            {
                looptimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        private void TimerInterval_Callback(object sender)
        {

        }
        #endregion

        /// <summary>
        /// Track event.
        /// </summary>
        /// <param name="eventitem">EventData</param>
        public async void TrackEvent(EventData eventitem)
        {
            eventitem.SetToken(apiKey);
            string queryString = $"data={eventitem.ToBase64()}";
            if (await IdentifyNetworkAvaiable())
            {
                var task = httpService.SendRequest(CommonDefine.TRACK_URI, queryString);
            }
            else
            {
                TempEventCollection.Add(eventitem);
                //await WriteEventToTemp(eventitem);
            }
        }

        public Task<bool> ImportEvents(List<EventData> eventList)
        {
            throw new NotImplementedException();
        }

        protected virtual Task<bool> IdentifyNetworkAvaiable()
        {
            throw new NotImplementedException();
        }

        protected virtual Task WriteEventToTemp(EventData data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveMixpanelTempData()
        {
            return Save();
        }

        protected virtual Task<bool> Save()
        {
            throw new NotImplementedException();
        }
    }
}