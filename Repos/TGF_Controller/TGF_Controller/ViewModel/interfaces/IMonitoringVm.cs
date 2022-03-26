using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.ViewModel.interfaces
{
    internal interface IMonitoringVm
    {
        //properties
        public int TabIndex { get; set; }
        public string Header { get; set; }
        public string CloseRoomButtonText { get; set; }
        public string SubjectType { get; set; }
        public string SubjectImgURI { get; set; }
        public string InterviewerType { get; set; }
        public string InterviewerImgURI { get; set; }
        public string PortNumber { get; set; }
        public object Content { get; set; }
        public ObservableCollection<IRoomVM> Tabs { get; set; }

        //Commands
        public ICommand CloseActiveRoom { get; set; }
        public ICommand TabChangeCommand { get; set; }

        //Functions
        public void AddTab(IRoom room);
        public void UpdateTab(int id, IMessage m);
        public void ChangeTab(int index);
        public void UpdateMessages(IMessage tempMessage, int roomID);
        public void CloseRoom(int index);
        public void CloseAllRooms();

    }
}
