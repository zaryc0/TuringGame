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
            client.IsListening(false);
            string typeTag = Constants.Message_Type_Visible_Tag;
            IMessage temp = client.SendMessage(typeTag, text);
            UpdateMessageBoard(temp);
            client.IsListening(true);
        }

        internal static void SubmitSubjectSelection(string v)
        {
            client.SendMessage(Constants.Message_Type_Submission_Tag, v);
        }

        internal static void Close()
        {
            bool killable = false;
            _lock.Dispose();
            while (!killable) 
            {
                killable = client.Kill();
            }

        }

        public static void HandleNewMessageRecieved(Message m)
        {
            if (m.TypeTag == Constants.Message_Type_Visible_Tag)
            {
                _lock.WaitOne();
                UpdateMessageBoard(m);
                client.IsListening(false);
                _lock.ReleaseMutex();
            }
        }

        internal static void UpdateMessageBoard(IMessage message)
        {
            subjectVM.messageBoardVM.UpdateMessages(message);
            interviewerVM.messageBoardVM.UpdateMessages(message);
        }
    }
}
