using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF_Controller.Controller.Network.intefaces
{
    interface ITSocket
    {
        public void AcceptConnection();
        public void Close();
        public string Read();
        public void Write(string message);
    }
}
