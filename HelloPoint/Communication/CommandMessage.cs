using HelloPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloPoint
{
    [Serializable()]
    public class CommandMessage
    {


        /// <summary>
        /// Commands:
        /// PLAY
        /// PAUSE
        /// STOP
        /// ADDTOPLAYLIST
        /// REMOVEFROMPLAYLIST
        /// MOVEUP(IN QUEUE)
        /// MOVEDOWN(IN QUEUE)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        public CommandMessage(string command, PlaylistElement file)
        {
            Command = command;
            Element = file;
        }

        public string Command { get; private set; }
        public PlaylistElement Element { get; private set; }
    }

    
}
