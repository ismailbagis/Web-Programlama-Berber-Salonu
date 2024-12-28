using Eci_website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eci_website.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IdentityContext _context;

        public AdminController(IdentityContext context)
        {
            _context = context;
        }

        public IActionResult Randevular(DateTime? tarih)
        {
            // Varsayılan tarih bugün olsun
            DateTime filtreTarihi = tarih ?? DateTime.Today;

            // Randevuları belirtilen tarihe göre filtrele
            var randevular = _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Calisan)
                .Where(r => r.RandevuTarihi.Date == filtreTarihi.Date) // Tarih filtresi
                .ToList();

            // Seçili tarihi View'e gönder
            ViewBag.SeciliTarih = filtreTarihi;

            return View(randevular);
        }


        public async Task<IActionResult> Onayla(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Durum = RandevuDurum.Onaylandi;
            randevu.Onay = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Randevular));
        }

        public async Task<IActionResult> RedEt(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Durum = RandevuDurum.Rededildi;
            randevu.Onay = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Randevular));
        }

        public IActionResult Kazanc()
        {
            // Tüm onaylı randevuları al
            var onayliRandevular = _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Calisan)
                .Where(r => r.Onay) // Sadece onaylı randevular
                .ToList();

            // Çalışan bazlı kazançlar
            var calisanKazanc = onayliRandevular
                .GroupBy(r => r.Calisan)
                .Select(grup => new
                {
                    Calisan = grup.Key,
                    ToplamKazanc = grup.Sum(r => r.Hizmet.Ucret)
                })
                .ToList();

            // Toplam kazancı hesapla
            var toplamKazanc = onayliRandevular.Sum(r => r.Hizmet.Ucret);

            // Verileri ViewBag ile gönder
            ViewBag.CalisanKazanc = calisanKazanc;
            ViewBag.ToplamKazanc = toplamKazanc;

            return View();
        }

        public IActionResult GunlukKazanc()
        {
            // Tüm onaylı randevuları al
            var onayliRandevular = _context.Randevular
                .Include(r => r.Hizmet)
                .Where(r => r.Onay) // Sadece onaylı randevular
                .ToList();

            // Randevuları tarih bazında gruplandır
            var gunlukKazanc = onayliRandevular
                .GroupBy(r => r.RandevuTarihi.Date) // Tarihe göre gruplama (sadece gün bilgisi)
                .Select(grup => new
                {
                    Tarih = grup.Key,
                    ToplamKazanc = grup.Sum(r => r.Hizmet.Ucret)
                })
                .OrderBy(g => g.Tarih) // Tarihlere göre sıralama
                .ToList();

            // Verileri ViewBag ile gönder
            ViewBag.GunlukKazanc = gunlukKazanc;

            return View();
        }


    }
}
