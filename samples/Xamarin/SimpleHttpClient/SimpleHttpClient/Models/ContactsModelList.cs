﻿using System;
using System.Collections.Generic;
using System.Text;
using ThunderDesign.Net.Threading.DataCollections;

namespace SimpleHttpClient.Models
{
    public class ContactsModelList : ObservableDataDictionary<byte, ContactsModel>
    {
        #region properties
        public static ContactsModelList Instance
        {
            get
            {
                lock (_Locker)
                {
                    return _Instance ?? (_Instance = new ContactsModelList());
                }
            }
        }
        #endregion

        #region variables
        protected readonly static object _Locker = new object();
        private static ContactsModelList _Instance = null;
        #endregion
    }
}
