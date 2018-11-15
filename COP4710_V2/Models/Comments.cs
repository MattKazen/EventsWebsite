using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace COP4710_V2.Models
{
    public partial class Comments
    {
		[Key]
		public int CommentId { get; set; }
		[DisplayName("Event Name")]
		public int EventId { get; set; }
		[DisplayName("Commenter")]
		public string UserId { get; set; }
		[DisplayName("Comment")]
		public string Text { get; set; }
        public int? Rating { get; set; }
        public DateTime? Timestamp { get; set; }

        public Events Event { get; set; }
        public AspNetUsers User { get; set; }
    }
}
