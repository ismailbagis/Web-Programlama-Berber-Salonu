using Microsoft.EntityFrameworkCore;

namespace Eci_website.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
