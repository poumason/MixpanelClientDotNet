using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.ServiceModel
{
    interface IParameter
    {
        string ToQueryString();
    }
}