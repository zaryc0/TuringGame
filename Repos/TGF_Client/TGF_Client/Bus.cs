using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TGF_Client.Client_App;
using TGF_Client.Model;
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

        internal static string GetSender(string source)
        {
            return client.role.ToString() == source ? "You" : "?????";
        }
        internal static void SetPortNumber(int port)
        {
            string T = client.InitialiseConnection(port);
            if (T == Constants.Subject_Tag)
            {
                client.role = Roles.Subject;
                shellVM.ChangeOutputContent(Constants.Subject_View_ID);
            }
            else
            {
                client.role = Roles.Interviewer;
                shellVM.ChangeOutputContent(Constants.Interviewer_View_ID);
            }
            client.InitialiseThread();
        }

        internal static IPAddress CheckLocalIP()
        {
            return client.GetLocalIP();
        }

        internal static IEnumerable<Message> GetMessages()
        {
            return client.messages;
        }

        internal static void SendMessage(string text)
        {
            client.IsListening(false);
            string source = Constants.Interviewer_Tag;;
            string typeTag = Constants.Message_Type_Visible_Tag;
            Message temp = new Message("<ROOM/>", source, typeTag , text);
            client.AddMessageToList(typeTag, text);
            UpdateMessageBoard(temp);
            client.SendMessage(typeTag, text);
            client.IsListening(true);
        }

        internal static void SubmitSubjectSelection(string v)
        {
            client.SendMessage(Constants.Message_Type_Submission_Tag, v);
        }

        internal static void Close()
        {
            bool killable = false;
            while (!killable) 
            {
                killable = client.Kill();     
            }

        }

        public static void HandleNewMessageRecieved(Message m)
        {
            if (m.TypeTag == Constants.Message_Type_Visible_Tag)
            {
                UpdateMessageBoard(m);
                client.IsListening(false);
            }
        }

        internal static void UpdateMessageBoard(Message message)
        {
            subjectVM.messageBoardVM.UpdateMessages(message);
            interviewerVM.messageBoardVM.UpdateMessages(message);
        }
    }
}
