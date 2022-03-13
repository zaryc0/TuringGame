using ChatBotWrapper.Model;
using ChatBotWrapper.Network;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ChatBotWrapper
{
    class ChatBot
    {
        private SocketHandler _socket;
        private bool _active;
        static void Main(string[] args)
        {
            _ = new ChatBot(args);
        }

        public ChatBot(string[] argv)
        {
            _active = true;
            _socket = new SocketHandler(argv[0],argv[1]);
            Process chatter = new();
            ProcessStartInfo thisInfo = new ProcessStartInfo();
            thisInfo.FileName = "../../../Python_def/python.exe";
            thisInfo.Arguments = "../../../Chatter.py";
            thisInfo.RedirectStandardInput = true;
            thisInfo.RedirectStandardOutput = true;
            thisInfo.UseShellExecute = false;

            chatter.StartInfo = thisInfo;
            chatter.Start();

            StreamWriter ProcessWriter = chatter.StandardInput;
            StreamReader ProcessReader = chatter.StandardOutput;

            _socket.SetPort(int.Parse(argv[0]));

            while (_active)
            {
                _socket.Broadcast(new Message("", "", "", ProcessReader.ReadLine()));
                string messageText = _socket.Listen().Content;
                ProcessWriter.WriteLine(messageText);
            }
        }
    }
}
