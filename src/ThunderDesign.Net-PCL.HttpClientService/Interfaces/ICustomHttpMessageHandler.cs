using System.Net;

namespace ThunderDesign.Net.HttpClientService.Interfaces
{
    public interface ICustomHttpMessageHandler
    {
        #region properties
        bool UseCookies { get; set; }
        bool AllowAutoRedirect { get; set; }
        CookieContainer CookieContainer { get; set; }
        #endregion
    }
}
