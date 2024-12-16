namespace Eci_Barber.Models
{
    public class Calisan
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string UzmanlikAlani { get; set; } // örneğin "Saç Kesimi", "Makyaj"
        public string Uygunluk { get; set; } // örneğin "09:00 - 12:00, 13:00 - 18:00"
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
    }
}
