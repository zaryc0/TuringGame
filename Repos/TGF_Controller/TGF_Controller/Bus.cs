using System;
using System.ComponentModel;
using System.Threading;
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
        public static int hostPortNum;
        private static readonly Mutex _lock = new();


        internal static void LaunchApplication()
        {
            Random r = new();
            hostPortNum = r.Next(7000,9999);
            controller = new(hostPortNum);
            AddDebugMessage($"Opened port listenere on port {hostPortNum}");
            ChangeOutputContent(new WaitingVM(hostPortNum));
        }
        internal static void ChangeOutputContent(object vm)
        {
            AddDebugMessage($"Changed output Content to {vm.GetType()}");
            shellVM.ChangeOutputContent(vm);
        }
        internal static void CreateNewRoomView(IRoom room)
        {
            if (monitoringVM == null)
            {
                monitoringVM = new MonitoringVM(controller.roomList[0]);
                ChangeOutputContent(monitoringVM);
            }
            else
            {
                monitoringVM.AddTab(room);
            }
        }
        internal static void UpdateMessageBoards(IMessage tempMessage, int roomID)
        {
            _lock.WaitOne();
            monitoringVM.UpdateMessages(tempMessage, roomID);
            _lock.ReleaseMutex();
        }
        internal static void Close()
        {
            if (monitoringVM != null)
            {
                monitoringVM.CloseAllRooms();
            }
            if (controller! != null)
            {
                controller.Kill();
            }

        }
        public static IRoom GetRoom(int index)
        {
            return controller.GetRoom(index);
        }
        internal static void CloseRoom(int index)
        {
            controller.CloseRoom(index);
        }
        internal static void AddDebugMessage(string dmess)
        {
            shellVM.Debug = $"[{DateTime.Now}] -> {dmess}\n";
        }
    }
}
