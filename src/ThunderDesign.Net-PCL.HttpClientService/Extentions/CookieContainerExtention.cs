using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using ThunderDesign.Net.HttpClientService.EventArgs;
using ThunderDesign.Net.HttpClientService.EventHandlers;

namespace ThunderDesign.Net.HttpClientService.Extentions
{
    public static class CookieContainerExtention
    {
        #region methods
        static public void SetCookies(this CookieContainer cookieContainer, HttpHeaders httpHeaders, string defaultDomain, CookieContainerChangedEventHandler handler = null)
        {
            foreach (var header in httpHeaders)
            {
                if (!header.Key.Equals("set-cookie", StringComparison.OrdinalIgnoreCase))
                    continue;

                foreach (var value in header.Value)
                {
                    var cookieCollection = ParseCookieString(value, defaultDomain);

                    cookieContainer.Add(cookieCollection);
                    handler?.Invoke(cookieContainer, new CookieContainerChangedEventArgs(cookieCollection));
                }
            }
        }

        static private CookieCollection ParseCookieString(string cookieString, string defaultDomain)
        {
            bool secure = false;
            bool httpOnly = false;

            string domainFromCookie = String.Empty;
            string path = String.Empty;
            string expiresString = String.Empty;

            Dictionary<string, string> cookiesValues = new Dictionary<string, string>();

            var cookieValuePairsStrings = cookieString.Split(';');
            foreach (string cookieValuePairString in cookieValuePairsStrings)
            {
                var pairArr = cookieValuePairString.Split('=');
                int pairArrLength = pairArr.Length;
                for (int i = 0; i < pairArrLength; i++)
                {
                    pairArr[i] = pairArr[i].Trim();
                }
                string propertyName = pairArr[0];
                if (pairArrLength == 1)
                {
                    if (string.IsNullOrEmpty(propertyName))
                        continue;
                    if (propertyName.Equals("httponly", StringComparison.OrdinalIgnoreCase))
                        httpOnly = true;
                    else if (propertyName.Equals("secure", StringComparison.OrdinalIgnoreCase))
                        secure = true;
                    else
                        throw new InvalidOperationException(string.Format("Unknown cookie property \"{0}\". All cookie is \"{1}\"", propertyName, cookieString));
                    continue;
                }

                string propertyValue = pairArr[1];
                if (propertyName.Equals("expires", StringComparison.OrdinalIgnoreCase))
                    expiresString = propertyValue;
                else if (propertyName.Equals("domain", StringComparison.OrdinalIgnoreCase))
                    domainFromCookie = propertyValue;
                else if (propertyName.Equals("path", StringComparison.OrdinalIgnoreCase))
                    path = propertyValue;
                else
                    cookiesValues.Add(propertyName, propertyValue);
            }

            DateTime expiresDateTime;
            if (!string.IsNullOrEmpty(expiresString))
            {
                if (!DateTime.TryParse(expiresString, out expiresDateTime))
                    expiresDateTime = DateTime.MinValue;
                //expiresDateTime = DateTime.Parse(expiresString);
            }
            else
            {
                expiresDateTime = DateTime.MinValue;
            }
            if (string.IsNullOrEmpty(domainFromCookie))
            {
                domainFromCookie = defaultDomain;
            }

            CookieCollection cookieCollection = new CookieCollection();
            foreach (var pair in cookiesValues)
            {
                Cookie cookie = new Cookie(pair.Key, pair.Value, path, domainFromCookie)
                {
                    Secure = secure,
                    HttpOnly = httpOnly,
                    Expires = expiresDateTime
                };

                cookieCollection.Add(cookie);
            }
            return cookieCollection;
        }
        #endregion
    }
}
