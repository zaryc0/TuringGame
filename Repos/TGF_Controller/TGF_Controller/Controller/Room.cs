using System;
using System.Diagnostics;
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

        private Process _chatBotWrapped;
        private int _id;
        private int _hostPortNumber;
        private int _subjectPortNumber;
        private int _interviewerPortNumber;
        private bool _active;
        private Mutex _lock = new();
        private Thread _interviewerListener;
        private Thread _subjectListener;
        private Thread _BotLauncher;

        //Constructors
        public Room()
        {
            _id = 0;
            RoomName = $"Room {_id}";
            MessageBoard = null;
            HasInterviewer = false;
            HasSubject = false;
            _active = true;
            _lock = new();
            _subjectListener = new Thread(() => ListenToSubject());
            _interviewerListener = new Thread(() => ListenToInterviewer());
            _BotLauncher = new Thread(() => LaunchChatBot());
        }
        public Room(int subjectPortNumber, int interviewerPortNumber, int hostPortNumber, int id)
        {
            _id = id;
            _hostPortNumber = hostPortNumber;
            RoomName = $"Room {id}";
            MessageBoard = new MessageBoard();
            //PopulateMessageWithSpoofs();
            Subject = new SocketHandler(subjectPortNumber);
            Subject.SetFilters(_id);
            Interviewer = new SocketHandler(interviewerPortNumber);
            Interviewer.SetFilters(_id);
            HasInterviewer = false;
            HasSubject = false;
            _subjectPortNumber = subjectPortNumber;
            _interviewerPortNumber = interviewerPortNumber;
            _active = true;
            _lock = new();
            _subjectListener = new Thread(() => ListenToSubject());
            _interviewerListener = new Thread(() => ListenToInterviewer());
            _BotLauncher = new Thread(() => LaunchChatBot());

        }

        //Functions
        public void Run()
        {
            Bus.AddDebugMessage($"Room:{_id} has begun to Run");
            InitInterviewer();
            Bus.AddDebugMessage($"Room:{_id} has obtained interviewer");
            _BotLauncher.Start();
            InitSubject();
            Bus.AddDebugMessage($"Room:{_id} has obtained Subject");
            _interviewerListener.Start();
            _subjectListener.Start();
            Bus.AddDebugMessage($"Room:{_id} has begun transmission");
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
                if (tempMessage.TypeTag == Constants.Message_Type_Terminate_Tag) { return; }
                if (!_active) { break; }
                Interviewer.Broadcast(HandleMessageRecieved(tempMessage));
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
                if (tempMessage.TypeTag == Constants.Message_Type_Terminate_Tag) { return; }
                if (!_active) { break; }
                Subject.Broadcast(HandleMessageRecieved(tempMessage));
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
        private IMessage HandleMessageRecieved(IMessage m)
        {
            if (m.TypeTag == Constants.Message_Type_Visible_Tag)
            {
                UpdateMessageBoards(m);
                return m;
            }
            if (m.TypeTag == Constants.Message_Type_Submission_Tag)
            {
                m.TypeTag = Constants.Message_Type_Visible_Tag;
                m.Content = m.Content == Constants.Submission_Robot ? "I think you are a Robot!" : "I think you are a human!";
            }
            return m;
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
            if (HasSubject && HasRobot)
            {
                KillChatBot();
            }
            else
            {
                Subject.Broadcast(new Message("Null", "Null", Constants.Message_Type_Visible_Tag, "Connection has been severed by remote Host"));
            }
            Subject.Close(2);
        }
        private void LaunchChatBot()
        {
            Bus.AddDebugMessage($"Room:{_id} entered bot launch");
            Thread.Sleep(5000);
            Bus.AddDebugMessage($"Room:{_id} HasSubject = {HasSubject}");
            if (HasSubject != true)
            {
                Bus.AddDebugMessage($"Room:{_id} hit");
                ProcessStartInfo thisInfo = new ProcessStartInfo("ChatBotWrapper.exe");
                thisInfo.WorkingDirectory = "../../../../../ChatBotWrapper/bin/Release/net5.0";
                thisInfo.Arguments = $"{_hostPortNumber}";
                thisInfo.RedirectStandardInput = false;
                thisInfo.RedirectStandardOutput = false;
                thisInfo.UseShellExecute = true;
                thisInfo.WindowStyle = ProcessWindowStyle.Hidden;
                _ = Process.Start(thisInfo);
                HasRobot = true;
            }
        }
        private void KillChatBot()
        {
            Subject.Broadcast(new Message(Constants.kill_Chat_Bot_Message));
        }
    }
}
