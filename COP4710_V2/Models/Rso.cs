using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Rso
    {
        public Rso()
        {
            RsoCreatesEvents = new HashSet<RsoCreatesEvents>();
            RsoEvents = new HashSet<RsoEvents>();
        }

        public int RsoId { get; set; }
        public int? NumUsers { get; set; }

        public ICollection<RsoCreatesEvents> RsoCreatesEvents { get; set; }
        public ICollection<RsoEvents> RsoEvents { get; set; }
    }
}
