using ChatBotWrapper.Model.interfaces;
using ChatBotWrapper.Network.interfaces;

namespace ChatBotWrapper.Network.Filters
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
