using Microsoft.EntityFrameworkCore;
namespace RestWebAppl.Models
{
    public class SeedData
    {
        public static void FillDb(IApplicationBuilder app)
        {
            var ScopedFactory=app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();            
            using (var scope = ScopedFactory.CreateScope())
            {
                ApplicationDbContext context=scope.ServiceProvider.GetService<ApplicationDbContext>();
                 context.Database.Migrate();
                if (!context.Items.Any()) 
                {
                    context.Items.AddRange(
                    new Item { Id = Guid.NewGuid(), Name = "First", Price = 10,Category="Cat1",Description = "First Description", addedTime = DateTime.Now.ToLongTimeString() },
                    new Item { Id = Guid.NewGuid(), Name = "Second", Price = 20, Category = "Cat2", Description = "Second Description", addedTime = DateTime.Now.ToLongTimeString() },
                    new Item { Id = Guid.NewGuid(), Name = "Third", Price = 30, Category = "Cat1", Description = "Third Description", addedTime = DateTime.Now.ToLongTimeString() },
                    new Item { Id = Guid.NewGuid(), Name = "Fourth", Price = 40, Category = "Cat2", Description = "Fourth Description", addedTime = DateTime.Now.ToLongTimeString() },
                    new Item { Id = Guid.NewGuid(), Name = "Fifth", Price = 50, Category = "Cat1", Description = "Fifth Description", addedTime = DateTime.Now.ToLongTimeString() },
                    new Item { Id = Guid.NewGuid(), Name = "Sixth", Price = 60, Category = "Cat2", Description = "Sixth Description", addedTime = DateTime.Now.ToLongTimeString() },
                    new Item { Id = Guid.NewGuid(), Name = "Seventh", Price = 70, Category = "Cat3", Description = "Seventh Description", addedTime = DateTime.Now.ToLongTimeString() },
                    new Item { Id = Guid.NewGuid(), Name = "Eight", Price = 80, Category = "Cat2", Description = "Eight Description", addedTime = DateTime.Now.ToLongTimeString() },
                    new Item { Id = Guid.NewGuid(), Name = "Ninth", Price = 90, Category = "Cat1", Description = "Ninth Description", addedTime = DateTime.Now.ToLongTimeString() },
                    new Item { Id = Guid.NewGuid(), Name = "Tenth", Price = 100, Category = "Cat3", Description = "Tenth Description", addedTime = DateTime.Now.ToLongTimeString() },
                    new Item { Id = Guid.NewGuid(), Name = "Eleventh", Price = 110, Category = "Cat2", Description = "Eleventh Description", addedTime = DateTime.Now.ToLongTimeString() }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
