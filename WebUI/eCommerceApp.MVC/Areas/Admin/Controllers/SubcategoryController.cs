using AutoMapper;
using eCommerceApp.Application.DTOs.Subcategory;
using eCommerceApp.Application.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eCommerceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")] alt kategoriler ile ilgili olan kısımlara erişimi sadece admin rolü olanlara açtık. yani sadece rolü admin olan kullanıcı alt kategorilere erişim sağlayabilir. bu attribute sayesinde yetkisiz kişilerin bir yerin bir bölümüne erişimini kısıtlamış oluyoruz.

    //CRUD
    public class SubcategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public SubcategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(Guid id)//bir üst kategoriye ait alt kategorilerin listesini tutacak.
        {
            if (id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Kategori ID'si belirtimedi";
                return RedirectToAction("Index", "Categories");
            }
            //id'ye göre category getir.
            var category = await _categoryService.GetCategoryByIdAsync(id);
            //category boş(null) mu? eğer boş ise kullanıcıya kategori bulunamadı uyarısı verelim.
            if (category == null)
            {
                TempData["ErrorMessage"] = "Kategori bulunmadı";
                return RedirectToAction("Index", "Categories");
            }
            //herhangi bir sorun yoksa burdan devam edilir. 
            ViewData["Title"] = $"{category.Name} - Alt Kategoriler"; //hangi üst kategorinin alt kategorileri listelenecek sorusunun cevabı. ör: Teknoloji - Alt Kategorileri.
            ViewBag.CategoryName = category.Name;
            ViewBag.CategoryId = id;
            var subCategories = await _categoryService.GetSubCategoriesByCategoryIdAsync(id);
            return View(subCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Kategori ID'si belirtimedi";
                return RedirectToAction("Index", "Category");
            }
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Kategori bulunmadı";
                return RedirectToAction("Index", "Category");
            }
            ViewData["Title"] = $"{category.Name} - Alt Kategori Oluştur";
            var model = new CreateSubCategoryDto { CategoryId = id };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSubCategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen tüm alanları doğru doldurnuz";
                return View(model);
            }
            var (succeeded, errors) = await _categoryService.CreateSubCategoryAsync(model);
            if (succeeded)
            {
                TempData["SuccessMessage"] = "Alt kategori başarıyla oluşturuldu";
                return RedirectToAction("Index", new {id = model.CategoryId});
            }

            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            TempData["ErrorMessage"] = $"Alt kategori oluşturulurken bir hata oluştu: {string.Join(", ",errors)}";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var subcategoryDto = await _categoryService.GetSubcategoryByIdAsync(id);
            if (subcategoryDto == null)
            {
                TempData["ErrorMessage"] = "Alt kategori bulunamadı";
                return RedirectToAction("Index","Subcategory");
            }

            var categroy = await _categoryService.GetCategoryByIdAsync(subcategoryDto.CategoryId);
            if (categroy == null)
            {
                TempData["ErrorMessage"] = "Kategori bulunamadı";
                return RedirectToAction("Index", "Subcategory");
            }
            ViewData["Title"] = $"Alt kategori: {subcategoryDto.Name}";
            ViewBag.CategoryName = categroy.Name;
            var model = _mapper.Map<EditSubCategoryDto>(subcategoryDto);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSubCategoryDto model) //Upsert yapılabilir. Edit ve Create metotlarını birleştirebiliriz.
        {
            ViewData["Title"] = $"Alt Kategori Düzenle {model.Name}";
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen tüm alanları doğru doldurnuz";
                return View(model);
            }
            var (succeeded, errors) = await _categoryService.UpdateSubCategoryAsync(model);
            if (succeeded)
            {
                TempData["SuccessMessage"] = "Alt kategori başarıyla güncellendi";
                return RedirectToAction("Index", new { id = model.CategoryId });
            }

            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            TempData["ErrorMessage"] = $"Alt kategori güncellenirken bir hata oluştu: {string.Join(", ", errors)}";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var subcategoryDto = await _categoryService.GetSubcategoryByIdAsync(id);
            if (subcategoryDto == null)
            {
                TempData["ErrorMessage"] = "Alt kategori bulunamadı";
                return RedirectToAction("Index","Subcategory");
            }
            var category = await _categoryService.GetCategoryByIdAsync(subcategoryDto.CategoryId);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Kategori bulunamadı";
                return RedirectToAction("Index", "Subcategory");
            }
            ViewData["Title"] = $"Alt kategori silme onayı: {subcategoryDto.Name}";
            ViewBag.CategoryName = category.Name;
            return View(subcategoryDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subcategoryDto = await _categoryService.GetSubcategoryByIdAsync(id);
            if (subcategoryDto == null)
            {
                TempData["ErrorMessage"] = "Alt kategori bulunamadı";
                return RedirectToAction("Index", "Subcategory");
            }
            //burda silme işlemi gerçekleşsin
            var (succedeed, errors) = await _categoryService.DeleteSubCategoryAsync(id);
            if (succedeed)
            {
                TempData["SuccessMessage"] = "Alt kategori başarıyla silindi.";
            }
            else
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                TempData["ErrorMessage"] = $" Alt kategori silinirken bir hata oluştu: {string.Join(", ", errors)}";
            }
            return RedirectToAction("Index", new {id = subcategoryDto.CategoryId});
        }
    }
}
