using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF_Controller.Model.interfaces
{
    interface IMessageBoard
    {
        //Properties
        public List<IMessage> Messages { get; set; }

        //Functions
        public void AddMessage(IMessage message);
        public IMessage GetMessage(int id);
    }
}
