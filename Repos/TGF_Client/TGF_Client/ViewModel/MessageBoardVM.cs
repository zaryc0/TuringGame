using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TGF_Client.Model;
using TGF_Client.Model.interfaces;
using TGF_Client.ViewModel.interfaces;

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
                UpdateMessages(m);
            }
        }
        public void UpdateMessages(IMessage message)
        {
            IMessageVM m = new MessageVM(message);
            if (Bus.client.role == Roles.Interviewer)
            {
                if (message.Source == Constants.Interviewer_Tag)
                {
                    m = new MessageVM(message);
                }
                else if (message.Source == Constants.Subject_Tag)
                {
                    m = new Message2VM(message);
                }
            }
            else if (Bus.client.role == Roles.Subject)
            {
                if (message.Source == Constants.Interviewer_Tag)
                {
                    m = new Message2VM(message);
                }
                else if (message.Source == Constants.Subject_Tag)
                {
                    m = new MessageVM(message);
                }
            }
            App.Current.Dispatcher.Invoke(delegate
            {
                Messages.Add(m);
                NotifyPropertyChanged();
            });
        }
    }
}
