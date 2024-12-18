using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eci_website.Models
{
    public class Randevu
    {

        [Key]
        public int Id { get; set; }
        public DateTime RandevuTarihi { get; set; }
        public int HizmetId { get; set; }

        [ForeignKey(nameof(HizmetId))]
        public Hizmet Hizmet { get; set; } = null!;

        public int CalisanId { get; set; }

        [ForeignKey(nameof(CalisanId))]
        public Calisan Calisan { get; set; } = null!;
        public string? MusteriAdi { get; set; }
        public string? MusteriTelefon { get; set; }
        public bool Onay { get; set; }
    }
}
