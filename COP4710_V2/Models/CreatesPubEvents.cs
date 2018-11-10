using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class CreatesPubEvents
    {
        public int EventId { get; set; }
        public string SuperAdminId { get; set; }
        public string AdminId { get; set; }

        public Admins Admin { get; set; }
        public Events Event { get; set; }
        public Superadmins SuperAdmin { get; set; }
    }
}
