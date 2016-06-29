using MixpanelDotNet.ServiceModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using MixpanelDotNet.Utility;

namespace MixpanelDotNet
{
    public abstract class AbsMixpanelClient : IMixpanelClient
    {
        protected List<EventData> TempEventCollection { get; private set; }

        protected Timer looptimer { get; private set; }

        protected string Token { get; private set; }

        private HttpService httpService;

        public bool IsOpenIP { get; set; }

        protected INetworkHelper NetworkTool { get; set; }

        public AbsMixpanelClient(string token)
        {
            Token = token;
            httpService = new HttpService();
            TempEventCollection = new List<EventData>();
            IsOpenIP = false;
        }

        /// <summary>
        /// Track event.
        /// </summary>
        public async Task TrackEvent(EventData eventitem)
        {
            eventitem.SetToken(Token);
            string queryString = $"data={eventitem.ToBase64()}";
            if (IsOpenIP)
            {
                queryString += "&i=1";
            }
            if (NetworkTool.IsNetworkAvailable)
            {
                var task = await httpService.SendRequest(CommonDefine.TRACK_URI, queryString);
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

        /// <summary>
        /// Save cache Track events.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveMixpanelTempData()
        {
            EventDataCollection collectionItem = null;
            var tempJson = await ReadFile();
            if (!string.IsNullOrEmpty(tempJson))
            {
                collectionItem = JsonConvert.DeserializeObject<EventDataCollection>(tempJson);
            }
            if (collectionItem == null)
            {
                collectionItem = new EventDataCollection();
            }
            collectionItem.Collection.AddRange(TempEventCollection);
            TempEventCollection.Clear();
            string newJson = JsonConvert.SerializeObject(collectionItem);
            await WriteFile(newJson);
            return true;
        }

        protected abstract Task<string> ReadFile();

        protected abstract Task<bool> WriteFile(string data);

        protected abstract Task<object> GetMixpanelTempFile();

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
    }
}