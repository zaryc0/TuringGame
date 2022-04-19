using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TGF_Client.Client_App.Network.interfaces;
using TGF_Client.Model;
using TGF_Client.Model.interfaces;

namespace TGF_Client.Client_App.Network
{
    class SocketHandler : ISocketHandler
    {
        private int RoomPortNumber1 = 0;
        private int RoomPortNumber2 = 0;

        private TcpClient _primaryClient;
        private TcpClient _secondaryClient;

        private IPAddress controllerAddress;
        private IPAddress localAddress;

        private IPEndPoint _ipEp_Primary;
        private IPEndPoint _ipEp_Secondary;
        private IPEndPoint _ipEp_Controller;

        public Stream PrimaryStream { get; set; }
        public StreamReader PrimaryReader { get; set; }
        public StreamWriter PrimaryWriter { get; set; }

        public Stream SecondaryStream { get; set; }
        public StreamReader SecondaryReader { get; set; }
        public StreamWriter SecondaryWriter { get; set; }
        private IPipe _pipe;
        private IMessage temporaryMessage;

        public SocketHandler(IPAddress controllerAddress, IPAddress localAddress)
        {
            this.controllerAddress = controllerAddress;
            this.localAddress = localAddress;
            _pipe = new Pipe();
            _primaryClient = new TcpClient();
            _secondaryClient = new TcpClient();
        }

        public string SetPort(int port)
        {
            _ipEp_Controller = new IPEndPoint(controllerAddress, port);
            _primaryClient.Connect(_ipEp_Controller);
            PrimaryStream = _primaryClient.GetStream();
            PrimaryReader = new StreamReader(PrimaryStream);
            PrimaryWriter = new StreamWriter(PrimaryStream)
            {
                AutoFlush = true
            };

            temporaryMessage = new Message(PrimaryReader.ReadLine());
            RoomPortNumber1 = int.Parse(temporaryMessage.Content);
            RoomPortNumber2 = RoomPortNumber1 + 5;
            ConnectToSocket(1, RoomPortNumber1);
            ConnectToSocket(2, RoomPortNumber2);
            temporaryMessage = new Message(SecondaryReader.ReadLine());
            return temporaryMessage.Content;
        }
        private void ConnectToSocket(int id, int port)
        {
            if (id == 1)
            {
                _ipEp_Primary = new IPEndPoint(controllerAddress, port);
                _primaryClient.Close();
                _primaryClient = new TcpClient();
                _primaryClient.Connect(_ipEp_Primary);
                PrimaryStream = _primaryClient.GetStream();
                PrimaryReader = new StreamReader(PrimaryStream);
                PrimaryWriter = new StreamWriter(PrimaryStream)
                {
                    AutoFlush = true
                };
            }
            else if (id == 2)
            {
                _ipEp_Secondary = new IPEndPoint(controllerAddress, port);
                _secondaryClient.Connect(_ipEp_Secondary);
                SecondaryStream = _secondaryClient.GetStream();
                SecondaryReader = new StreamReader(SecondaryStream);
                SecondaryWriter = new StreamWriter(SecondaryStream)
                {
                    AutoFlush = true
                };
            }
        }

        public IMessage Broadcast(string source, string Type,string messagecontents)
        {
            IMessage temp = _pipe.ProcessMessage(new Message(controllerAddress.ToString(), source, Type, messagecontents));
            string tempMessage = temp.CompileMessage();
            PrimaryWriter.WriteLine(tempMessage);
            return temp;
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
        public void addFilters(string destinationTag, string sourceTag)
        {
            _pipe.RegisterFilter(new SourceFilter(sourceTag));
            _pipe.RegisterFilter(new DestinationFilter(destinationTag));
        }

        public void Close(int id)
        {
            if(id == 1 && _primaryClient != null)
            {
                if (_primaryClient.Connected)
                {
                    try
                    {
                        _ = Broadcast("this", Constants.Message_Type_Terminate_Tag, "has Disconnected from Room");
                    }
                    catch
                    {

                    }
                    _ = Listen();
                }
                _primaryClient.Close();
            }
            if(id == 2 && _secondaryClient != null)
            {
                _secondaryClient.Close();
            }
        }


    }
}
