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
            PopulateMessageWithSpoofs();
            Subject = new SocketHandler(subjectPortNumber);
            Interviewer = new SocketHandler(interviewerPortNumber);
            HasRobot = hasRobot;
            HasSubject = hasRobot;
            HasInterviewer = false;
            _active = true;
        }

        private void PopulateMessageWithSpoofs()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    MessageBoard.Messages.Add(new Message($"<SUBJECT/>,<INTERVIEWER/>,{Constants.Message_Type_Visible_Tag},{DateTime.Now},Nulla id mollis est. Vestibulum massa ligula posuere semper arcu velestas. Nunc tristique odio arcu nec congue quam eleifend sit amet. Aenean in ex nulla. Nam interdum erat turpis. Morbi in enim vitae libero maximus porttitor. Praesent quis fringilla"));
                }
                else
                {
                    MessageBoard.Messages.Add(new Message($"<INTERVIEWER/>,<SUBJECT/>,{Constants.Message_Type_Visible_Tag},{DateTime.Now},Nulla id mollis est. Vestibulum massa ligula posuere semper arcu velestas. Nunc tristique odio arcu nec congue quam eleifend sit amet. Aenean in ex nulla. Nam interdum erat turpis. Morbi in enim vitae libero maximus porttitor. Praesent quis fringilla"));
                }
            }
        }

        //Functions
        public void Run()
        {
            InitInterviewer();
            InitSubject();
            IMessage tempMessage;
            while (_active)
            {
                try
                {
                    tempMessage = Interviewer.Listen(); 
                    if(!_active) { break; }
                    MessageBoard.AddMessage(tempMessage);
                    Bus.UpdateMessageBoards(tempMessage, _id);
                    Subject.Broadcast(tempMessage);
                    tempMessage = Subject.Listen();
                    if (!_active) { break; }
                    MessageBoard.AddMessage(tempMessage);
                    Bus.UpdateMessageBoards(tempMessage, _id);
                    Interviewer.Broadcast(tempMessage);
                }
                catch
                {

                }
            }
            Interviewer.Broadcast(new Message("Null", "Null", Constants.Message_Type_Visible_Tag, "Connection has been severed by remote Host"));
            Interviewer.Close(2);
            Subject.Broadcast(new Message("Null", "Null", Constants.Message_Type_Visible_Tag, "Connection has been severed by remote Host"));
            Subject.Close(2);
        }

        public int GetID()
        {
            return _id;
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
            _ = Interviewer.WaitForPrimaryConnection();
            _ = Interviewer.WaitForSecondaryConnection();
            HasInterviewer = true;
            Interviewer.Broadcast(new Message($"<CLIENT/>,<CONTROLLER/>,<ASSIGNMENT/>,{DateTime.Now},{Constants.Interviewer_Tag},<MessageEnd/>"));
        }

        private void InitSubject()
        {
            _ = Subject.WaitForPrimaryConnection();
            _ = Subject.WaitForSecondaryConnection();
            HasSubject = true;
            Subject.Broadcast(new Message($"<CLIENT/>,<CONTROLLER/>,<ASSIGNMENT/>,{DateTime.Now},{Constants.Subject_Tag},<MessageEnd/>"));
        }

        public void Kill()
        {
            _active = false;
            Interviewer.Close(1);
            Subject.Close(1);
        }
    }
}
