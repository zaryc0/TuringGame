using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TGF_Client.ViewModel
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        //property changed event handler

        public void NotifyPropertyChanged(string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
