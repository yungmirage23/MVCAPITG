using Microsoft.AspNetCore.Identity;
namespace mysecondshop.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var scopedFactory =app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using(var scope = scopedFactory.CreateScope())
            {
                UserManager<IdentityUser> userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();

                IdentityUser user = await userManager.FindByIdAsync(adminUser);
                if (user == null)
                {
                    user = new IdentityUser("Admin");
                    await userManager.CreateAsync(user, adminPassword);
                }
            }
           
        }
    }
}
