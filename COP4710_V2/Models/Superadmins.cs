using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Superadmins
    {
        public Superadmins()
        {
            CreatesPrivEvents = new HashSet<CreatesPrivEvents>();
            CreatesPubEvents = new HashSet<CreatesPubEvents>();
            PendingEvents = new HashSet<PendingEvents>();
            University = new HashSet<University>();
        }

        public string SuperAdminId { get; set; }

        public AspNetUsers SuperAdmin { get; set; }
        public ICollection<CreatesPrivEvents> CreatesPrivEvents { get; set; }
        public ICollection<CreatesPubEvents> CreatesPubEvents { get; set; }
        public ICollection<PendingEvents> PendingEvents { get; set; }
        public ICollection<University> University { get; set; }
    }
}
