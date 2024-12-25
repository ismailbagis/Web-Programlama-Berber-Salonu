using Eci_website.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Eci_website.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<Rol> _roleManager;
        private readonly UserManager<Kullanici> _userManager;

        public RolesController(RoleManager<Rol> roleManager, UserManager<Kullanici> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var roles = _roleManager.Roles.ToList(); // ToList() ile listeye dönüştürüyoruz
            var rol = _roleManager.Roles;

            var model = new RolViewModel
            {
                Roles = roles,
                Rol = rol
            };

            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rol model)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // Yeni method: Bir rolün kullanıcılarını listeleme
        public async Task<IActionResult> UserInRole(string roleId)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            var users = _userManager.Users.ToList(); // Tüm kullanıcıları alıyoruz

            var usersInRole = users.Where(user => _userManager.IsInRoleAsync(user, role.Name).Result).ToList();

            ViewBag.RoleName = role.Name;
            return View(usersInRole);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null && role.Name != null)
            {
                ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
                return View(role);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Rol model)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    if(role.Name != null)
                    ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);

                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction("Index");
        }

    }
}
