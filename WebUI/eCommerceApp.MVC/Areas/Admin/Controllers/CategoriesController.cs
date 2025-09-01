using eCommerceApp.Application.DTOs.Category;
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDto model)
        {
            //doldurulması zorunlu alan eğer doldurulmadıysa yani boş bırakıldıysa kullanıcıya alert verdirelim.
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen tüm zorunlu alanları doldurun!";
                return View(model);
            }
            var (succeeded, errors) = await _categoryService.CreateCategoryAsync(model);
            if (succeeded)
            {
                TempData["SuccessMessage"] = "Kategori başarıyla oluşturuldu";
                return RedirectToAction("Index");
            }

            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            TempData["ErrorMessage"] = "Kategori oluşturulurken bir hata oluştu: " + string.Join(", ",errors);
            return View(model);
        }
    }
}
