using System;
using System.Collections.Generic;
using System.ComponentModel;

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
		[DisplayName("University")]
		public string UniName { get; set; }
		[DisplayName("Super Admin")]
		public string CreatorId { get; set; }
		[DisplayName("Description")]
		public string UniDesc { get; set; }
		[DisplayName("Location")]
		public string UniLocation { get; set; }
		[DisplayName("Number of Students")]
		public int? NumStudents { get; set; }
		[DisplayName("University Email")]
		public string UniEmail { get; set; }

        public Superadmins Creator { get; set; }
        public ICollection<PendingRso> PendingRso { get; set; }
        public ICollection<Rso> Rso { get; set; }
        public ICollection<UserUniversity> UserUniversity { get; set; }
    }
}
