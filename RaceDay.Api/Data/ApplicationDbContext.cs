using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RaceDay.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for our entities
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;
        public DbSet<Organiser> Organisers { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Enrolment> Enrolments { get; set; } = null!;
        public DbSet<Result> Results { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Table-Per-Hierarchy (TPH) for User inheritance
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasDiscriminator<string>("UserType")
                .HasValue<Participant>("Participant")
                .HasValue<Organiser>("Organiser");

            // Configure Event -> Organiser relationship
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organiser)
                .WithMany(o => o.OrganizedEvents)
                .HasForeignKey(e => e.OrganiserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Enrolment -> Participant relationship
            modelBuilder.Entity<Enrolment>()
                .HasOne(en => en.Participant)
                .WithMany(p => p.Enrolments)
                .HasForeignKey(en => en.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Enrolment -> Result (1-to-1 relationship)
            modelBuilder.Entity<Enrolment>()
                .HasOne(en => en.Result)
                .WithOne(r => r.Enrolment)
                .HasForeignKey<Result>(r => r.EnrolmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
