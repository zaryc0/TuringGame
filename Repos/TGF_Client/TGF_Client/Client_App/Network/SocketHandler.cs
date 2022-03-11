using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TGF_Client.Model;

namespace TGF_Client.Client_App.Network
{
    class SocketHandler
    {
        private string hostName ; 
        private int controllerPortNumber = 0;
        private int RoomPortNumber1 = 0;
        private int RoomPortNumber2 = 0;

        private TcpClient _primaryClient;
        private TcpClient _secondaryClient;

        private IPAddress controllerAddress;
        private IPAddress localAddress;

        public Stream PrimaryStream;
        public StreamReader PrimaryReader;
        public StreamWriter PrimaryWriter;

        public Stream SecondaryStream;
        public StreamReader SecondaryReader;
        public StreamWriter SecondaryWriter;
        private Message temporaryMessage;

        public SocketHandler(IPAddress controllerAddress, IPAddress localAddress)
        {
            this.hostName = Dns.GetHostName();
            this.controllerPortNumber = 0;
            this.controllerAddress = controllerAddress;
            this.localAddress = localAddress;
        }

        internal string SetPort(int port)
        {
            controllerPortNumber = port;
            ConnectToSocket(1,port);
            temporaryMessage = new Message(PrimaryReader.ReadLine());
            RoomPortNumber1 = int.Parse(temporaryMessage.Content);
            RoomPortNumber2 = RoomPortNumber1 + 5;
            ConnectToSocket(1, RoomPortNumber1);
            ConnectToSocket(2, RoomPortNumber2);
            temporaryMessage = new Message(SecondaryReader.ReadLine());
            return temporaryMessage.Content;
        }
        private void ConnectToSocket(int id,int port)
        {
            if(id == 1) 
            {
                _primaryClient = new TcpClient(hostName, port);
                PrimaryStream = _primaryClient.GetStream();
                PrimaryReader = new StreamReader(PrimaryStream);
                PrimaryWriter = new StreamWriter(PrimaryStream)
                {
                    AutoFlush = true
                };
            }
            else if(id == 2)
            {
                _secondaryClient = new TcpClient(hostName, port);
                SecondaryStream = _secondaryClient.GetStream();
                SecondaryReader = new StreamReader(SecondaryStream);
                SecondaryWriter = new StreamWriter(SecondaryStream)
                {
                    AutoFlush = true
                };
            }

        }

        public void Broadcast(string source, string Type,string messagecontents)
        {
            Message temp = new Message(controllerAddress.ToString(), source, Type, messagecontents);
            string tempMessage = temp.CompileMessage();
            PrimaryWriter.WriteLine(tempMessage);
        }

        public string Listen()
        {
            try
            {
                return SecondaryReader.ReadLine();
            }
            catch
            {
                return $"<CLIENT/>,<CONTROLLER/>,{Constants.Message_Type_Visible_Tag},{DateTime.Now},Connection Terminated,<MessageEnd/>";
            }
        }

        public void Close(int id)
        {
            if(id == 1 && _primaryClient != null)
            {
                _primaryClient.Close();
            }
            if(id == 2 && _secondaryClient != null)
            {
                _secondaryClient.Close();
            }
        }
    }
}
