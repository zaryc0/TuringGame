using System;
using System.Collections.Generic;
using System.Text;

namespace Client_UI_Elements.interfaces
{
    interface IMessage
    {
        public string GetAuthor();
        public string GetMessage();
        public string GetTimeStamp();
        public void SetAuthor(string author);
        public void SetMessage(string message);
        public void SetTimeStamp();
    }
}
