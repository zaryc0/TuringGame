using System;
using TGF_Controller.Controller;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.Model.interfaces;
using TGF_Controller.Shell;
using TGF_Controller.ViewModel;

namespace TGF_Controller
{
    internal static class Bus
    {
        public static Control controller;
        public static ShellViewModel shellVM;
        public static InitialViewVM initialVM;
        public static MonitoringVM monitoringVM;

        internal static void LaunchApplication()
        {
            controller = new();
            ChangeOutputContent(Constants.Monitor_View_ID);
        }

        internal static void ChangeOutputContent(int view_ID)
        {
            shellVM.ChangeOutputContent(view_ID);
        }
        internal static string GetSubjectIP(int room_ID)
        {
            return controller.GetRoom(room_ID).Subject.GetClientIP().ToString();
        }

        internal static string GetInterviewerIP(int room_ID)
        {
            return controller.GetRoom(room_ID).Interviewer.GetClientIP().ToString();
        }

        internal static void CreateNewRoomView(IRoom room)
        {
            monitoringVM.UpdateTabs(room);
        }

        internal static void UpdateMessageBoards(IMessage tempMessage, int roomID)
        {
            monitoringVM.RoomTabVMs[roomID].UpdateMessages(tempMessage);
        }
    }
}
