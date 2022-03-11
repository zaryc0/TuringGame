using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TGF_Client.Client_App.Network;
using TGF_Client.Model;

namespace TGF_Client.Client_App
{
    class Client
    {
        public Config config;
        public List<Message> messages = new List<Message>();
        public Roles role;
        private SocketHandler _socket;
        private Thread _messageChecker;
        private bool _active;
        private bool _Listening;

        public Client()
        {
            _active = true;
            _Listening = false;
            for(int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    messages.Add(new Message($"{Constants.Subject_Tag},{Constants.Interviewer_Tag},{Constants.Message_Type_Visible_Tag},{DateTime.Now},Nulla id mollis est. Vestibulum massa ligula posuere semper arcu velestas. Nunc tristique odio arcu nec congue quam eleifend sit amet. Aenean in ex nulla. Nam interdum erat turpis. Morbi in enim vitae libero maximus porttitor. Praesent quis fringilla"));
                }
                else
                {
                    messages.Add(new Message($"{Constants.Interviewer_Tag},{Constants.Subject_Tag},{Constants.Message_Type_Visible_Tag},{DateTime.Now},Nulla id mollis est. Vestibulum massa ligula posuere semper arcu velestas. Nunc tristique odio arcu nec congue quam eleifend sit amet. Aenean in ex nulla. Nam interdum erat turpis. Morbi in enim vitae libero maximus porttitor. Praesent quis fringilla"));
                }
            }
            config = new Config();
            _socket = new SocketHandler(GetControllerIP(), GetLocalIP());
            _socket = new SocketHandler(GetControllerIP(), GetLocalIP());
        }

        internal string InitialiseConnection(int port)
        {
            return _socket.SetPort(port);
        }

        internal void SendMessage(string typeTag, string text)
        {
            string source = "";
            if(role == Roles.Interviewer)
            {
                source = Constants.Interviewer_Tag;
            }
            else if(role == Roles.Subject)
            {
                source = Constants.Subject_Tag;
            }
            _socket.Broadcast(source,typeTag, text);
        }

        public void IsListening(bool state)
        {
            _Listening = state;
        }

        public void AddMessageToList(Message message)
        {
            messages.Add(message);
        }
        public void AddMessageToList(string type, string content)
        {
            messages.Add(new Message(GetControllerIP().ToString(), GetLocalIP().ToString(),type,content));
        }
        public void InitialiseThread()
        {
            _messageChecker = new Thread(() => WaitForMessage());
            _messageChecker.Start();
        }
        public void WaitForMessage()
        {
            while (_active)
            {
                string temp = _socket.Listen();
                if (temp != null)
                {
                    Message _M = new Message(temp);
                    Bus.HandleNewMessageRecieved(_M);
                }
            }
        }

        public IPAddress GetControllerIP()
        {
            //return config.controllerIP;
            return IPAddress.Parse("10.228.199.121");
        }

        public IPAddress GetLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public bool Kill()
        {
            _active = false;
            _socket.Close(2);
            if (_messageChecker != null)
            {
                while (_messageChecker.IsAlive) { } 
            }
            _socket.Close(1);
            return true;
        }

    }
}
