using Mixpanel.Net.Client;
using Mixpanel.Net.Client.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Mixpanel.Net.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MixpanelClient client;

        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;

            // get mixpanel project token
            client = new MixpanelClient("");
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartTimer();
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
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

        private async void Timer_Tick(object sender, object e)
        {
            timer.Stop();
            client.TrackEvent(new EventData()
            {
                Name = "test_wpf"
            });
            timer.Start();
        }
    }
}