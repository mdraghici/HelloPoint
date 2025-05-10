using HelloPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloPoint.Communication
{
    public class ResponseMessage
    {
        public List<PlaylistElement> Playlist { get; set; }


        public ResponseMessage() : base() { }

        public ResponseMessage(List<PlaylistElement> playList)
        {
            Playlist = playList.ToList<PlaylistElement>();
        }

        
    }
}
