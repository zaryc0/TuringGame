using System.Collections.Generic;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Model
{
    class MessageBoard : IMessageBoard
    {
        //Properties
        public List<IMessage> Messages { get; set; }
        //Constructors
        public MessageBoard()
        {
            Messages = new List<IMessage>();
        }
        //Functions
        public void AddMessage(IMessage message)
        {
            Messages.Add(message);
        }
        public IMessage GetMessage(int id)
        {
            return Messages[id];
        }
    }
}
