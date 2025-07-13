using eCommerceApp.Application.Interface;
using eCommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        //private readonly AppDbContext _appDbContext; bunun yerine generic olan yapı üzerinden DI uyguluyor olacağız.
        private readonly IUserService _userService;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly UserManager<AppUser> _userManager;

        public UsersController(IUserService userService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _userService = userService;
            //_roleManager = roleManager;
            //_userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllActiveUsersAsync();
            return View(users);
        }
    }
}
