using Mixpanel.Net.Client.SDK.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.SDK
{
    public interface IMixpanelClient
    {
        void TrackEvent(EventData eventItem);

        Task<bool> ImportEvents(List<EventData> eventList);

        Task<bool> SaveMixpanelTempData();
    }
}