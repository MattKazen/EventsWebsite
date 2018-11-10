using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Superadmin
    {
        public Superadmin()
        {
            CreatesUni = new HashSet<CreatesUni>();
        }

        public string Id { get; set; }

        public AspNetUsers IdNavigation { get; set; }
        public ICollection<CreatesUni> CreatesUni { get; set; }
    }
}
