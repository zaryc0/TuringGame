using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TGF_Client.Shell;

namespace TGF_Client.ViewModel
{
    class UserTextInputVM : BaseViewModel
    {
        //properties
        private string _text;
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

        //constructor
        public UserTextInputVM()
        {
            Text = "";
            SubmitText = new RelayCommand(o => SendText(_text));
        }

        //Commands
        public ICommand SubmitText { get; set; }

        //Command Functions
        public void ClearText()
        {
            Text = "";
            NotifyPropertyChanged();
        }
        public void SendText(string text)
        {
            Bus.SendMessage(text);
            ClearText();
        }
    }
}
