using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TGF_Client.Model;
using TGF_Client.Model.interfaces;

namespace TGF_Client.Client_App.Network.interfaces
{
    internal interface ISocketHandler
    {
        public Stream PrimaryStream { get; set; }
        public StreamReader PrimaryReader { get; set; }
        public StreamWriter PrimaryWriter { get; set; }
        public Stream SecondaryStream { get; set; }
        public StreamReader SecondaryReader { get; set; }
        public StreamWriter SecondaryWriter { get; set; }

        public string SetPort(int port);
        public IMessage Broadcast(string source, string Type, string messagecontents);
        public string Listen();
        public void addFilters(string destinationTag, string sourceTag);
        public void Close(int id);
    }
}
