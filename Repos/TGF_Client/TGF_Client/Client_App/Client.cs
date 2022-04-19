﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TGF_Client.Client_App.Network;
using TGF_Client.Model;
using TGF_Client.Model.interfaces;

namespace TGF_Client.Client_App
{
    class Client
    {
        //Variables
        public Config config;
        public List<IMessage> messages = new List<IMessage>();
        public Roles role;
        private SocketHandler _socket;
        private Thread _messageChecker;
        private bool _active;
        private bool _closing;
        //Constructor
        public Client()
        {
            _active = true;
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
        }
        //Functions
        internal void SetRole(Roles newRole)
        {
            role = newRole;
            if (newRole == Roles.Interviewer)
            {
                _socket.addFilters(Constants.Subject_Tag, Constants.Interviewer_Tag);
            }
            else
            {
                _socket.addFilters(Constants.Interviewer_Tag, Constants.Subject_Tag);
            }

        }
        internal string InitialiseConnection(int port)
        {
            return _socket.SetPort(port);
        }
        internal IMessage SendMessage(string typeTag, string text)
        {
            string source = Bus.client.role == Roles.Interviewer ? Constants.Interviewer_Tag : Constants.Subject_Tag;
            IMessage temp = _socket.Broadcast(source, typeTag, text);
            messages.Add(temp);
            return temp;
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
                    if (_M.Content == "Connection Terminated")
                    {
                        _active = false;
                    }
                    if (_active)
                    {
                        Bus.HandleNewMessageRecieved(_M, 1); 
                    }
                }
            }
        }
        public IPAddress GetControllerIP()
        {
            return IPAddress.Parse(config.d["Controller IP"]);
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
            _socket.Close(1);
            return true;
        }

    }
}
