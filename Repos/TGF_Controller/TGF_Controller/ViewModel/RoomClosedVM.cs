using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.ViewModel.interfaces;

namespace TGF_Controller.ViewModel
{
    class RoomClosedVM : BaseViewModel, IRoomClosedVM
    {
        //Properties
        private string _text;
        private string _imgURI;

        //Constructors
        public RoomClosedVM()
        {
            TextToDisplay = "The current Room is Not Available\n";
            ImageURI = Constants.Unavailable_img_File_Path;
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

        //Commands

        //Functions

    }
}
