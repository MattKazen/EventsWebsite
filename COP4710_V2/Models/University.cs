using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace COP4710_V2.Models
{
    public partial class University
    {
        [Display(Name = "Name")]
        public string UniName { get; set; }
        public string CreatorId { get; set; }
        [Display(Name = "Description/Info")]
        public string UniDesc { get; set; }
        [Display(Name = "Location")]
        public string UniLocation { get; set; }
        [Display(Name = "Number of Students")]
        public int? NumStudents { get; set; }
        [Display(Name = "E-Mail Address")]
        public string UniEmail { get; set; }

        public Superadmins Creator { get; set; }
    }
}
