using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixpanel.Net.Client.SDK.Utility
{
    public static class StringExtension
    {
        public static string ToBase64(this string content)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(content);
            string base64 = Convert.ToBase64String(data);
            return base64;
        }
    }
}