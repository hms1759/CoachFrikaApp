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
        public DbSet<Students> Students { get; set; }
        public DbSet<NewsSubscription> NewsSubscription { get; set; }
        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<StudentScoreSheet> StudentScoreSheet { get; set; }
        //public DbSet<Batches> Batches { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<ChildSponsor> ChildSponsor { get; set; }
        public DbSet<Recommendations> Recommendations { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<SchoolTeacherRequest> SchoolTeacherRequest { get; set; }
        public DbSet<Schemes> Schemes { get; set; }
        
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

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    
        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _webHelpers.CurrentUser();
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = _webHelpers.CurrentUser();
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified; // soft delete
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedDate = DateTime.UtcNow;
                    entry.Entity.DeletedBy = _webHelpers.CurrentUser();
                }
            }
        }
        

        private static LambdaExpression ConvertFilterExpression(Type type)
        {
            var param = Expression.Parameter(type, "e");
            var prop = Expression.Property(param, nameof(BaseEntity.IsDeleted));
            var condition = Expression.Equal(prop, Expression.Constant(false));
            return Expression.Lambda(condition, param);
        }
    }

}
