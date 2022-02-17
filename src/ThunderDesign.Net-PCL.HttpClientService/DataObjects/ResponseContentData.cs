using System.Net.Http;
using ThunderDesign.Net.Threading.Extentions;

namespace ThunderDesign.Net.HttpClientService.DataObjects
{
    public class ResponseContentData : ResponseData
    {
        #region constructors
        public ResponseContentData() : base() { }
        public ResponseContentData(HttpResponseMessage httpResponseMessage) : base(httpResponseMessage) { }
        #endregion

        #region properties
        public string Content
        {
            get { return this.GetProperty(ref _Content, _Locker); }
            set { this.SetProperty(ref _Content, value, _Locker); }
        }
        #endregion

        #region variables
        private string _Content = "";
        #endregion
    }
}
