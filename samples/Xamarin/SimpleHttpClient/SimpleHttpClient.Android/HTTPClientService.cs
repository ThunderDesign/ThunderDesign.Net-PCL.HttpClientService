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
using ThunderDesign.Net.HttpClientService;

namespace SimpleHttpClient.Droid
{
    public class HTTPClientService
    {
        public static void Init()
        {
            //{.}{.} overlook self singed certs
            var androidClientHandler = new CustomAndroidClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                //CookieContainer = new System.Net.CookieContainer(),
                UseCookies = true,
                AllowAutoRedirect = true,
            };
            HttpClientService.DefaultHttpClientHandler = androidClientHandler;
        }
    }
}