using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class UserUniversity
    {
        public string StudentId { get; set; }
        public string UniversityId { get; set; }

        public AspNetUsers Student { get; set; }
        public University University { get; set; }
    }
}
