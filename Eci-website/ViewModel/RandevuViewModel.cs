using Eci_website.Models;

namespace Eci_website.ViewModel
{
    public class RandevuAlViewModel
    {
        public List<Calisan> Calisanlar { get; set; }
        public List<Hizmet> Hizmetler { get; set; }
        public DateTime RandevuTarihi { get; set; }
    }
}
