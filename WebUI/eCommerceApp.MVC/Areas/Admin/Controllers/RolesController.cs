using eCommerceApp.Application.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eCommerceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Yeni Role Oluştur";
            var roles = await _roleService.GetAllRolesAsync();
            return View();
        }
    }
}
