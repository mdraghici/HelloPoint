using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloPoint.Communication
{
    public class ResponseMonitorState
    {
        public bool ON { get; set; }

        public ResponseMonitorState() : base() { }

        public ResponseMonitorState(bool ON_)
        {
            ON = ON_;
        }
    }
}