using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TGF_Client.Model;

namespace TGF_Client.Client_App.Network
{
    class SocketHandler
    {
        public string hostName ;
        public TcpClient myClient;
        public IPAddress controllerAddress;
        public int controllerPortNumber = 0;
        public int RoomPortNumber = 0;
        public IPAddress localAddress;
        public Socket socket;
        public Stream myStream;
        public StreamReader reader;
        public StreamWriter writer;
        private Message temporaryMessage;

        public SocketHandler(IPAddress controllerAddress, IPAddress localAddress)
        {
            this.hostName = Dns.GetHostName();
            this.controllerPortNumber = 0;
            this.controllerAddress = controllerAddress;
            this.localAddress = localAddress;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        internal string SetPort(int port)
        {
            controllerPortNumber = port;
            temporaryMessage = ConnectToSocket(port);

            int RoomPortNumber = int.Parse(temporaryMessage.Content);

            temporaryMessage = ConnectToSocket(RoomPortNumber);

            return temporaryMessage.Content;
        }
        private Message ConnectToSocket(int port)
        {
            myClient = new TcpClient(hostName, port);
            myStream = myClient.GetStream();
            reader = new StreamReader(myStream);
            writer = new StreamWriter(myStream)
            {
                AutoFlush = true
            };
            return new Message(Listen());
        }

        public void Broadcast(string Type,string messagecontents)
        {
            Message temp = new Message(controllerAddress.ToString(), localAddress.ToString(), Type, messagecontents);
            string tempMessage = temp.CompileMessage();
            writer.WriteLine(tempMessage);
        }
        public string Listen()
        {
            return reader.ReadLine();
        }
    }
}
