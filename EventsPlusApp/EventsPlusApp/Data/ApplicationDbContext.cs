using EventsPlusApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventsPlusApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAssignment> EventAssignments { get; set; }
        public DbSet<EventLocation> EventLocations { get; set; }
        public DbSet<EventManager> EventManagers { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<ManagerCredentials> ManagersCredentials { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<ParticipantCredentials> ParticipantsCredentials { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<EventAssignment>().ToTable("EventAssignment");
            modelBuilder.Entity<EventLocation>().ToTable("EventLocation");
            modelBuilder.Entity<EventManager>().ToTable("EventManager");
            modelBuilder.Entity<EventParticipant>().ToTable("EventParticipant");
            modelBuilder.Entity<ManagerCredentials>().ToTable("ManagerCredentials");
            modelBuilder.Entity<Owner>().ToTable("Owner");
            modelBuilder.Entity<ParticipantCredentials>().ToTable("ParticipantCredentials");

            modelBuilder.Entity<EventAssignment>().HasKey(c => new { c.EventManagerID, c.EventID });
            modelBuilder.Entity<EventManager>()
            .HasOne(a => a.ManagerCredentials)
            .WithOne(b => b.EventManager)
            .HasForeignKey<ManagerCredentials>(b => b.ManagerID);
            modelBuilder.Entity<EventParticipant>()
            .HasOne(a => a.ParticipantCredentials)
            .WithOne(b => b.EventParticipant)
            .HasForeignKey<ParticipantCredentials>(b => b.ParticipantID);
        }

    }
}
