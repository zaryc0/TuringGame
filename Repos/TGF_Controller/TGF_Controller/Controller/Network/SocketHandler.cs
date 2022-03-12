using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TGF_Controller.Controller.Network.interfaces;
using TGF_Controller.Model.interfaces;
using TGF_Controller.Model;
using TGF_Controller.Controller.Network.Filters;

namespace TGF_Controller.Controller.Network
{
    class SocketHandler : ISocketHandler
    {
        //Properties
        private readonly string hostName;

        
        private IPAddress _localAddress;
        private int _portNumber;

        private TcpListener _primaryListener;
        private Socket _primarySocket;
        private Stream _primaryStream;
        private StreamReader _primaryReader;
        private StreamWriter _primaryWriter;

        private TcpListener _secondaryListener;
        private Socket _secondarySocket;
        private Stream _secondaryStream;
        private StreamReader _secondaryReader;
        private StreamWriter _secondaryWriter;

        private Pipe _pipe;

        public bool isConnected;
        //Constructor
        public SocketHandler(int port)
        {
            hostName = Dns.GetHostName();
            _localAddress = GetLocalIP();
            _pipe = new Pipe();
            _portNumber = port;
            _primaryListener = new TcpListener(_localAddress, _portNumber);
            _secondaryListener = new TcpListener(_localAddress, _portNumber + 5);


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
            _secondaryWriter.WriteLine(message.CompileMessage());
        }

        public void BroadcastOnPrimary(IMessage message)
        {
            IMessage filteredMessage = _pipe.ProcessMessage(message);
            _primaryWriter.WriteLine(filteredMessage.CompileMessage());
        }

        public void SetFilters(int roomID)
        {
            _pipe.RegisterFilter(new TerminateFilter(roomID));
        }

        public IMessage Listen()
        {
            try
            {
                return _pipe.ProcessMessage(new Message(_primaryReader.ReadLine()));
            }
            catch 
            {
                return new Message("room", "room", Constants.Message_Type_Visible_Tag, "irrelevant");
            }
        }

        public bool WaitForPrimaryConnection()
        {
            _primaryListener.Start();
            _primarySocket = _primaryListener.AcceptSocket();
            _primaryStream = new NetworkStream(_primarySocket);
            _primaryReader = new StreamReader(_primaryStream);
            _primaryWriter = new StreamWriter(_primaryStream)
            {
                AutoFlush = true
            };
            return true;
        }
        public bool WaitForSecondaryConnection()
        {
            _secondaryListener.Start();
            _secondarySocket = _secondaryListener.AcceptSocket();
            _secondaryStream = new NetworkStream(_secondarySocket);
            _secondaryReader = new StreamReader(_secondaryStream);
            _secondaryWriter = new StreamWriter(_secondaryStream)
            {
                AutoFlush = true
            };
            return true;
        }

        public void Close(int id)
        {
            if (_primarySocket != null && id == 1)
            {
                _primarySocket.Close();
            }
            else if (_secondarySocket != null && id == 2)
            {
                _secondarySocket.Close();
            }
        }

    }
}
