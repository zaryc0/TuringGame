using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.ViewModel.interfaces;

namespace TGF_Controller.ViewModel
{
    internal class MonitoringVM
    {
        //model
        public ObservableCollection<IRoomTabVM> RoomTabVMs;

        public MonitoringVM()
        {
            RoomTabVMs = new ObservableCollection<IRoomTabVM>();
        }

        public void UpdateTabs(IRoom room)
        {
            RoomTabVMs.Add(new RoomTabVM(room));
        }
    }
}
