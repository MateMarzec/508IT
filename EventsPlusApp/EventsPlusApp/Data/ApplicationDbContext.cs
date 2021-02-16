using EventsPlusApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Manager>().ToTable("Manager");
            modelBuilder.Entity<Participant>().ToTable("Participant");
            modelBuilder.Entity<Owner>().ToTable("Owner");

            
            //Seed user
            var appUser = new IdentityUser
            {
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
            };
            //Seed roles
            var userRole = new IdentityRole { Name = "User", NormalizedName = "USER" };
            var adminRole = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };
            //Hash password for seeded user
            var hasher = new PasswordHasher<IdentityUser>();
            appUser.PasswordHash = hasher.HashPassword(appUser, "Password");

            modelBuilder.Entity<IdentityRole>().HasData(
                userRole
            );
            modelBuilder.Entity<IdentityRole>().HasData(
                adminRole
            );
            modelBuilder.Entity<IdentityUser>().HasData(
                appUser
            );
            //Assign seeded user to the admin role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRole.Id,
                UserId = appUser.Id
            });
        }

    }
}
