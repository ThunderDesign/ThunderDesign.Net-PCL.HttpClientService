using System.Net;

namespace ThunderDesign.Net.HttpClientService.EventArgs
{
    public class CookieContainerChangedEventArgs : System.EventArgs
    {
        #region constructor
        public CookieContainerChangedEventArgs(CookieCollection cookieCollection)
        {
            CookieCollection = cookieCollection;
        }
        #endregion

        #region properties
        public CookieCollection CookieCollection { get; private set; }
        #endregion
    }
}
