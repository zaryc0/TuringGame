using System;
using System.Collections.ObjectModel;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.Model;
using TGF_Controller.Model.interfaces;
using TGF_Controller.ViewModel.interfaces;

namespace TGF_Controller.ViewModel
{
    class RoomVM : BaseViewModel, IRoomVM
    {
        //Model
        private IRoom _room;
        //Constructor
        public RoomVM(IRoom room)
        {
            _room = room;
            MessageVMs = new ObservableCollection<IMessageVM>();

            foreach (IMessage m in _room.MessageBoard.Messages)
            {
                if (m.Source == Constants.Subject_Tag)
                {
                    MessageVMs.Add(new MessageVM(m));
                }
                else
                {
                    MessageVMs.Add(new Message2VM(m));
                }
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

        public int ID
        {
            get => _room.GetID();
        }

        public string GetRoomName()
        {
            return $"Room{ID + 1}";
        }
        public IRoom GetRoom()
        {
            return _room;
        }

        public void UpdateMessages(IMessage message)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                MessageVMs.Add(new MessageVM(message));
                NotifyPropertyChanged();
            });
        }
    }
}
