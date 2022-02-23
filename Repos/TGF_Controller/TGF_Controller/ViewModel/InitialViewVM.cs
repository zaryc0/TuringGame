using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TGF_Controller.ViewModel
{
    class InitialViewVM : BaseViewModel
    {
        //Properties
        private string _text;
        public string TextBlock
        {
            get => _text;
            set
            {
                if(value != _text)
                {
                    _text = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private string _buttonText;
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if (value != _buttonText)
                {
                    _buttonText = value;
                    NotifyPropertyChanged();
                }
            }
        }

        //Commands
        public ICommand LaunchButtonPress { get; set; }

        //Constructors
        public InitialViewVM()
        {
            TextBlock = "Press button to launch session";
            ButtonText = "Launch";
            LaunchButtonPress = new RelayCommand(o => LaunchApplication());
        }

        //Functions
        public void LaunchApplication()
        {
            Bus.LaunchApplication();
        }
    }
}
