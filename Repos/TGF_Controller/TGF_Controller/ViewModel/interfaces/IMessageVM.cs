﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF_Controller.ViewModel.interfaces
{
    internal interface IMessageVM
    {
        public string Destination { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public string TimeStamp { get; set; }
        public string Content { get; set; }
    }
}
