using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Controller.Network.intefaces
{
    internal interface IFilter
    {
        public IMessage Run(IMessage message);
    }
}
