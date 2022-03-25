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
        private IPEndPoint _ipEp_Controller;

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
        public SocketHandler(string port)
        {
            _localAddress = GetControllerIP();
            _pipe = new Pipe();
            _portNumber = int.Parse(port);
            _ipEp_Controller = new IPEndPoint(_localAddress, _portNumber);
            _primaryClient = new TcpClient();
            _secondaryClient = new TcpClient();
        }
        private IPAddress GetControllerIP()
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


        //Functions
        public void SetPort(int port)
        {
            _controllerportnumber = port;

            _primaryClient.Connect(_ipEp_Controller);
            _primaryStream = _primaryClient.GetStream();
            _primaryReader = new StreamReader(_primaryStream);
            _primaryWriter = new StreamWriter(_primaryStream)
            {
                AutoFlush = true
            };

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
                _primaryClient.Close();
                _primaryClient = new TcpClient();
                _primaryClient.Connect(_ipEp_Primary);
                _primaryStream = _primaryClient.GetStream();
                _primaryReader = new StreamReader(_primaryStream);
                _primaryWriter = new StreamWriter(_primaryStream)
                {
                    AutoFlush = true
                };
            }
            else if (id == 2)
            {
                _ipEp_Secondary = new IPEndPoint(_localAddress, port);
                _secondaryClient.Connect(_ipEp_Secondary);
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
            _pipe.RegisterFilter(new TagFilter(Constants.Message_Type_Visible_Tag));
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
