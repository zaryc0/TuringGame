using ChatBotWrapper.Model.interfaces;
using ChatBotWrapper.Network.interfaces;
using System.Collections.Generic;

namespace ChatBotWrapper.Network
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
