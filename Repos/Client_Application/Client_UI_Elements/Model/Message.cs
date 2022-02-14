using Client_UI_Elements.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Models
{
    class Message : IMessage
    {
        private string _author;
        private string _message;
        private string _timeStamp;

        public string GetAuthor()
        {
            return _author;
        }

        public string GetMessage()
        {
            return _message;
        }

        public string GetTimeStamp()
        {
            return _timeStamp;
        }

        public void SetAuthor(string author)
        {
            _author = author;
        }

        public void SetMessage(string message)
        {
            _message = message;
        }

        public void SetTimeStamp()
        {
            _timeStamp = DateTime.Now.ToString();
        }
    }
}
