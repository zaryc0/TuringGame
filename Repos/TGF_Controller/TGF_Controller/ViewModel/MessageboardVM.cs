using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Model.interfaces;
using TGF_Controller.ViewModel.interfaces;

namespace TGF_Controller.ViewModel
{
    internal class MessageboardVM : BaseViewModel, IMessageboardVM
    {
        public MessageboardVM(IMessageBoard mB)
        {
            Messages = new ObservableCollection<IMessageVM>();
            foreach (IMessage m in mB.Messages)
            {
                Messages.Add(new MessageVM(m));
            }
        }

        public ObservableCollection<IMessageVM> Messages { get; private set; }

        public void UpdateMessages(IMessage message)
        {
            Messages.Add(new MessageVM(message));
            NotifyPropertyChanged();
        }
    }
}
