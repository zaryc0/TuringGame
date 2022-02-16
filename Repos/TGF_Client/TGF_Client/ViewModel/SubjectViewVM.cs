using System;
using System.Collections.Generic;
using System.Text;

namespace TGF_Client.ViewModel
{
    class SubjectViewVM : BaseViewModel
    {
        public MessageBoardVM messageBoardVM { get; set; }
        public UserTextInputVM userTextInputVM { get; set; }

        public SubjectViewVM()
        {
            messageBoardVM = new MessageBoardVM();
            userTextInputVM = new UserTextInputVM();
        }
    }
}
