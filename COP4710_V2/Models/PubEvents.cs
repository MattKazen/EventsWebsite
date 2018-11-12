using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class PubEvents
    {
        public int PublicEventId { get; set; }

        public Events PublicEvent { get; set; }
        public CreatesPubEvents CreatesPubEvents { get; set; }
    }
}
