using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TGF_Controller.Controller.Network.interfaces;
using TGF_Controller.Model.interfaces;
using TGF_Controller.Model;
using TGF_Controller.Controller.Network.Filters;
using TGF_Controller.Controller.Network.intefaces;

namespace TGF_Controller.Controller.Network
{
    class SocketHandler : ISocketHandler
    {
        //Properties
        private readonly string hostName;

        
        private IPAddress _localAddress;
        private int _portNumber;

        private ITSocket _primary_socket;

        private ITSocket _secondary_socket;


        private Pipe _pipe;

        public bool isConnected;
        //Constructor
        public SocketHandler(int port)
        {
            hostName = Dns.GetHostName();
            _localAddress = GetLocalIP();
            _pipe = new Pipe();
            _portNumber = port;
            _primary_socket = new TSocket(_localAddress, _portNumber);
            _secondary_socket = new TSocket(_localAddress, _portNumber + 5);
        }

        //Functions
        public IPAddress GetLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(hostName);
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public void Broadcast(IMessage message)
        {
            IMessage filteredMessage =_pipe.ProcessMessage(message);
            _secondary_socket.Write(message.CompileMessage());
        }
        public void BroadcastOnPrimary(IMessage message)
        {
            IMessage filteredMessage = _pipe.ProcessMessage(message);
            _primary_socket.Write(filteredMessage.CompileMessage());
        }
        public void SetFilters(int roomID)
        {
            _pipe.RegisterFilter(new TerminateFilter(roomID));
        }
        public IMessage Listen()
        {
            try
            {
                return _pipe.ProcessMessage(new Message(_primary_socket.Read()));
            }
            catch
            {
                return new Message("room", "room", Constants.Message_Type_Terminate_Tag, "irrelevant");
            }
        }
        public bool WaitForPrimaryConnection()
        {
            _primary_socket.AcceptConnection();
            return true;
        }
        public bool WaitForSecondaryConnection()
        {
            _secondary_socket.AcceptConnection();
            return true;
        }
        public void Close(int id)
        {
            if (_primary_socket != null && id == 1)
            {
                _primary_socket.Close();
            }
            else if (_secondary_socket != null && id == 2)
            {
                _secondary_socket.Close();
            }
        }
    }
}
