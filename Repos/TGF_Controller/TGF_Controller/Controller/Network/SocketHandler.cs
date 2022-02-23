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
        private string hostName;

        private TcpListener _listener;
        private readonly IPAddress _localAddress;
        private IPAddress _clientAddress;
        private int _portNumber;

        private Socket _socket;
        private Stream _myStream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private Pipe _pipe;

        //Constructor
        public SocketHandler(int port)
        {
            _localAddress = GetLocalIP();
            
            _pipe = new Pipe();
            _pipe.RegisterFilter(new DestinationFilter(GetLocalIP()));
            _portNumber = port;
            
            _listener = new TcpListener(_localAddress, _portNumber);
            hostName = Dns.GetHostName();
            _clientAddress = null;
            //socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Start();
            _socket = _listener.AcceptSocket();
            _myStream = new NetworkStream(_socket);
            _reader = new StreamReader(_myStream);
            _writer = new StreamWriter(_myStream)
            {
                AutoFlush = true
            };
        }

        //Functions
        public IPAddress GetLocalIP()
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

        public void Broadcast(IMessage message)
        {
            IMessage filteredMessage =_pipe.ProcessMessage(message);
            _writer.WriteLine(filteredMessage.CompileMessage());
        }

        public void SetClientIP(IPAddress iP)
        {
            _clientAddress = iP;
            _pipe.RegisterFilter(new SourceFilter(iP));
        }

        public IPAddress GetClientIP()
        {
            return _clientAddress;
        }

        public IMessage Listen()
        {
            return new Message(_reader.ReadLine());
        }
    }
}
