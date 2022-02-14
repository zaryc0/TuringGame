using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Client_Application
{
    class Shell
    {
        private UserControl _content;

        public UserControl GetContent()
        {
            return _content;
        }
        public void SetContent(UserControl newContent)
        {
            _content = newContent;
        }
    }
}
