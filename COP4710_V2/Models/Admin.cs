using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Admin
    {
        public string Id { get; set; }

        public AspNetUsers IdNavigation { get; set; }
    }
}
