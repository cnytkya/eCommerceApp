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

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe, lockoutOnFailure: true);
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
                        //admin ana sayfasına yönlendirme
                        return RedirectToAction("Index", "Main", new {area = "Admin"});
                    }
                }
                if (result.IsLockedOut)
                {
                    //kullanıcı başarısız girişten sonra hesap kilitlendi mesajı verdirelim
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

    }
}
