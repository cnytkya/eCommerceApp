using eCommerceApp.Domain.Entities;
using eCommerceApp.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
		public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
		{
			ViewData["ReturnUrl"]= returnUrl;
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
				if (result.Succeeded)
				{
					//Tempdata kullanılacak.
					TempData["SuccessMessage"] = "Başarıyla giriş yapıldı. Admin paneline yönlendiriliyorsunuz.";
					if(Url.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}
					else
					{
						//Admin Ana sayfasına yönlendirme.
						return RedirectToAction("Index", "Main", new {area="Admin"});
					}
				}
				if(result.IsLockedOut)
				{
					//Kullanıcı başarısız girişten sonra hesap kilitlendi mesajı verdirelim.
					ModelState.AddModelError(string.Empty, "Hesabınız kilitlendi Lütfen daha sonra tekrar deneyiniz!");
					TempData["ErrorMessage"] = "Hesabınız kilitlendi Lütfen daha sonra tekrar deneyiniz!";
					return View(model);
				}
			}
			return View();
		}
	}
}
