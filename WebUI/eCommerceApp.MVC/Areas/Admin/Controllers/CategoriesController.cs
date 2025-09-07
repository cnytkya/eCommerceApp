using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);
            if (categoryDto == null)
            {
                TempData["ErrorMessage"] = "Kategori bulunamadı";
            }
            var model = _mapper.Map<EditCategoryDto>(categoryDto);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//tehditlere karşı koruma sağlar
        public async Task<IActionResult> Edit(EditCategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen zorunlu alanları boş bırakmayınız!";
                return View(model);
            }

            var (succeeded,errors) = await _categoryService.UpdateCategoryAsync(model);
            if (succeeded)
            {
                TempData["SuccessMessage"] = "Kategori başarıyla güncellendi";
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index");
            }

            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            TempData["ErrorMessage"] = "Kategori güncellenirken bir hata oluştu: " + string.Join (", ",errors);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);
            if (categoryDto == null)
            {
                TempData["ErrorMessage"] = "Kategori bulunamadı";
            }
            return View(categoryDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)//hard delete uygulanacak
        {
            var (succeeded,errors) = await _categoryService.DeleteCategoryAsync(id);
            if (succeeded)
            {
                TempData["SuccessMessage"] = "Kategori başarıyla silindi";
            }
            else
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty,error);
                }
                TempData["ErrorMessage"] = $"Kategori silinirken bir hata oluştu: {string.Join(", ",errors)}";
            }
            return RedirectToAction("Index");
        }
    }
}
