using eCommerceApp.Application.DTOs.Category;
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
                var categoriesWithCounts = await _categoryService.GetCategoriesWithProductCountsAsync();

                var popularCategories = categoriesWithCounts
                    .Where(c => c.ProductCount > 0)
                    .OrderByDescending(c => c.ProductCount)
                    .Take(8)
                    .Select(c => new CategoryViewModel
                    {
                        Id = c.CategoryId,
                        Name = c.CategoryName,
                        Icon = GetCategoryIcon(c.CategoryName),
                        ProductCount = c.ProductCount,
                        Url = $"/categories/{c.Slug}",
                        Color = GetCategoryColor(c.CategoryName)
                    })
                    .ToList();

                return View(popularCategories);
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error loading popular categories");
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
                string name when name.Contains("çanta") => "fas fa-briefcase",
                _ => "fas fa-shopping-bag"
            };
        }

        private string GetCategoryColor(string categoryName)
        {
            return categoryName.ToLower() switch
            {
                string name when name.Contains("elektronik") || name.Contains("teknoloji") => "#6366f1",
                string name when name.Contains("giyim") || name.Contains("moda") => "#8b5cf6",
                string name when name.Contains("ev") || name.Contains("yaşam") => "#06d6a0",
                string name when name.Contains("spor") => "#f59e0b",
                string name when name.Contains("kozmetik") || name.Contains("güzellik") => "#ec4899",
                string name when name.Contains("kitap") => "#10b981",
                string name when name.Contains("oyuncak") => "#f97316",
                string name when name.Contains("mücevher") || name.Contains("takı") => "#8b5cf6",
                _ => "#6366f1"
            };
        }
    }
}