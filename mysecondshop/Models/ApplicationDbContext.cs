using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;


namespace mysecondshop.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        Database.EnsureCreated();
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
       

    }
}
