using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MixpanelDotNet;
using MixpanelClientDotNet.WPF.Utility;
using MixpanelDotNet.ServiceModel;

namespace MixpanelClientDotNet.WPF
{
    public class MixpanelClient : AbsMixpanelClient
    {
        private FileInfo TempFile { get; set; }

        public MixpanelClient(string token) : base(token)
        {
            NetworkTool = new NetworkHelper();
        }

        public override Task<bool> ImportEvents(List<EventData> eventList)
        {
            throw new NotImplementedException();
        }

        protected override async Task<string> ReadFile()
        {
            string json = string.Empty;
            var tempFile = await GetMixpanelTempFile();
            if (tempFile != null)
            {
                FileInfo target = tempFile as FileInfo;
                using (StreamReader reader = new StreamReader(target.Open(FileMode.OpenOrCreate)))
                {
                    json = reader.ReadToEnd();
                    reader.Dispose();
                }
            }
            tempFile = null;
            return json;
        }

        protected override async Task<bool> WriteFile(string data)
        {
            bool result = false;
            var file = await GetMixpanelTempFile();
            if (file == null || string.IsNullOrEmpty(data))
            {
                return result;
            }

            try
            {
                FileInfo target = file as FileInfo;
                using (StreamWriter writer = new StreamWriter(target.OpenWrite()))
                {
                    writer.Write(data);
                    writer.Flush();
                    writer.Dispose();
                }
                result = true;
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (Exception ex)
            {
                DebugLogger.Write(ex);
            }
            return result;
        }

        protected override async Task<object> GetMixpanelTempFile()
        {
            if (TempFile == null)
            {
                TempFile = await Task.Run(() =>
               {
                   string rootFolder = Directory.GetCurrentDirectory();
                   // create mixpanel folde
                   DirectoryInfo mixpanelFolder = Directory.CreateDirectory($"{rootFolder}\\{CommonDefine.MIXPANEL_FOLDER_NAME}");
                   FileInfo[] files = mixpanelFolder.GetFiles("*.mtf");
                   var file = files.Where(x => x.Name == CommonDefine.MIXPANEL_TRACK_FILE_NAME).FirstOrDefault();
                   if (file == null)
                   {
                       string path = $"{mixpanelFolder.FullName}\\{CommonDefine.MIXPANEL_TRACK_FILE_NAME}";
                       var stream = File.Open(path, FileMode.OpenOrCreate);
                       stream.Dispose();
                       stream = null;
                   }
                   return file;
               });
            }
            return TempFile;
        }
    }
}