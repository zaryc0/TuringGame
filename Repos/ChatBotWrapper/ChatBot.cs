using ChatBotWrapper.Model;
using ChatBotWrapper.Model.interfaces;
using ChatBotWrapper.Network;
using ChatBotWrapper.Network.interfaces;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ChatBotWrapper
{
    class ChatBot
    {
        private ISocketHandler _socket;
        private bool _active;
        private IMessage m;
        static void Main(string[] args)
        {
            _ = new ChatBot(args);
        }

        public ChatBot(string[] argv)
        {
            _active = true;
            _socket = new SocketHandler(argv[0]);
            Process chatter = new();
            ProcessStartInfo thisInfo = new ProcessStartInfo();
            thisInfo.FileName = "../../../chatbot/dist/chatter.exe";
            thisInfo.RedirectStandardInput = true;
            thisInfo.RedirectStandardOutput = true;
            thisInfo.UseShellExecute = false;

            chatter.StartInfo = thisInfo;
            chatter.Start();

            StreamWriter ProcessWriter = chatter.StandardInput;
            StreamReader ProcessReader = chatter.StandardOutput;

            _socket.SetPort(int.Parse(argv[0]));
            _socket.AddFilters(Constants.Interviewer_Tag, Constants.Subject_Tag);
            while (_active)
            {
                _socket.Broadcast(new Message("", "", "", ProcessReader.ReadLine()));
                m = _socket.Listen();
                if (m.TypeTag == Constants.Message_Type_Terminate_Tag)
                {
                    _active = false;
                    m.Content = "quit";
                }
                ProcessWriter.WriteLine(m.Content);
                Thread.Sleep(1000);
            }
            chatter.Dispose();
        }
    }
}
