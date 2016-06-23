using Mixpanel.Net.Client.SDK.ServiceModel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.SDK
{
    public abstract class AbsMixpanelClient : IMixpanelClient
    {
        protected List<EventData> TempEventCollection { get; private set; }

        protected Timer looptimer { get; private set; }

        protected string Token { get; private set; }

        private HttpService httpService;

        public AbsMixpanelClient(string token)
        {
            Token = token;
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
        public async void TrackEvent(EventData eventitem)
        {
            eventitem.SetToken(Token);
            string queryString = $"data={eventitem.ToBase64()}";
            if (await IdentifyNetworkAvaiable())
            {
                var task = httpService.SendRequest(CommonDefine.TRACK_URI, queryString);
            }
            else
            {
                TempEventCollection.Add(eventitem);
            }
        }

        /// <summary>
        /// Import more events. (over 5 days).
        /// </summary>
        public abstract Task<bool> ImportEvents(List<EventData> eventList);

        protected abstract Task<bool> Save();

        protected abstract Task<bool> IdentifyNetworkAvaiable();

#if DEBUG
        /// <summary>
        /// Save cache Track events.
        /// </summary>
        /// <returns></returns>
        public Task<bool> SaveMixpanelTempData()
        {
            return Save();
        }
#endif
    }
}