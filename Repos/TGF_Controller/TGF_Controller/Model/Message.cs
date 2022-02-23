using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Controller;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Model
{
    class Message : IMessage
    {
        //Properties
        public string Destination { get; set; }
        public string Source { get; set; }
        public string TypeTag { get; set; }
        public string TimeStamp { get; set; }
        public string Content { get; set; }

        //Constructors
        public Message(string message)
        {
            string[] messageContent = message.Split(',');
            Destination = messageContent[0];
            Source = messageContent[1];
            TypeTag = messageContent[2];
            TimeStamp = messageContent[3];
            Content = messageContent[4];
        }

        public Message(string destination, string source, string type, string Body)
        {
            Destination = destination;
            Source = source;
            TypeTag = type;
            TimeStamp = DateTime.Now.ToString();
            Content = Body;
        }

        //Functions
        public string CompileMessage()
        {
            return $"{Destination},{Source},{TypeTag},{TimeStamp},{Content},{Constants.Message_End_Tag}";
        }

        public bool MessageIsNotValid(string[] input)
        {
            return input.Length != 6 || input[5] != Constants.Message_End_Tag;
        }
    }
}

