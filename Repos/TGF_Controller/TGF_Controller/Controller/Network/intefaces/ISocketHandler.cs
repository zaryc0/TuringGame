using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Controller.Network.interfaces
{
    interface ISocketHandler
    {
        public void Broadcast(IMessage message);
        public IMessage Listen();
        public void SetClientIP(IPAddress iP);
        public IPAddress GetClientIP();
        public IPAddress GetLocalIP();
    }
}
