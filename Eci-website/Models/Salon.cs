using System.ComponentModel.DataAnnotations;

namespace Eci_website.Models
{
    public class Salon
    {
        [Key]
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Adres { get; set; }
        public string? TelefonNumarasi { get; set; }
        public string? CalismaSaatleri { get; set; } // örneğin "09:00 - 18:00"
        public ICollection<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();
    }
}
