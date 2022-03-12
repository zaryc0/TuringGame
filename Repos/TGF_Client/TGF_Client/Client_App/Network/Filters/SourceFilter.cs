using TGF_Client.Model.interfaces;

namespace TGF_Client.Client_App.Network.interfaces
{
    class SourceFilter : IFilter
    {
        private readonly string _source;
        public SourceFilter(string SourceTag)
        {
            _source = SourceTag;
        }
        public IMessage Run(IMessage message)
        {
            message.Source = _source;
            return message;
        }
    }
}
