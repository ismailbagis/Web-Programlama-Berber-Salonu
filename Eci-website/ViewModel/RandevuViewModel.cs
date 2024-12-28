using Eci_website.Models;

namespace Eci_website.ViewModel
{
    public class RandevuViewModel
    {
        public DateTime RandevuTarihi { get; set; }
        public TimeSpan RandevuSaati { get; set; } // Randevu saati bilgisi için eklendi.
        public int HizmetId { get; set; }
        public int CalisanId { get; set; }
        public required List<Hizmet> Hizmetler { get; set; }
        public required List<Calisan> Calisanlar { get; set; }
    }
}
