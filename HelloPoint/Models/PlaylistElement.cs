using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloPoint.Models
{
    public class PlaylistElement
    {
        public string Description { get; set; }
        public int PlayTime { get; set; }
        public string Guid { get; set; }
        public string Type { get; set; }
        public bool IsPaused { get; set; }
        public bool IsSelected { get; set; }
        public int ID { get; set; }
        public int Repetitions { get; set; }

        public PlaylistElement()
        {
            Description = null;
            PlayTime = 0;
            Guid = null;
            Type = null;
            IsPaused = false;
            IsSelected = false;
            ID = -1;
            Repetitions = -1;
        }

        public PlaylistElement(string Description_,int Playtime_, string Guid_, string Type_, bool IsPaused_, bool IsSelected_, int ID_, int Repetitions_)
        {
            Description = Description_;
            PlayTime = Playtime_;
            Guid = Guid_;
            Type = Type_;
            IsPaused = IsPaused_;
            IsSelected = IsSelected_;
            ID = ID_;
            Repetitions = Repetitions_;
        }

        public PlaylistElement(string Description_, string Guid_, string Type_, bool IsPaused_, bool IsSelected_, int ID_, int Repetitions_)
        {
            Description = Description_;
            PlayTime = 1;
            Guid = Guid_;
            Type = Type_;
            IsPaused = IsPaused_;
            IsSelected = IsSelected_;
            ID = ID_;
            Repetitions = Repetitions_;
        }

        public PlaylistElement(string Guid_)
        {
            Description = "";
            PlayTime = 0;
            Guid = Guid_;
            Type = "";
            IsPaused = false;
            IsSelected = false;
            Repetitions = -1;
        }

        public PlaylistElement(string Description_, int PlayTime_, string Guid_, string Type_, int ID_, int Repetitions_)
        {
            Description = Description_;
            PlayTime = PlayTime_;
            Guid = Guid_;
            Type = Type_;
            IsPaused = false;
            IsSelected = false;
            ID = ID_;
            Repetitions = Repetitions_;
        }

        public PlaylistElement(string Description_, int PlayTime_, string Guid_, string Type_, int ID_)
        {
            Description = Description_;
            PlayTime = PlayTime_;
            Guid = Guid_;
            Type = Type_;
            IsPaused = false;
            IsSelected = false;
            ID = ID_;
            Repetitions = 1;
        }

        public PlaylistElement(int ID_)
        {
            Description = null;
            PlayTime = 0;
            Guid = null;
            Type = null;
            IsPaused = false;
            IsSelected = false;
            ID = ID_;
            Repetitions = -1;
        }

        public PlaylistElement(int ID_, int Repetitions_)
        {
            Description = null;
            PlayTime = -1;
            Guid = null;
            Type = null;
            IsPaused = false;
            IsSelected = false;
            ID = ID_;
            Repetitions = Repetitions_;
        }

        public PlaylistElement Clone()
        {
            return new PlaylistElement(Description, PlayTime, Guid, Type, IsPaused, IsSelected, ID, Repetitions);
        }
    }
}
