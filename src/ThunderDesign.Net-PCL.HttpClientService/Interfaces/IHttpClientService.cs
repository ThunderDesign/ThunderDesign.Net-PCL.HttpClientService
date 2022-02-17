using System.Net.Http;

namespace ThunderDesign.Net.HttpClientService.Interfaces
{
    public interface IHttpClientService
    {
        #region properties
        HttpMessageHandler HttpClientHandler { get; }
        #endregion
    }
}
