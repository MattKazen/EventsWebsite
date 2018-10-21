using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using COP4710_V2.Models;

namespace COP4710_V2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<COP4710_V2.Models.UserMessages> UserMessages { get; set; }
        public DbSet<COP4710_V2.Models.Events> Events { get; set; }
    }
}
