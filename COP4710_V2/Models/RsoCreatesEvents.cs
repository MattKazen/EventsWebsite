using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class RsoCreatesEvents
    {
        public int RsoEventId { get; set; }
        public string AdminId { get; set; }
        public int Rsoid { get; set; }

        public Admins Admin { get; set; }
        public Rso Rso { get; set; }
        public RsoEvents RsoEvent { get; set; }
    }
}
