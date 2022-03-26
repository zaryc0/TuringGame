using ChatBotWrapper.Network.intefaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotWrapper.Network
{
    class TSocket : ITSocket
    {
        private TcpClient _client;
        private Stream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;

        public TSocket()
        {
            _client = new();
        }

        public void Close()
        {
            _client.Close();
            _client = new();
        }

        public void Connect(IPEndPoint ipep)
        {
            _client.Connect(ipep);
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream)
            {
                AutoFlush = true
            };
        }

        public bool IsConnected()
        {
            return _client.Connected;
        }

        public string Read()
        {
            return _reader.ReadLine();
        }

        public void Write(string message)
        {
            _writer.WriteLine(message);
        }
    }
}
