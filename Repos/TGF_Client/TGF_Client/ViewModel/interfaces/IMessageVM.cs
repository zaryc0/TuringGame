using System;
using System.Collections.Generic;
using System.Text;
using TGF_Client.Model;

namespace TGF_Client.ViewModel.interfaces
{
    interface IMessageVM
    {
        public string Destination { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public string TimeStamp { get; set; }
        public string Content { get; set; }
    }
}
