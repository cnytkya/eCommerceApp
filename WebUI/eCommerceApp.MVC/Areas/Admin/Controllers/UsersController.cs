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
        //private readonly UserManager<AppUser> _userManager;

        public UsersController(IUserService userService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _roleManager = roleManager;
            //_userManager = userManager;
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
            return View(new CreateUserDto { IsActive = true});//Yeni bir boş CreateUserDto gönder.
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
            var roles =await _roleManager.Roles.ToListAsync();
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
                Email= userDto.Email,
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
        public async Task<IActionResult> Edit()
        {
            //View data tanımlama
            ViewData["Title"] = "Kullanıcı Düzenle";
            //IsActive için eğer checkbox işaretle
        }
    }
}
