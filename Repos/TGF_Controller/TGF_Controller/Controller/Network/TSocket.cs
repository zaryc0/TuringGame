using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Controller.Network.intefaces;

namespace TGF_Controller.Controller.Network
{
    class TSocket : ITSocket
    {
        private TcpListener _listener;
        private Socket _socket;
        private Stream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;

        public TSocket(IPAddress address, int portNumber)
        {
            _listener = new TcpListener(address, portNumber);
        }

        public void AcceptConnection()
        {
            _listener.Start();
            _socket = _listener.AcceptSocket();
            _stream = new NetworkStream(_socket);
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream)
            {
                AutoFlush = true
            };
        }
        public void Close()
        {
            _socket.Close();
        }
        public string Read()
        {
            return _reader.ReadLine();
        }
        public void Write(string message)
        {
            try
            {
                _writer.WriteLine(message);
            }
            catch
            {

            }
        }
    }
}
