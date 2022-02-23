using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Controller;

namespace TGF_Controller.Model.interfaces
{
    interface IRoom
    {
        //Properties
        public IMessageBoard messageBoard { get; set; }
        public SocketHandler Subject { get; set; }
        public SocketHandler Interviewer { get; set; }
        public bool HasSubject { get; set; }
        public bool HasInterviewer { get; set; }
        public bool HasRobot { get; set; }

        //Functions
        public void Run();
    }
}
