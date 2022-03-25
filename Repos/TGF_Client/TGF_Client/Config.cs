using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TGF_Client
{
    class Config
    {
        public Dictionary<string, string> d;

        public Config()
        {
            d = new Dictionary<string, string>();
            string[] text = System.IO.File.ReadAllLines("../../../TGF_Config.txt");
            foreach (string line in text)
            {
                string[] kV_pair = line.Split(',');
                d.Add(kV_pair[0],kV_pair[1]);
            }
        }
    }
}
