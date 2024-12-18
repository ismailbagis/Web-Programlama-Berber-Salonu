using Eci_website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Eci_website.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
