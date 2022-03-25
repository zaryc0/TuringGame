using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TGF_Client.ViewModel;

namespace TGF_Client.Shell
{
    class ShellViewModel : BaseViewModel
    {
        //nested View Models
        public InitialViewVM InitialVM { get; }
        public SubjectViewVM SubjectVM { get; }
        public InterviewerViewVM InterviewerVM { get; }

        //Properties
        private object _outputContent;
        private string _title;
        private Visibility _debug_flag;
        private string _debug;

        //Property masks
        public string Title
        {
            get => _title;
            set
            {
                if (value != this._title)
                {
                    this._title = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public object OutputContent
        {
            get => _outputContent;
            set
            {
                if(value != _outputContent)
                {
                    this._outputContent = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public Visibility DebugFlag
        {
            get => _debug_flag;
            set
            {
                _debug_flag = value;
                NotifyPropertyChanged();
            }
        }
        public string Debug
        {
            get => _debug;
            set
            {
                _debug += value;
                NotifyPropertyChanged();
            }
        }

        //Constructor
        public ShellViewModel()
        {
            Title = " Turing Game Student";
            InitialVM = new InitialViewVM();
            SubjectVM = new SubjectViewVM();
            InterviewerVM = new InterviewerViewVM();

            ChangeOutputContent(0);
            Bus.shellVM = this;
            Bus.initialVM = InitialVM;
            Bus.subjectVM = SubjectVM;
            Bus.interviewerVM = InterviewerVM;

            Debug = $"[{DateTime.Now}] -> Application Launched\n";
            DebugFlag = Visibility.Collapsed;
            DebugToggle = new RelayCommand(o => ToggleDebugFlag());
        }
        //Commands
        public ICommand DebugToggle { get; set; }
        //Command Functions
        public void ChangeOutputContent(int id)
        {
            OutputContent = id switch
            {
                Constants.Initial_View_ID => InitialVM,
                Constants.Subject_View_ID => SubjectVM,
                Constants.Interviewer_View_ID => InterviewerVM,
                _ => InitialVM,
            };
        }

        public void ClosingApplication()
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
