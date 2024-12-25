using Eci_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Eci_website.Controllers
{
    public class SalonController : Controller
    {
        private readonly IdentityContext _context;

        public SalonController(IdentityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var salonlar = await _context.Salonlar.ToListAsync();

            return View(salonlar);
        }

        public IActionResult Create()
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Salon model, IFormFile? imageFile)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (imageFile != null)
            {
                // Resim dosyasını wwwroot/images klasörüne kaydedin
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Modeldeki Image alanına dosya yolunu ekleyin
                model.Image = "/img/" + fileName;
            }

            _context.Salonlar.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var salon = await _context.Salonlar.FindAsync(id);
            if (salon == null)
                return NotFound();

            return View(salon); // Modeli View'e gönderiyoruz.
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Salon model, IFormFile? imageFile)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(model); // Model doğrulama başarısızsa aynı sayfayı göster.
            }

            var salon = await _context.Salonlar.FindAsync(id);
            if (salon == null)
                return NotFound();

            salon.Ad = model.Ad;
            salon.Adres = model.Adres;
            salon.TelefonNumarasi = model.TelefonNumarasi;
            salon.CalismaSaatleri = model.CalismaSaatleri;

            if (imageFile != null)
            {
                // Yeni resim yükleme işlemi
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Eski resmi sil
                if (!string.IsNullOrEmpty(salon.Image))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", salon.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                salon.Image = "/img/" + fileName;
            }

            _context.Salonlar.Update(salon);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var salon = await _context.Salonlar.FindAsync(id);
            if (salon == null)
            {
                return NotFound();
            }

            _context.Salonlar.Remove(salon);
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
