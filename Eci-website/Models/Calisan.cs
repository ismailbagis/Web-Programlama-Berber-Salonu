using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eci_website.Models
{
    public class Calisan
    {
        [Key]
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Telefon { get; set; }
        public string? UzmanlikAlani { get; set; } // örneğin "Saç Kesimi", "Makyaj"
        public string? Uygunluk { get; set; } // örneğin "09:00 - 12:00, 13:00 - 18:00"
        public int SalonId { get; set; }
        [ForeignKey(nameof(SalonId))]
        public Salon Salon { get; set; } = null!;
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}
