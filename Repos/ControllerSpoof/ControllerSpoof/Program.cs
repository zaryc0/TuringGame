using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ControllerSpoof
{
    class Program
    {
        public IPAddress[] ipList;
        public string hostName;
        public Socket socket;
        public bool connected;
        public int roomPort;
        static void Main(string[] args)
        {
            _ = new Program();
        }
        public Program()
        {
            hostName = Dns.GetHostName();
            ipList = Dns.GetHostEntry(hostName).AddressList;
            TcpListener listener = new(ipList[0], 4839);

            for (int i = 0; i < 10; i++)
            {
                roomPort = 3000 + i;
                
                Console.WriteLine(hostName);
                Console.WriteLine($"Start,{4839}");
                
                Console.WriteLine("waiting For Connection");
                listener.Start();
                socket = listener.AcceptSocket();
                Console.WriteLine("Connection established, assigning role");
                Stream myStream = new NetworkStream(socket);
                StreamReader reader = new(myStream);
                StreamWriter writer = new(myStream);
                writer.AutoFlush = true;
                writer.WriteLine($"<CLIENT/>,<CONTROLLER/>,<ASSIGNMENT/>,{DateTime.Now},{roomPort},<MessageEnd/>");
                connected = true;
                socket.Close();
                Console.WriteLine("socketClosed");
            
            }
        }
    }
}
