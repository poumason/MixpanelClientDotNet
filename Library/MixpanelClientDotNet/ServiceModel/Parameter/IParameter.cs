using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixpanelDotNet.ServiceModel
{
    interface IParameter
    {
        string ToQueryString();
    }
}