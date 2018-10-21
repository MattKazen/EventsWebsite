using System;
using System.Collections.Generic;

namespace COP4710_V2.Models
{
    public partial class UserMessages
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string From { get; set; }
    }
}
