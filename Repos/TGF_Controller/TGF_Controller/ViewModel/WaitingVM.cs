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
        //models
        private string _text;
        private int _portNum;
        //Constructors
        public WaitingVM(int portnum)
        {
            _portNum = portnum;
            Text = $"Enter Code: {_portNum}\nWaiting for users to connect";
        }
        //Property access masks
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
