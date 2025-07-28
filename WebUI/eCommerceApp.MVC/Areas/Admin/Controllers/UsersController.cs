using eCommerceApp.Application.DTOs.User;
using eCommerceApp.Application.Interface.Services;
using eCommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // UserManager, RoleManager için
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectListItem için
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // ClaimTypes için

namespace eCommerceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]  Bu Controller'a sadece Admin rolüne sahip kullanıcılar erişebilir (henüz aktif değil)
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public UsersController(IUserService userService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // KULLANICILARI LİSTELEME (READ)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Kullanıcılar";
            var users = await _userService.GetAllActiveUsersAsync();
            return View(users);
        }

        // KULLANICI EKLEME (CREATE) - Formu Göster
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Yeni Kullanıcı Ekle";
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");

            return View(new CreateUserDto { IsActive = true }); // Yeni bir boş CreateUserDto gönder
        }

        // KULLANICI EKLEME (CREATE) - Formu İşle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto model, string selectedRole)
        {
            ViewData["Title"] = "Yeni Kullanıcı Ekle";

            // Rol seçimi zorunlu değil, varsayılan atama UserService'te yapılacak
            // selectedRole boş gelirse, bu durum hata olarak algılanmaz
            if (ModelState.IsValid)
            {
                var (succeeded, errors) = await _userService.CreateUserAsync(model, selectedRole);

                if (succeeded)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu ve sisteme eklendi.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    TempData["ErrorMessage"] = $"Kullanıcı eklenirken bir hata oluştu: {string.Join(", ", errors)}";
                }
            }

            // Model validasyonu başarısız olursa veya hata oluşursa, rolleri tekrar yükleyerek View'ı döndür
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Name", "Name", selectedRole);
            return View(model);
        }

        // KULLANICI DÜZENLEME (UPDATE) - Formu Göster
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Title"] = "Kullanıcı Düzenle";

            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Düzenlenecek kullanıcı ID'si belirtilmedi.";
                return RedirectToAction(nameof(Index));
            }

            var userDto = await _userService.GetUserByIdAsync(id);
            if (userDto == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            // UserDto'dan EditUserViewModel'e manuel mapping
            var model = new Models.ViewModels.EditUserViewModel
            {
                Id = userDto.Id,
                Fullname = userDto.Fullname,
                Email = userDto.Email,
                Bio = userDto.Bio,
                ProfilImgUrl = userDto.ProfilImgUrl,
                Location = userDto.Location,
                IsActive = userDto.IsActive,
                CurrentRoles = userDto.Roles.ToList()
            };

            // Sistemdeki tüm rolleri çekip dropdown için hazırla
            var allRoles = await _roleManager.Roles.ToListAsync();
            model.AllRoles = new SelectList(allRoles, "Name", "Name");

            // Kullanıcının mevcut rollerini multi-select dropdown'da seçili olarak göstermek için
            // SelectedRoles'i mevcut rollerle dolduruyoruz.
            model.SelectedRoles = userDto.Roles.ToList();

            return View(model);
        }

        // KULLANICI DÜZENLEME (UPDATE) - Formu İşle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Models.ViewModels.EditUserViewModel model)
        {
            ViewData["Title"] = "Kullanıcı Düzenle";

            // Eğer model.IsActive checkbox'ı işaretlenmediyse (formdan gelmezse) false olarak al
            // Checkbox işaretlenmediğinde form verisinde yer almaz, bu yüzden ModelState.ContainsKey kullanılır.
            if (!ModelState.ContainsKey(nameof(model.IsActive)))
            {
                model.IsActive = false;
            }

            // CurrentRoles bir input değildir, sadece gösterim amaçlıdır.
            // Model binding mekanizmasının onu validasyona dahil etmemesi için kaldırıyoruz.
            ModelState.Remove("CurrentRoles");

            // ModelState.IsValid kontrolünü yapmadan önce, SelectedRoles'in null gelme ihtimaline karşı defensive programlama
            // SelectedRoles bir liste olduğu için null yerine boş bir liste olarak başlatılıyor
            // ve bu sayede aşağıdaki LINQ Except metodu hata vermez.
            // Bu kısım zaten ViewModel tarafından hallediliyor (new List<string>())
            // var selectedRoles = model.SelectedRoles ?? new List<string>();

            if (ModelState.IsValid)
            {
                // EditUserViewModel'den EditUserDto'ya manuel mapping
                var editUserDto = new EditUserDto
                {
                    Id = model.Id,
                    Fullname = model.Fullname,
                    Email = model.Email,
                    Bio = model.Bio,
                    ProfilImgUrl = model.ProfilImgUrl,
                    Location = model.Location,
                    IsActive = model.IsActive
                };

                // User bilgilerini güncelle (email değişimi ve diğer profil bilgileri)
                var (succeeded, errors) = await _userService.UpdateUserAsync(editUserDto);

                if (succeeded)
                {
                    // Rol Yönetimi: Kullanıcının mevcut rollerini çek
                    var user = await _userManager.FindByIdAsync(model.Id);
                    var currentRoles = await _userManager.GetRolesAsync(user);

                    // Formdan seçilen rolleri al (null gelirse boş liste olarak kabul et)
                    // Rol seçimi yapılmadığında SelectedRoles boş bir liste olarak gelecektir.
                    var selectedRoles = model.SelectedRoles;

                    // Kaldırılacak roller: Mevcut rollerde olup, seçilenlerde olmayanlar
                    var rolesToRemove = currentRoles.Except(selectedRoles).ToList();
                    if (rolesToRemove.Any())
                    {
                        var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                        if (!removeResult.Succeeded)
                        {
                            TempData["ErrorMessage"] = "Roller kaldırılırken hata oluştu: " + string.Join(", ", removeResult.Errors.Select(e => e.Description));
                            // Hata oluştuğunda View'ı tekrar döndürelim ve mevcut durumu yükleyelim
                            model.AllRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name", model.SelectedRoles);
                            model.CurrentRoles = (await _userManager.GetRolesAsync(user)).ToList();
                            return View(model);
                        }
                    }

                    // Eklenecek roller: Seçilen rollerde olup, mevcut rollerde olmayanlar
                    var rolesToAdd = selectedRoles.Except(currentRoles).ToList();
                    if (rolesToAdd.Any())
                    {
                        var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                        if (!addResult.Succeeded)
                        {
                            TempData["ErrorMessage"] = "Roller eklenirken hata oluştu: " + string.Join(", ", addResult.Errors.Select(e => e.Description));
                            // Hata oluştuğunda View'ı tekrar döndürelim ve mevcut durumu yükleyelim
                            model.AllRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name", model.SelectedRoles);
                            model.CurrentRoles = (await _userManager.GetRolesAsync(user)).ToList();
                            return View(model);
                        }
                    }

                    TempData["SuccessMessage"] = "Kullanıcı bilgileri ve rolleri başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    TempData["ErrorMessage"] = $"Kullanıcı güncellenirken bir hata oluştu: {string.Join(", ", errors)}";
                }
            }

            // Model validasyonu başarısız olursa veya hata oluşursa, View'ı tekrar döndür
            // Tüm rolleri tekrar yüklemeyi ve seçili olanları korumayı unutma
            var allRolesOnFailure = await _roleManager.Roles.ToListAsync();
            model.AllRoles = new SelectList(allRolesOnFailure, "Name", "Name", model.SelectedRoles);
            var userOnFailure = await _userManager.FindByIdAsync(model.Id);
            if (userOnFailure != null)
            {
                model.CurrentRoles = (await _userManager.GetRolesAsync(userOnFailure)).ToList();
            }

            return View(model);
        }

        // KULLANICI SİLME (DELETE) - Onay Sayfasını Göster
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            ViewData["Title"] = "Kullanıcı Silme Onayı";
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Silinecek kullanıcı ID'si belirtilmedi.";
                return RedirectToAction(nameof(Index));
            }

            var userToDelete = await _userService.GetUserByIdAsync(id);
            if (userToDelete == null)
            {
                TempData["ErrorMessage"] = "Silinecek kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(userToDelete);
        }

        // KULLANICI SİLME (DELETE) - Silme İşlemini Gerçekleştir (Soft Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                TempData["ErrorMessage"] = "Silme işlemi için oturum bilgisi alınamadı.";
                return RedirectToAction(nameof(Index));
            }
            if (id == currentUserId)
            {
                TempData["ErrorMessage"] = "Kendi hesabınızı bu ekrandan silemezsiniz.";
                return RedirectToAction(nameof(Index));
            }

            var (succeeded, errors) = await _userService.SoftDeleteUserAsync(id, currentUserId);

            if (succeeded)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi (pasif hale getirildi).";
            }
            else
            {
                TempData["ErrorMessage"] = $"Kullanıcı silinirken bir hata oluştu: {string.Join(", ", errors)}";
            }

            return RedirectToAction(nameof(Index));
        }

        // ÇÖP KUTUSU (TRASH) - Soft Silinmiş Kullanıcıları Listele
        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            ViewData["Title"] = "Çöp Kutusu";
            var deletedUsers = await _userService.GetAllDeletedUsersAsync();
            return View(deletedUsers);
        }

        // KULLANICIYI GERİ YÜKLEME (RESTORE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(string id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                TempData["ErrorMessage"] = "Geri yükleme işlemi için oturum bilgisi alınamadı.";
                return RedirectToAction(nameof(Trash));
            }

            var (succeeded, errors) = await _userService.RestoreUserAsync(id, currentUserId);

            if (succeeded)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla geri yüklendi ve aktif hale getirildi.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Kullanıcı geri yüklenirken bir hata oluştu: {string.Join(", ", errors)}";
            }

            return RedirectToAction(nameof(Trash));
        }
    }
}