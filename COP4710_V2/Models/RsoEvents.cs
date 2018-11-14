using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class RsoEvents
    {
        public int RsoEventId { get; set; }
        public int Rso { get; set; }
        public string CreatorId { get; set; }

        public Admins Creator { get; set; }
        public Events RsoEvent { get; set; }
        public Rso RsoNavigation { get; set; }
    }
}
