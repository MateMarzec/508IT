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
        public DbSet<Location> Locations { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Owner> Owners { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<EventAssignment>().ToTable("EventAssignment");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Manager>().ToTable("Manager");
            modelBuilder.Entity<Participant>().ToTable("Participant");
            modelBuilder.Entity<Owner>().ToTable("Owner");

            modelBuilder.Entity<EventAssignment>().HasKey(c => new { c.ManagerID, c.EventID });
            modelBuilder.Entity<Booking>().HasKey(c => new { c.ParticipantID, c.EventID });
        }

    }
}
