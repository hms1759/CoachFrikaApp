using CoachFrika.APIs.Entity;
using CoachFrika.Common.Extension;
using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace coachfrikaaaa.Common
{
    public class AppDbContext : IdentityDbContext
    {
        private readonly IWebHelpers _webHelpers;
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Coaches> Coaches { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<CoachFrikaUsers> CoachFrikaUsers { get; set; }
        public DbSet<SchoolEnrollmentRequest> SchoolEnrollmentRequest { get; set; }
        public DbSet<NewsSubscription> NewsSubscription { get; set; }
        public DbSet<Subjects> Subjects { get; set; }
        //public DbSet<Batches> Batches { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<ChildSponsor> ChildSponsor { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IWebHelpers webHelpers = null) : base(options)
        {
            _webHelpers = webHelpers;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // Make sure to call base method

            // Custom configurations for CoachFrikaUsers (derived from IdentityUser)
            modelBuilder.Entity<CoachFrikaUsers>(entity =>
            {
                entity.Property(c => c.FullName).HasMaxLength(50);
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
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _webHelpers.CurrentUser();
                    entry.Entity.CreatedDate = DateTime.UtcNow;

                    // Optionally, you can set ModifiedBy, ModifiedDate, etc.
                }

                // Handle updates or deletions if needed.
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedBy = _webHelpers.CurrentUser();
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }

}
