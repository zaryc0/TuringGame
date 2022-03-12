using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Controller;
using TGF_Controller.Controller.interfaces;
using TGF_Controller.Model.interfaces;
using TGF_Controller.ViewModel.interfaces;

namespace TGF_Controller.ViewModel
{
    class RoomClosedVM : BaseViewModel, IRoomClosedVM , IRoomVM
    {
        //Properties
        private string _text;
        private string _imgURI;

        //Constructors
        public RoomClosedVM()
        {
            TextToDisplay = "The current Room is Not Available\n";
            ImageURI = Constants.Unavailable_img_File_Path;
            MessageVMs = new();
            TabTitle = "";
            ID = 0;
        }

        //Property Masks
        public string TextToDisplay
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value; NotifyPropertyChanged();
                }
            }
        }
        public string ImageURI
        {
            get => _imgURI;
            set
            {
                if (value != _imgURI)
                {
                    _imgURI = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<IMessageVM> MessageVMs { get; private set; }

        public string TabTitle { get; private set; }

        public int ID { get; private set; }

        //Commands

        //Functions
        public IRoom GetRoom()
        {
            return new Room();
        }

        public string GetRoomName()
        {
            throw new NotImplementedException();
        }

        public void UpdateMessages(IMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
