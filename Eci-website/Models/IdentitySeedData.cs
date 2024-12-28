using Eci_website.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Eci_website.ViewModel
{
    public static class IdentitySeedData
    {
        private const string adminUser = "admin";
        private const string adminPassword = "sau";
        private const string adminEmail = "ismail.bagis@ogr.sakarya.edu.tr";
        private const string adminPhoneNumber = "05469388323";
        private const string adminFullName = "İsmail Bağış";

        public static async Task IdentityTestUser(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();

            // Veritabanının güncel olup olmadığını kontrol et
            if (context.Database.GetAppliedMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<Kullanici>>();
            var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<Rol>>(); // Rol sınıfı kullanılacak

            // Admin rolünün olup olmadığını kontrol et, yoksa oluştur
            var roleExist = await roleManager.RoleExistsAsync("admin");
            if (!roleExist)
            {
                var adminRole = new Rol { Name = "admin" }; // Rol sınıfını kullanarak admin rolü oluştur
                await roleManager.CreateAsync(adminRole);
            }

            // Admin kullanıcısını bul
            var user = await userManager.FindByNameAsync(adminUser);

            // Kullanıcı bulunmazsa yeni admin kullanıcısı oluştur
            if (user == null)
            {
                user = new Kullanici
                {
                    FullName = adminFullName,
                    UserName = adminUser,
                    Email = adminEmail,
                    PhoneNumber = adminPhoneNumber
                };

                // Kullanıcıyı oluştur
                var result = await userManager.CreateAsync(user, adminPassword);

                // Kullanıcı başarıyla oluşturulmuşsa, admin rolünü ata
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
            else
            {
                // Kullanıcı varsa, admin rolüne ekle
                if (!await userManager.IsInRoleAsync(user, "admin"))
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
        }
    }
}
