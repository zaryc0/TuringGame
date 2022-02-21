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
            OutputContent = id switch
            {
                Constants.Initial_View_ID => InitialVM,
                Constants.Subject_View_ID => SubjectVM,
                Constants.Interviewer_View_ID => InterviewerVM,
                _ => InitialVM,
            };
        }
    }
}
