using Mixpanel.Net.Client.SDK;
using Mixpanel.Net.Client.WPF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.WPF
{
    public class MixpanelClient : AbsMixpanelClient
    {
        public MixpanelClient(string token) : base(token)
        {
        }

        protected override Task<bool> IdentifyNetworkAvaiable()
        {
            return Task.FromResult(NetworkHelper.IsNetworkAvailable);
        }

        protected override Task<bool> Save()
        {
            throw new NotImplementedException();
        }
    }
}