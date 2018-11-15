using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
		[DisplayName("Event Name")]
		public string EventName { get; set; }
		[DisplayName("Time")]
		public int? StartTime { get; set; }
		[DisplayName("Day")]
		public int? StartDay { get; set; }
		[DisplayName("Month")]
		public int? StartMonth { get; set; }
		[DisplayName("Description")]
		public string EventDesc { get; set; }
        public string Category { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public bool? IsPending { get; set; }

        public EventLocation Location { get; set; }
        public PendingEvents PendingEvents { get; set; }
        public PrivEvents PrivEvents { get; set; }
        public PubEvents PubEvents { get; set; }
        public RsoEvents RsoEvents { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}
