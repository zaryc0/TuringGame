using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using TGF_Client.Client_App;
using TGF_Client.Model;
using TGF_Client.Model.interfaces;
using TGF_Client.Shell;
using TGF_Client.ViewModel;

namespace TGF_Client
{
    internal static class Bus
    {
        public static Client client = new Client();
        public static ShellViewModel shellVM;
        public static InitialViewVM initialVM;
        public static SubjectViewVM subjectVM;
        public static InterviewerViewVM interviewerVM;
        private static Mutex _lock = new Mutex();

        internal static string GetSender(string source)
        {
            return client.role.ToString() == source ? "You" : "?????";
        }
        internal static void SetPortNumber(int port)
        {
            string T = client.InitialiseConnection(port);
            if (T == Constants.Subject_Tag)
            {
                client.SetRole(Roles.Subject);
                shellVM.ChangeOutputContent(Constants.Subject_View_ID);
            }
            else
            {
                client.SetRole(Roles.Interviewer);
                shellVM.ChangeOutputContent(Constants.Interviewer_View_ID);
            }
            client.InitialiseThread();
        }

        internal static IPAddress CheckLocalIP()
        {
            return client.GetLocalIP();
        }

        internal static IEnumerable<IMessage> GetMessages()
        {
            return client.messages;
        }

        internal static void SendMessage(string text)
        {
            string typeTag = Constants.Message_Type_Visible_Tag;
            IMessage temp = client.SendMessage(typeTag, text);
            UpdateMessageBoard(temp,0);
        }

        internal static void SubmitSubjectSelection(string v)
        {
            client.SendMessage(Constants.Message_Type_Submission_Tag, v);
        }

        internal static void Close()
        {
            _ = client.Kill();
            _lock.Dispose();
        }

        public static void HandleNewMessageRecieved(Message m,int type)
        {
            if (m.TypeTag == Constants.Message_Type_Visible_Tag)
            {
                _ = _lock.WaitOne();
                UpdateMessageBoard(m,type);
                _lock.ReleaseMutex();
            }
        }

        internal static void UpdateMessageBoard(IMessage message, int type)
        {
            subjectVM.messageBoardVM.UpdateMessages(message,type);
            interviewerVM.messageBoardVM.UpdateMessages(message,type);
        }
        internal static void AddDebugMessage(string dmess)
        {
            shellVM.Debug = $"[{DateTime.Now}] -> {dmess}\n";
        }
    }
}
