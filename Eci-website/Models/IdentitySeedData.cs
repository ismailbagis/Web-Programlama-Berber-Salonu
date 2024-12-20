using Eci_website.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Eci_website.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";

        private const string adminPassword = "Admin_123";

        public static async void IdentityTestUser(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();

            if (context.Database.GetAppliedMigrations().Any())
            {
                context.Database.Migrate();
            }

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<Kullanici>>();
        
            var user =  await userManager.FindByNameAsync(adminUser);

            if (user == null)
            {
                user = new Kullanici {
                    FullName = "İsmail Bağış",
                    UserName = adminUser,
                    Email = "admin@ismailbagis.com",
                    PhoneNumber = "05469388323"
                };
                await userManager.CreateAsync(user,adminPassword);
            }
        }
    }
}
