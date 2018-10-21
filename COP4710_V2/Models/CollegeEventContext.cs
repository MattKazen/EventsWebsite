using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace COP4710_V2.Models
{
    public partial class CollegeEventContext : DbContext
    {
        public CollegeEventContext()
        {
        }

        public CollegeEventContext(DbContextOptions<CollegeEventContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hello> Hello { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=cop4710.database.windows.net;database=College Event;uid=dbadmin;pwd=Ucfdbs!!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hello>(entity =>
            {
                entity.HasKey(e => e.Test);

                entity.Property(e => e.Test)
                    .HasColumnName("test")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.Property(e => e.StudentId)
                    .HasColumnName("StudentID")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.NumClasses).HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Ssn);

                entity.Property(e => e.Ssn)
                    .HasColumnName("ssn")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Username).HasMaxLength(10);
            });
        }
    }
}
