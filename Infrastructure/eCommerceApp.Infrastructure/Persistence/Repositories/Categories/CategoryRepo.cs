using eCommerceApp.Application.Interface.Repositories.Categories;
using eCommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Persistence.Repositories.Categories
{
    public class CategoryRepo : Repository<Category>, ICategoryRepo
    {
        public CategoryRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug)
        {
            return await _dbSet.Include(c => c.Subcategories)//Alt kategorileri de dahil et.
                .Where(c => c.Slug == slug)
                .FirstOrDefaultAsync();
        }
    }
}
