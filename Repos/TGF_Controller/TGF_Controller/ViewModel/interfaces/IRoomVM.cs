using System.Collections.ObjectModel;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.ViewModel.interfaces
{
    interface IRoomVM
    {
        public ObservableCollection<IMessageVM> MessageVMs { get; }
        public string TabTitle { get; }
        public int ID { get; }
        public void UpdateMessages(IMessage message);
        public string GetRoomName();
        public IRoom GetRoom();
    }
}
