using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class University
    {
        public University()
        {
            CreatesUni = new HashSet<CreatesUni>();
        }

        public string UniName { get; set; }
        public string UniDesc { get; set; }
        public string UniLocation { get; set; }
        public int? NumStudents { get; set; }

        public ICollection<CreatesUni> CreatesUni { get; set; }
    }
}
