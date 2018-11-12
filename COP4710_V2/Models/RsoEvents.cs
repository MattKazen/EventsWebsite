using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class RsoEvents
    {
        public int RsoEventId { get; set; }
        public int RsoId { get; set; }

        public Rso Rso { get; set; }
        public Events RsoEvent { get; set; }
        public RsoCreatesEvents RsoCreatesEvents { get; set; }
    }
}
