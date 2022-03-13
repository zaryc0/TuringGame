using ChatBotWrapper.Model.interfaces;
using ChatBotWrapper.Network.interfaces;

namespace ChatBotWrapper.Network.Filters
{
    internal class DestinationFilter : IFilter
    {
        private readonly string _address;
        public DestinationFilter(string DestinationTag)
        {
            _address = DestinationTag;
        }
        public IMessage Run(IMessage message)
        {
            message.Destination = _address;
            return message;
        }
    }
}
