using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloPoint.Communication
{
    public class ResponseConfigurationMessage
    {
        public int MarginX { get; set; }
        public int MarginY { get; set; }
        public int Scale { get; set; }
        public int Template { get; set; }


        public ResponseConfigurationMessage() : base() { }

        public ResponseConfigurationMessage(int marginX, int marginY, int scale)
        {
            MarginX = marginX;
            MarginY = marginY;
            Scale = scale;
            Template = 1;
        }

        public ResponseConfigurationMessage(int marginX, int marginY, int scale, int template)
        {
            MarginX = marginX;
            MarginY = marginY;
            Scale = scale;
            Template = template;
        }


    }
}