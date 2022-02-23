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
        public IMessageBoard messageBoard { get; set; }
        public ISocketHandler Subject { get; set; }
        public ISocketHandler Interviewer { get; set; }
        public bool HasSubject { get; set; }
        public bool HasInterviewer { get; set; }
        public bool HasRobot { get; set; }

        private bool _active;

        //Constructors
        public Room(int subjectPortNumber, int interviewerPortNumber, bool hasRobot)
        {
            messageBoard = new MessageBoard();
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
            while (_active)
            {
                IMessage tempMessage = Interviewer.Listen();
                TypeTag type = FilterMessage(tempMessage);
                if (type == TypeTag.Terminate)
                {
                    if (type != TypeTag.Submit)
                    {
                        Subject.Broadcast(tempMessage);
                        messageBoard.AddMessage(tempMessage);
                        tempMessage = Subject.Listen();
                        Interviewer.Broadcast(tempMessage);
                        messageBoard.AddMessage(tempMessage);
                    }
                    else
                    {
                        Subject.Broadcast(new Message("", "",Constants.Message_Type_Submission_Tag,$"The interviewer Believes You are a: {tempMessage.Content}"));
                        _active = false;
                    }
                }
                else
                {
                    _active = false;
                }
            }
            Subject.Broadcast(new Message("","",Constants.Message_Type_Terminate_Tag,"Connection has been severd by remote Host"));
            Interviewer.Broadcast(new Message("", "", Constants.Message_Type_Terminate_Tag, "Connection has been severd by remote Host"));
        }

        private TypeTag FilterMessage(IMessage tempMessage)
        {
            return tempMessage.TypeTag == Constants.Message_Type_Terminate_Tag
                ? TypeTag.Terminate
                : tempMessage.TypeTag == Constants.Message_Type_Init_Tag
                ? TypeTag.Init
                : tempMessage.TypeTag == Constants.Message_Type_Question_Tag
                ? TypeTag.Question
                : tempMessage.TypeTag == Constants.Message_Type_Answer_Tag
                ? TypeTag.Answer
                : tempMessage.TypeTag == Constants.Message_Type_Submission_Tag
                ? TypeTag.Submit : TypeTag.Init;
        }

        private void InitInterviewer()
        {
            IMessage handshake = Interviewer.Listen();
            if (handshake.TypeTag != Constants.Message_Type_Init_Tag)
            {
                throw new Exception(message: $"incorrect message type from Interviewer expected <INIT/> found:{handshake.TypeTag}");
            }
            else
            {
                Interviewer.SetClientIP(IPAddress.Parse(handshake.Source));
            }

        }

        private void InitSubject()
        {
            IMessage handshake = Subject.Listen();
            if (handshake.TypeTag != Constants.Message_Type_Init_Tag)
            {
                throw new Exception(message: $"incorrect message type from Subject expected <INIT/> found:{handshake.TypeTag}");
            }
            else
            {
                Subject.SetClientIP(IPAddress.Parse(handshake.Source));
            }
        }
    }
}
