using Eci_website.Models;
using Eci_website.ViewModel;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Form veri uzunluk sınırlamaları (100 MB)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});


// Veritabanı Bağlantısı
builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity servisleri
builder.Services.AddIdentity<Kullanici, Rol>().AddEntityFrameworkStores<IdentityContext>();

// Identity ayarları
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1; // Minimum 1 karakter
    options.Password.RequiredUniqueChars = 0; // Benzersiz karakter sayısı gereksinimi kaldırıldı

    // Kullanıcı ve kilitleme ayarları
    options.User.RequireUniqueEmail = true; // E-posta adresleri benzersiz olmalı
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // Kilitleme süresi 1 dakika
    options.Lockout.MaxFailedAccessAttempts = 10; // Maksimum giriş deneme sayısı
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("HairstyleClient", client =>
{  //9
    client.BaseAddress = new Uri("https://hairstyle-changer.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "464e0bb017msh6fa5a9938be79eep1ce7f6jsnd3ee2fe6faa6");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "hairstyle-changer.p.rapidapi.com");
});





var app = builder.Build();

// Geliştirme ortamında Swagger'ı etkinleştir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger'ı etkinleştir
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eci Website API v1");
        c.RoutePrefix = "swagger"; // Swagger'ı /swagger adresinde göster
    });
}
else
{
    // Üretim ortamında hata yönetimini aktif et
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed işlemi için admin kullanıcı oluşturma
IdentitySeedData.IdentityTestUser(app);

app.Run();
