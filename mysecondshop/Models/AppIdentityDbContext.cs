using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RestWebAppl.Models;
namespace RestWebAppl.Models
{
    public class AppIdentityDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }

        public DbSet<Review> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .Property(e => e.FirstName).HasMaxLength(50);
            builder.Entity<ApplicationUser>()
                .Property(e => e.LastName).HasMaxLength(50);
            builder.Entity<ApplicationUser>()
                .Property(e => e.PatronymicName).HasMaxLength(50);
        }
    }
}
