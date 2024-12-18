using Eci_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Eci_website.Controllers
{
    public class SalonController : Controller
    {
        private readonly DataContext _context;

        public SalonController(DataContext context)
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Salon model)
        {
            _context.Salonlar.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Edit()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
