using eCommerceApp.Application.Dtos.User;
using eCommerceApp.Application.DTOs.User;
using eCommerceApp.Application.Interface;
using eCommerceApp.Domain.Entities;
using eCommerceApp.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        //private readonly AppDbContext _appDbContext; bunun yerine generic olan yapı üzerinden DI uyguluyor olacağız.
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        //private readonly UserManager<AppUser> _userManager;

        public UsersController(IUserService userService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllActiveUsersAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //sayfa başlığı
            ViewData["Title"] = "Yeni Kullanıcı Ekle";
            var roles = await _roleManager.Roles.ToListAsync();//Bütün rolleri dropdown içerisine akatarabilmemiz gerekir.
            ViewBag.Roles = new SelectList(roles, "Name", "Name");//Dropdown için
            return View(new CreateUserDto { IsActive = true });//Yeni bir boş CreateUserDto gönder.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto createUserDto, string selectedRole) //CreateUserDto yu model olarak aldık.
        {
            ViewData["Title"] = "Yeni Kullanıcı Ekle";
            if (string.IsNullOrEmpty(selectedRole))
            {
                ModelState.AddModelError("selectedRole", "Lütfen bir rol seçiniz.");
            }
            if (ModelState.IsValid)
            {
                //UserService'ten CreateUserAsync metodunu çağır(artık şifre DTO içinde)
                var (succeeded, errors) = await _userService.CreateUserAsync(createUserDto, selectedRole);
                if (succeeded)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu.";
                    //return RedirectToAction("Index");
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
            //Model validasyonu başarısız olursa veya hata oluşursa, rolleri tekrar yükleyerek View'ı döndür
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Name", "Name", selectedRole);//seçili rolü koru.
            return View(createUserDto);
        }

        [HttpGet]//.../Users/Edit/id 
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Title"] = "Kullaınıcı Güncelle";
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Güncellenecek kullanıcının ID'si bulunamadı";
                return RedirectToAction(nameof(Index));
            }
            var userDto = await _userService.GetUserByIdAsync(id);
            if (userDto == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı";
                return RedirectToAction(nameof(Index));
            }

            var model = new EditUserViewModel
            {
                Id = userDto.Id,
                Fullname = userDto.Fullname,
                Email = userDto.Email,
                Bio = userDto.Bio,
                ImgUrl = userDto.ImgUrl,
                Location = userDto.Location,
                IsActive = userDto.IsActive,
                CurrentRoles = userDto.Roles.ToList()
            };
            var allRoles = await _roleManager.Roles.ToListAsync(); //her seferinde bütün roller dropdown içerisine aktarmak için.
            model.AllRoles = new SelectList(allRoles, "Name", "Name");//Rol isimlerini kullan.
            return View(model);
        }

        [HttpPost]//.../Users/Edit/post =>form işleme
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            //View data tanımlama
            ViewData["Title"] = "Kullanıcı Düzenle";
            //IsActive için eğer checkbox işaretlenmediyse false olarak ayarla

            if (!ModelState.ContainsKey(nameof(model.IsActive)))
            {
                model.IsActive = false;
            }
            if (ModelState.IsValid)
            {
                //EditUserViewModel'den EditUserDto'ya manuel mapping yapacağız.
                //Service katmanı UI EditUserViewModel'ı bilmeidği için DTO'ya dönüştürmemiz gerekiyor.
                var editUserDto = new EditUserDto
                {
                    Id = model.Id,
                    Fullname = model.Fullname,
                    Email = model.Email,
                    Bio = model.Bio,
                    ImgUrl = model.ImgUrl,
                    Location = model.Location,
                    IsActive = model.IsActive,

                };
                //Kullanıcı bilgileri güncelleme.
                var (succeded, errors) = await _userService.UpdateUserAsync(editUserDto);

                if (succeded)
                {
                    //Role Yönetimi: Kullanıcının mevcut rolünü çek.
                    var user = await _userManager.FindByIdAsync(model.Id);
                    var currentRoles = await _userManager.GetRolesAsync(user);

                    //Formdan rolleri al
                    var selectedRoles = model.SelectedRole ?? new List<string>();//null gelirse boş liste 

                    //Kaldırılacak roller: Mevcuut rollerde olup, seçilenlerde olmayanları kaldıralım.

                    var rolesToRemove = currentRoles.Except(selectedRoles).ToList();

                    if (rolesToRemove.Any())
                    {
                        //Rolleri kaldır
                        var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                        if (!removeResult.Succeeded)
                        {
                            TempData["Error Message"] = "Roller kaldırılırken hata oluştur:" + string.Join(", ", removeResult.Errors.Select(e => e.Description));
                            //Hata oluşursa tekrar View'ı döndür ve mevcut durumu yükle.
                            model.AllRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name", selectedRoles);

                            model.CurrentRoles = (await _userManager.GetRolesAsync(user)).ToList();
                            return View(model);
                        }
                    }

                    //Eklenecek roller: Seçilen rollerden mevcut rollerde olmayanları ekle.
                    var rolesToAdd = selectedRoles.Except(currentRoles).ToList();
                    if (rolesToAdd.Any())
                    {
                        var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                        if (!addResult.Succeeded)
                        {
                            TempData["ErrorMessage"] = "Roller eklenirken hata oluştu: " + string.Join(", ", addResult.Errors.Select(e => e.Description));
                            //Hata oluşursa tekrar View'ı döndür ve mevcut durumu yükle.
                            model.AllRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name", selectedRoles);

                            model.CurrentRoles = (await _userManager.GetRolesAsync(user)).ToList();
                            return View(model);
                        }
                        TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi: ";
                        //Eğer düzenleme başarılı ise kullanıcıyı Users/Idex'e kullanıcı listeye yönlendir.
                        return RedirectToAction("Index");
                    }
                }

                else
                {
                    //Hata mesajlarını model state'e ekle
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    TempData["ErrorMessage"] = $"Kullanıcı güncellenirken bir hata oluştu: {string.Join(", ", errors)}";
                }
            }
            //Model validasyonu başarısız olursa veya hata oluşursa, rolleri tekrar yükleyerek View'ı döndür
            //Sayfa tekrar yüklenirken seçili olan rolleri korumalıyız.
            var allRolesOnFailure = await _roleManager.Roles.ToListAsync();
            model.AllRoles = new SelectList(allRolesOnFailure, "Name", "Name", model.SelectedRole);


            //Rolleri çektikten sonra View'e göndermemiz gerekir. 
            var userOnFailure = await _userManager.FindByIdAsync(model.Id);
            if (userOnFailure != null)
            {
                model.CurrentRoles = (await _userManager.GetRolesAsync(userOnFailure)).ToList();
            }
            return View(model);
        }
    }
}
