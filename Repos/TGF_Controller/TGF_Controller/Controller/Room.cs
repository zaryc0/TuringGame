using System;
using System.Net;
using System.Threading;
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
        private Mutex _lock = new();
        private Thread _interviewerListener;
        private Thread _subjectListener;

        //Constructors
        public Room()
        {
            _id = 0;
            RoomName = $"Room {_id}";
            MessageBoard = null;
            HasInterviewer = false;
            _active = true;
            _lock = new();
            _subjectListener = new Thread(() => ListenToSubject());
            _interviewerListener = new Thread(() => ListenToInterviewer());
        }
        public Room(int subjectPortNumber, int interviewerPortNumber, bool hasRobot, int id)
        {
            _id = id;
            RoomName = $"Room {id}";
            MessageBoard = new MessageBoard();
            PopulateMessageWithSpoofs();
            Subject = new SocketHandler(subjectPortNumber);
            Subject.SetFilters(_id);
            Interviewer = new SocketHandler(interviewerPortNumber);
            Interviewer.SetFilters(_id);
            HasRobot = hasRobot;
            HasSubject = hasRobot;
            HasInterviewer = false;
            _active = true;
            _lock = new();
            _subjectListener = new Thread(() => ListenToSubject());
            _interviewerListener = new Thread(() => ListenToInterviewer());
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
            _interviewerListener.Start();
            _subjectListener.Start();
        }

        public int GetID()
        {
            return _id;
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

        private void ListenToSubject()
        {
            IMessage tempMessage;
            while (_active)
            {

                tempMessage = Subject.Listen();
                if (!_active) { break; }
                UpdateMessageBoards(tempMessage);
                Interviewer.Broadcast(tempMessage);
            }
            try
            {
                Interviewer.Broadcast(new Message("Null", "Null", Constants.Message_Type_Visible_Tag, "Subject has Left the room Connection will be Terminated"));
            }
            catch
            {
                return;
            }
        }
        private void ListenToInterviewer()
        {
            IMessage tempMessage;
            while (_active)
            {
                tempMessage = Interviewer.Listen();
                if (!_active) { break; }
                UpdateMessageBoards(tempMessage);
                Subject.Broadcast(tempMessage);
            }
            try
            {
                Interviewer.Broadcast(new Message("Null", "Null", Constants.Message_Type_Visible_Tag, "Interviewer has Left the room Connection will be Terminated"));
            }
            catch
            {
                return;
            }
        }
        private void UpdateMessageBoards(IMessage m)
        {
            _ = _lock.WaitOne();
            MessageBoard.AddMessage(m);
            Bus.UpdateMessageBoards(m, _id);
            _lock.ReleaseMutex();
        }
        public void Kill()
        {
            _active = false;
            Interviewer.Close(1);
            Subject.Close(1);
            _lock.Dispose();
            Interviewer.Broadcast(new Message("Null", "Null", Constants.Message_Type_Visible_Tag, "Connection has been severed by remote Host"));
            Interviewer.Close(2);
            Subject.Broadcast(new Message("Null", "Null", Constants.Message_Type_Visible_Tag, "Connection has been severed by remote Host"));
            Subject.Close(2);
        }
    }
}
