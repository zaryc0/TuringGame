using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TGF_Client.ViewModel
{
    class InitialViewVM : BaseViewModel
    {
        //properties
        private string _input;
        public string Input
        {
            get => _input;
            set
            {
                if (value != this._input)
                {
                    this._input = value;
                    NotifyPropertyChanged();
                }
            }
        }

        //constructor
        public InitialViewVM()
        {
            Clear();
            Input = "test";
            SubmitInput = new RelayCommand(o => Send(_input));
        }

        //Commands
        public ICommand SubmitInput { get; set; }

        //Command Functions
        public void Clear()
        {
            Input = "";
        }
        public void Send(string text)
        {
            if (text.Length > 4)
            {
                return;
            }
            int portnumber = 0;
            if (int.TryParse(text, out portnumber))
            {
                Bus.SetPortNumber(portnumber);
            }
            Clear();
            return;
            
        }
    }
}
