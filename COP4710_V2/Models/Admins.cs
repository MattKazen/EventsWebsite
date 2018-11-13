using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Admins
    {
        public Admins()
        {
            PendingEvents = new HashSet<PendingEvents>();
            RsoCreatesEvents = new HashSet<RsoCreatesEvents>();
        }

        public string AdminId { get; set; }

        public AspNetUsers Admin { get; set; }
        public ICollection<PendingEvents> PendingEvents { get; set; }
        public ICollection<RsoCreatesEvents> RsoCreatesEvents { get; set; }
    }
}
