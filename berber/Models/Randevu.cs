using berber.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace berber.Models
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; }
        public DateTime RandevuTarihi { get; set; }
        public int HizmetId { get; set; }

        [ForeignKey(nameof(HizmetId))]
        public Hizmet Hizmet { get; set; }

        public int CalisanId { get; set; }

        [ForeignKey(nameof(CalisanId))]
        public Calisan Calisan { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriTelefon { get; set; }
        public bool Onay { get; set; }
    }
}
