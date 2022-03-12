using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Controller.Network.intefaces;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Controller.Network.Filters
{
    class TerminateFilter : IFilter
    {
        private int _roomID;
        public TerminateFilter(int Id)
        {
            _roomID = Id;
        }
        public IMessage Run(IMessage message)
        {
            if (message.TypeTag == Constants.Message_Type_Terminate_Tag)
            {
                if (message.Source == Constants.Subject_Tag)
                {
                    Bus.controller.roomList[_roomID].Subject.Close(1);
                    Bus.controller.roomList[_roomID].Subject.Broadcast(message);
                    Bus.controller.roomList[_roomID].Subject.Close(2);
                    message.Content = "Subject " + message.Content;
                }
                else
                {
                    Bus.controller.roomList[_roomID].Interviewer.Close(1);
                    Bus.controller.roomList[_roomID].Interviewer.Broadcast(message);
                    Bus.controller.roomList[_roomID].Interviewer.Close(2);
                    message.Content = "Interviewer " + message.Content;
                }
                message.TypeTag = Constants.Message_Type_Visible_Tag;
            }
            return message;
        }
    }
}
