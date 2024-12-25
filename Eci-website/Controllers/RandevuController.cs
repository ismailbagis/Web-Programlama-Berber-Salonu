using Eci_website.Models;
using Eci_website.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eci_website.Controllers
{
    public class RandevuController : Controller
    {
        private readonly IdentityContext _context;

        public RandevuController(IdentityContext context)
        {
            _context = context;
        }

        // Randevu almak için çalışanlar, hizmetler ve uygun saatler gösterilecek
        public async Task<IActionResult> RandevuAl()
        {
            // Çalışanlar ve hizmetler veritabanından çekiliyor
            var calisanlar = await _context.Calisanlar.ToListAsync();
            var hizmetler = await _context.Hizmetler.ToListAsync();

            // Model oluşturulacak ve veriler View'a gönderilecek
            var viewModel = new RandevuAlViewModel
            {
                Calisanlar = calisanlar,
                Hizmetler = hizmetler
            };

            return View(viewModel);
        }

        // Randevu ekleme işlemi
        [HttpPost]
        public async Task<IActionResult> RandevuAl(int calisanId, int hizmetId, DateTime randevuTarihi)
        {
            // Çalışanın çalışma saatleri kontrol edilecek
            var calisan = await _context.Calisanlar.FindAsync(calisanId);
            if (calisan == null)
            {
                return NotFound();
            }

            // Çalışanın çalışma saatleri
            var calismaBaslangic = calisan.CalismaBaslangic;
            var calismaBitis = calisan.CalismaBitis;

            // Seçilen saat diliminin geçerli olup olmadığı kontrol edilecek
            if (randevuTarihi.TimeOfDay < calismaBaslangic || randevuTarihi.TimeOfDay >= calismaBitis)
            {
                ModelState.AddModelError("", "Geçersiz saat dilimi seçtiniz. Lütfen çalışanın çalışma saatlerine dikkat edin.");
                return View();
            }

            // Hizmet süresi bilgisi alınacak
            var hizmet = await _context.Hizmetler.FindAsync(hizmetId);
            if (hizmet == null || !hizmet.Sure.HasValue)
            {
                ModelState.AddModelError("", "Seçilen hizmetin süresi bulunamadı.");
                return View();
            }

            // Hizmet süresi dikkate alınarak saat aralığı kontrol edilecek
            var hizmetSure = TimeSpan.FromMinutes(hizmet.Sure.Value);
            var randevuBitis = randevuTarihi.Add(hizmetSure);

            // Çalışanın randevuları ile çakışma kontrolü yapılacak
            var mevcutRandevular = await _context.Randevular
                .Where(r => r.CalisanId == calisanId &&
                            r.RandevuTarihi < randevuBitis &&
                            r.RandevuTarihi.AddMinutes(r.Hizmet.Sure ?? 0) > randevuTarihi)
                .ToListAsync();

            if (mevcutRandevular.Any())
            {
                ModelState.AddModelError("", "Bu saat diliminde çalışan zaten randevu almış. Lütfen başka bir saat seçin.");
                return View();
            }

            // Yeni randevu ekleme işlemi
            var yeniRandevu = new Randevu
            {
                CalisanId = calisanId,
                HizmetId = hizmetId,
                RandevuTarihi = randevuTarihi,
                MusteriAdi = "Müşteri Adı",  // Kullanıcı bilgileri burada alınacak
                MusteriTelefon = "Telefon Numarası",  // Kullanıcı telefonu burada alınacak
                Durum = RandevuDurum.Bekliyor
            };

            _context.Randevular.Add(yeniRandevu);
            await _context.SaveChangesAsync();

            return RedirectToAction("RandevuListesi");  // Randevuların listeleneceği sayfaya yönlendir
        }

        // Kullanıcıya uygun randevular ve saat dilimlerini gösteren ekran
        public async Task<IActionResult> RandevuListesi()
        {
            var randevular = await _context.Randevular.ToListAsync();
            return View(randevular);
        }
    }
}
