using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class PrivEvents
    {
        public int PrivateEventId { get; set; }

        public Events PrivateEvent { get; set; }
        public CreatesPrivEvents CreatesPrivEvents { get; set; }
    }
}
