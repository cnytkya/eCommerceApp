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
            }
            catch (Exception ex)
            {
                //hata varsa ex fırlatacak
                return View(new List<CategoryViewModel>());
            }
        }
    }
}
