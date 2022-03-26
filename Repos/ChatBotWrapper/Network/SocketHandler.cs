using ChatBotWrapper.Model;
using ChatBotWrapper.Model.interfaces;
using ChatBotWrapper.Network.Filters;
using ChatBotWrapper.Network.intefaces;
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

        private ITSocket _primary_socket;
        private ITSocket _secondary_socket;


        private IPipe _pipe;

        public bool isConnected;
        //Constructor
        public SocketHandler(string port)
        {
            _localAddress = GetControllerIP();
            _pipe = new Pipe();
            _portNumber = int.Parse(port);
            _ipEp_Controller = new IPEndPoint(_localAddress, _portNumber);
            _primary_socket = new TSocket();
            _secondary_socket = new TSocket();
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

            _primary_socket.Connect(_ipEp_Controller);
            IMessage temporaryMessage = new Message(_primary_socket.Read());
            RoomPortNumber1 = int.Parse(temporaryMessage.Content);
            RoomPortNumber2 = RoomPortNumber1 + 5;
            ConnectToSocket(1, RoomPortNumber1);
            ConnectToSocket(2, RoomPortNumber2);
            _ = new Message(_secondary_socket.Read());
        }
        private void ConnectToSocket(int id, int port)
        {
            if (id == 1)
            {
                _ipEp_Primary = new IPEndPoint(_localAddress, port);
                _primary_socket.Close();
                _primary_socket.Connect(_ipEp_Primary);
            }
            else if (id == 2)
            {
                _ipEp_Secondary = new IPEndPoint(_localAddress, port);
                _secondary_socket.Connect(_ipEp_Secondary);
            }
        }
        public void Broadcast(IMessage message)
        {
            IMessage filteredMessage = _pipe.ProcessMessage(message);
            _primary_socket.Write(filteredMessage.CompileMessage());
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
                return new Message(_secondary_socket.Read());
            }
            catch
            {
                return new Message("room", "chatbot", Constants.Message_Type_Visible_Tag, "irrelevant");
            }
        }
        public void Close(int id)
        {
            if (id == 1 && _primary_socket != null)
            {
                if (_primary_socket.IsConnected())
                {
                    Broadcast(new Message(" ","", Constants.Message_Type_Terminate_Tag, "has Disconnected from Room"));
                    _ = Listen();
                }
                _primary_socket.Close();
            }
            if (id == 2 && _secondary_socket != null)
            {
                _secondary_socket.Close();
            }
        }

    }
}
