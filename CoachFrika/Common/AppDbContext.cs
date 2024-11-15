using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace coachfrikaaaa.Common
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Coaches> Coaches { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<CoachFrikaUsers> CoachFrikaUsers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // Make sure to call base method

            // Custom configurations for CoachFrikaUsers (derived from IdentityUser)
            modelBuilder.Entity<CoachFrikaUsers>(entity =>
            {
                entity.Property(c => c.FirstName).HasMaxLength(50);
                entity.Property(c => c.LastName).HasMaxLength(50);
                entity.Property(c => c.TweeterUrl).HasMaxLength(250);
                entity.Property(c => c.LinkedInUrl).HasMaxLength(250);
                entity.Property(c => c.InstagramUrl).HasMaxLength(250);
                entity.Property(c => c.FacebookUrl).HasMaxLength(250);
            });

            // Soft delete filter for Teachers and Coaches
            modelBuilder.Entity<Teachers>()
                .HasQueryFilter(t => !t.IsDeleted);

            modelBuilder.Entity<Coaches>()
                .HasQueryFilter(c => !c.IsDeleted);
        }
    }

}
