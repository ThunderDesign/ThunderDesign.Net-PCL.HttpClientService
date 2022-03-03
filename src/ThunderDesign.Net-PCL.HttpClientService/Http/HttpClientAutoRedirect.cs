using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
            if (handler == null)
                return;

            //When you create your custom HttpMessageHandler include ICustomHttpMessageHandler to quickly read property values
            if (handler is ICustomHttpMessageHandler customHttpMessageHandler)
            {
                this.CookieContainer = customHttpMessageHandler.CookieContainer ?? new CookieContainer();
                this.UseCookies = customHttpMessageHandler.UseCookies;
                this.AllowAutoRedirect = customHttpMessageHandler.AllowAutoRedirect;

                //need to turn off AllowAutoRedirect since we are using our own
                if (customHttpMessageHandler.AllowAutoRedirect)
                    customHttpMessageHandler.AllowAutoRedirect = false;
            }
            //android will be HttpClientHandler, but ios won't. Recommend creating custom inherited class that uses ICustomHttpMessageHandler
            else if (handler is HttpClientHandler httpClientHandler)
            {
                this.CookieContainer = httpClientHandler.CookieContainer ?? new CookieContainer();
                this.UseCookies = httpClientHandler.UseCookies;
                this.AllowAutoRedirect = httpClientHandler.AllowAutoRedirect;

                //need to turn off AllowAutoRedirect since we are using our own
                if (httpClientHandler.AllowAutoRedirect)
                    httpClientHandler.AllowAutoRedirect = false;
            }
            //using reflection to read property values
            else
            {
                PropertyInfo propertyInfo;
                propertyInfo = handler.GetType().GetProperty(nameof(ICustomHttpMessageHandler.CookieContainer));
                if (propertyInfo?.PropertyType == typeof(CookieContainer))
                    this.CookieContainer = (CookieContainer)propertyInfo.GetValue(handler) ?? new CookieContainer();

                propertyInfo = handler.GetType().GetProperty(nameof(ICustomHttpMessageHandler.UseCookies));
                if (propertyInfo?.PropertyType == typeof(bool))
                    this.UseCookies = (bool)propertyInfo.GetValue(handler);

                propertyInfo = handler.GetType().GetProperty(nameof(ICustomHttpMessageHandler.AllowAutoRedirect));
                if (propertyInfo?.PropertyType == typeof(bool))
                    this.AllowAutoRedirect = (bool)propertyInfo.GetValue(handler);

                if (this.AllowAutoRedirect)
                    //need to turn off AllowAutoRedirect since we are using our own
                    handler.GetType().GetProperty(nameof(ICustomHttpMessageHandler.AllowAutoRedirect)).SetValue(handler, false);
            }
        }
        #endregion

        #region event handlers
        public event CookieContainerChangedEventHandler CookieContainerChangedEvent;
        #endregion

        #region properties
        public bool UseCookies { get; private set; }
        public bool AllowAutoRedirect { get; private set; }
        public CookieContainer CookieContainer { get; private set; }
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

            if (this.UseCookies && responseMessage.Headers.Contains("set-cookie"))
            {
                this.CookieContainer?.SetCookies(responseMessage.Headers, responseMessage.RequestMessage.RequestUri.Host, CookieContainerChangedEvent);
            }

            if (this.AllowAutoRedirect && Enumerable.Range(300, 399).Contains((int)responseMessage.StatusCode) && responseMessage.Headers.Contains("location"))
            {
                if (responseMessage.Headers.TryGetValues("location", out var location))
                {
                    using (HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(location.Single()) })
                    {
                        try
                        {
                            return SendAsync(requestMessage, cancellationToken);
                        }
                        finally
                        {
                            responseMessage.Dispose();
                        }
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
        #endregion
    }
}
