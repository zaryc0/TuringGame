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
        internal static void SetPortNumber(int port)
        {
            int id = client.socket.SetPort(port);
            shellVM.ChangeOutputContent(id);
        }

        internal static MessageBoardVM GetSubjectVM()
        {
            throw new NotImplementedException();
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
            Message temp = new Message(client.socket.controllerAddress.ToString(), client.socket.localAddress.ToString(), "Message", text);
            client.AddMessageToList("message", text);
            client.socket.Broadcast("message", text);
            UpdateMessageBoard(temp);
        }
        internal static void UpdateMessageBoard(Message message)
        {
            subjectVM.messageBoardVM.UpdateMessages(message);
        }
    }
}
