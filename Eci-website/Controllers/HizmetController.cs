﻿using Eci_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Eci_website.Controllers
{
    public class HizmetController: Controller
    {
        private readonly IdentityContext _context;

        public HizmetController(IdentityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hizmet = await _context.Hizmetler.Include(x => x.Salon).ToListAsync();
            return View(hizmet);
        }


        public async Task<IActionResult> Create()
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Salonlar = new SelectList(await _context.Salonlar.ToListAsync(), "Id", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hizmet model)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            _context.Hizmetler.Add(model);
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
