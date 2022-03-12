using System;
using System.Collections.Generic;
using System.Text;
using TGF_Client.Client_App.Network.interfaces;
using TGF_Client.Model.interfaces;

namespace TGF_Client.Client_App.Network
{
    class Pipe : IPipe
    {
        private List<IFilter> _filters;
        public Pipe()
        {
            _filters = new List<IFilter>();
        }
        public IMessage ProcessMessage(IMessage message)
        {
            foreach (IFilter filter in _filters)
            {
                message = filter.Run(message);
            }
            return message;
        }

        public void RegisterFilter(IFilter filter)
        {
            _filters.Add(filter);
        }
    }
}
