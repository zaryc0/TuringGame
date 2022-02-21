using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TGF_Client.Model;

namespace TGF_Client
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
            hostName = Dns.GetHostName();
            this.controllerPortNumber = 0;
            this.controllerAddress = controllerAddress;
            this.localAddress = localAddress;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        internal (string,int) SetPort(int port)
        {
            controllerPortNumber = port;
            myClient = new TcpClient(hostName, port);
            myStream = myClient.GetStream();
            reader = new StreamReader(myStream);
            writer = new StreamWriter(myStream)
            {
                AutoFlush = true
            };
            Broadcast(Constants.Message_Type_Init_Tag, "NULL");
            temporaryMessage = new Message(Listen());
            string originalContent = temporaryMessage.Content;
            string[] vs = originalContent.Split(':');
            string R = vs[0];
            int I = int.Parse(vs[1]);
            return (R,I);
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
