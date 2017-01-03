using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.Storage;
using System.Collections.Generic;
using MixpanelDotNet.UWP.Utility;
using MixpanelDotNet.ServiceModel;
using MixpanelDotNet.UWP.IO;

namespace MixpanelDotNet.UWP
{
    public class MixpanelClient : AbsMixpanelClient
    {
        private StorageFile TempFile { get; set; }

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
            string content = string.Empty;
            var file = await GetMixpanelTempFile();
            try
            {
                if (file != null)
                {
                    StorageFile target = file as StorageFile;
                    content = await FileIO.ReadTextAsync(target);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (Exception ex)
            {
                DebugLogger.Write(ex);
            }
            return content;
        }

        protected override async Task<bool> WriteFile(string data)
        {
            bool result = false;
            var file = await GetMixpanelTempFile();
            if (file == null)
            {
                return result;
            }
            try
            {
                StorageFile target = file as StorageFile;
                await FileIO.WriteTextAsync(target, data);
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
                StorageFolder folder = await StorageHelper.GetFolder(CommonDefine.MIXPANEL_FOLDER_NAME);
                TempFile = await folder.CreateFileAsync(CommonDefine.MIXPANEL_TRACK_FILE_NAME, CreationCollisionOption.OpenIfExists);
            }
            return TempFile;
        }
    }
}