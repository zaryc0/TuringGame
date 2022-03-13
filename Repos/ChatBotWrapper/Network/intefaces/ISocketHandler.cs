using ChatBotWrapper.Model.interfaces;
using System.IO;

namespace ChatBotWrapper.Network.interfaces
{
    internal interface ISocketHandler
    {
        public void SetPort(int port);
        public void Broadcast(IMessage message);
        public IMessage Listen();
        public void AddFilters(string destinationTag, string sourceTag);
        public void Close(int id);
    }
}
