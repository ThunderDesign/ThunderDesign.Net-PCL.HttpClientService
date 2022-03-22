using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThunderDesign.Net.Threading.Interfaces;

namespace SimpleHttpClient.Interfaces
{
    public interface IBaseViewModel : IBindableObject
    {
        #region properties
        bool IsBusy { get; set; }
        #endregion

        #region methods
        Task<bool> LoadViewModelAsync(bool forceRefresh = false);
        #endregion
    }
}
