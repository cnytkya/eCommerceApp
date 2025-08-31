using eCommerceApp.Application.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        //DI ile kategori CRUD için ICategoryService i buraya inject(uygula) ediyoruz.
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        //kategori listeleme sayfasının metodu aynı zamanda kategorinin ana sayfası.
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }
    }
}
