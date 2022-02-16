using System;
using System.Net;
using System.Net.Sockets;
using TGF_Client.Model;

namespace TGF_Client
{
    class SocketHandler
    {
        public IPAddress controllerAddress;
        public int controllerPortNumber;
        public IPAddress localAddress;
        public Socket socket;

        public SocketHandler(IPAddress controllerAddress, IPAddress localAddress)
        {
            this.controllerPortNumber = 0;
            this.controllerAddress = controllerAddress;
            this.localAddress = localAddress;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        internal int SetPort(int port)
        {
            controllerPortNumber = port;
            Broadcast("initialise", "NULL");
            Listen();
            return 1;
          //  return 2;
        }

        public void Broadcast(string Type,string messagecontents)
        {
            new Message(controllerAddress.ToString(), localAddress.ToString(), "initialise", "NULL");
            //broadcast
            return;
        }
        public string Listen()
        {
            //listen
            return"";
        }
    }
}
