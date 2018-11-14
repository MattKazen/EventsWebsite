using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class University
    {
        public University()
        {
            PendingRso = new HashSet<PendingRso>();
            Rso = new HashSet<Rso>();
            UserUniversity = new HashSet<UserUniversity>();
        }

        public string UniName { get; set; }
        public string CreatorId { get; set; }
        public string UniDesc { get; set; }
        public string UniLocation { get; set; }
        public int? NumStudents { get; set; }
        public string UniEmail { get; set; }

        public Superadmins Creator { get; set; }
        public ICollection<PendingRso> PendingRso { get; set; }
        public ICollection<Rso> Rso { get; set; }
        public ICollection<UserUniversity> UserUniversity { get; set; }
    }
}
