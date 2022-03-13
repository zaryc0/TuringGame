using ChatBotWrapper.Model.interfaces;

namespace ChatBotWrapper.Network.interfaces
{
    internal interface IFilter
    {
        public IMessage Run(IMessage message);
    }
}
