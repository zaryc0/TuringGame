using System;
using System.Collections.Generic;
using System.Text;

namespace TGF_Client.Model.interfaces
{
    interface IMessage
    {
        //Properties
        public string Destination { get; set; }
        public string Source { get; set; }
        public string TypeTag { get; set; }
        public string TimeStamp { get; set; }
        public string Content { get; set; }

        //Functions
        public string CompileMessage();
        public bool MessageIsNotValid(string[] input);
    }
}
