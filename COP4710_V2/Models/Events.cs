using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Events
    {
        public Events()
        {
            Comments = new HashSet<Comments>();
        }

        public int EventId { get; set; }
        public int? LocationId { get; set; }
        public string Name { get; set; }
        public TimeSpan? Time { get; set; }
        public DateTime? Date { get; set; }
        public string Desc { get; set; }
        public string Category { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public EventLocation Location { get; set; }
        public PrivEvents PrivEvents { get; set; }
        public PubEvents PubEvents { get; set; }
        public RsoEvents RsoEvents { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}
