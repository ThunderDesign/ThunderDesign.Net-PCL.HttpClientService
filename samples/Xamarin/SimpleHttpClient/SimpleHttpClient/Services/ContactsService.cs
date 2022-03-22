using SimpleHttpClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThunderDesign.Net.HttpClientService.DataObjects;
using ThunderDesign.Net.ToolBox.Extentions;
using SimpleHttpClient.APIs;
using SimpleHttpClient.JSON.RandomName;
using System.Diagnostics;

namespace SimpleHttpClient.Services
{
    public static class ContactsService
    {
        #region properties
        public static byte UniqueId
        {
            get { return (byte)Interlocked.Increment(ref _UniqueId); }
            private set { Interlocked.Exchange(ref _UniqueId, value); }
        }
        #endregion

        #region methods
        public async static Task<ContactsModelList> GetContactsAsync()
        {
            ContactsModelList.Instance.Clear();
            UniqueId = 0;

            try
            {
                using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {

                    ResponseContentData responseContentData = await API.GenerateRandomNamesAsync(10, cancellationTokenSource.Token).ConfigureAwait(false);
                    if (responseContentData?.IsSuccessStatusCode ?? false)
                    {
                        if (cancellationTokenSource.IsCancellationRequested)
                            throw new TaskCanceledException();

                        RandomName_Response randomName_Response = RandomName_Response.FromJson(responseContentData.Content);
                        foreach(var result in randomName_Response.Results)
                        {
                            ContactsModelList.Instance.Add(new ContactsModel() { Id = UniqueId, FirstName = result.Name.First, LastName = result.Name.Last });
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("TaskCanceledException");
            }

            return ContactsModelList.Instance;
        }
        #endregion

        #region variables
        private static int _UniqueId = 0;
        #endregion
    }
}
