using eCommerceApp.Application.Interface.Repositories.Products;
using eCommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Persistence.Repositories.Products
{
    public class ProductRepo : Repository<Product>, IProductRepositories
    {
        public ProductRepo(AppDbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<Product>> GetAllProductsWithSubcategoryAsync()
        {
            //Ürünleri, ait oldukları alt kategorileri ile birlikte getir.
            return await _dbSet.Include(p => p.Subcategory)
                .ThenInclude(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithSubcategoryIdAsync(Guid id)
        {
            //Belirli bir ürünü, ait olduğu alt kategoriyle birlikte ID'ye göre getir.
            return await _dbSet.Include(p => p.Subcategory)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<int> CountProductsBySubcategoryIdAsync(Guid subcategoryId)//Belirli bir kategoriye ait aktif ürün sayısını asenkron olarak alır ve sayar.
        {
            return await _dbSet.CountAsync(p => p.SubcategoryId == subcategoryId);
        }
    }
}
