using System.Net;
using ThunderDesign.Net.HttpClientService.EventHandlers;

namespace ThunderDesign.Net.HttpClientService.Interfaces
{
    public interface IHttpClientAutoRedirect
    {
        #region event handlers
        event CookieContainerChangedEventHandler CookieContainerChangedEvent;
        #endregion

        #region properties
        bool UseCookies { get; }
        bool AllowAutoRedirect { get; }
        CookieContainer CookieContainer { get; }
        #endregion
    }
}
