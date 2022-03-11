using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //Constructor
        public ShellViewModel()
        {
            Title = " Turing Game Teacher";
            InitialVM = new InitialViewVM();

            Bus.shellVM = this;
            Bus.initialVM = InitialVM;

            ChangeOutputContent(Bus.initialVM);
        }
        //Commands
        public ICommand Close { get; set; }
        //Command Functions
        public void ChangeOutputContent(object vm)
        {
            OutputContent = vm;
        }
    }
}
