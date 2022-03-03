using System.Collections.ObjectModel;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.ViewModel.interfaces
{
    interface IRoomTabVM
    {
        public ObservableCollection<IMessageVM> MessageVMs { get; }
        public void UpdateMessages(IMessage message);
        public string TabTitle { get; }
    }
}
