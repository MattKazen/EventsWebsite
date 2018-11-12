using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class PendingEvents
    {
        public int PendingEventId { get; set; }
        public string CreatorId { get; set; }
        public string ApproverId { get; set; }

        public Superadmins Approver { get; set; }
        public Admins Creator { get; set; }
    }
}
