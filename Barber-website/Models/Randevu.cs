namespace Eci_Barber.Models
{
    public class Randevu
    {
        public int Id { get; set; }
        public DateTime RandevuTarihi { get; set; }
        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriTelefon { get; set; }
        public bool Onay { get; set; }
    }
}
