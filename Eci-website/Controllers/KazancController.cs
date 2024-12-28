using Eci_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Eci_website.Controllers
{
    public class KazancController : Controller
    {
        private readonly IdentityContext _context;

        public KazancController(IdentityContext context)
        {
            _context = context;
        }

        // Günlük kazançları listeleyen sayfa
        public async Task<IActionResult> Index(DateTime? date)
        {
            // Eğer tarih verilmemişse, bugünün tarihi kullanılır.
            var selectedDate = date ?? DateTime.Today;

            // Günlük kazançları hesaplamak için tüm randevular alınır
            var randevular = await _context.Randevular
                                           .Include(r => r.Hizmet)
                                           .Include(r => r.Calisan)
                                           .Where(r => r.RandevuTarihi.Date == selectedDate.Date)
                                           .ToListAsync();

            // Çalışanların kazançlarını hesaplayalım
            var kazancListesi = randevular
                .GroupBy(r => r.Calisan)
                .Select(g => new
                {
                    CalisanId = g.Key.Id,
                    CalisanAdSoyad = g.Key.AdSoyad,
                    Kazanc = g.Sum(r => r.Hizmet.Ucret ?? 0)
                })
                .ToList();

            // Kazançları view'a göndermek
            return View(kazancListesi);
        }
    }
}
