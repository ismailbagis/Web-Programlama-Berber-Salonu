using Eci_website.Models;
using Eci_website.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class RandevuController : Controller
{
    private readonly UserManager<Kullanici> _userManager;
    private readonly IdentityContext _context;

    public RandevuController(UserManager<Kullanici> userManager, IdentityContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> RandevuAl()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            // Kullanıcı giriş yapmamış, giriş sayfasına yönlendir
            return RedirectToAction("Login", "Account");
        }

        // Kullanıcı giriş yapmış, randevu alma sayfasına yönlendir
        var hizmetler = _context.Hizmetler.ToList();
        var calisanlar = _context.Calisanlar.ToList();

        var viewModel = new RandevuViewModel
        {
            Hizmetler = hizmetler,
            Calisanlar = calisanlar
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> RandevuAl(RandevuViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var randevu = new Randevu
        {
            RandevuTarihi = model.RandevuTarihi,
            RandevuSaati = model.RandevuSaati, // Saat bilgisini burada kaydediyoruz
            HizmetId = model.HizmetId,
            CalisanId = model.CalisanId,
            MusteriAdi = user.FullName,
            MusteriTelefon = user.PhoneNumber,
            Durum = RandevuDurum.Bekliyor,
            Onay = false
        };


        _context.Randevular.Add(randevu);
        await _context.SaveChangesAsync();
        return RedirectToAction("Randevularim");
    }

    // Kullanıcıya ait randevuları listeleyen action
    public async Task<IActionResult> Randevularim()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Kullanıcının tüm randevularını al
        var randevular = await _context.Randevular
            .Where(r => r.MusteriTelefon == user.PhoneNumber) // Kullanıcının telefon numarasına göre filtrele
            .Include(r => r.Hizmet)  // Hizmet bilgilerini dahil et
            .Include(r => r.Calisan) // Çalışan bilgilerini dahil et
            .ToListAsync();

        return View(randevular);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var randevu = await _context.Randevular
            .Include(r => r.Hizmet)
            .Include(r => r.Calisan)
            .FirstOrDefaultAsync(r => r.Id == id && r.MusteriAdi == user.FullName); // Kullanıcıya ait mi kontrolü

        if (randevu == null)
        {
            return NotFound();
        }

        // Form için gerekli hizmet ve çalışan verilerini ViewData ile gönderiyoruz
        ViewData["Hizmetler"] = _context.Hizmetler.ToList();
        ViewData["Calisanlar"] = _context.Calisanlar.ToList();

        return View(randevu);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Randevu model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var randevu = await _context.Randevular.FindAsync(id);

        if (randevu == null || randevu.MusteriAdi != user.FullName) // Kullanıcıya ait değilse
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            // Tekrar hizmet ve çalışan verilerini yükleyelim
            ViewData["Hizmetler"] = _context.Hizmetler.ToList();
            ViewData["Calisanlar"] = _context.Calisanlar.ToList();
            return View(model);
        }

        // Randevu bilgilerini güncelle
        randevu.HizmetId = model.HizmetId;
        randevu.CalisanId = model.CalisanId;
        randevu.RandevuTarihi = model.RandevuTarihi;

        _context.Randevular.Update(randevu);
        await _context.SaveChangesAsync();

        return RedirectToAction("Randevularim");
    }



    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var randevu = await _context.Randevular.FindAsync(id);

        if (randevu == null || randevu.MusteriAdi != user.FullName)
        {
            return NotFound(); // Randevu yoksa veya bu kullanıcıya ait değilse
        }

        _context.Randevular.Remove(randevu); // Randevuyu sil
        await _context.SaveChangesAsync();

        return RedirectToAction("Randevularim");
    }


}
