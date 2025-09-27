using eCommerceApp.Application.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace eCommerceApp.Application.Interface.Services
{
    public interface IProductService
    {
        //Product CRUD operations
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<(bool succedeed, IEnumerable<string> errors)> CreateProductAsync(CreateProductDto model, IFormFile formFile);
        Task<(bool succedeed, IEnumerable<string> errors)> UpdateProductAsync(EditProductDto entity);
        Task<(bool succedeed, IEnumerable<string> errors)> DeleteProductByIdAsync(Guid id);
    }
}
