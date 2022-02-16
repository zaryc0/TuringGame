using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TGF_Client
{
    class Config
    {
        public IPAddress controllerIP;

        public Config()
        {
            //string[] text = System.IO.File.ReadAllLines("TGF_Config.txt");
           // controllerIP = IPAddress.Parse(text[0]);
        }
    }
}
