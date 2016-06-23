using MixpanelDotNet.ServiceModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MixpanelDotNet
{
    public interface IMixpanelClient
    {
        void TrackEvent(EventData eventItem);

        Task<bool> ImportEvents(List<EventData> eventList);

        Task<bool> SaveMixpanelTempData();
    }
}