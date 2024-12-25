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

        public IActionResult Randevular()
        {
            var randevular = _context.Randevular.Include(r => r.Hizmet).Include(r => r.Calisan).ToList();
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
    }

}
