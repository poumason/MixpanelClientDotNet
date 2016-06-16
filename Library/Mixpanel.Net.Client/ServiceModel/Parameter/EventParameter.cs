using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.SDK.ServiceModel
{
    public class EventParameter: IParameter
    {

        public string ToQueryString()
        {
            return string.Empty;
        }
    }
}