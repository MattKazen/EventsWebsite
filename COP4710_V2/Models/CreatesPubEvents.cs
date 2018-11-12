using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class CreatesPubEvents
    {
        public int PublicEventId { get; set; }
        public string SuperAdminId { get; set; }
        public string AdminId { get; set; }

        public Admins Admin { get; set; }
        public PubEvents PublicEvent { get; set; }
        public Superadmins SuperAdmin { get; set; }
    }
}
