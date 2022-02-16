using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TGF_Client.Model;

namespace TGF_Client.ViewModel
{
    class MessageBoardVM : BaseViewModel
    {
        public ObservableCollection<IMessageVM> Messages { get; set; }

        public MessageBoardVM()
        {
            Messages = new ObservableCollection<IMessageVM>();
            foreach (Message m in Bus.GetMessages())
            {
                Messages.Add(new MessageVM(m));
            }
        }
        public void UpdateMessages(Message message)
        {
            Messages.Add(new MessageVM(message));
            NotifyPropertyChanged();
        }
    }
}
