using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Controller.Network.intefaces;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Controller.Network.Filters
{
    class SourceFilter : IFilter
    {
        private readonly IPAddress _localAddress;
        public SourceFilter(IPAddress address)
        {
            _localAddress = address;
        }
        public IMessage Run(IMessage message)
        {
            message.Source = _localAddress.ToString();
            return message;
        }
    }
}
