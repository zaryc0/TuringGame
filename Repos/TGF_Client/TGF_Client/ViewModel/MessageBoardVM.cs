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
        }
        public void UpdateMessages(IMessage message,int type)
        {
            IMessageVM m = new MessageVM(message);
            if (type == 0)
            {
                m = new MessageVM(message)
                {
                    Source = "you"
                };
            }
            else if (type == 1)
            {
                m = new Message2VM(message)
                {
                    Source = "???"
                };
            }
            App.Current.Dispatcher.Invoke(delegate
            {
                Messages.Add(m);
                NotifyPropertyChanged();
            });
        }
    }
}
