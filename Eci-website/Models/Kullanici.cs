﻿using System.ComponentModel.DataAnnotations;

namespace Eci_website.Models
{
    public class Kullanici
    {
        [Key]
        public int Id { get; set; }
        public string? KullaniciAdi { get; set; }
        public string? Eposta { get; set; }
        public string? Sifre { get; set; }
        public string? Rol { get; set; } // örneğin "Admin", "Musteri"
    }
}