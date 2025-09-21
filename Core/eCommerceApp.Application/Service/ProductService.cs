using AutoMapper;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.Interface.Repositories.Categories;
using eCommerceApp.Application.Interface.Repositories.Products;
using eCommerceApp.Application.Interface.Services;

namespace eCommerceApp.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepositories _productRepo;
        private readonly ISubCategoryRepo _subCategoryRepo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepositories productRepo, ISubCategoryRepo subCategoryRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _subCategoryRepo = subCategoryRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            //Ürünleri alt kategoriyle birlikte listele.
            var products = await _productRepo.GetAllProductsWithSubcategoryAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        public Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<(bool succedeed, IEnumerable<string> errors)> CreateProductAsync(CreateProductDto model)
        {
            throw new NotImplementedException();
        }

        public Task<(bool succedeed, IEnumerable<string> errors)> DeleteProductByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }



        public Task<(bool succedeed, IEnumerable<string> errors)> UpdateProductAsync(EditProductDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
