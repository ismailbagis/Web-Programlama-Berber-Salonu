namespace Eci_Barber.Models
{
    public class Hizmet
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public decimal Ucret { get; set; }
        public int Sure { get; set; } // dakika cinsinden
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
    }
}
