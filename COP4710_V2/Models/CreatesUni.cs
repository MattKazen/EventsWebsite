using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class CreatesUni
    {
        public string UniName { get; set; }
        public string SuperAdminId { get; set; }

        public Superadmins SuperAdmin { get; set; }
        public University UniNameNavigation { get; set; }
    }
}
