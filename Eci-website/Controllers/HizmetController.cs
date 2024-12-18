using Eci_website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Eci_website.Controllers
{
    public class HizmetController: Controller
    {
        private readonly ILogger<HizmetController> _logger;

        public HizmetController(ILogger<HizmetController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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
