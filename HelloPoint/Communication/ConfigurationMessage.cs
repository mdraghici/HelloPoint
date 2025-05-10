using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloPoint.Communication
{
    public class ConfigurationMessage
    {
        public string Command { get; set; }
        public int Screen { get; set; }

        public ConfigurationMessage(string Command_, int Screen_)
        {
            Command = Command_;
            Screen = Screen_;
        }

    }
}
