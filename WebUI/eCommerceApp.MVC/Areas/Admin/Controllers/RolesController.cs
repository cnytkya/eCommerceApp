using eCommerceApp.Application.DTOs.Role;
using eCommerceApp.Application.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleDto createRoleDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Rol adı boş bırakılamaz.";
                return View(createRoleDto);
            }
            var (succeeded, errors) = await _roleService.CreateRoleAsync(createRoleDto);

            if (succeeded)
            {
                TempData["SuccessMessage"] = "Rol başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                TempData["ErrorMessage"] = "Rol oluşturulurken bir hata oluştu";
                return View(createRoleDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                TempData["ErrorMessage"] = "Düzenlenecek rol Id'si bulunamadı!!!";
                return RedirectToAction("Index");
            }
            var roleDto = await _roleService.GetRoleByIdAsync(roleId);
            if (roleDto == null)
            {
                TempData["ErrorMessage"] = "Rol bulunamadı!!!";
                return RedirectToAction("Index");
            }
            //EditRoleDto'ya dönüştürüp view'e gönderelim
            var editRoleDto = new EditRoleDto
            {
                Id = roleId,
                Name = roleDto.Name
            };
            return View(editRoleDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleDto editRoleDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Rol adı boş bırakılamaz.";
                return View(editRoleDto);
            }
            var (succeeded, errors) = await _roleService.UpdateRoleAsync(editRoleDto);
            if (succeeded)
            {
                TempData["SuccessMessage"] = "Rol başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                TempData["ErrorMessage"] = "Rol güncellenirken bir hata oluştu";
                return View(editRoleDto);
            }
        }
    }
}
