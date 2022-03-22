using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThunderDesign.Net.HttpClientService.Interfaces;
using Xamarin.Android.Net;

namespace SimpleHttpClient.Droid
{
    public class CustomAndroidClientHandler : AndroidClientHandler, ICustomHttpMessageHandler
    {
    }
}