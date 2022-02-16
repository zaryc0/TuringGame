using System;
using System.Collections.Generic;
using System.Text;

namespace TGF_Client.Model
{
    class Message
    {
        //Header
        public string Destination { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public string TimeStamp {get; set;}

        //body
        public string Content { get; set; }

        public string CompileMessage()
        {
            return $"{Destination},{Source},{Type},{TimeStamp},{Content},<MessageEnd/>";
        }
        public Message(string message)
        {
            string[] messageContent = message.Split(',');
            Destination = messageContent[0];
            Source = messageContent[1];
            Type = messageContent[2];
            TimeStamp = messageContent[3];
            Content = messageContent[4];

        }
        public Message(string destination, string source, string type, string Body)
        {
            Destination = destination;
            Source = source;
            Type = type;
            TimeStamp = DateTime.Now.ToString();
            Content = Body;
        }
    }
}
