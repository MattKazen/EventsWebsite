using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class PubEvents
    {
        public int EventId { get; set; }

        public Events Event { get; set; }
    }
}
