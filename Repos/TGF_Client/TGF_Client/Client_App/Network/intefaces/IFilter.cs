using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Client.Model.interfaces;

namespace TGF_Client.Client_App.Network.interfaces
{
    internal interface IFilter
    {
        public IMessage Run(IMessage message);
    }
}
