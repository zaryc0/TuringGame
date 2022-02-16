using System;
using System.Collections.Generic;
using System.Text;
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
        private string _title;
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

        private object _outputContent;
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
        }

        //Command Functions
        public void ChangeOutputContent(int id)
        {
            switch (id)
            {
                case 0:
                    OutputContent = InitialVM;
                    break;
                case 1:
                    OutputContent = SubjectVM;
                    break;
                case 2:
                    OutputContent = InterviewerVM;
                    break;
                default:
                    OutputContent = InitialVM;
                    break;
            }
        }
    }
}
