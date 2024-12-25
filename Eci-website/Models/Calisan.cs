using Eci_website.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Calisan
{
    [Key]
    public int Id { get; set; }
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string AdSoyad
    {
        get
        {
            return this.Ad + " " + this.Soyad;
        }
    }
    public string? Telefon { get; set; }
    public string? UzmanlikAlani { get; set; } // Örneğin "Saç Kesimi", "Makyaj"

    // Çalışma saatleri
    public TimeSpan CalismaBaslangic { get; set; } // Örneğin 09:00
    public TimeSpan CalismaBitis { get; set; }    // Örneğin 18:00

    public string CalismaBaslangicFormatted
    {
        get
        {
            return CalismaBaslangic.ToString(@"hh\:mm"); // "HH:mm" formatında string
        }
    }

    public string CalismaBitisFormatted
    {
        get
        {
            return CalismaBitis.ToString(@"hh\:mm"); // "HH:mm" formatında string
        }
    }

    public int SalonId { get; set; }
    [ForeignKey(nameof(SalonId))]
    public Salon Salon { get; set; } = null!;

    public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
}
