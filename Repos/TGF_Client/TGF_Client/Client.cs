using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TGF_Client.Model;

namespace TGF_Client
{
    class Client
    {
        public Config config;
        public List<Message> messages = new List<Message>();
        public SocketHandler socket;

        public Client()
        {
            for(int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    messages.Add(new Message($"10.228.199.121,10.228.199.123,Answer,{DateTime.Now},Nulla id mollis est. Vestibulum massa ligula posuere semper arcu velestas. Nunc tristique odio arcu nec congue quam eleifend sit amet. Aenean in ex nulla. Nam interdum erat turpis. Morbi in enim vitae libero maximus porttitor. Praesent quis fringilla"));
                }
                else
                {
                    messages.Add(new Message($"10.228.199.123,10.228.199.121,Question,{DateTime.Now},Nulla id mollis est. Vestibulum massa ligula posuere semper arcu velestas. Nunc tristique odio arcu nec congue quam eleifend sit amet. Aenean in ex nulla. Nam interdum erat turpis. Morbi in enim vitae libero maximus porttitor. Praesent quis fringilla"));
                }
            }
            config = new Config();
            socket = new SocketHandler(GetControllerIP(),GetLocalIP());
        }

        public void AddMessageToList(Message message)
        {
            messages.Add(message);
        }
        public void AddMessageToList(string type, string content)
        {
            messages.Add(new Message(socket.controllerAddress.ToString(), socket.localAddress.ToString(),type,content));
        }
        public void WaitForMessage()
        {
            messages.Add(new Message(socket.Listen()));
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
    }
}
