using ThunderDesign.Net.HttpClientService.EventHandlers;

namespace ThunderDesign.Net.HttpClientService.Interfaces
{
    public interface IHttpClientAutoRedirect
    {
        #region event handlers
        event CookieContainerChangedEventHandler CookieContainerChangedEvent;
        #endregion
    }
}
