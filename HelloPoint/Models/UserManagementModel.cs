using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloPoint.Models
{
    public class UserManagementModel
    {
        public string UserName { get; set; }
        public List<string> OriginalFileName = new List<string>();
        public List<string> SavedFileName = new List<string>();
        public List<string> Type  = new List<string>();
        public List<string> Description  = new List<string>();

        public List<PlaylistElement> playlist= new List<PlaylistElement>();
    }
}