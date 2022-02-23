using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Controller;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Model
{
    class Room : IRoom
    {
        //Properties
        public IMessageBoard messageBoard { get; set; }
        public SocketHandler Subject { get; set; }
        public SocketHandler Interviewer { get; set; }
        public bool HasSubject { get; set; }
        public bool HasInterviewer { get; set; }
        public bool HasRobot { get; set; }

        //Constructors
        public Room(int subjectPortNumber, int interviewerPortNumber, bool hasRobot)
        {
            messageBoard = new MessageBoard();
            Subject = new SocketHandler(subjectPortNumber);
            Interviewer = new SocketHandler(interviewerPortNumber);
            HasRobot = hasRobot;
            HasSubject = hasRobot;
            HasInterviewer = false;
        }

        //Functions
        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
