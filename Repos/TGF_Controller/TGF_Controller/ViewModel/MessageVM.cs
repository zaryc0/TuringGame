﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Model.interfaces;
using TGF_Controller.ViewModel.interfaces;

namespace TGF_Controller.ViewModel
{
    internal class MessageVM : BaseViewModel, IMessageVM
    {
        //Model
        private IMessage _m;

        //Constructor
        public MessageVM(IMessage m)
        {
            _m = m;
        }
        //Property masks
        public string Destination
        {
            get => _m.Destination;
            set
            {
                if (_m.Destination != value)
                {
                    _m.Destination = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Source
        {
            get => _m.Source;
            set
            {
                if (_m.Source != value)
                {
                    _m.Source = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Type
        {
            get => _m.TypeTag;
            set
            {
                if (_m.TypeTag != value)
                {
                    _m.TypeTag = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string TimeStamp
        {
            get => _m.TimeStamp;
            set
            {
                if (_m.TimeStamp != value)
                {
                    _m.TimeStamp = value;
                    NotifyPropertyChanged();
                }
            }

        }
        public string Content { get => _m.Content;
            set
            {
                if (_m.Content != value)
                {
                    _m.Content = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Sender { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //Functions
        public IMessageVM GetMessageVM()
        {
            return this;
        }
    }
}
