using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Superadmins
    {
        public Superadmins()
        {
            PendingEvents = new HashSet<PendingEvents>();
            University = new HashSet<University>();
        }

        public string SuperAdminId { get; set; }

        public AspNetUsers SuperAdmin { get; set; }
        public ICollection<PendingEvents> PendingEvents { get; set; }
        public ICollection<University> University { get; set; }
    }
}
