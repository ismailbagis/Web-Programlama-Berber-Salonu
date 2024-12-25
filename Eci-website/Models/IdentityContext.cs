using Eci_website.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eci_website.Models
{
    public class IdentityContext : IdentityDbContext<Kullanici, Rol, string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }

        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(u => new { u.LoginProvider, u.ProviderKey }); // Birincil anahtar olarak LoginProvider ve ProviderKey'i tanımladık
            modelBuilder.Entity<Hizmet>()
                    .Property(h => h.Ucret)
                    .HasPrecision(18, 2);

            modelBuilder.Entity<Randevu>()
       .HasOne(r => r.Hizmet)
       .WithMany(h => h.Randevular) // Randevu koleksiyonu eklemelisiniz
       .HasForeignKey(r => r.HizmetId)
       .OnDelete(DeleteBehavior.Restrict); // Cascade delete devre dışı

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Calisan)
                .WithMany(c => c.Randevular) // Randevu koleksiyonu eklemelisiniz
                .HasForeignKey(r => r.CalisanId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
