using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class PendingRso
    {
        public int PendingRsoId { get; set; }
        public int? PendingNumMem { get; set; }
        public string PendingRsoCreatorId { get; set; }
        public string PendingRsoUniversityId { get; set; }
        public string PendingRsoName { get; set; }

        public AspNetUsers PendingRsoCreator { get; set; }
        public Rso PendingRsoNavigation { get; set; }
        public University PendingRsoUniversity { get; set; }
    }
}
