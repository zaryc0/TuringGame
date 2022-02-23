using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.ViewModel;

namespace TGF_Controller.Shell
{
    class ShellViewModel : BaseViewModel
    {
        //nested View Models
        public InitialViewVM InitialVM { get; }
        public MonitoringVM MonitorVM { get; }

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
            MonitorVM = new MonitoringVM();

            OutputContent = InitialVM;
            Bus.shellVM = this;
            Bus.initialVM = InitialVM;
            Bus.monitoringVM = MonitorVM;
        }

        //Command Functions
        public void ChangeOutputContent(int id)
        {
            OutputContent = id switch
            {
                Constants.Initial_View_ID => InitialVM,
                Constants.Monitor_View_ID => MonitorVM,
                _ => InitialVM,
            };
        }
    }
}
