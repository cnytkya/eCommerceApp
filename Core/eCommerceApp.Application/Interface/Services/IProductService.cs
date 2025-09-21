using eCommerceApp.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Interface.Services
{
    public interface IProductService
    {
        //Product CRUD operations
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<(bool succedeed, IEnumerable<string> errors)> CreateProductAsync(CreateProductDto model);
        Task<(bool succedeed, IEnumerable<string> errors)> UpdateProductAsync(EditProductDto entity);
        Task<(bool succedeed, IEnumerable<string> errors)> DeleteProductByIdAsync(Guid id);
    }
}
