using TGF_Controller.Controller.Network.interfaces;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Controller.interfaces
{
    internal interface IRoom
    {
        //Properties
        public IMessageBoard messageBoard { get; set; }
        public ISocketHandler Subject { get; set; }
        public ISocketHandler Interviewer { get; set; }
        public bool HasSubject { get; set; }
        public bool HasInterviewer { get; set; }
        public bool HasRobot { get; set; }

        //Functions
        public void Run();
    }
}
