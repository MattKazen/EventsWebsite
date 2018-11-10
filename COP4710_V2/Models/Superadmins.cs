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
        }

        public string SuperAdminId { get; set; }

        public AspNetUsers SuperAdmin { get; set; }
        public ICollection<CreatesPrivEvents> CreatesPrivEvents { get; set; }
        public ICollection<CreatesPubEvents> CreatesPubEvents { get; set; }
    }
}
