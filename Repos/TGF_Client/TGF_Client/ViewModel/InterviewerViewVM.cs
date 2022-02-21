using System;
using System.Collections.Generic;
using System.Text;

namespace TGF_Client.ViewModel
{
    class InterviewerViewVM : BaseViewModel
    {
        public MessageBoardVM messageBoardVM { get; set; }
        public UserTextInputVM userTextInputVM { get; set; }

        public SubjectSelectionVM subjectSelectionVM { get; set; }

        public InterviewerViewVM()
        {
            messageBoardVM = new MessageBoardVM();
            userTextInputVM = new UserTextInputVM();
            subjectSelectionVM = new SubjectSelectionVM();
        }
    }
}
