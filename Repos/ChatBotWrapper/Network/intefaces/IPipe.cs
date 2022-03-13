using ChatBotWrapper.Model.interfaces;

namespace ChatBotWrapper.Network.interfaces
{
    internal interface IPipe
    {
        public void RegisterFilter(IFilter filter);
        public IMessage ProcessMessage(IMessage message);
    }
}
