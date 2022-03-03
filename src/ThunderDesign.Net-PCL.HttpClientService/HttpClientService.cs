using System.Net.Http;
using ThunderDesign.Net.HttpClientService.Http;
using ThunderDesign.Net.Threading.Objects;

namespace ThunderDesign.Net.HttpClientService
{
    public class HttpClientService : ThreadObject
    {
        #region constructors
        public HttpClientService() : this(DefaultHttpClientHandler)
        {
        }

        public HttpClientService(HttpMessageHandler httpClientHandler)
        {
            HttpClient = httpClientHandler != null ?
                new HttpClientAutoRedirect(httpClientHandler) : new HttpClientAutoRedirect();
        }
        #endregion

        #region properties
        public static HttpMessageHandler DefaultHttpClientHandler
        {
            get { lock (_Locker) { return _DefaultHttpClientHandler; } }
            set { lock (_Locker) { _DefaultHttpClientHandler = value; } }
        }

        public static HttpClientService Instance
        {
            get
            {
                lock (_Locker)
                {
                    return _Instance ?? (_Instance = new HttpClientService());
                }
            }
        }

        public HttpClientAutoRedirect HttpClient
        {
            get;
            private set;
        }
        #endregion

        #region variables
        private static HttpClientService _Instance = null;
        private static HttpMessageHandler _DefaultHttpClientHandler = null;
        #endregion
    }
}
