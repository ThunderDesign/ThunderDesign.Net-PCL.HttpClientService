using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ThunderDesign.Net.HttpClientService.EventHandlers;
using ThunderDesign.Net.HttpClientService.Extentions;
using ThunderDesign.Net.HttpClientService.Interfaces;

namespace ThunderDesign.Net.HttpClientService.Http
{
    public class HttpClientAutoRedirect : HttpClient, IHttpClientAutoRedirect
    {
        #region constructors
        public HttpClientAutoRedirect() : this(new HttpClientHandler())
        {
        }

        public HttpClientAutoRedirect(HttpMessageHandler handler) : this(handler, true)
        {
        }

        public HttpClientAutoRedirect(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler)
        {
            if (handler is HttpClientHandler)
                _HttpClientHandler = handler as HttpClientHandler;
        }
        #endregion

        #region event handlers
        public event CookieContainerChangedEventHandler? CookieContainerChangedEvent;
        #endregion

        #region methods
        public new Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return SendAsync(request, defaultCompletionOption, CancellationToken.None);
        }

        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsync(request, defaultCompletionOption, cancellationToken);
        }

        public new Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            return SendAsync(request, completionOption, CancellationToken.None);
        }

        public new Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            Task<HttpResponseMessage> task = base.SendAsync(request, completionOption, cancellationToken);
            task.Wait(cancellationToken);
            HttpResponseMessage responseMessage = task.Result;

            if ((_HttpClientHandler?.UseCookies ?? false) && responseMessage.Headers.Contains("set-cookie"))
            {
                _HttpClientHandler?.CookieContainer.SetCookies(responseMessage.Headers, responseMessage.RequestMessage.RequestUri.Host, CookieContainerChangedEvent);
            }

            if ((_HttpClientHandler?.AllowAutoRedirect ?? false) && responseMessage.StatusCode == HttpStatusCode.Redirect && responseMessage.Headers.Contains("location"))
            {
                if (responseMessage.Headers.TryGetValues("location", out var location))
                {
                    using HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(location.Single()) };
                    try
                    {
                        return SendAsync(requestMessage, cancellationToken);
                    }
                    finally
                    {
                        responseMessage.Dispose();
                    }
                }
                return task;
            }
            else
                return task;
        }
        #endregion

        #region variables
        private const HttpCompletionOption defaultCompletionOption = HttpCompletionOption.ResponseContentRead;
        private readonly HttpClientHandler? _HttpClientHandler;
        #endregion
    }
}
