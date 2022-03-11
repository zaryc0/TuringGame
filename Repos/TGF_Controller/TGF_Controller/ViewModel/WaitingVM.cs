using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.ViewModel.interfaces;

namespace TGF_Controller.ViewModel
{
    class WaitingVM : BaseViewModel, IWaitingVM
    {
        //model
        private string _text;
        public WaitingVM()
        {
            Text = "Waiting for a user to connect";
        }
        public string Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
