using System.Net;
using System.Net.Http;
using ThunderDesign.Net.Threading.Extentions;
using ThunderDesign.Net.Threading.Objects;

namespace ThunderDesign.Net.HttpClientService.DataObjects
{
    public class ResponseData : ThreadObject
    {
        #region constructors
        public ResponseData() : base() { }
        public ResponseData(HttpResponseMessage httpResponseMessage) : base() 
        {
            _IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            _ReasonPhrase = httpResponseMessage.ReasonPhrase;
            _StatusCode = httpResponseMessage.StatusCode;
        }
        #endregion

        #region properties
        public bool IsSuccessStatusCode
        {
            get { return this.GetProperty(ref _IsSuccessStatusCode, _Locker); }
            set { this.SetProperty(ref _IsSuccessStatusCode, value, _Locker); }
        }

        public string ReasonPhrase
        {
            get { return this.GetProperty(ref _ReasonPhrase, _Locker); }
            set { this.SetProperty(ref _ReasonPhrase, value, _Locker); }
        }
        
        public HttpStatusCode StatusCode
        {
            get { return this.GetProperty(ref _StatusCode, _Locker); }
            set { this.SetProperty(ref _StatusCode, value, _Locker); }
        }
        #endregion

        #region variables
        private bool _IsSuccessStatusCode;
        private string _ReasonPhrase = "";
        private HttpStatusCode _StatusCode;
        #endregion
    }
}
