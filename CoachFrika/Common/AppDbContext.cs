using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace coachfrikaaaa.Common
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Coaches> Coaches { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // Configuring soft delete globally in OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
