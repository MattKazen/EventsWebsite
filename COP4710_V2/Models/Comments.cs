using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class Comments
    {
        public int CommentId { get; set; }
        public int EventId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public int? Rating { get; set; }
        public DateTime? Timestamp { get; set; }

        public Events Event { get; set; }
        public AspNetUsers User { get; set; }
    }
}
