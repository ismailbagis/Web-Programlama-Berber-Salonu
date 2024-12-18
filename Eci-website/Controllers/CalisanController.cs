using Eci_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Eci_website.Controllers
{
    public class CalisanController : Controller
    {

        private readonly DataContext _context;

        public CalisanController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var calisanlar = await _context.Calisanlar.ToListAsync();

            return View(calisanlar);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Salonlar = new SelectList(await _context.Salonlar.ToListAsync(), "Id", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Calisan model)
        {
            _context.Calisanlar.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Calisanlar == null)
            {
                return NotFound();
            }

            var calisan = await _context.Calisanlar.FindAsync(id);
            if (calisan == null)
            {
                return NotFound();
            }

            // Salonlar dropdown için ViewBag'e verileri ekliyoruz.
            ViewBag.Salonlar = new SelectList(await _context.Salonlar.ToListAsync(), "Id", "Ad", calisan.SalonId);

            return View(calisan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Calisan model)
        {
            // Gelen modelin Id'si doğru değilse hata döndür.
            if (id != model.Id)
            {
                return NotFound();
            }

            // Model doğrulama geçerli mi?
            if (ModelState.IsValid)
            {
                try
                {
                    // Güncelleme işlemini yap.
                    _context.Update(model);
                    await _context.SaveChangesAsync(); // Veritabanına kaydet.
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Eğer çalışan hala yoksa 404 döndür.
                    if (!_context.Calisanlar.Any(o => o.Id == model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Başka bir hata varsa fırlat.
                    }
                }

                // Güncelleme başarılıysa Index'e yönlendir.
                return RedirectToAction(nameof(Index));
            }

            // Eğer doğrulama başarısızsa, Salonlar listesini tekrar doldur ve aynı sayfaya dön.
            ViewBag.Salonlar = new SelectList(await _context.Salonlar.ToListAsync(), "Id", "Ad", model.SalonId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var calisan = await _context.Calisanlar.FindAsync(id);
            if (calisan == null)
            {
                return NotFound();
            }

            _context.Calisanlar.Remove(calisan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
