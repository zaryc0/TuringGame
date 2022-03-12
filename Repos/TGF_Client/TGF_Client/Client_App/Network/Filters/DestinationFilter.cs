using TGF_Client.Model.interfaces;

namespace TGF_Client.Client_App.Network.interfaces
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
