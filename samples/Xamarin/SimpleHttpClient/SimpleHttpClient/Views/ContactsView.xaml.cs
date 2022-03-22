using SimpleHttpClient.Models;
using SimpleHttpClient.ViewModels;
using SimpleHttpClient.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleHttpClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsView : BaseView<ContactsViewModel>
    {
        #region constructors
        public ContactsView() : base()
        {
            InitializeComponent();
        }
        #endregion
    }
}