using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class CreatesPrivEvents
    {
        public int PrivateEventId { get; set; }
        public string SuperAdminId { get; set; }
        public string AdminId { get; set; }

        public Admins Admin { get; set; }
        public PrivEvents PrivateEvent { get; set; }
        public Superadmins SuperAdmin { get; set; }
    }
}
