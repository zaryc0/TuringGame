using TGF_Client.Model.interfaces;

namespace TGF_Client.Client_App.Network.interfaces
{
    internal interface IPipe
    {
        public void RegisterFilter(IFilter filter);
        public IMessage ProcessMessage(IMessage message);
    }
}
