using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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
            (string, int) T = client.socket.SetPort(port);
            if (T.Item1 == Constants.Subject_Tag)
            {
                client.role = Roles.Subject;
                shellVM.ChangeOutputContent(Constants.Subject_View_ID);
            }
            else
            {
                client.role = Roles.Interviewer;
                shellVM.ChangeOutputContent(Constants.Interviewer_View_ID);
            }
            client.socket.RoomPortNumber = T.Item2;
        }

        internal static IPAddress CheckLocalIP()
        {
            return client.socket.localAddress;
        }

        internal static IEnumerable<Message> GetMessages()
        {
            return client.messages;
        }

        internal static void SendMessage(string text)
        {
            string source = "";
            string typeTag = "";

            switch (client.role)
            {
                case Roles.Interviewer:
                    source = Constants.Interviewer_Tag;
                    typeTag = Constants.Message_Type_Question_Tag;
                    break;
                case Roles.Subject:
                    source = Constants.Subject_Tag;
                    typeTag = Constants.Message_Type_Answer_Tag;
                    break;
                case Roles.None:
                    break;
            }

            Message temp = new Message("<ROOM/>", source, typeTag , text);
            client.AddMessageToList(typeTag, text);
            client.socket.Broadcast(typeTag, text);
            UpdateMessageBoard(temp);
            temp = new Message(client.socket.Listen());
            UpdateMessageBoard(temp);
        }

        internal static void SubmitSubjectSelection(string v)
        {
            client.socket.Broadcast(Constants.Message_Type_Submission_Tag, v);
        }

        internal static void UpdateMessageBoard(Message message)
        {
            subjectVM.messageBoardVM.UpdateMessages(message);
            interviewerVM.messageBoardVM.UpdateMessages(message);
        }
    }
}
