using ChatBotWrapper.Model;
using ChatBotWrapper.Model.interfaces;
using ChatBotWrapper.Network.Filters;
using ChatBotWrapper.Network.interfaces;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ChatBotWrapper.Network
{
    class SocketHandler : ISocketHandler
    {
        //Properties
        private int _controllerportnumber;
        private IPAddress _localAddress;
        private IPEndPoint _ipEp_Primary;
        private IPEndPoint _ipEp_Secondary;

        private int _portNumber;
        private int RoomPortNumber1 = 0;
        private int RoomPortNumber2 = 0;

        private TcpClient _primaryClient;
        private TcpClient _secondaryClient;

        private Stream _primaryStream;
        private StreamReader _primaryReader;
        private StreamWriter _primaryWriter;

        private Stream _secondaryStream;
        private StreamReader _secondaryReader;
        private StreamWriter _secondaryWriter;

        private Pipe _pipe;

        public bool isConnected;
        //Constructor
        public SocketHandler(string port, string controllerIP)
        {
            _ipEp_Primary = new IPEndPoint(IPAddress.Parse(controllerIP), int.Parse(port));
            _localAddress = IPAddress.Parse(controllerIP);
            _pipe = new Pipe();
            _portNumber = int.Parse(port);
        }

        //Functions
        public void SetPort(int port)
        {
            _controllerportnumber = port;
            ConnectToSocket(1, port);
            IMessage temporaryMessage = new Message(_primaryReader.ReadLine());
            RoomPortNumber1 = int.Parse(temporaryMessage.Content);
            RoomPortNumber2 = RoomPortNumber1 + 5;
            ConnectToSocket(1, RoomPortNumber1);
            ConnectToSocket(2, RoomPortNumber2);
            _ = new Message(_secondaryReader.ReadLine());
        }
        private void ConnectToSocket(int id, int port)
        {
            if (id == 1)
            {
                _ipEp_Primary = new IPEndPoint(_localAddress, port);
                _primaryClient = new TcpClient(_ipEp_Primary);
                while (!_primaryClient.Connected) { }
                _primaryStream = _primaryClient.GetStream();
                _primaryReader = new StreamReader(_primaryStream);
                _primaryWriter = new StreamWriter(_primaryStream)
                {
                    AutoFlush = true
                };
            }
            else if (id == 2)
            {
                _ipEp_Secondary.Port = new IPEndPoint(_localAddress, port);
                _secondaryClient = new TcpClient(_ipEp_Secondary);
                while (!_primaryClient.Connected) { }
                _secondaryStream = _secondaryClient.GetStream();
                _secondaryReader = new StreamReader(_secondaryStream);
                _secondaryWriter = new StreamWriter(_secondaryStream)
                {
                    AutoFlush = true
                };
            }
        }

        public void Broadcast(IMessage message)
        {
            IMessage filteredMessage = _pipe.ProcessMessage(message);
            _primaryWriter.WriteLine(filteredMessage.CompileMessage());
        }

        public void AddFilters(string destinationTag, string sourceTag)
        {
            _pipe.RegisterFilter(new SourceFilter(sourceTag));
            _pipe.RegisterFilter(new DestinationFilter(destinationTag));
        }

        public IMessage Listen()
        {
            try
            {
                return new Message(_secondaryReader.ReadLine());
            }
            catch
            {
                return new Message("room", "chatbot", Constants.Message_Type_Visible_Tag, "irrelevant");
            }
        }

        public void Close(int id)
        {
            if (id == 1 && _primaryClient != null)
            {
                if (_primaryClient.Connected)
                {
                    Broadcast(new Message(" ","", Constants.Message_Type_Terminate_Tag, "has Disconnected from Room"));
                    _ = Listen();
                }
                _primaryClient.Close();
            }
            if (id == 2 && _secondaryClient != null)
            {
                _secondaryClient.Close();
            }
        }

    }
}
