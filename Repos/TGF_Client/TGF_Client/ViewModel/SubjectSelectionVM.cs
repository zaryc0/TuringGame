using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace TGF_Client.ViewModel
{
    class SubjectSelectionVM : BaseViewModel
    {
        //Properties
        private string _header;
        private string _humanImgURI;
        private string _robotImgURI;
        private string _buttonText;
        private SubjectEnum _selection;

        //Constructors
        public SubjectSelectionVM()
        {
            Header = "Who are you talking to?";
            RobotURI = Constants.Robot128_Img_File_Path;
            HumanURI = Constants.Human128_Img_File_Path;
            ButtonText = "Select an Image";
            Selection = SubjectEnum.None;
            SubmitSelection = new RelayCommand(o => Submit());
        }
        //Property Masks
        public string Header
        {
            get => _header;
            set
            {
                if (value != _header)
                {
                    _header = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string HumanURI
        {
            get => _humanImgURI;
            set
            {
                if (value != _humanImgURI)
                {
                    _humanImgURI = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string RobotURI
        {
            get => _robotImgURI;
            set
            {
                if (value != _robotImgURI)
                {
                    _robotImgURI = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public SubjectEnum Selection
        {
            get => _selection;
            set
            {
                if (value != _selection)
                {
                    _selection = value;
                    NotifyPropertyChanged();
                }
            }
        }
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

        //Functions
        public void SelectedRobot()
        {
            Selection = SubjectEnum.Robot;
            ButtonText = "Robot";
        }

        public void SelectedHuman()
        {
            Selection = SubjectEnum.Human;
            ButtonText = "Human";
        }

        //Commands
        public ICommand SubmitSelection { get; set; }

        //Command Functions
        public void SetSelection(SubjectEnum subject)
        {
            Selection = subject;
        }
        public void Submit()
        {
            switch (Selection)
            {
                case SubjectEnum.Robot:
                    Bus.SubmitSubjectSelection(Constants.Submission_Robot_Tag);
                    break;
                case SubjectEnum.Human:
                    Bus.SubmitSubjectSelection(Constants.Submission_Human_Tag);
                    break;
                case SubjectEnum.None:
                    break;
                default:
                    break;
            }
        }
    }
}
