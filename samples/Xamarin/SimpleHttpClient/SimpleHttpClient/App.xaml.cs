using SimpleHttpClient.Services;
using SimpleHttpClient.Views;
using System;
using System.Diagnostics;
using System.Threading;
using ThunderDesign.Net.HttpClientService;
using ThunderDesign.Net.HttpClientService.DataObjects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleHttpClient
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
