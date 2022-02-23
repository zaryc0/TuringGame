using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF_Controller.Model.interfaces
{
    interface IMessage
    {
        //Properties
        public string Destination { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public string TimeStamp { get; set; }
        public string Content { get; set; }
        
        //Functions
        public string CompileMessage();
        public bool MessageIsNotValid(string[] input);
    }
}
