using Mixpanel.Net.Client.SDK;
using System;
using System.Threading.Tasks;
using Mixpanel.Net.Client.SDK.ServiceModel;
using Mixpanel.Net.Client.Universal.Utility;
using Windows.UI.Xaml;
using Windows.Storage;
using Mixpanel.Net.Client.Universal.IO;
using Newtonsoft.Json;
using Mixpanel.Net.Client.SDK.Utility;

namespace Mixpanel.Net.Client.Universal
{
    public class MixpanelClient : AbsMixpanelClient
    {
        private StorageFile tempFile { get; set; }

        public MixpanelClient(string token) : base(token)
        {
            Application.Current.Suspending += Current_Suspending;
        }

        protected override Task<bool> IdentifyNetworkAvaiable()
        {
            return Task.FromResult(NetworkHelper.IsNetworkAvailable);
        }

        private async void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            await SaveMixpanelTempData();
            deferral.Complete();
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

        #region Read / Write Mixpanel temp file

        private async Task<string> ReadFile()
        {
            string content = string.Empty;
            var tempFile = await GetMixpanelTempFile();
            try
            {
                if (tempFile != null)
                {
                    content = await FileIO.ReadTextAsync(tempFile);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            return content;
        }

        private async Task<bool> WriteFile(string data)
        {
            var file = await GetMixpanelTempFile();
            if (file == null)
            {
                return false;
            }

            try
            {
                await FileIO.WriteTextAsync(file, data);
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

        private async Task<StorageFile> GetMixpanelTempFile()
        {
            if (tempFile == null)
            {
                StorageFolder folder = await StorageHelper.GetFolder(CommonDefine.MIXPANEL_FOLDER_NAME);
                tempFile = await folder.CreateFileAsync(CommonDefine.MIXPANEL_TRACK_FILE_NAME, CreationCollisionOption.OpenIfExists);
            }
            return tempFile;
        }
        #endregion
    }
}