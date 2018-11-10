using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Admins
    {
        public Admins()
        {
            CreatesPrivEvents = new HashSet<CreatesPrivEvents>();
            CreatesPubEvents = new HashSet<CreatesPubEvents>();
            RsoCreatesEvents = new HashSet<RsoCreatesEvents>();
        }

        public string AdminId { get; set; }

        public AspNetUsers Admin { get; set; }
        public ICollection<CreatesPrivEvents> CreatesPrivEvents { get; set; }
        public ICollection<CreatesPubEvents> CreatesPubEvents { get; set; }
        public ICollection<RsoCreatesEvents> RsoCreatesEvents { get; set; }
    }
}
