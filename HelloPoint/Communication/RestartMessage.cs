using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloPoint.Communication
{
    public class RestartMessage
    {
        public string Command { get; set; }
        public RestartMessage() { }
        public RestartMessage(string Command_)
        {
            Command = Command_;
        }
    }
}