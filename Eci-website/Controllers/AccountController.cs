using Eci_website.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eci_website.Controllers
{
    public class AccountController:Controller
    {

        private readonly UserManager<Kullanici> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private SignInManager<Kullanici> _signInManager;
        public AccountController(UserManager<Kullanici> userManager, 
            RoleManager<Rol> roleManager, 
            SignInManager<Kullanici> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager= signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>  Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password,model.RememberMe,true);

                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);

                        return RedirectToAction("Index","Home");

                    }
                    else if (result.IsLockedOut)
                    {
                        var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeLeft = lockoutDate.Value - DateTimeOffset.UtcNow;
                        ModelState.AddModelError("", $"Hesabınız Kilitlendi,Lütfen {timeLeft.Minutes} dakika sonra deneyiniz. ");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Parolanız Hatalı !!!");
                    }


                }
                else
                {
                    ModelState.AddModelError("", "Bu E-mail adresine bağlı hesap bulunamadı.");
                }
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Kullanici
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Kullanıcıya varsayılan bir rol atama
                    if (await _roleManager.RoleExistsAsync("User"))
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    // Kullanıcıyı otomatik giriş yapmaya yönlendirme
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }

}
