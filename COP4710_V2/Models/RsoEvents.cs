using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class RsoEvents
    {
        public int EventId { get; set; }
        public int RsoId { get; set; }

        public Events Event { get; set; }
        public Rso Rso { get; set; }
    }
}
