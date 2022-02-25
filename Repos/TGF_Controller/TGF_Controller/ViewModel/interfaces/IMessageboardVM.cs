using System.Collections.ObjectModel;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.ViewModel.interfaces
{
    interface IMessageboardVM
    {
        public ObservableCollection<IMessageVM> Messages { get; }
        public void UpdateMessages(IMessage message);
    }
}
