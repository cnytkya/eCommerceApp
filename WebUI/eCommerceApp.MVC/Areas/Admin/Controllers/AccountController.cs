using eCommerceApp.Domain.Entities;
using eCommerceApp.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    //TempData kullanılacak
                    TempData["SuccessMessage"] = "Başarıyla giriş yaptınız. Admin paneline yönlendiriliyorsunuz.";
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Main", new { area = "Admin" });  //admin ana sayfasına yönlendirme
                    }
                }
                if (result.IsLockedOut)
                {
                    //kullanıcı başarısız girişten sonra hesap kilitlendi mesajı 
                    ModelState.AddModelError(string.Empty, "Hesabınız kilitlendi. Lütfen daha sonra tekrar deneyiniz!");
                    TempData["ErrorMessage"] = "Hesabınız kilitlendi. Lütfen daha sonra tekrar deneyiniz!";
                    return View(model);
                }
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }

        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma sağlar.
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen tüm zorunlu alanları doğru şekilde doldurunuz.";
                return View(model); // Formu hatalarla birlikte tekrar göster
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Fullname = model.FullName,
                IsActive = true,
                RegistrationDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                string defaultRoleName = "User"; // Atanacak varsayılan rol adı

                if (!await _roleManager.RoleExistsAsync(defaultRoleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(defaultRoleName));
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, defaultRoleName);

                if (!addRoleResult.Succeeded)
                {
                    foreach (var error in addRoleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    TempData["ErrorMessage"] = "Kullanıcı başarıyla oluşturuldu ancak rol atamasında hata oluştu.";
                    return View(model);
                }

                TempData["SuccessMessage"] = "Kaydınız başarıyla tamamlandı. Artık giriş yapabilirsiniz.";

                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                TempData["ErrorMessage"] = "Kayıt işlemi sırasında bir hata oluştu. Lütfen tekrar deneyiniz.";

                return View(model);
            }
        }

    }
}
