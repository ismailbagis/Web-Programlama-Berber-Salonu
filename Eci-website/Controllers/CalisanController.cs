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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
