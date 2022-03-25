using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TGF_Controller.ViewModel;

namespace TGF_Controller.Shell
{
    class ShellViewModel : BaseViewModel
    {
        //nested View Models
        public InitialViewVM InitialVM { get; }
        public MonitoringVM MonitorVM { get; }
        public WaitingVM WaitVM { get; }

        //Properties
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (value != this._title)
                {
                    _title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _debug;
        public string Debug
        {
            get => _debug;
            set
            {
                _debug += value;
                NotifyPropertyChanged();
            }
        }

        private object _outputContent;
        public object OutputContent
        {
            get => _outputContent;
            set
            {
                if (value != _outputContent)
                {
                    _outputContent = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Visibility _debug_flag;
        public Visibility DebugFlag
        {
            get => _debug_flag;
            set
            {
                _debug_flag = value;
                NotifyPropertyChanged();
            }
        }

        //Constructor
        public ShellViewModel()
        {
            Title = " Turing Game Teacher";
            Debug = $"[{DateTime.Now}] -> Application Launched\n";
            DebugFlag = Visibility.Collapsed;
            InitialVM = new InitialViewVM();

            Bus.shellVM = this;
            Bus.initialVM = InitialVM;
            ChangeOutputContent(Bus.initialVM);
            
            DebugToggle = new RelayCommand(o => ToggleDebugFlag());
        }
        //Commands
        public ICommand DebugToggle { get; set; }

        //Command Functions
        public void ChangeOutputContent(object vm)
        {
            OutputContent = vm;
        }
        internal void ClosingApplication()
        {
            Bus.Close();
        }
        internal bool ToggleDebugFlag()
        {
            DebugFlag = DebugFlag != Visibility.Collapsed ? Visibility.Collapsed : Visibility.Visible;
            if (DebugFlag == Visibility.Collapsed)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


    }
}
