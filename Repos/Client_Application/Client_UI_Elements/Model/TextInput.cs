using Client_UI_Elements.interfaces;
using System;

namespace Client_Models
{
    public class TextIput : ITextInput
    {
        private string _text;
        public string GetTextInput()
        {
            return _text;
        }

        public void SetTextInput(string text)
        {
            _text = text;
        }
    }
}
