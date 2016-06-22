using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Mixpanel.Net.Client.Universal.Utility
{
    public static class NetworkHelper
    {
        public static event EventHandler<bool> NetworkAvailabilityChange;

        private static bool isNetworkAvailable;
        public static bool IsNetworkAvailable
        {
            get
            {
                return isNetworkAvailable;
            }
            private set
            {
                if (isNetworkAvailable == value)
                {
                    return;
                }

                isNetworkAvailable = value;
                NetworkAvailabilityChange.Invoke(null, isNetworkAvailable);
            }
        }

        static NetworkHelper()
        {
            try
            {
                NetworkInformation.NetworkStatusChanged += NetworkInformationOnNetworkStatusChanged;
                CheckInternetAccess();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private static void CheckInternetAccess()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                IsNetworkAvailable = (connectionProfile != null &&
                                 connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private static void NetworkInformationOnNetworkStatusChanged(object sender)
        {
            CheckInternetAccess();
        }
        
        public static bool CheckInternetAvailable()
        {
            bool result = false;
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (connectionProfile != null)
            {
                switch (connectionProfile.GetNetworkConnectivityLevel())
                {
                    case NetworkConnectivityLevel.None:
                    case NetworkConnectivityLevel.LocalAccess:
                        break;
                    case NetworkConnectivityLevel.ConstrainedInternetAccess:
                        result = true;
                        break;
                    case NetworkConnectivityLevel.InternetAccess:
                        result = true;
                        break;
                }
            }

            return result;
        }
        
        public static IList<string> GetCurrentConnectionNames()
        {
            var internetConnectionProfiles = NetworkInformation.GetConnectionProfiles();

            if (internetConnectionProfiles != null)
            {
                foreach (var connectionProfile in internetConnectionProfiles)
                {
                    if (connectionProfile.IsWwanConnectionProfile)
                    {
                        try
                        {
                            var networkNames = connectionProfile.GetNetworkNames();
                            return networkNames.ToList();
                        }
                        catch (Exception)
                        {
                            return new List<string>();
                        }
                    }
                }
            }

            return null;
        }
        
        public static bool CheckConnectedName(string targetName)
        {
            var connectionNames = GetCurrentConnectionNames();
            if (connectionNames != null && connectionNames.Count > 0)
            {
                foreach (var name in connectionNames)
                {
                    if (name.Equals(targetName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        public static bool GetCurrentConnectionIsWwan()
        {
            return NetworkInformation.GetInternetConnectionProfile().IsWwanConnectionProfile;
        }
        
        public static async Task<bool> IsWiFiEnabled()
        {
            //Get the Internet connection profile
            string ssid = string.Empty;
            ConnectionProfileFilter filter = new ConnectionProfileFilter();
            filter.IsConnected = true;
            filter.IsWlanConnectionProfile = true;
            var result = await NetworkInformation.FindConnectionProfilesAsync(filter);
            if (result.Count > 0)
            {
                foreach (var profile in result)
                {
                    if (profile.IsWlanConnectionProfile)
                    {
                        ssid += profile.WlanConnectionProfileDetails.GetConnectedSsid();
                    }
                }
            }
            if (string.IsNullOrEmpty(ssid))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}