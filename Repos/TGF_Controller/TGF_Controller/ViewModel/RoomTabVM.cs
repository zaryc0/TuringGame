using System;
using System.Collections.ObjectModel;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.Model.interfaces;
using TGF_Controller.ViewModel.interfaces;

namespace TGF_Controller.ViewModel
{
    class RoomTabVM : BaseViewModel, IRoomTabVM
    {
        //Model
        private IRoom _room;
        //Constructor
        public RoomTabVM(IRoom room)
        {
            _room = room;
            MessageVMs = new ObservableCollection<IMessageVM>();
            foreach (IMessage m in _room.MessageBoard.Messages)
            {
                MessageVMs.Add(new MessageVM(m));
            }
        }

        //Property masks
        public ObservableCollection<IMessageVM> MessageVMs { get; private set; }
        public string TabTitle
        {
            get => _room.RoomName;
            set
            {
                if (_room.RoomName != value)
                {
                    _room.RoomName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void UpdateMessages(IMessage message)
        {
            _room.MessageBoard.AddMessage(message);
            MessageVMs.Add(new MessageVM(message));
            NotifyPropertyChanged();
        }
    }
}
