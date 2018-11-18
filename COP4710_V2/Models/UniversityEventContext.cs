﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace COP4710_V2.Models
{
    public partial class UniversityEventContext : DbContext
    {
        public UniversityEventContext()
        {
        }

        public UniversityEventContext(DbContextOptions<UniversityEventContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<EventLocation> EventLocation { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<PendingEvents> PendingEvents { get; set; }
        public virtual DbSet<PendingRso> PendingRso { get; set; }
        public virtual DbSet<PrivEvents> PrivEvents { get; set; }
        public virtual DbSet<PubEvents> PubEvents { get; set; }
        public virtual DbSet<Rso> Rso { get; set; }
        public virtual DbSet<RsoEvents> RsoEvents { get; set; }
        public virtual DbSet<StudentsInRsos> StudentsInRsos { get; set; }
        public virtual DbSet<Superadmins> Superadmins { get; set; }
        public virtual DbSet<University> University { get; set; }
        public virtual DbSet<UserUniversity> UserUniversity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=cop4017-2.database.windows.net;database=University Event;uid=dbadmin;pwd=Ucfdbs!!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admins>(entity =>
            {
                entity.HasKey(e => e.AdminId);

                entity.ToTable("admins");

                entity.Property(e => e.AdminId).ValueGeneratedNever();

                entity.Property(e => e.AdminEmail).HasMaxLength(256);

                entity.HasOne(d => d.Admin)
                    .WithOne(p => p.Admins)
                    .HasForeignKey<Admins>(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__admins__AdminId__70DDC3D8");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.EventId });

                entity.ToTable("comments");

                entity.Property(e => e.CommentId)
                    .HasColumnName("CommentID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Text)
                    .HasColumnName("text_")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp_")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comments__EventI__7C4F7684");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comments__UserId__7B5B524B");
            });

            modelBuilder.Entity<EventLocation>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasMaxLength(20);

                entity.Property(e => e.LocationName).HasMaxLength(50);

                entity.Property(e => e.Long)
                    .HasColumnName("long")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.ToTable("events_");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.ContactEmail).HasMaxLength(20);

                entity.Property(e => e.ContactPhone).HasMaxLength(20);

                entity.Property(e => e.EventDesc).HasMaxLength(50);

                entity.Property(e => e.EventName).HasMaxLength(30);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__events___Locatio__75A278F5");
            });

            modelBuilder.Entity<PendingEvents>(entity =>
            {
                entity.HasKey(e => e.PendingEventId);

                entity.Property(e => e.PendingEventId)
                    .HasColumnName("PendingEventID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApproverId)
                    .HasColumnName("ApproverID")
                    .HasMaxLength(450);

                entity.Property(e => e.CreatorId)
                    .HasColumnName("CreatorID")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Approver)
                    .WithMany(p => p.PendingEvents)
                    .HasForeignKey(d => d.ApproverId)
                    .HasConstraintName("FK__PendingEv__Appro__7167D3BD");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.PendingEvents)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__PendingEv__Creat__7073AF84");

                entity.HasOne(d => d.PendingEvent)
                    .WithOne(p => p.PendingEvents)
                    .HasForeignKey<PendingEvents>(d => d.PendingEventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PendingEv__Pendi__6F7F8B4B");
            });

            modelBuilder.Entity<PendingRso>(entity =>
            {
                entity.Property(e => e.PendingRsoId).ValueGeneratedNever();

                entity.Property(e => e.PendingRsoCreatorId).HasMaxLength(450);

                entity.Property(e => e.PendingRsoName).HasMaxLength(50);

                entity.Property(e => e.PendingRsoUniversityId).HasMaxLength(50);

                entity.HasOne(d => d.PendingRsoCreator)
                    .WithMany(p => p.PendingRso)
                    .HasForeignKey(d => d.PendingRsoCreatorId)
                    .HasConstraintName("FK__PendingRs__Pendi__1699586C");

                entity.HasOne(d => d.PendingRsoNavigation)
                    .WithOne(p => p.PendingRso)
                    .HasForeignKey<PendingRso>(d => d.PendingRsoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PendingRs__Pendi__15A53433");

                entity.HasOne(d => d.PendingRsoUniversity)
                    .WithMany(p => p.PendingRso)
                    .HasForeignKey(d => d.PendingRsoUniversityId)
                    .HasConstraintName("FK__PendingRs__Pendi__178D7CA5");
            });

            modelBuilder.Entity<PrivEvents>(entity =>
            {
                entity.HasKey(e => e.PrivateEventId);

                entity.Property(e => e.PrivateEventId).ValueGeneratedNever();

                entity.Property(e => e.EventUniversityId).HasMaxLength(50);

                entity.HasOne(d => d.PrivateEvent)
                    .WithOne(p => p.PrivEvents)
                    .HasForeignKey<PrivEvents>(d => d.PrivateEventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__priv_even__Priva__282DF8C2");
            });

            modelBuilder.Entity<PubEvents>(entity =>
            {
                entity.HasKey(e => e.PublicEventId);

                entity.Property(e => e.PublicEventId)
                    .HasColumnName("Public_Event_ID")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.PublicEvent)
                    .WithOne(p => p.PubEvents)
                    .HasForeignKey<PubEvents>(d => d.PublicEventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__pub_event__Publi__32AB8735");
            });

            modelBuilder.Entity<Rso>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.RsoAdminId).HasMaxLength(450);

                entity.Property(e => e.RsoUniversityId).HasMaxLength(50);

                entity.HasOne(d => d.RsoAdmin)
                    .WithMany(p => p.Rso)
                    .HasForeignKey(d => d.RsoAdminId)
                    .HasConstraintName("FK__Rso__RsoAdminId__038683F8");

                entity.HasOne(d => d.RsoUniversity)
                    .WithMany(p => p.Rso)
                    .HasForeignKey(d => d.RsoUniversityId)
                    .HasConstraintName("FK__Rso__RsoUniversi__047AA831");
            });

            modelBuilder.Entity<RsoEvents>(entity =>
            {
                entity.HasKey(e => e.RsoEventId);

                entity.Property(e => e.RsoEventId)
                    .HasColumnName("RsoEventID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatorId)
                    .HasColumnName("CreatorID")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.RsoEvents)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__RsoEvents__Creat__093F5D4E");

                entity.HasOne(d => d.RsoNavigation)
                    .WithMany(p => p.RsoEvents)
                    .HasForeignKey(d => d.Rso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RsoEvents__Rso__084B3915");

                entity.HasOne(d => d.RsoEvent)
                    .WithOne(p => p.RsoEvents)
                    .HasForeignKey<RsoEvents>(d => d.RsoEventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RsoEvents__RsoEv__075714DC");
            });

            modelBuilder.Entity<StudentsInRsos>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.MemberofRso });

                entity.HasOne(d => d.MemberofRsoNavigation)
                    .WithMany(p => p.StudentsInRsos)
                    .HasForeignKey(d => d.MemberofRso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentsI__Membe__0EF836A4");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentsInRsos)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentsI__Stude__0E04126B");
            });

            modelBuilder.Entity<Superadmins>(entity =>
            {
                entity.HasKey(e => e.SuperAdminId);

                entity.ToTable("superadmins");

                entity.Property(e => e.SuperAdminId).ValueGeneratedNever();

                entity.Property(e => e.SuperAdminEmail).HasMaxLength(256);

                entity.HasOne(d => d.SuperAdmin)
                    .WithOne(p => p.Superadmins)
                    .HasForeignKey<Superadmins>(d => d.SuperAdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__superadmi__Super__6A30C649");
            });

            modelBuilder.Entity<University>(entity =>
            {
                entity.HasKey(e => e.UniName);

                entity.ToTable("university");

                entity.Property(e => e.UniName)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatorId)
                    .HasColumnName("CreatorID")
                    .HasMaxLength(450);

                entity.Property(e => e.NumStudents).HasColumnName("num_students");

                entity.Property(e => e.UniDesc).HasMaxLength(50);

                entity.Property(e => e.UniEmail).HasMaxLength(50);

                entity.Property(e => e.UniLocation).HasMaxLength(50);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.University)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_create_uni_creatorID");
            });

            modelBuilder.Entity<UserUniversity>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.Property(e => e.StudentId)
                    .HasColumnName("StudentID")
                    .ValueGeneratedNever();

                entity.Property(e => e.UniversityId)
                    .HasColumnName("UniversityID")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Student)
                    .WithOne(p => p.UserUniversity)
                    .HasForeignKey<UserUniversity>(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserUnive__Stude__6BAEFA67");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.UserUniversity)
                    .HasForeignKey(d => d.UniversityId)
                    .HasConstraintName("FK__UserUnive__Unive__6ABAD62E");
            });
        }
    }
}
