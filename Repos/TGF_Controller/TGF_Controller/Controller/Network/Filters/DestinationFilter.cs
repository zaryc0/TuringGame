using System;
using System.Net;
using TGF_Controller.Controller.Network.intefaces;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Controller.Network.Filters
{
    internal class DestinationFilter : IFilter
    {
        private readonly IPAddress _address ;
        public DestinationFilter(IPAddress address)
        {
            _address = address;
        }
        public IMessage Run(IMessage message)
        {
            message.Destination = _address.ToString();
            return message;
        }
    }
}
