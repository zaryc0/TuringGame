using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TGF_Controller.Controller.interfaces;

namespace TGF_Controller.ViewModel.interfaces
{
    internal interface IMonitoringVm
    {
        //properties
        public int TabIndex { get; }
        public string Title { get; }

        //Commands
        public ICommand CloseActiveRoom();

        //Functions
        public IMessageboardVM GetMessageBoardVM();


    }
}
