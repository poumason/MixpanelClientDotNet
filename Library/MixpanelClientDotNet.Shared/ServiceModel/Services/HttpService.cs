using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MixpanelDotNet.ServiceModel
{
    public class HttpService
    {
        private HttpClient httpClient;

        public HttpService()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> SendRequest(string uri, string query)
        {
            try
            {
                string url = $"{uri}?{query}";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await httpClient.SendAsync(request);
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                DebugLogger.Write(ex);

                return string.Empty;
            }
        }
    }
}