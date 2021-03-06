﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COP4710_V2.Models
{
    public partial class Events
    {
        [Key]
        public int Eid { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public TimeSpan? Time { get; set; }
        public DateTime? Date { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
    }
}
