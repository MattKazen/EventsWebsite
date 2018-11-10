using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class EventLocation
    {
        public EventLocation()
        {
            Events = new HashSet<Events>();
        }

        public int LocationId { get; set; }
        public string Addres { get; set; }
        public string Long { get; set; }
        public string Lat { get; set; }

        public ICollection<Events> Events { get; set; }
    }
}
