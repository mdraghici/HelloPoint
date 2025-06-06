//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HelloPoint.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class File
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OrigninalFileName { get; set; }
        public string SavedFileName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }

        public File() : base() { }

        public File(int id, string u, string s, string o, string t, string d, string ad, string md)
        {
            Id = id;
            UserName = u;
            OrigninalFileName = o;
            SavedFileName = s;
            Type = t;
            Description = d;
            AddedDate = Convert.ToDateTime(ad);
            ModifyDate = Convert.ToDateTime(md);
        }
    }
}
