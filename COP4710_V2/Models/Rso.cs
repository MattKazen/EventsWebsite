using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Rso
    {
        public Rso()
        {
            RsoEvents = new HashSet<RsoEvents>();
            StudentsInRsos = new HashSet<StudentsInRsos>();
        }

        public int RsoId { get; set; }
        public int? NumMembers { get; set; }
        public string RsoAdminId { get; set; }
        public string RsoUniversityId { get; set; }
        public bool? IsPending { get; set; }
        public string Name { get; set; }

        public Admins RsoAdmin { get; set; }
        public University RsoUniversity { get; set; }
        public PendingRso PendingRso { get; set; }
        public ICollection<RsoEvents> RsoEvents { get; set; }
        public ICollection<StudentsInRsos> StudentsInRsos { get; set; }
    }
}
