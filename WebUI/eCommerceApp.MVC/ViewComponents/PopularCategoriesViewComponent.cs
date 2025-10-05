using eCommerceApp.Application.Interface.Services;
using eCommerceApp.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.MVC.ViewComponents
{
    public class PopularCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public PopularCategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                //hata yoksa çalışacak kod bloğu
                var categoriesWithCount = await _categoryService.GetCategoriesWithProductCountsAsync();

                var popularCategories = categoriesWithCount
                    .Where(c => c.ProductCount > 0)
                    .OrderByDescending(c => c.ProductCount)
                    .Take(8)
                    .Select(c => new CategoryViewModel
                    {
                        Id = c.CategoryId,
                        Name = c.CategoryName,
                        ProductCount = c.ProductCount,
                        URL = $"/categories/{c.Slug}"
                    }).ToList();
                return View(popularCategories);
            }
            catch (Exception ex)
            {
                //hata varsa ex fırlatacak
                return View(new List<CategoryViewModel>());
            }
        }

        private string GetCategoryIcon(string categoryName)
        {
            return categoryName.ToLower() switch
            {
                string name when name.Contains("elektronik") || name.Contains("teknoloji") => "fas fa-laptop",
                string name when name.Contains("giyim") || name.Contains("moda") => "fas fa-tshirt",
                string name when name.Contains("ev") || name.Contains("yaşam") => "fas fa-home",
                string name when name.Contains("spor") => "fas fa-dumbbell",
                string name when name.Contains("kozmetik") || name.Contains("güzellik") => "fas fa-spa",
                string name when name.Contains("kitap") => "fas fa-book",
                string name when name.Contains("oyuncak") => "fas fa-gamepad",
                string name when name.Contains("mücevher") || name.Contains("takı") => "fas fa-gem",
                string name when name.Contains("ayakkabı") => "fas fa-shoe-prints",
                string name when name.Contains("çanta") => "fas fa-brifecase",
                _ =>"fas fa-shopping-bag"
            };
        }
    }
}
