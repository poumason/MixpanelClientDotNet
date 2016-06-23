using Mixpanel.Net.Client.SDK;
using Mixpanel.Net.Client.WPF.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mixpanel.Net.Client.SDK.ServiceModel;
using System.Security.AccessControl;
using Mixpanel.Net.Client.SDK.Utility;
using Newtonsoft.Json;

namespace Mixpanel.Net.Client.WPF
{
    public class MixpanelClient : AbsMixpanelClient
    {
        public MixpanelClient(string token) : base(token)
        {
        }

        public override Task<bool> ImportEvents(List<EventData> eventList)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> IdentifyNetworkAvaiable()
        {
            return Task.FromResult(NetworkHelper.IsNetworkAvailable);
        }

        protected override async Task<bool> Save()
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

        private async Task<string> ReadFile()
        {
            string json = string.Empty;
            var tempFile = await GetMixpanelTempFile();
            if (tempFile != null)
            {
                using (StreamReader reader = new StreamReader(tempFile.Open(FileMode.OpenOrCreate)))
                {
                    json = reader.ReadToEnd();
                    reader.Dispose();
                }
            }
            tempFile = null;
            return json;
        }

        private async Task<bool> WriteFile(string data)
        {
            var file = await GetMixpanelTempFile();
            if (file == null || string.IsNullOrEmpty(data))
            {
                return false;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(file.OpenWrite()))
                {
                    writer.Write(data);
                    writer.Flush();
                    writer.Dispose();
                }
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception ex)
            {
                DebugLogger.Write(ex);
            }
            return true;
        }

        private async Task<FileInfo> GetMixpanelTempFile()
        {
            return await Task.Run(() =>
             {
                 string rootFolder = Directory.GetCurrentDirectory();
                 // create mixpanel folde
                 DirectoryInfo mixpanelFolder = Directory.CreateDirectory($"{rootFolder}\\{CommonDefine.MIXPANEL_FOLDER_NAME}");
                 FileInfo[] files = mixpanelFolder.GetFiles("*.mtf");
                 var tempFile = files.Where(x => x.Name == CommonDefine.MIXPANEL_TRACK_FILE_NAME).FirstOrDefault();
                 return tempFile;
             });
        }
    }
}