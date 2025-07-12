using eCommerceApp.Domain.Entities;
using eCommerceApp.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            // Çıkış sonrası ana sayfaya yönlendir ve başarı mesajı göster
            TempData["SuccessMessage"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email, // Email'i aynı zamanda UserName olarak kullanıyoruz
                    Email = model.Email,
                    Fullname = model.FullName,
                    IsActive = true, // Kayıt olan kullanıcıyı varsayılan olarak aktif yap
                    RegistrationDate = DateTime.Now,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                    // Diğer AppUser alanları Identity tarafından otomatik olarak yönetilir veya varsayılan değerler alır
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Kullanıcıya varsayılan "User" rolünü ata
                    var defaultRole = "User";
                    if (!await _roleManager.RoleExistsAsync(defaultRole))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(defaultRole));
                    }
                    await _userManager.AddToRoleAsync(user, defaultRole);

                    TempData["SuccessMessage"] = "Hesabınız başarıyla oluşturuldu! Şimdi giriş yapabilirsiniz.";
                    return RedirectToAction("Login", "Account", new { area = "Admin" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    TempData["ErrorMessage"] = "Kayıt işlemi başarısız: " + error.Description; // Toastr için
                }
            }
            // Model validasyonu başarısız olursa veya Identity hatası oluşursa formu tekrar göster
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı";
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            var model = new ProfileViewModel{
                Fullname = user.Fullname,
                Email = user.Email,
                Bio = user.Bio,
                ProfileImgUrl = user.ProfilImgUrl,
                Location = user.Location
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı";
                return RedirectToAction("Login","Account", new {area = "Admin"});
            }

            if (ModelState.IsValid)
            {
                //Update user information
                user.Fullname = model.Fullname;
                user.Bio = model.Bio;
                user.ProfilImgUrl = model.ProfileImgUrl;
                user.Location = model.Location;
                user.ModifiedDate = DateTime.UtcNow;

                //Email update logic
                if (user.Email != model.Email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        foreach (var error in setEmailResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        TempData["ErrorMessage"] = "Email güncelleme başarısız." + string.Join(", ", setEmailResult.Errors.Select(e => e.Description));
                        return View(model);
                    }
                    user.EmailConfirmed = false;
                    TempData["WarningMessage"] = "Email adresiniz değiştirildi. Yeni email adresinizi onaylamanız gerekebilir.";
                }

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    TempData["ErrorMessage"] = "Profil güncelleme başarısız." + string.Join(", ", updateResult.Errors.Select(e => e.Description));
                    return View(model);
                }
                //Password change logic
                if (!string.IsNullOrEmpty(model.OldPassword) || !string.IsNullOrEmpty(model.NewPassword) || !string.IsNullOrEmpty(model.ConfirmNewPassword))
                {
                    if (string.IsNullOrEmpty(model.OldPassword))
                    {
                        ModelState.AddModelError("OldPassword", "Şifrenizi değiştirmek için mevcut şifrenizi girmelisiniz");
                        TempData["ErrorMessage"] = "Şifrenizi değiştirmek için mevcut şifrenizi girmelisiniz";
                        return View(model);
                    }

                    var changePasswordResult = await _userManager.ChangePasswordAsync(user,model.OldPassword,model.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {
                        foreach (var error in changePasswordResult.Errors)
                        {
                            if (error.Code == "PasswordMismatch")
                            {
                                ModelState.AddModelError("PasswordMismatch","Mevcut şifreniz yanlış.");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    TempData["SuccessMessage"] = (TempData["SuccessMessage"] !=null) ? TempData["SuccessMessage"] + " " : " " + "Şifreniz başarıyla güncellendi";
                }
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = (TempData["SuccessMessage"] != null) ? TempData["SuccessMessage"] + " " : " " + "Profile başarıyla güncellendi";
                return RedirectToAction("Profile", "Account", new {area = "Admin"});
            }

            return View(model);
        }

    }
}
