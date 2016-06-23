using MixpanelDotNet.Utility;
using System.Net.NetworkInformation;
using System;
using System.Runtime.InteropServices;

namespace MixpanelClientDotNet.WPF.Utility
{
    public class NetworkHelper : INetworkHelper, IDisposable
    {
        [DllImport("wininet")]
        public static extern bool InternetGetConnectedState(
            ref uint lpdwFlags,
            uint dwReserved
            );

        public bool IsNetworkAvailable
        {
            get
            {
                uint flags = 0x0;
                return InternetGetConnectedState(ref flags, 0);
            }
        }

        public NetworkHelper()
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}