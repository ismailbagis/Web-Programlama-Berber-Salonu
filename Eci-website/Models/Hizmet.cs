using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eci_website.Models
{
    public class Hizmet
    {

        [Key]
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Aciklama { get; set; }
        public decimal Ucret { get; set; }
        public int Sure { get; set; } // dakika cinsinden
        public int SalonId { get; set; }
        [ForeignKey(nameof(SalonId))]
        public Salon Salon { get; set; } = null!;
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}
