using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TGF_Controller.Model;
using TGF_Controller.Model.interfaces;

namespace TGF_Controller.Controller
{
    class SocketHandler
    {
        //Properties
        public string hostName;

        private TcpClient myClient;
        private IPAddress localAddress;
        private IPAddress clientAddress;
        private int portNumber;

        private Socket socket;
        private Stream myStream;
        private StreamReader reader;
        private StreamWriter writer;

        private IMessage temporaryMessage;

        //Constructor
        public SocketHandler(int port)
        {
            portNumber = port;
            hostName = Dns.GetHostName();
            localAddress = GetLocalIP();
            clientAddress = null;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            myClient = new TcpClient(hostName, port);
            myStream = myClient.GetStream();
            reader = new StreamReader(myStream);
            writer = new StreamWriter(myStream)
            {
                AutoFlush = true
            };
        }

        //Functions
        public static IPAddress GetLocalIP()
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
            IMessage filteredMessage = message;
            filteredMessage.Destination = clientAddress.ToString();
            filteredMessage.Source = localAddress.ToString();
            writer.WriteLine(filteredMessage.CompileMessage());
        }

        public IMessage Listen()
        {
            return new Message(reader.ReadLine());
        }
    }
}
