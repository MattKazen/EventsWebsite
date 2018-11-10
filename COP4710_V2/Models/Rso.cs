using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Rso
    {
        public string Rsoid { get; set; }
        public string Name { get; set; }
        public string AdminId { get; set; }
        public string Descr { get; set; }
        public int? Size { get; set; }

        public Rso RsoNavigation { get; set; }
        public Rso InverseRsoNavigation { get; set; }
    }
}
