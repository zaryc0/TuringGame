using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public MessageBoard(IMessage message)
        {
            Messages = new List<IMessage>
            {
                message
            };
        }
        public MessageBoard(List<IMessage> messages)
        {
            Messages = messages;
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
