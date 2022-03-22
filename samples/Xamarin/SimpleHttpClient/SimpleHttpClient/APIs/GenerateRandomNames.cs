using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThunderDesign.Net.HttpClientService;
using ThunderDesign.Net.HttpClientService.DataObjects;

namespace SimpleHttpClient.APIs
{
    public static partial class API
    {
        public static async Task<ResponseContentData> GenerateRandomNamesAsync(byte generateCount, CancellationToken cancellationToken)
        {
            //Task<bool> task = null;

            ResponseContentData result = null;
            try
            {
                //string queryURI = "https://10.0.0.10:13345/OrderAPI/v0.4/Ping/Get";
                //string clientID = "2b038643595ded737c0335e8a6503b1dd85882d1";
                //string clientSecret = "8322eb4e5346db3f1e2c863d6b313a0842d42691";
                //string requestBody = string.Empty;
                //string unixTimestamp = ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                //string hash = AuthorisationHeader.BuildAuth(requestData.QueryURI, requestData.ClientID, requestData.ClientSecret, requestBody, unixTimestamp);

                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://randomuser.me/api/?nat=us&results={generateCount}")))
                {
                    requestMessage.Headers.Add("accept", "application/json");

                    if (cancellationToken.IsCancellationRequested)
                        throw new TaskCanceledException();

                    using (HttpResponseMessage responseMessage = await HttpClientService.Instance.HttpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false))
                    {
                        result = new ResponseContentData(responseMessage);
                        result.Content = await responseMessage.Content.ReadAsStringAsync(/*cancellationToken*/).ConfigureAwait(false);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                if (result == null)
                    result = new ResponseContentData();

                result.ReasonPhrase = "Task was cancelled";
                //Debug.WriteLine("Task was cancelled");
            }
            catch (Exception ex)
            {
                if (result == null)
                    result = new ResponseContentData();
                result.ReasonPhrase = "HResult: " + ex.HResult.ToString() + ", Message: " + ex.Message + ", InnerException: " + ex.InnerException;
                //Debug.WriteLine(ex);
            }
            //await Task.Delay(1000);
            return result;
        }
    }
}
