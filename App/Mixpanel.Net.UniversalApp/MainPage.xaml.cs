using MixpanelDotNet;
using MixpanelDotNet.ServiceModel;
using MixpanelDotNet.UWP;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Mixpanel.Net.UniversalApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            Unloaded += MainPage_Unloaded;
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            timer?.Stop();
        }

        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(15);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            timer.Stop();
            App.MixpanelClient.TrackEvent(new EventData()
            {
                Name = "test_uwp"
            });
            timer.Start();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            StartTimer();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await App.MixpanelClient.SaveMixpanelTempData();
        }
    }
}