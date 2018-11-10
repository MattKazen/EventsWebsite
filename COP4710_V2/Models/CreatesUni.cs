using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class CreatesUni
    {
        public string Id { get; set; }
        public int Unid { get; set; }

        public Superadmin IdNavigation { get; set; }
        public University Un { get; set; }
    }
}
