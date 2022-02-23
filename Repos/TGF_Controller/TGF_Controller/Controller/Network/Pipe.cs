using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Controller.Network.intefaces;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Controller.Network
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
            foreach(IFilter filter in _filters)
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
