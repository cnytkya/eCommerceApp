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
            return await _dbSet.Include(p => p.Subcategory).ToListAsync();
        }

        public async Task<Product?> GetProductWithSubcategoryIdAsync(Guid id)
        {
            //Belirli bir ürünü, ait olduğu alt kategoriyle birlikte ID'ye göre getir.
            return await _dbSet.Include(p => p.Subcategory).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
