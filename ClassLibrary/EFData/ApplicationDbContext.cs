using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;


namespace RestWebAppl.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }    
        public DbSet<PromoCode> PromoCodes { get; set; }

    }
}
