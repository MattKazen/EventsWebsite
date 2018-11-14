using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class StudentsInRsos
    {
        public string StudentId { get; set; }
        public int MemberofRso { get; set; }

        public Rso MemberofRsoNavigation { get; set; }
        public AspNetUsers Student { get; set; }
    }
}
