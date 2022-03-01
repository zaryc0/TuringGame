using System;
using System.Net;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.Controller.Network;
using TGF_Controller.Controller.Network.interfaces;
using TGF_Controller.Model;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Controller
{
    internal class Room : IRoom
    {
        //Properties
        public IMessageBoard MessageBoard { get; set; }
        public ISocketHandler Subject { get; set; }
        public ISocketHandler Interviewer { get; set; }
        public bool HasSubject { get; set; }
        public bool HasInterviewer { get; set; }
        public bool HasRobot { get; set; }
        public string RoomName { get; set; }

        private int _id;
        private bool _active;

        //Constructors
        public Room(int subjectPortNumber, int interviewerPortNumber, bool hasRobot, int id)
        {
            _id = id;
            RoomName = $"Room {id}";
            MessageBoard = new MessageBoard();
            Subject = new SocketHandler(subjectPortNumber);
            Interviewer = new SocketHandler(interviewerPortNumber);
            HasRobot = hasRobot;
            HasSubject = hasRobot;
            HasInterviewer = false;
            _active = true;
        }

        //Functions
        public void Run()
        {
            InitInterviewer();
            InitSubject();
            IMessage tempMessage;
            while (_active)
            {
                tempMessage = Interviewer.Listen();
                MessageBoard.AddMessage(tempMessage);
                Bus.UpdateMessageBoards(tempMessage, _id);
                Subject.Broadcast(tempMessage);
                tempMessage = Subject.Listen();
                MessageBoard.AddMessage(tempMessage);
                Bus.UpdateMessageBoards(tempMessage, _id);
                Interviewer.Broadcast(tempMessage);
            }
            Subject.Broadcast(new Message("","",Constants.Message_Type_Terminate_Tag,"Connection has been severed by remote Host"));
            Interviewer.Broadcast(new Message("", "", Constants.Message_Type_Terminate_Tag, "Connection has been severed by remote Host"));
        }

        public void CheckMessage(IMessage message)
        {
            if (message.TypeTag != Constants.Message_Type_Terminate_Tag)
            {
                if (message.TypeTag == Constants.Message_Type_Submission_Tag)
                {
                    Subject.Broadcast(new Message("", "", Constants.Message_Type_Submission_Tag, $"The interviewer Believes You are a: {message.Content}"));
                    _active = false;
                }
                else
                {
                    _active = true;
                }
            }
            else
            {
                _active = false;
            }
        }

        private void InitInterviewer()
        {
            _ = Interviewer.WaitForConnection();
            Interviewer.Broadcast(new Message($"<CLIENT/>,<CONTROLLER/>,<ASSIGNMENT/>,{DateTime.Now},{Constants.Interviewer_Tag},<MessageEnd/>"));
        }

        private void InitSubject()
        {
            _ = Subject.WaitForConnection();
            Subject.Broadcast(new Message($"<CLIENT/>,<CONTROLLER/>,<ASSIGNMENT/>,{DateTime.Now},{Constants.Subject_Tag},<MessageEnd/>"));

        }
    }
}
