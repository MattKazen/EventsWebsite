using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Admins
    {
        public Admins()
        {
            PendingEvents = new HashSet<PendingEvents>();
            Rso = new HashSet<Rso>();
            RsoEvents = new HashSet<RsoEvents>();
        }

        public string AdminId { get; set; }
        public string AdminEmail { get; set; }

        public AspNetUsers Admin { get; set; }
        public ICollection<PendingEvents> PendingEvents { get; set; }
        public ICollection<Rso> Rso { get; set; }
        public ICollection<RsoEvents> RsoEvents { get; set; }
    }
}
