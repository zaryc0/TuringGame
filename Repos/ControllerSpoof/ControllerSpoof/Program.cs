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
        public string role;
        public bool connected;
        public int roomPort;
        static void Main(string[] args)
        {
            _ = new Program();
        }
        public Program()
        {
            role = "<INTERVIEWER/>";
            roomPort = 3214;
            hostName = Dns.GetHostName();
            ipList = Dns.GetHostEntry(hostName).AddressList;
            Console.WriteLine(hostName);
            Console.WriteLine($"Start,{4839}");
            Console.WriteLine("waiting For Connection");
            TcpListener listener = new(ipList[0], 4839);
            listener.Start();
            socket = listener.AcceptSocket();
            Console.WriteLine("Connection established, assigning role");
            Stream myStream = new NetworkStream(socket);
            StreamReader reader = new(myStream);
            StreamWriter writer = new(myStream);
            writer.AutoFlush = true;
            Console.WriteLine(reader.ReadLine());
            writer.WriteLine($"<CLIENT/>,<CONTROLLER/>,<ASSIGNMENT/>,{DateTime.Now},{role}:{roomPort},<MessageEnd/>");
            connected = true;
            while (connected)
            {
                try
                {
                    string input = reader.ReadLine();
                    string[] message = input.Split(',');
                    if(MesageisNotValid(message))
                    {
                        Console.WriteLine(input);
                        Console.WriteLine("message was corrupt");
                        break;
                    }
                    if (message[2] == "<TERMINATE/>")
                    {
                        connected = false;
                    }
                    Console.WriteLine(input);
                    writer.WriteLine($"<CLIENT/>,<CONTROLLER/>,<ANSWER/>,{DateTime.Now},{roomPort++},<MessageEnd/>");
                }
                catch
                {
                    Console.WriteLine("ERROROROROROROORRORORR");
                    connected = false;
                }

            }
            socket.Close();

        }

        private static bool MesageisNotValid(string[] input)
        {
            if (input.Length != 6)
            {
                Console.WriteLine($"[ERROR] - Message Length Incorrect: {input.Length}");
                return true;
            }
            if (input[5] != "<MESSAGEEND/>") return true;
                
            return false;
        }
    }
}
