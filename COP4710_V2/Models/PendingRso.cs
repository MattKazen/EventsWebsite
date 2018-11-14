using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COP4710_V2.Models
{
    public partial class PendingRso
    {
        public int PendingRsoId { get; set; }
		[Display(Name = "Number of Members")]
		public int? PendingNumMem { get; set; }
        public string PendingRsoCreatorId { get; set; }
		[Display(Name = "University")]
		public string PendingRsoUniversityId { get; set; }
		[Display(Name = "Name of Rso")]
		public string PendingRsoName { get; set; }

        public AspNetUsers PendingRsoCreator { get; set; }
        public Rso PendingRsoNavigation { get; set; }
        public University PendingRsoUniversity { get; set; }
    }
}
