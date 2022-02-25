using System;
using TGF_Controller.Controller;
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
            shellVM.ChangeOutputContent(Constants.Monitor_View_ID);
        }
    }
}
