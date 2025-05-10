using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloPoint.Models
{
    public class UserAndRole
    {
        public List<string> UserName { get; set; } = new List<string>();

        public List<string> Role { get; set; } = new List<string>();
    }
}