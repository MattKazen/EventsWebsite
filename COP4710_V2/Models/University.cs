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

        public int Unid { get; set; }
        public string Locat { get; set; }
        public string Descr { get; set; }
        public int? Students { get; set; }

        public ICollection<CreatesUni> CreatesUni { get; set; }
        
    }
}
