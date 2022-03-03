﻿using System;
using System.Collections.Generic;
using System.Text;
using TGF_Client.Model;

namespace TGF_Client.ViewModel
{
    class MessageVM : BaseViewModel , IMessageVM
    {
        private Message _message;
        //Properties
        public string Sender
        {
            get => Bus.GetSender(Source);

        }

        public string Destination
        {
            get => _message.Destination;
            set
            {
                if (value != _message.Destination)
                {
                    _message.Destination = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Source 
        {
            get => _message.Source;
            set
            {
                if(value != _message.Source)
                {
                    _message.Destination = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Type
        {
            get => _message.TypeTag;
            set
            {
                if(value != _message.TypeTag)
                {
                    _message.TypeTag = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string TimeStamp 
        { 
            get => _message.TimeStamp;
            set
            {
                if(value != _message.TimeStamp)
                {
                    _message.TimeStamp = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Content
        {
            get => _message.Content;
            set
            {
                if (value != _message.Content)
                {
                    _message.Content = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public MessageVM(Message message)
        {
            _message = message;
        }
    }
}
